using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Universe.Postgres.ServersAndSnapshots
{
    public class PostgresServerDiscovery
    {

        public static ServerBinaries[] GetServers()
        {
            List<ServerBinaries> ret = new List<ServerBinaries>();

            List<string> candidates = new List<string>();
            // Console.WriteLine($"TinyCrossInfo.IsWindows: {TinyCrossInfo.IsWindows}");
            if (TinyCrossInfo.IsWindows)
            {
                List<string> programFilesCandidates = new List<string>();
                
#if !NETCOREAPP1_1
                programFilesCandidates.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles));
                programFilesCandidates.Add(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86));
#endif

                
                foreach (var varName in new[] { "ProgramFiles(x86)", "ProgramFiles", "ProgramW6432" })
                {
                    var varValue = Environment.GetEnvironmentVariable(varName);
                    if (!string.IsNullOrEmpty(varValue))
                    {
                        programFilesCandidates.Add(varValue);
                    }
                }

                List<string> rootList = new List<string>() { "C:\\" };
#if !NETCOREAPP1_1
                TryAndForget.Execute(() => rootList.Add(Path.GetPathRoot(Environment.SystemDirectory))); 
#endif
                var systemDriveVar = Environment.GetEnvironmentVariable("SystemDrive");
                if (!string.IsNullOrEmpty(systemDriveVar)) rootList.Add(systemDriveVar);
                rootList = rootList.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                var subProgramList = new[] { "Program Files", "Program Files (x86)" };
                foreach (var root in rootList)
                foreach (var subProgram in subProgramList)
                {
                    var pf3 = Path.Combine(root, subProgram);
                    programFilesCandidates.Add(pf3);
                }

                programFilesCandidates = programFilesCandidates.Distinct(StringComparer.CurrentCultureIgnoreCase).ToList();
                foreach (var programFilesCandidate in programFilesCandidates)
                {
                    candidates.AddRange(TryPostgresSubfolders(Path.Combine(programFilesCandidate, "PostgreSQL")));
                }
            }
            else
            {
                // linux or macos
                candidates.AddRange(TryPostgresSubfolders("/usr/lib/postgresql"));
                candidates.AddRange(TryPostgresSubfolders("/usr/local/lib/postgresql"));

                // 
                candidates.Add("/usr");
                candidates.Add("/usr/local");

                // MacPorts
                candidates.AddRange(TryPostgresSubfolders("/opt/local/lib", subDirPattern: "postgresql*"));

                foreach (var macOsFolder in WeakMacOsSearchForHomeBrew())
                {
                    // Console.WriteLine($"macOsFolder: '{macOsFolder}'");
                    candidates.AddRange(TryPostgresSubfolders(macOsFolder));
                }
            }

            candidates.AddRange(TryEnvVars());
            candidates = candidates.Distinct().ToList();
            Console.WriteLine($"[DEBUG] PostgreSQL Candidates: [{string.Join(", ", candidates)}]");

            foreach (string candidate in candidates)
            {
                var doesContainPostgresBinaries = DoesContainPostgresBinaries(candidate);
                Console.WriteLine($"[DEBUG]   \"{candidate}\": contains binaries? {doesContainPostgresBinaries}");
                if (doesContainPostgresBinaries)
                {
                    Version version = GetPostgresVersion(candidate);
                    Console.WriteLine($"[DEBUG]   \"{candidate}\": version? {version}");
                    if (version != null)
                        ret.Add(new ServerBinaries()
                        {
                            InitDbFullPath = GetFullPathByCandidate(candidate, "initdb"),
                            PgCtlFullPath = GetFullPathByCandidate(candidate, "pg_ctl"),
                            Version = version,
                        });
                }
            }

            return ret
                .OrderByDescending(x => x.Version)
                .ThenBy(x => x.PgCtlFullPath, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        static string GetFullPathByCandidate(string candidate, string binFileName)
        {
            string ext = TinyCrossInfo.IsWindows ? ".exe" : "";
            return Path.Combine(candidate, $"bin{Path.DirectorySeparatorChar}{binFileName}{ext}");
        }

        /*
/usr/local/Cellar/postgresql@14/14.5_5/bin/initdb
/usr/local/Cellar/postgresql@12/12.12_3/bin/initdb
         */
        static IEnumerable<string> WeakMacOsSearchForHomeBrew()
        {
            DirectoryInfo[] dirsHomeBrew = TryAndForget.Evaluate(() => new DirectoryInfo("/usr/local/Cellar").GetDirectories("postgresql@*"));
            // Console.WriteLine($"/usr/local/Cellar/postgresql@* count: {dirs?.Length}");
            foreach (var dir in dirsHomeBrew ?? new DirectoryInfo[0])
            {
                yield return dir.FullName;
            }
        }

        private static Version GetPostgresVersion(string candidate)
        {
            string ext = TinyCrossInfo.IsWindows ? ".exe" : "";
            var pgCtlBin = Path.Combine(candidate, $"bin{Path.DirectorySeparatorChar}pg_ctl{ext}");

            var result = ExecProcessHelper.HiddenExec(pgCtlBin, "--version", millisecondsTimeout: 12000);
            Console.WriteLine($"[DEBUG]   \"{candidate}\": pg_ctl --version output: {result.OutputText}");
            Console.WriteLine($"[DEBUG]   \"{candidate}\": pg_ctl --version exit code: {result.ExitCode}");
            return TryEval<Version>(() =>
            {
                try
                {
                    result.DemandGenericSuccess("Query pg_ctl version");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[DEBUG]   \"{candidate}\": pg_ctl --version error:{Environment.NewLine}{ex}");
                }

                var versionRaw = result.OutputText;
                Console.WriteLine($"[DEBUG]   \"{candidate}\": versionRaw :{versionRaw}");
                return TryParsePostgresVersion(versionRaw);
            });
        }

        static Version TryParsePostgresVersion(string raw)
        {
            var words = raw.Split(' ');
            foreach (var word in words)
            {
                var wordPatched = word;
                string[] betaSuffixes = new[] { "devel", "beta" };
                foreach (var betaSuffix in betaSuffixes)
                {
                    var p = wordPatched.IndexOf(betaSuffix, StringComparison.OrdinalIgnoreCase);
                    if (p > 0)
                    {
                        wordPatched = wordPatched.Substring(0, 1) + ".99999";
                    }
                }
                /*
                // 16devel?
                if (word.EndsWith("devel", StringComparison.OrdinalIgnoreCase))
                    // wordPatched = word.Length > 5 ? word.Substring(0, word.Length - 5) : word;
                    wordPatched = word.Replace("devel", ".99999");
                    */

                StringBuilder verCandidate = new StringBuilder();
                foreach (var ch in wordPatched)
                {
                    if (!(Char.IsDigit(ch) || ch == '.')) break;
                    verCandidate.Append(ch);
                }

                if (verCandidate.Length > 0)
                    if (Version.TryParse(verCandidate.ToString(), out var ret))
                        if (ret.Major > 0)
                            return ret;
            }

            return null;
        }

        static IEnumerable<string> TryPostgresSubfolders(string dir, string subDirPattern = "*")
        {
            if (!Directory.Exists(dir)) yield break;

            var subDirList = TryEval(() => new DirectoryInfo(dir).GetDirectories()) ?? new DirectoryInfo[0];
            foreach (var subDir in subDirList)
            {
                if (DoesContainPostgresBinaries(subDir.FullName))
                {
                    yield return subDir.FullName;
                }
            }
        }

        static IEnumerable<string> TryEnvVars()
        {
            var envVars = Environment.GetEnvironmentVariables().Keys.OfType<object>().Select(x => x.ToString());
            foreach (var envVar in envVars)
            {
                if (envVar.StartsWith("PG_SERVER_BINARY"))
                {
                    var pathCandidate = Environment.GetEnvironmentVariable(envVar);
                    if (pathCandidate != null)
                        if (DoesContainPostgresBinaries(pathCandidate))
                            yield return pathCandidate;
                }
            }
        }

        static bool DoesContainPostgresBinaries(string dir)
        {
            var subDir = dir;
            string ext = TinyCrossInfo.IsWindows ? ".exe" : "";
            return
                File.Exists(Path.Combine(subDir, $"bin{Path.DirectorySeparatorChar}initdb{ext}"))
                && File.Exists(Path.Combine(subDir, $"bin{Path.DirectorySeparatorChar}pg_ctl{ext}"));
        }

        static T TryEval<T>(Func<T> factory)
        {
            return TryAndForget.Evaluate(factory);
        }

    }
}