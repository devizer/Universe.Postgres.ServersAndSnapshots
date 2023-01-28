using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    internal class TestUtils
    {
        public static int Port = 5433;

        public static string GetUnicodePostgresLocale()
        {
            if (CrossInfo.ThePlatform == CrossInfo.Platform.Linux) // Debian derivatives
                return "en_US.UTF-8";
            else if (CrossInfo.ThePlatform == CrossInfo.Platform.Windows)
                return "en-US";
            else if (CrossInfo.ThePlatform == CrossInfo.Platform.MacOSX)
                return "en_US.UTF-8";
            else
                throw new NotSupportedException();
        }

        public static string RootSnapshotFolder => Path.Combine(RootWorkFolder, "Snapshots");
        public static string RootWorkFolder
        {
            get
            {

                var ret = RuntimeInformation.IsOSPlatform(OSPlatform.Windows)

                    ? Path.Combine(
                        Path.GetPathRoot(Environment.SystemDirectory),
                        "Temp\\Postgres-Work-Folder")
                    : "/tmp/Postgres-Work-Folder";

                TryAndForget.Execute(() => Directory.CreateDirectory(ret));
                return ret;
            }
        }
    }

    public class ArtifactsUtility
    {
        public static bool Can7z => _Can7z.Value;
        public static string Directory => _Directory.Value;

        private static Lazy<string> _Directory = new Lazy<string>(GetArtifactDirectory, LazyThreadSafetyMode.ExecutionAndPublication);

        private static string GetArtifactDirectory()
        {
            var ret = Environment.GetEnvironmentVariable("SYSTEM_ARTIFACTSDIRECTORY");
            return string.IsNullOrEmpty(ret) ? null : ret;
        }


        private static Lazy<bool> _Can7z = new Lazy<bool>(GetCan7z, LazyThreadSafetyMode.ExecutionAndPublication);

        private static bool GetCan7z()
        {
            try
            {
                var result = ExecProcessHelper.HiddenExec("7z", "", 10000);
                result.DemandGenericSuccess("Try 7z");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Try 7z failed{Environment.NewLine}{ex}");
                return false;
            }
        }
    }

}
