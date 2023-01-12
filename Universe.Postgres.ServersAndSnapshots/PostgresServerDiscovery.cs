using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Universe.Postgres.ServersAndSnapshots
{
    public class PostgresServerDiscovery
    {

        public static ServerBinaries[] GetServers()
        {
            List<ServerBinaries> ret = new List<ServerBinaries>();

            List<string> candidates = new List<string>();
            if (IsWindows)
            {
                var pf1Postgres = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "PostgreSQL");
                candidates.AddRange(TryPostgresSubfolders(pf1Postgres));
                var pf2Postgres = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "PostgreSQL");
                candidates.AddRange(TryPostgresSubfolders(pf2Postgres));

                string[] rootList = new[] { "C:\\", TryEval(() => Path.GetPathRoot(Environment.SystemDirectory)) };
                rootList = rootList.Distinct(StringComparer.InvariantCultureIgnoreCase).ToArray();
                var subProgramList = new[] { "Program Files", "Program Files (x86)" };
                foreach (var root in rootList)
                foreach (var subProgram in subProgramList)
                {
                    var pf3Progres = Path.Combine(root, subProgram, "PostgreSQL");
                    candidates.AddRange(TryPostgresSubfolders(pf3Progres));
                }
            }
            else
            {
                candidates.AddRange(TryPostgresSubfolders("/usr/lib/postgresql"));
                candidates.AddRange(TryPostgresSubfolders("/usr/local/lib/postgresql"));
            }

            candidates.AddRange(TryEnvVars());
            candidates = candidates.Distinct().ToList();

            foreach (var candidate in candidates)
            {
                if (DoesContainPostgresBinaries(candidate))
                {
                    var version = GetPostgresVersion(candidate);
                    if (version != null)
                        ret.Add(new ServerBinaries()
                        {
                            ServerPath = Path.GetFullPath(candidate).TrimEnd(new[] { '/', '\\' }),
                            IsValid = true,
                            Version = version,
                        });
                }
            }

            return ret.ToArray();
        }

        private static Version GetPostgresVersion(string candidate)
        {
            string ext = IsWindows ? ".exe" : "";
            var pgCtlBin = Path.Combine(candidate, $"bin{Path.DirectorySeparatorChar}pg_ctl{ext}");

            var result = ExecProcessHelper.HiddenExec(pgCtlBin, "--version");
            return TryEval<Version>(() =>
            {
                result.DemandGenericSuccess("Query pg_ctl version");
                var versionRaw = result.OutputText;
                return TryParsePostgresVersion(versionRaw);
            });
        }

        static Version TryParsePostgresVersion(string raw)
        {
            var words = raw.Split(' ');
            foreach (var word in words)
            {
                StringBuilder verCandidate = new StringBuilder();
                foreach (var ch in word)
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

        static IEnumerable<string> TryPostgresSubfolders(string dir)
        {
            if (!Directory.Exists(dir)) yield break;

            var subDirList = TryEval(() => new DirectoryInfo(dir).GetDirectories()) ?? Array.Empty<DirectoryInfo>();
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
            string ext = IsWindows ? ".exe" : "";
            return
                File.Exists(Path.Combine(subDir, $"bin{Path.DirectorySeparatorChar}initdb{ext}"))
                && File.Exists(Path.Combine(subDir, $"bin{Path.DirectorySeparatorChar}pg_ctl{ext}"));
        }

        private static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        static T TryEval<T>(Func<T> factory)
        {
            try
            {
                return factory();
            }
            catch
            {
                return default(T);
            }
        }

    }
}