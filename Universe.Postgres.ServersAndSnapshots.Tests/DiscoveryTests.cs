using System;
using System.IO;
using System.Reflection;
using System.Threading;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class AllTests : NUnitTestsBase
    {
        static int Port = 5432;
        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestInitDb(ServerBinaries serverBinaries)
        {
            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")),
                ServerPort = Interlocked.Increment(ref Port),
            };
            var result = PostgresServerManager.CreateServerInstance(serverBinaries, options);
            Console.WriteLine(result.OutputText);
        }

    }

    public class PgServerTestCase
    {
        public ServerBinaries ServerBinaries { get; set; }
        public static ServerBinaries[] GetServers() => PostgresServerDiscovery.GetServers();

        public override string ToString()
        {
            return ServerBinaries.ToString();
        }
    }

    public class DiscoveryTests : NUnitTestsBase
    {

        [Test]
        public void TestNothing()
        {

            Version.TryParse("16.99999", out var v);
            TestContext.Progress.WriteLine($"Version: {v}");

            TestContext.Progress.WriteLine(Path.GetPathRoot(Environment.SystemDirectory));
            TestContext.Progress.WriteLine(new DirectoryInfo("C:\\X").FullName);
            TestContext.Progress.WriteLine(new DirectoryInfo("C:\\X\\").FullName);
        }

        [Test]
        [TestCase("First")]
        [TestCase("Next")]
        public void TestDiscovery(string id)
        {
            ServerBinaries[] servers = PostgresServerDiscovery.GetServers();
            TestContext.Progress.WriteLine($"Servers: {servers.Length}");
            foreach (var server in servers)
            {
                TestContext.Progress.WriteLine($"• {server}");
            }
        }
    }
}