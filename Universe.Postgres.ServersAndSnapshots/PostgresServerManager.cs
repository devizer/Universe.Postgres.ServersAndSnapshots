using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Universe.Postgres.ServersAndSnapshots
{
    public static class PostgresServerManager
    {
        // Fuzzy logic
        public static ServerBinaries[] FindPostgresServers(this ServerBinariesRequest serverBinariesRequest)
        {
            // Linux: /usr/lib/postgresql/14/bin/initdb
            // Windows: C:\Program Files\PostgreSQL\{}, C:\Program Files (x86)\PostgreSQL\{}
            // Any: ENV PG_SERVER_BINARY{any}
            return PostgresServerDiscovery.GetServers();
        }

        private static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        public static ExecProcessHelper.ExecResult CreateServerInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions)
        {
            var passwordFileName = Guid.NewGuid().ToString("N");
            using var passwordFile = DisposableTempFile.Create(passwordFileName, instanceOptions.SystemPassword);
            var ext = IsWindows ? ".exe" : "";
            var exe = Path.Combine(serverBinaries.ServerPath, $"bin{Path.DirectorySeparatorChar}initdb{ext}");
            var args = $"-D \"{instanceOptions.DataPath}\" --pwfile \"{passwordFileName}\" -U \"{instanceOptions.SystemUser}\"";

            var ret = ExecProcessHelper.HiddenExec(exe, args);
            ret.DemandGenericSuccess($"InitDb invocation of '{exe}'");

            var socketDir = $"{instanceOptions.DataPath}{Path.DirectorySeparatorChar}socket-dir";
            TryAndForget.Execute(() => Directory.CreateDirectory(socketDir));
            var confFileName = Path.Combine(instanceOptions.DataPath, "postgresql.conf");
            using(FileStream fs = new FileStream(confFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter wr = new StreamWriter(fs, Encoding.UTF8))
            {
                wr.WriteLine(@$"{Environment.NewLine}
port = {instanceOptions.ServerPort:f0}
unix_socket_directories = '{socketDir}'
");
            }

            return ret;
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

        public static implicit operator ServerBinariesRequest(ServerBinaries arg) => new ServerBinariesRequest() { ServerPath = arg.ServerPath };
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
