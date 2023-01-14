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


        public static ExecProcessHelper.ExecResult CreateServerInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions)
        {
            var passwordFileName = Guid.NewGuid().ToString("N");
            using var passwordFile = DisposableTempFile.Create(passwordFileName, instanceOptions.SystemPassword);
            var ext = TinyCrossInfo.IsWindows ? ".exe" : "";
            var exe = Path.Combine(serverBinaries.ServerPath, $"bin{Path.DirectorySeparatorChar}initdb{ext}");
            var localeParam = string.IsNullOrEmpty(instanceOptions.Locale) ? "" : $"--locale={instanceOptions.Locale} ";
            var args = $"{localeParam}-D \"{instanceOptions.DataPath}\" --pwfile \"{passwordFileName}\" -U \"{instanceOptions.SystemUser}\"";

            var ret = ExecProcessHelper.HiddenExec(exe, args);
            ret.DemandGenericSuccess($"InitDb invocation of '{exe}'");

            var socketDir = $"{instanceOptions.DataPath}{Path.DirectorySeparatorChar}socket-dir";
            TryAndForget.Execute(() => Directory.CreateDirectory(socketDir));
            var confFileName = Path.Combine(instanceOptions.DataPath, "postgresql.conf");
            using(FileStream fs = new FileStream(confFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
            using (StreamWriter wr = new StreamWriter(fs, Encoding.UTF8))
            {
                wr.WriteLine(@$"{Environment.NewLine}port = {instanceOptions.ServerPort:f0}");
                if (!TinyCrossInfo.IsWindows)
                    wr.WriteLine(@$"{Environment.NewLine}unix_socket_directories = '{socketDir}'");
            }

            return ret;
        }

        public static ExecProcessHelper.ExecResult StartInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions, bool waitFor = true)
        {
            return InvokePgCtl(serverBinaries, instanceOptions, "start", waitFor);
        }

        public static ExecProcessHelper.ExecResult StopInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions, bool waitFor = true, StopMode mode = StopMode.Smart)
        {
            var options = $"--mode={mode.ToString().ToLower()}";
            return InvokePgCtl(serverBinaries, instanceOptions, "stop", waitFor, options);
        }

        public enum StopMode
        {
            Smart,
            Fast,
            Immediate,
        }

        public static ExecProcessHelper.ExecResult KillInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions)
        {
            // "kill SIGKILL {PID}"
            // return InvokePgCtl(serverBinaries, instanceOptions, "stop", waitFor: false);
            return StopInstance(serverBinaries, instanceOptions, false, StopMode.Immediate);
        }

        private static ExecProcessHelper.ExecResult InvokePgCtl(ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions, string command, bool waitFor, string options = null)
        {
            var ext = TinyCrossInfo.IsWindows ? ".exe" : "";
            var exe = Path.Combine(serverBinaries.ServerPath, $"bin{Path.DirectorySeparatorChar}pg_ctl{ext}");
            var logFile = Path.Combine(instanceOptions.DataPath, "server.log");
            // time sudo -u "$user" "$pgbin/pg_ctl" -w -D "$data" -l "$data/server.log" start
            var waitForArgs = waitFor ? "-w " : "";
            var optionsArg = string.IsNullOrEmpty(options) ? "" : (options.EndsWith(" ") ? options : options + " ");
            var args = $"-D \"{instanceOptions.DataPath}\" {waitForArgs}{optionsArg}-l \"{logFile}\" {command}";


            var ret = ExecProcessHelper.HiddenExec(exe, args, 15000);
            ret.DemandGenericSuccess($"Command '{command}' for '{exe}' using data at '{instanceOptions.DataPath}'");
            return ret;
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
        public string Locale { get; set; }
    }

    public class PostgresInstanceSnapshot
    {
        public string SnapshotPath { get; set; }
    }
}
