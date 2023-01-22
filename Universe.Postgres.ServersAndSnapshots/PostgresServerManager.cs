using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Universe.Postgres.ServersAndSnapshots
{
    public static class PostgresServerManager
    {
        // Fuzzy logic
        public static ServerBinaries[] FindPostgresServers()
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
            var exe = serverBinaries.InitDbFullPath;
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

                if (!string.IsNullOrEmpty(instanceOptions.StatementLogFolder))
                {
                    wr.WriteLine(@$"
log_statement = 'all'
log_filename = 'statements'
log_duration = on
log_destination = 'csvlog'
logging_collector = on
log_directory = '{instanceOptions.StatementLogFolder}'
log_file_mode = 0666
# disable log rotation
log_rotation_age = 0
log_rotation_size = 0
");

                }
            }

            return ret;
        }

        public static ExecProcessHelper.ExecResult StartInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions, bool waitFor = true)
        {
            return InvokePgCtl(serverBinaries, instanceOptions, "start", waitFor);
        }

        public static ExecProcessHelper.ExecResult StopInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions, bool waitFor = true, StopMode mode = StopMode.Fast)
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

        public static ExecProcessHelper.ExecResult StopInstanceSmarty(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions)
        {
            if (TinyCrossInfo.IsWindows)
                return StopInstance(serverBinaries, instanceOptions, true, StopMode.Fast);
            else
                return StopInstance(serverBinaries, instanceOptions, false, StopMode.Immediate);

            // "kill SIGKILL {PID}"?
            // return InvokePgCtl(serverBinaries, instanceOptions, "stop", waitFor: false);
        }

        // True if process killed, otherwise stop command used
        public static bool KillInstance(this ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions)
        {
            var pidList = new List<int>();
            uint? pid = null;
            var pidFileName = Path.Combine(instanceOptions.DataPath, "postmaster.pid");
            if (File.Exists(pidFileName))
            {
                using (FileStream fs = new FileStream(pidFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (StreamReader rdr = new StreamReader(fs, new UTF8Encoding(false)))
                {
                    var firstLine = rdr.ReadLine();
                    if (firstLine != null && UInt32.TryParse(firstLine, out var realPid))
                        pid = realPid;
                }
            }

            if (pid == null)
            {
                StopInstanceSmarty(serverBinaries, instanceOptions);
                return false;
            }
            else
            {
                pidList.Add((int)pid.Value);
                // On windows logs are empty
                if (TinyCrossInfo.IsWindows)
                {
                    var processes = WindowsProcessInterop.GetAllProcesses();
                    var children = processes.AsChildrenDictionary().GetDeepChildren(pid.Value).Reverse().ToList();
                    if (EnableKillLog)
                        Console.WriteLine($"Postgres {pid} children: [{string.Join(",", children)}]");

                    foreach (var idChild in children) pidList.Add((int)idChild);
                }
            }

            Stopwatch startKillAt;
            StringBuilder killLog;
            if (EnableKillLog)
            {
                startKillAt = Stopwatch.StartNew();
                killLog = new StringBuilder();
            }

            Parallel.ForEach(pidList, idProcess =>
            {
                var status = "success";
                try
                {
                    var process = Process.GetProcessById(idProcess);
                    process.Kill();
                }
                catch (Exception ex)
                {
                    if (EnableKillLog) status = $"[{ex.GetType()}]: {ex.Message}";
                }

                if (EnableKillLog)
                {
                    var msec = startKillAt.ElapsedMilliseconds;
                    lock (killLog)
                    {
                        killLog.AppendLine($"[KillInstance] {msec,12:n0} Kill postgres process {idProcess}: {status}");
                    }
                }
            });
            if (EnableKillLog)
            {
                killLog.AppendLine($"[KillInstance] finished in {startKillAt.ElapsedMilliseconds:n0} milliseconds. Root PID is {pid}");
                Console.WriteLine(killLog);
            }

            return true;
        }

#if DEBUG || true
        private const bool EnableKillLog = true;
#else 
        private const bool EnableKillLog = false;
#endif


        private static ExecProcessHelper.ExecResult InvokePgCtl(ServerBinariesRequest serverBinaries, PostgresInstanceOptions instanceOptions, string command, bool waitFor, string options = null)
        {
            var exe = serverBinaries.PgCtlFullPath;
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
        public string PgCtlFullPath { get; set; }
        public string InitDbFullPath { get; set; }
    }

    public class ServerBinaries
    {
        public string PgCtlFullPath { get; set; }
        public string InitDbFullPath { get; set; }
        public Version Version { get; set; }

        public static implicit operator ServerBinariesRequest(ServerBinaries arg) => new ServerBinariesRequest() { InitDbFullPath = arg.InitDbFullPath, PgCtlFullPath = arg.PgCtlFullPath};

        public override string ToString()
        {
            var path = Path.GetDirectoryName(InitDbFullPath);
            return $"PostgreSQL Server Version {Version} '{path}'";
        }

    }

    public class PostgresInstanceOptions
    {
        public string SystemUser { get; set; } = "postgres";
        public string SystemPassword { get; set; } = "p@ssw0rd";
        public string DataPath { get; set; } = "/tmp/postgres-server-data";
        public int ServerPort { get; set; } = 5432;
        public bool LocalhostOnly { get; set; } = true;
        public string Locale { get; set; }

        public string StatementLogFolder { get; set; } = null;
    }

    public class PostgresInstanceSnapshot
    {
        public string SnapshotPath { get; set; }
    }
}
