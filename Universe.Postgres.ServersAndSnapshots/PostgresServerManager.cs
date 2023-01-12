using System;

namespace Universe.Postgres.ServersAndSnapshots
{
    public static class PostgresServerManager
    {
        public static Version GetVersion(this ServerBinariesRequest serverBinariesRequest)
        {
            throw new NotImplementedException();
        }

        // Fuzzy logic
        public static Version FindPostgresServers(this ServerBinariesRequest serverBinariesRequest)
        {
            // Linux: /usr/lib/postgresql/14/bin/initdb
            // Windows: C:\Program Files\PostgreSQL\{}, C:\Program Files (x86)\PostgreSQL\{}
            // Any: ENV PG_SERVER_BINARY{any}
            throw new NotImplementedException();
        }

        public static void CreateServerInstance(this ServerBinariesRequest serverBinariesRequest, PostgresInstanceOptions instanceOptions)
        {
        }

        public static void StartInstance(this ServerBinariesRequest serverBinariesRequest, PostgresInstanceOptions instanceOptions, bool waitFor = true)
        {
        }

        public static void StopInstance(this ServerBinariesRequest serverBinariesRequest, PostgresInstanceOptions instanceOptions, bool waitFor = true)
        {
        }

        public static void CreateSnapshot(this ServerBinariesRequest serverBinariesRequest, PostgresInstanceOptions instanceOptions, PostgresInstanceSnapshot snapshot)
        {
        }

        public static void RestoreSnapshot(this ServerBinariesRequest serverBinariesRequest, PostgresInstanceOptions instanceOptions, PostgresInstanceSnapshot snapshot)
        {
        }

    }

    public class ServerBinariesRequest
    {
        public string ServerPath { get; set; }
    }

    public class ServerBinaries
    {
        public string ServerPath { get; set; }
        public bool IsValid { get; set; }
        public Version Version { get; set; }

        public override string ToString()
        {
            return $"PostgreSQL Server Version {Version} '{ServerPath}'";
        }
    }

    public class PostgresInstanceOptions
    {
        public string SystemUser { get; set; } = "postgres";
        public string SystemPassword { get; set; } = "p@ssw0rd";
        public string DataPath { get; set; } = "/temp/postgres-server-data";
        public int ServerPort { get; set; } = 5432;
        public bool LocalhostOnly { get; set; } = false;
    }

    public class PostgresInstanceSnapshot
    {
        public string SnapshotPath { get; set; }
    }
}
