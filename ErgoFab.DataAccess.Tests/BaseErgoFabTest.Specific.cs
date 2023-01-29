using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Universe.Postgres.ServersAndSnapshots;
using Universe.Postgres.ServersAndSnapshots.Tests;

namespace ErgoFab.DataAccess.Tests
{
    public partial class BaseErgoFabTest
    {
        protected virtual ServerBinaries TryServerDefinitionByArgument(object argument)
        {
            if (argument is PgServerTestCase testCase)
                return testCase.ServerBinaries;

            return null;
        }

        protected virtual ServerBinaries GetPreferredServer()
        {
            // By Default get latest pre-installed postgres server
            return PostgresServerDiscovery.GetServers().MaxBy(x => x.Version);
        }




    }
}
