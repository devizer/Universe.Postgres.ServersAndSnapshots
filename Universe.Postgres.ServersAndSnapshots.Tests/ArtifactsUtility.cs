using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exception = System.Exception;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
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
            catch(Exception ex)
            {
                Console.WriteLine($"Try 7z failed{Environment.NewLine}{ex}");
                return false;
            }
        }
    }
}
