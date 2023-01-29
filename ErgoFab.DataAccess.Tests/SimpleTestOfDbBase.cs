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
        public void TestEmptyInitialization()
        {
            Console.WriteLine($"ConnectionString: [{this.ConnectionString}]");
            Assert.IsNotNull(this.ConnectionString);

            using var conn = new NpgsqlConnection(this.ConnectionString);
            conn.CheckConnectivity(timeout: 0);
            Console.WriteLine(conn.CheckConnectivity(timeout: 5000));
        }

        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        public void TestEmptyInitializationTwice(string id)
        {
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
        public void TestBasicSeederTwice(string id)
        {
            if (_LatestServer.Value?.Version.Major < 10) return;

            Console.WriteLine($"ConnectionString: [{this.ConnectionString}]");
            Assert.IsNotNull(this.ConnectionString);

            using var conn = new NpgsqlConnection(this.ConnectionString);
            conn.CheckConnectivity(timeout: 0);
            Console.WriteLine(conn.CheckConnectivity(timeout: 5000));

            var db = ErgoFabDbContextFactory.CreateErgoFabDbContext(ConnectionString);
            Assert.IsTrue(db.Organization.Any(), "At least one organization is expected");
        }

    }
}
