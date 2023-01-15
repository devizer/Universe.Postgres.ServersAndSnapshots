using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
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
            var (connection, options) = InitDb(testCase);
            OnDispose(() => Directory.Delete(options.DataPath, true));
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestKillServer(PgServerTestCase testCase)
        {
            var serverBinaries = testCase.ServerBinaries;
            var (connection, options) = InitDb(testCase);
            OnDisposeSilent(() => Directory.Delete(options.DataPath, true));

            Stopwatch startAt = Stopwatch.StartNew();
            var resultStart = PostgresServerManager.StartInstance(serverBinaries, options, waitFor: true);
            Console.WriteLine(@$"START SERVER Output (took {startAt.ElapsedMilliseconds:n0} milliseconds):{Environment.NewLine}{resultStart.OutputText}");

            WaitForServer(testCase, options, connection, 15000, expectSuccess: true);

            Stopwatch killAt = Stopwatch.StartNew();
            var resultKill = PostgresServerManager.KillInstance(serverBinaries, options);
            Console.WriteLine(@$"KILL SERVER Output (took {killAt.ElapsedMilliseconds:n0} milliseconds):{Environment.NewLine}{resultKill.OutputText}");

            WaitForServer(testCase, options, connection, 3000, expectSuccess: false);
        }


        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestStartServer(PgServerTestCase testCase)
        {
            var (connection, options) = InitDb(testCase); 
            var serverBinaries = testCase.ServerBinaries;

            Stopwatch startAt = Stopwatch.StartNew();
            var resultStart = PostgresServerManager.StartInstance(serverBinaries, options, waitFor: true);
            Console.WriteLine(@$"START SERVER Output (took {startAt.ElapsedMilliseconds:n0} milliseconds):{Environment.NewLine}{resultStart.OutputText}");

            WaitForServer(testCase, options, connection, 15000, expectSuccess: true);

            if (ArtifactsUtility.Can7z && ArtifactsUtility.Directory != null)
            {
                var fileName = $"Data [{(string.IsNullOrEmpty(testCase.Locale) ? "Default Locale" : testCase.Locale)}] {testCase.ServerBinaries.Version} (running server)";
                var fullFileName = Path.Combine(ArtifactsUtility.Directory, fileName);
                var listProcessesCmd = $"-c \"echo; ps -aux | grep postgres > '{fullFileName}.processes.log' \"";
                ExecProcessHelper.HiddenExec("7z", $"a -ms=on -mqs=on -mx=1 \"{fullFileName}.7z\" \"{options.DataPath}\"");
                var res2 = ExecProcessHelper.HiddenExec("bash", listProcessesCmd);
                Console.WriteLine($"DEBUG COMMAND:{Environment.NewLine}bash {listProcessesCmd}");
                Console.WriteLine($"ps aux output:{Environment.NewLine}{res2.OutputText}");
                
                res2.DemandGenericSuccess("Invoke ps via bash");

            }

            TryAndForget.Execute(() => PostgresServerManager.StopInstance(serverBinaries, options));
            TryAndForget.Execute(() => Directory.Delete(options.DataPath, true));

            WaitForServer(testCase, options, connection, 3000, expectSuccess: false);
        }

        void WaitForServer(PgServerTestCase testCase, PostgresInstanceOptions options, NpgsqlConnectionStringBuilder connection, int timeoutMilleconds, bool expectSuccess)
        {
            Stopwatch waitForStart = NpgsqlWaitForExtensions.WaitForPgsqllDb(connection.ToString(), timeoutMilleconds, out var serverVersion, out var error);

            string prefixSuccess = expectSuccess ? "OK:" : "WARNING:";
            string prefixFail = expectSuccess ? "WARNING:" : "OK:";
            if (error != null)
                Console.WriteLine($"[Wait for '{testCase.ServerBinaries.ServerPath}'] {prefixFail} CONNECTION ERROR: {error.Message}{(expectSuccess ? Environment.NewLine + error : error.Message)}");
            else
            {
                Console.WriteLine($"[Wait for '{testCase.ServerBinaries.ServerPath}'] {prefixSuccess} SUCCESSFUL CONNECTION in {waitForStart.ElapsedMilliseconds:n0} milliseconds{Environment.NewLine}{serverVersion}");
                Console.WriteLine($"[LOCALE '{options.Locale}'] {new NpgsqlConnection(connection.ConnectionString).GetCurrentDatabaseLocale()}");
            }

            if (expectSuccess)
            {
                Assert.IsNull(error, "Successful connection is expected");
                Assert.IsNotNull(serverVersion, "Successful server version is expected");
            }
            else
            {
                Assert.IsNotNull(error, "Missed connection is expected");
                Assert.IsNull(serverVersion, "Missed server version is expected");
            }
        }

        (NpgsqlConnectionStringBuilder, PostgresInstanceOptions) InitDb(PgServerTestCase testCase)
        {
            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + "-" + testCase.ServerBinaries.Version + "-start"),
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
                Locale = testCase.Locale,
            };

            var resultInit = PostgresServerManager.CreateServerInstance(testCase.ServerBinaries, options);
            Console.WriteLine(@$"INIT DB Output:{Environment.NewLine}{resultInit.OutputText}");

            NpgsqlConnectionStringBuilder csBuilder = new NpgsqlConnectionStringBuilder()
            {
                Host = "localhost",
                Port = options.ServerPort,
                Username = options.SystemUser,
                Password = options.SystemPassword,
                Timeout = 1,
                CommandTimeout = 1,
            };

            return (csBuilder, options);
        }

    }
}