using System;
using System.IO;
using NUnit.Framework;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class DiscoveryTests
    {

        [Test]
        public void Test1()
        {
            TestContext.Progress.WriteLine(Path.GetPathRoot(Environment.SystemDirectory));
            TestContext.Progress.WriteLine(new DirectoryInfo("C:\\X").FullName);
            TestContext.Progress.WriteLine(new DirectoryInfo("C:\\X\\").FullName);
        }

        [Test]
        [TestCase("First")]
        [TestCase("Next")]
        public void TestDiscovery(string id)
        {
            var servers = PostgresServerDiscovery.GetServers();
            TestContext.Progress.WriteLine($"Servers: {servers.Length}");
            foreach (var server in servers)
            {
                TestContext.Progress.WriteLine($"• {server}");
            }
        }
    }
}