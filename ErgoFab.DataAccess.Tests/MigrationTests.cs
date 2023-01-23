using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ErgoFab.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NUnit.Framework;
using Universe.NUnitTests;
using Universe.Postgres.ServersAndSnapshots;
using Universe.Postgres.ServersAndSnapshots.Tests;

namespace ErgoFab.DataAccess.Tests
{
    public class MigrationTests : NUnitTestsBase
    {
        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        public void Migrate(string id)
        {
            MigrateImplementation();
        }

        private ErgoFabDbContext MigrateImplementation()
        {
            var latestServer = PostgresServerDiscovery.GetServers().OrderByDescending(x => x.Version).FirstOrDefault();
            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, Guid.NewGuid().ToString("N")),
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
            };

            PostgresServerManager.CreateServerInstance(latestServer, options);
            PostgresServerManager.StartInstance(latestServer, options);

            OnDisposeSilent("Stop Server", () => PostgresServerManager.StopInstanceSmarty(latestServer, options));
            OnDisposeSilent($"Delete Folder {options.DataPath}", () => Directory.Delete(options.DataPath, true));

            NpgsqlConnectionStringBuilder connectionStringOptions = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Port = options.ServerPort,
                Username = options.SystemUser,
                Password = options.SystemPassword,
            };

            var connectionString = connectionStringOptions.ConnectionString;
            var connection = new NpgsqlConnection(connectionString);

            var dbOptions = new DbContextOptionsBuilder<ErgoFabDbContext>();
            dbOptions.UseNpgsql(connectionString);
            ErgoFabDbContext db = new ErgoFabDbContext(dbOptions.Options);

            db.Database.Migrate();
            return db;
        }

        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        public void MigrateAndSeed(string id)
        {
            var db = MigrateImplementation();
            ErgoFabInitialSeeder seeder = new ErgoFabInitialSeeder(db);
            // seeder.Seed();
        }

    }
    }
