using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using NUnit.Framework;
using Universe.NpglExtensions;

namespace ErgoFab.DataAccess.Tests
{
    [TestFixture]
    public class SimpleTestOfDbBase : BaseErgoFabTest
    {

        [Test]
        [MinimumPostgresVersion(10)]
        public void TestEmptyInitialization()
        {
            // if (LatestVersionMajor < 10) return;
            Console.WriteLine($"ConnectionString: [{this.ConnectionString}]");
            Assert.IsNotNull(this.ConnectionString);

            using var conn = new NpgsqlConnection(this.ConnectionString);
            conn.CheckConnectivity(timeout: 0);
            Console.WriteLine(conn.CheckConnectivity(timeout: 5000));
        }

        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        [MinimumPostgresVersion(10)]
        public void TestEmptyInitializationTwice(string id)
        {
            // if (LatestVersionMajor < 10) return;
            Console.WriteLine($"ConnectionString: [{this.ConnectionString}]");
            Assert.IsNotNull(this.ConnectionString);

            using var conn = new NpgsqlConnection(this.ConnectionString);
            conn.CheckConnectivity(timeout: 0);
            Console.WriteLine(conn.CheckConnectivity(timeout: 5000));
        }

        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        [DbSeeder(typeof(BasicDbSeeder))]
        [MinimumPostgresVersion(10)]
        public void TestBasicSeederTwice(string id)
        {
            Console.WriteLine($"Latest Server: {_LatestServer.Value}");
            // if (LatestVersionMajor < 10) return;

            Console.WriteLine($"ConnectionString: [{this.ConnectionString}]");
            Assert.IsNotNull(this.ConnectionString);

            using var conn = new NpgsqlConnection(this.ConnectionString);
            conn.CheckConnectivity(timeout: 0);
            Console.WriteLine(conn.CheckConnectivity(timeout: 5000));

            var db = ErgoFabDbContextFactory.CreateErgoFabDbContext(ConnectionString);
            Console.WriteLine($"Total Organizations: {db.Organization.Count()}");
            Assert.IsTrue(db.Organization.Any(), "At least one organization is expected");

        }

    }
}
