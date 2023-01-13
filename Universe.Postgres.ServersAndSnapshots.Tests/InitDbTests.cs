using System;
using System.IO;
using System.Threading;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class InitDbTests : NUnitTestsBase
    {
        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestInitDb(PgServerTestCase testCase)
        {
            var serverBinaries = testCase.ServerBinaries;
            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + "-" + serverBinaries.Version + "-init"),
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
                Locale = testCase.Locale,
            };
            var result = PostgresServerManager.CreateServerInstance(serverBinaries, options);
            Console.WriteLine(result.OutputText);

            TryAndForget.Execute(() => Directory.Delete(options.DataPath));
        }
    }
}