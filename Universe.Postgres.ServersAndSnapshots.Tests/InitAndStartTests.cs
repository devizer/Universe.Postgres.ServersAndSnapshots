using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using Dapper;
using Npgsql;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class InitAndStartTests : NUnitTestsBase
    {
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
            Console.WriteLine(@$"INIT DB Output:
{resultInit.OutputText}");

            var resultStart = PostgresServerManager.StartInstance(serverBinaries, options);
            Console.WriteLine(@$"START SERVER Output:
{resultStart.OutputText}");

            NpgsqlConnectionStringBuilder csBuilder = new NpgsqlConnectionStringBuilder();
            csBuilder.Host = "localhost";
            csBuilder.Port = options.ServerPort;
            csBuilder.Username = options.SystemUser;
            csBuilder.Password = options.SystemPassword;
            csBuilder.Timeout = 1;
            csBuilder.CommandTimeout = 1;

            
            Stopwatch waitForStart = WaitForPgsqllDb(csBuilder.ToString(), 15000, out var serverVersion, out var conError);

            if (conError != null)
                Console.WriteLine($"CONNECTION ERROR: {conError.Message}{Environment.NewLine}{conError}");
            else
            {
                Console.WriteLine($"SUCCESSFUL CONNECTION in {waitForStart.ElapsedMilliseconds:n0} milliseconds{Environment.NewLine}{serverVersion}");
                Console.WriteLine($"[LOCALE] {new NpgsqlConnection(csBuilder.ConnectionString).GetCurrentDatabaseLocale()}");
            }


            TryAndForget.Execute(() => PostgresServerManager.StopInstance(serverBinaries, options));
            TryAndForget.Execute(() => Directory.Delete(options.DataPath));
            
            // STOP
            Stopwatch waitForStopDb = WaitForPgsqllDb(csBuilder.ToString(), 2000, out var serverVersionOnStop, out var conErrorOnStop);
            if (conErrorOnStop != null)
                Console.WriteLine($"[ON STOP] connection error as expected: {conErrorOnStop.Message}");
            else
                Console.WriteLine($"[ON STOP] Warning! Unexpected successful connection in {waitForStopDb.ElapsedMilliseconds:n0} milliseconds{Environment.NewLine}{serverVersionOnStop}");


            Assert.IsNull(conError);
            Assert.IsNotNull(serverVersion);

            Assert.IsNotNull(conErrorOnStop);
            Assert.IsNull(serverVersionOnStop);
        }

        private static Stopwatch WaitForPgsqllDb(string connectionString, int connectivityTimeout, out string serverVersion, out Exception conError)
        {
            Stopwatch waitForDb = Stopwatch.StartNew();
            serverVersion = null;
            conError = null;
            do
            {
                using NpgsqlConnection con = new NpgsqlConnection(connectionString);
                try
                {
                    serverVersion = con.QueryFirst<string>("Select Version();");
                    conError = null;
                    break;
                }
                catch (Exception ex)
                {
                    conError = ex;
                }

                Thread.Sleep(2);
            } while (waitForDb.ElapsedMilliseconds <= connectivityTimeout);

            return waitForDb;
        }
    }
}