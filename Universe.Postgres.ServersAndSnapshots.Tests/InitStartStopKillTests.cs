using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            var (connection, options) = InitDb(testCase, "Init");
            OnDispose(() => Directory.Delete(options.DataPath, true));
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestStopInstanceSmarty(PgServerTestCase testCase)
        {
            var serverBinaries = testCase.ServerBinaries;
            var (connection, options) = InitDb(testCase, "Kill");
            OnDisposeSilent(() => Directory.Delete(options.DataPath, true));

            Stopwatch startAt = Stopwatch.StartNew();
            var resultStart = PostgresServerManager.StartInstance(serverBinaries, options, waitFor: true);
            Console.WriteLine(@$"START SERVER Output (took {startAt.ElapsedMilliseconds:n0} milliseconds):{Environment.NewLine}{resultStart.OutputText}");

            AssertConnectivity(testCase, options, connection, 15000, expectSuccess: true);

            Stopwatch killAt = Stopwatch.StartNew();
            var resultKill = PostgresServerManager.StopInstanceSmarty(serverBinaries, options);
            Console.WriteLine(@$"KILL SERVER Output (took {killAt.ElapsedMilliseconds:n0} milliseconds):{Environment.NewLine}{resultKill.OutputText}");

            AssertConnectivity(testCase, options, connection, 3000, expectSuccess: false);
        }

        enum StopMode
        {
            Kill,
            Stop,
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        public void TestStartStopServer(PgServerTestCase testCase)
        {
            TestStartStopServer_Implementation(testCase, StopMode.Stop, pooling: true);
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        [RequiredWindows]
        public void TestStartKillServerWithPooling(PgServerTestCase testCase)
        {
            TestStartStopServer_Implementation(testCase, StopMode.Kill, pooling: true);
        }

        [Test, TestCaseSource(typeof(PgServerTestCase), nameof(PgServerTestCase.GetServers))]
        [RequiredWindows]
        public void TestStartKillServerWithoutPooling(PgServerTestCase testCase)
        {
            TestStartStopServer_Implementation(testCase, StopMode.Kill, pooling: false);
        }

        private void TestStartStopServer_Implementation(PgServerTestCase testCase, StopMode stopMode, bool pooling)
        {
            var (connection, options) = InitDb(testCase, $"Start-and-{stopMode}", pooling);
            var serverBinaries = testCase.ServerBinaries;

            Stopwatch startAt = Stopwatch.StartNew();
            var resultStart = PostgresServerManager.StartInstance(serverBinaries, options, waitFor: true);
            Console.WriteLine(@$"START SERVER Output (took {startAt.ElapsedMilliseconds:n0} milliseconds):{Environment.NewLine}{resultStart.OutputText}");

            AssertConnectivity(testCase, options, connection, 15000, expectSuccess: true);

            if (ArtifactsUtility.Directory != null && ArtifactsUtility.Can7z)
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
                // Console.WriteLine($"DEBUG COMMAND:{Environment.NewLine}{cmd} {args}");
                // Console.WriteLine($"Invoke processlist output:{Environment.NewLine}{res2.OutputText}");
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

            // TryAndForget.Execute(() => Directory.Delete(options.DataPath, true));
            OnDisposeSilent($"'Delete DataPath={options.DataPath}'", () => Directory.Delete(options.DataPath, true));
            var msec = sw.ElapsedTicks * 1000d / Stopwatch.Frequency;
            Console.WriteLine($"[{mode}] server took {msec:n2} milliseconds");
            
            AssertConnectivity(testCase, options, connection, 3000, expectSuccess: false);
        }

        void AssertConnectivity(PgServerTestCase testCase, PostgresInstanceOptions options, NpgsqlConnectionStringBuilder connection, int timeoutMilliseconds, bool expectSuccess)
        {
            Stopwatch waitForStart = NpgsqlWaitForExtensions.WaitForPgsqllDb(connection.ToString(), timeoutMilliseconds, out var serverVersion, out var error);

            string prefixSuccess = expectSuccess ? "OK:" : "WARNING:";
            string prefixFail = expectSuccess ? "WARNING:" : "OK:";
            if (error != null)
                Console.WriteLine($"[Wait for '{testCase.ServerBinaries.PgCtlFullPath}'] {prefixFail} CONNECTION ERROR {(expectSuccess ? "AS EXPECTED" : "")}: {error.Message}{(expectSuccess ? Environment.NewLine + error : "")}");
            else
            {
                Console.WriteLine($"[Wait for '{testCase.ServerBinaries.PgCtlFullPath}'] {prefixSuccess} SUCCESSFUL CONNECTION in {waitForStart.ElapsedMilliseconds:n0} milliseconds{Environment.NewLine}{serverVersion}");
                // Sometimes fail if pooling=on
                var locale = TryAndForget.Evaluate(() => new NpgsqlConnection(connection.ConnectionString).GetCurrentDatabaseLocale());
                Console.WriteLine($"[LOCALE '{options.Locale}'] {locale}");
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

        (NpgsqlConnectionStringBuilder, PostgresInstanceOptions) InitDb(PgServerTestCase testCase, string suffix, bool pooling = true)
        {
            // TOO LONG
            // var dataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + "-v" + testCase.ServerBinaries.Version + $"-{suffix}-Pooling-{(pooling ? "On" : "Off")}");
            // OK
            // var dataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff") + "-v" + testCase.ServerBinaries.Version);
            var dataPath = FindDataPathPrefix() + "-v" + testCase.ServerBinaries.Version + $"-{suffix}-Pooling-{(pooling ? "On" : "Off")}";

            // dataPath = dataPath.ToLower();

            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = dataPath,
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
                Locale = testCase.Locale,
                StatementLogFolder = "CSV Logs",
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
                ApplicationName = $"Tests on port {options.ServerPort}",
                Pooling = pooling, // False is needed by killing server
            };

            return (csBuilder, options);
        }

        string FindDataPathPrefix()
        {
            if (!Directory.Exists(TestUtils.RootWorkFolder))
                return Path.Combine(TestUtils.RootWorkFolder, "00001");

            var subDirs = new DirectoryInfo(TestUtils.RootWorkFolder)
                .GetDirectories()
                .Select(x => x.Name)
                .ToArray();

            for (int i = 1; i < 99999; i++)
            {
                string prefix = i.ToString("00000");
                bool exists = subDirs.Any(x => x.StartsWith(prefix));
                if (!exists)
                    return Path.Combine(TestUtils.RootWorkFolder, prefix);
            }

            throw new InvalidOperationException($"Can't create the new subfolder {Path.Combine(TestUtils.RootWorkFolder, "NNNNN")}");
        }

    }
}