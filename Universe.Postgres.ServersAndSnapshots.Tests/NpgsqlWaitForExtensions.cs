using System;
using System.Diagnostics;
using System.Threading;
using Dapper;
using Npgsql;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class NpgsqlWaitForExtensions
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

    }
}