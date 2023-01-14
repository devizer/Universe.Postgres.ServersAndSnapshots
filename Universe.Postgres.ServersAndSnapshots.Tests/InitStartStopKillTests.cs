using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Npgsql;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class InitStartStopKillTests : NUnitTestsBase
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
            var resultInit = PostgresServerManager.CreateServerInstance(serverBinaries, options);
            Console.WriteLine(@$"INIT DB Output:{Environment.NewLine}{resultInit.OutputText}");

            OnDisposeSilent(() => Directory.Delete(options.DataPath, true));
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestStartDb(PgServerTestCase testCase)
        {
            var serverBinaries = testCase.ServerBinaries;
            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + "-" + serverBinaries.Version + "-start"),
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
                Locale = testCase.Locale,
            };

            var resultInit = PostgresServerManager.CreateServerInstance(serverBinaries, options);
            Console.WriteLine(@$"INIT DB Output:{Environment.NewLine}{resultInit.OutputText}");

            var resultStart = PostgresServerManager.StartInstance(serverBinaries, options);
            Console.WriteLine(@$"START SERVER Output:{Environment.NewLine}{resultStart.OutputText}");

            NpgsqlConnectionStringBuilder csBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Port = options.ServerPort,
                Username = options.SystemUser,
                Password = options.SystemPassword,
                Timeout = 1,
                CommandTimeout = 1,
            };

            Stopwatch waitForStart = NpgsqlWaitForExtensions.WaitForPgsqllDb(csBuilder.ToString(), 15000, out var serverVersion, out var conError);

            if (conError != null)
                Console.WriteLine($"CONNECTION ERROR: {conError.Message}{Environment.NewLine}{conError}");
            else
            {
                Console.WriteLine($"SUCCESSFUL CONNECTION in {waitForStart.ElapsedMilliseconds:n0} milliseconds{Environment.NewLine}{serverVersion}");
                Console.WriteLine($"[LOCALE '{options.Locale}'] {new NpgsqlConnection(csBuilder.ConnectionString).GetCurrentDatabaseLocale()}");
            }

            
            TryAndForget.Execute(() => PostgresServerManager.StopInstance(serverBinaries, options));
            TryAndForget.Execute(() => Directory.Delete(options.DataPath, true));

            // STOP
            Stopwatch waitForStopDb = NpgsqlWaitForExtensions.WaitForPgsqllDb(csBuilder.ToString(), 2000, out var serverVersionOnStop, out var conErrorOnStop);
            if (conErrorOnStop != null)
                Console.WriteLine($"[ON STOP] connection error as expected: {conErrorOnStop.Message}");
            else
                Console.WriteLine($"[ON STOP] Warning! Unexpected successful connection in {waitForStopDb.ElapsedMilliseconds:n0} milliseconds{Environment.NewLine}{serverVersionOnStop}");

            Assert.IsNull(conError);
            Assert.IsNotNull(serverVersion);

            Assert.IsNotNull(conErrorOnStop);
            Assert.IsNull(serverVersionOnStop);
        }

    }
}