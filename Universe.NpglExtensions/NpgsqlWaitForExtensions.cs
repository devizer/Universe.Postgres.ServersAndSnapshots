using System;
using System.Data;
using System.Diagnostics;
using System.Threading;
using Dapper;
using Npgsql;

namespace Universe.NpglExtensions
{
    public static class NpgsqlWaitForExtensions
    {
        public static Stopwatch WaitForPgsqllDb(string connectionString, int connectivityTimeout, out string serverVersion, out Exception conError)
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

        public static ServerConnectivityResult CheckConnectivity(this IDbConnection postgresConnection, int timeout = 0)
        {
            ServerConnectivityResult ret = new ServerConnectivityResult();
            Stopwatch waitForDb = Stopwatch.StartNew();
            do
            {
                try
                {
                    var version = postgresConnection.QueryFirst<string>("Select Version();");
                    ret.Ping = waitForDb.ElapsedTicks * 1000d / Stopwatch.Frequency;
                    ret.Error = null;
                    ret.Version = version;
                    break;
                }
                catch (Exception ex)
                {
                    ret.Error = ex;
                }

                Thread.Sleep(0);
            } while (timeout < 0 || waitForDb.ElapsedMilliseconds <= timeout);

            return ret;
        }

        public class ServerConnectivityResult
        {
            public string Version { get; set; }
            public double? Ping { get; set; }
            public Exception Error { get; set; }

            public override string ToString()
            {
                if (Error != null)
                    return Error.GetType().Name + ": " + Error.Message;
                else
                    return $"Server Version: {Version}, ping: {Ping:n2} milliseconds";
            }
        }

    }
}