using System;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class InitAndStartTests : NUnitTestsBase
    {
        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestInitDb(ServerBinaries serverBinaries)
        {
            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + "-" + serverBinaries.Version + "-start"),
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
            };
            var resultInit = PostgresServerManager.CreateServerInstance(serverBinaries, options);
            Console.WriteLine(@$"INIT DB Output:
{resultInit.OutputText}");

            var resultStart = PostgresServerManager.StartInstance(serverBinaries, options);
            Console.WriteLine(@$"START SERVER Output:
{resultStart.OutputText}");

            TryAndForget.Execute(() => PostgresServerManager.StopInstance(serverBinaries, options));
            TryAndForget.Execute(() => Directory.Delete(options.DataPath));
        }
    }
}