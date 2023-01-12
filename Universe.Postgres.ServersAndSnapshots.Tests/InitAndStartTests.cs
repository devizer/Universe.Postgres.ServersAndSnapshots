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
        public void TestStartDb(ServerBinaries serverBinaries)
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

            NpgsqlConnectionStringBuilder csBuilder = new NpgsqlConnectionStringBuilder();
            csBuilder.Host = "localhost";
            csBuilder.Port = options.ServerPort;
            csBuilder.Username = options.SystemUser;
            csBuilder.Password = options.SystemPassword;
            csBuilder.Timeout = 1;
            csBuilder.CommandTimeout = 1;

            Stopwatch waitForDb = Stopwatch.StartNew();
            Exception conError = null;
            string serverVersion = null;
            do
            {
                using NpgsqlConnection con = new NpgsqlConnection(csBuilder.ConnectionString);
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
            } while (waitForDb.ElapsedMilliseconds < 15000);

            if (conError != null)
                Console.WriteLine($"CONNECTION ERROR: {conError.Message}{Environment.NewLine}{conError}");
            else
                Console.WriteLine($"SUCCESSFUL CONNECTION in {waitForDb.ElapsedMilliseconds:n0} milliseconds");


            TryAndForget.Execute(() => PostgresServerManager.StopInstance(serverBinaries, options));
            TryAndForget.Execute(() => Directory.Delete(options.DataPath));
        }
    }
}