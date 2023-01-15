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
            var resultKill = PostgresServerManager.StopInstanceSmarty(serverBinaries, options);
            Console.WriteLine(@$"KILL SERVER Output (took {killAt.ElapsedMilliseconds:n0} milliseconds):{Environment.NewLine}{resultKill.OutputText}");

            WaitForServer(testCase, options, connection, 3000, expectSuccess: false);
        }


        enum StopMode
        {
            Kill,
            Stop,
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestStartStopServer(PgServerTestCase testCase)
        {
            TestStartStopServer_Implementation(testCase, StopMode.Stop);
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestStartKillServer(PgServerTestCase testCase)
        {
            TestStartStopServer_Implementation(testCase, StopMode.Kill);
        }

        private void TestStartStopServer_Implementation(PgServerTestCase testCase, StopMode stopMode)
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

                string cmd = null, args = null;
                ExecProcessHelper.ExecResult res2;
                if (TinyCrossInfo.IsWindows)
                {
                    args =
                        $"-c \"(Get-WmiObject Win32_Process -Filter \"\"\"name like '%postgres%'\"\"\") | ft Handle,Name,CommandLine > \"\"\"{options.DataPath}{Path.DirectorySeparatorChar}Processes.log\"\"\"\"";
                    cmd = "powershell";
                }
                else
                {
                    // linux ok, mac os ok
                    args = $"-c \"echo; ps aux | grep postgres > '{options.DataPath}{Path.DirectorySeparatorChar}Processes.log'\"";
                    cmd = "bash";
                }

                res2 = ExecProcessHelper.HiddenExec(cmd, args);
                ExecProcessHelper.HiddenExec("7z", $"a -ms=on -mqs=on -mx=1 \"{fullFileName}.7z\" \"{options.DataPath}\"");
                Console.WriteLine($"DEBUG COMMAND:{Environment.NewLine}{cmd} {args}");
                Console.WriteLine($"Invoke processlist output:{Environment.NewLine}{res2.OutputText}");
                res2.DemandGenericSuccess($"Invoke processlist via {cmd}");
            }

            Stopwatch sw = Stopwatch.StartNew();
            string mode = stopMode.ToString();
            if (stopMode == StopMode.Stop)
            {
                TryAndForget.Execute(() => PostgresServerManager.StopInstance(serverBinaries, options));
            }
            else
            {
                TryAndForget.Execute(() =>
                {
                    bool isKill = PostgresServerManager.KillInstance(serverBinaries, options);
                    if (!isKill) mode = "Kill (nope, stop)";
                });
            }

            var msec = sw.ElapsedTicks * 1000d / Stopwatch.Frequency;
            Console.WriteLine($"{mode} server took {msec:n2} milliseconds");
            
            WaitForServer(testCase, options, connection, 3000, expectSuccess: false);
            TryAndForget.Execute(() => Directory.Delete(options.DataPath, true));
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