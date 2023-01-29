using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ErgoFab.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NUnit.Framework;
using Universe.NUnitTests;
using Universe.Postgres.ServersAndSnapshots;
using Universe.Postgres.ServersAndSnapshots.Tests;

namespace ErgoFab.DataAccess.Tests
{
    public interface IDbSeeder
    {
        // Multiple DB Context is supported
        void Seed(string connectionString);
    }

    public class BasicDbSeeder : IDbSeeder
    {

        public void Seed(string connectionString)
        {
            
            using ErgoFabDbContext ergoFabDbContext = ErgoFabDbContextFactory.CreateErgoFabDbContext(connectionString);
            ergoFabDbContext.Database.Migrate();
            var seeder = new ErgoFabInitialSeeder(ergoFabDbContext);
            seeder.Seed(maxEmployeesPerOrg: 4);
        }
    }

    public class LargeDbSeeder : IDbSeeder
    {
        public void Seed(string connectionString)
        {
            using var ergoFabDbContext = ErgoFabDbContextFactory.CreateErgoFabDbContext(connectionString);
            ergoFabDbContext.Database.Migrate();
            var seeder = new ErgoFabInitialSeeder(ergoFabDbContext);
            seeder.Seed(maxEmployeesPerOrg: 100);
        }
    }

    public class ErgoFabDbContextFactory
    {
        public static ErgoFabDbContext CreateErgoFabDbContext(string connectionString)
        {
            var dbOptions = new DbContextOptionsBuilder<ErgoFabDbContext>();
            dbOptions.UseNpgsql(connectionString);
            return new ErgoFabDbContext(dbOptions.Options);
        }

        public string ConnectionString { get; set; }
    }

    public enum DbRecreateMode
    {
        PerTestMethod,
        PerTestClass,
        Manually,
    }

    public class DbRecreateAttribute : Attribute
    {
        public readonly DbRecreateMode Mode;
        public string DbId { get; set; }
        public DbRecreateAttribute(DbRecreateMode mode)
        {
            Mode = mode;
        }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class DbSeederAttribute : Attribute
    {
        public Type Seeder { get; private set; }
        public DbSeederAttribute(Type seeder)
        {
            Seeder = seeder;
        }
    }

    public partial class BaseErgoFabTest : NUnitTestsBase
    {

        // The Goal is to prepare database on Startup and initialize using the DbSeederAttribute's seeder
        // Multiple test cases also supported
        protected string ConnectionString;


        private ServerBinaries FindServerDefinition()
        {
            foreach (var testArgument in TestContext.CurrentContext.Test.Arguments)
            {
                var ret = TryServerDefinitionByArgument(testArgument);
                if (ret != null) return ret;
            }

            return null;
        }

        [SetUp]
        public void BaseErgoFabTestSetup()
        {
            Console.WriteLine("[Db Setup] Starting ...");

            // 2. Server Definition:
            // Either by TryServerDefinitionByArgument
            ServerBinaries server = FindServerDefinition();
            if (server == null)
            {
                // Or Latest by Discovery
                server = GetPreferredServer();
            }


            // 1. Seeder
            DbSeederAttribute attr = GetTestAttribute<DbSeederAttribute>();
            IDbSeeder seeder = null;
            if (attr != null)
            {
                var seederType = attr.Seeder;
                var seederRaw = Activator.CreateInstance(seederType);
                seeder = seederRaw as IDbSeeder;
                if (seeder == null)
                    throw new NotImplementedException($"Seeder {seederType} for Test {TestContext.CurrentContext.Test.MethodName} is not valid IDbSeeder");
            }

            Console.WriteLine($"[Db Setup] Seeder is '{seeder}', Server is '{server}'");

            if (server == null)
            {
                ConnectionString = null;
                return;
            }


            var cacheKey = $"{server.PgCtlFullPath}⇛{seeder?.GetType()}";
            NpgsqlConnectionStringBuilder connection;
            PostgresInstanceOptions options;
            if (Backups.TryGetValue(cacheKey, out var backupFolder))
            {
                (connection, options) = RestoreDatabase(server, backupFolder.Folder, backupFolder.DbName);
            }
            else
            {
                (connection, options) = CreateDatabase(server);
                backupFolder = new BackupState()
                {
                    Folder = Path.Combine(TestUtils.RootSnapshotFolder, Guid.NewGuid().ToString("N")),
                    DbName = connection.Database,
                };
                TestUtils.CopyDirectory(options.DataPath, backupFolder.Folder, recursive: true);
                // On Global Dispose: Directory.Delete(backupFolder.Folder);
            }

            if (seeder != null)
            {
                // Cache By Seeder
                seeder.Seed(connection.ConnectionString);
            }

            ConnectionString = connection.ConnectionString;
            Console.WriteLine($"[Db Setup] Completed '{ConnectionString}'");

        }

        private static IDictionary<string, BackupState> Backups = new ConcurrentDictionary<string, BackupState>();

        private class BackupState
        {
            public string Folder, DbName;
        }

        private (NpgsqlConnectionStringBuilder, PostgresInstanceOptions) RestoreDatabase(ServerBinaries server, string backupFolder, string dbName)
        {
            throw new NotImplementedException();
        }

        private (NpgsqlConnectionStringBuilder, PostgresInstanceOptions) CreateDatabase(ServerBinaries server)
        {
            var newDbName = $"ErgoFab DB {Guid.NewGuid().ToString("N")}";
            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, Guid.NewGuid().ToString("N")),
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
                Locale = TestUtils.GetUnicodePostgresLocale(),
                StatementLogFolder = "CSV Logs",
            };

            PostgresServerManager.CreateServerInstance(server, options);
            PostgresServerManager.StartInstance(server, options);
            // TODO: Replace by Global Tear Down
            // https://stackoverflow.com/questions/3619735/nunit-global-initialization-bad-idea
            OnDisposeSilentAsync($"Stop Server and Clean up DB {newDbName}", () =>
            {
                TryAndForget.Execute(() => PostgresServerManager.StopInstanceSmarty(server, options));
                TryAndForget.Execute(() => Directory.Delete(options.DataPath));
            });

            NpgsqlConnectionStringBuilder ret = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Port = options.ServerPort,
                Username = options.SystemUser,
                Password = options.SystemPassword,
                Pooling = true,
            };

            var connection = new NpgsqlConnection(ret.ConnectionString);
            connection.Execute($"Create Database \"{newDbName}\"");
            ret.Database = newDbName;
            return (ret, options);
        }

        TA GetTestAttribute<TA>() where TA : Attribute
        {
            TA GetAttribute(IEnumerable<Attribute> attributes)
            {
                return attributes?.OfType<TA>().FirstOrDefault();
            }

            var test = TestContext.CurrentContext.Test;
            var methodInfo = this.GetType().GetMethods().FirstOrDefault(x => x.Name == test.MethodName);
            var attr = GetAttribute(methodInfo?.GetCustomAttributes(typeof(TA)));
            if (attr != null) return attr;
            attr = GetAttribute(this.GetType().GetCustomAttributes(typeof(TA)));
            return attr;
        }
    }
}
