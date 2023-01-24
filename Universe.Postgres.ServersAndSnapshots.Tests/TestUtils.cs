using System;
using System.IO;
using System.Runtime.InteropServices;

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
}
