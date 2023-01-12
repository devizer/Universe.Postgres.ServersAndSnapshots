using System.Linq;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class PgServerTestCase
    {
        public ServerBinaries ServerBinaries { get; set; }
        public string Locale { get; set; }


        public static ServerBinaries[] GetServers()
        {
            ServerBinaries[] ret = PostgresServerDiscovery.GetServers();
            return ret.Concat(ret).ToArray();
        }

        public override string ToString()
        {
            return ServerBinaries.ToString();
        }
    }
}