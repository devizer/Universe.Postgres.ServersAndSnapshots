using System;
using System.IO;
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

            TryAndForget.Execute(() => Directory.Delete(options.DataPath));
        }

    }
}