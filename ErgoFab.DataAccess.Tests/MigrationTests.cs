﻿using System.Diagnostics;
using ErgoFab.DataAccess.Model;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using NUnit.Framework;
using Universe.NpglExtensions;
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
        public void JustMigrate(string id)
        {
            MigrateImplementation();
        }

        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        public void MigrateAndSeed(string id)
        {
            var db = MigrateImplementation();
            if (db == null) return;
            ErgoFabInitialSeeder seeder = new ErgoFabInitialSeeder(db);
            seeder.Seed();
        }


        private ErgoFabDbContext MigrateImplementation()
        {
            var latestServer = PostgresServerDiscovery.GetServers().OrderByDescending(x => x.Version).FirstOrDefault();
            if (latestServer == null || latestServer.Version.Major < 10) return null;

            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, Guid.NewGuid().ToString("N")),
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
                Locale = TestUtils.GetUnicodePostgresLocale(),
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
                Pooling = false,
            };

            var connectionString = connectionStringOptions.ConnectionString;
            var connection = new NpgsqlConnection(connectionString);

            var locale = SilentEvaluate(() => new NpgsqlConnection(connection.ConnectionString).GetCurrentDatabaseLocale());
            Console.WriteLine($"[LOCALE '{options.Locale}'] {locale}");

            var dbOptions = new DbContextOptionsBuilder<ErgoFabDbContext>();
            dbOptions.UseNpgsql(connectionString);
            ErgoFabDbContext db = new ErgoFabDbContext(dbOptions.Options);

            Stopwatch sw = Stopwatch.StartNew();
            db.Database.Migrate();
            Console.WriteLine($"Migration took {sw.ElapsedMilliseconds:n0} milliseconds");
            return db;
        }

    }
}
