using System.Diagnostics;

namespace Universe.Postgres.ServersAndSnapshots
{

    public class ProcessHierarchy
    {
        public static void KillProcessAndChildren(int pid)
        {
            var process = Process.GetProcessById(pid);
        }
    }
}