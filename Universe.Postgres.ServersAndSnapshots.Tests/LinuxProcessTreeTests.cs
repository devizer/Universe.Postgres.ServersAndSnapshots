using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Universe.LinuxTaskStats;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class LinuxProcessTreeTests : NUnitTestsBase
    {
        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        public void ShowSupportedFeatures(string id)
        {
            Console.WriteLine($"IsGetTidSupported: {LinuxTaskStatsReader.IsGetTidSupported}");
            Console.WriteLine($"IsGetPidSupported: {LinuxTaskStatsReader.IsGetPidSupported}");
            Console.WriteLine($"IsGetTaskStatByProcessSupported: {LinuxTaskStatsReader.IsGetTaskStatByProcessSupported}");
            Console.WriteLine($"IsGetTaskStatByThreadSupported: {LinuxTaskStatsReader.IsGetTaskStatByThreadSupported}");
            if (LinuxTaskStatsReader.IsGetTaskStatByProcessSupported)
            {
                Console.WriteLine($"GeTaskStatsVersion(): {LinuxTaskStatsReader.GeTaskStatsVersion()}");
                LinuxTaskStats.LinuxTaskStats? currentStat = LinuxTaskStatsReader.GetByProcess(Process.GetCurrentProcess().Id);
                Console.WriteLine($"Has Permission: {(currentStat == null ? "Yes" : "No")}");
            }
        }

        [Test]
        [TestCase("Warmup")]
        [TestCase("Run")]
        [RequiredOs(Os.Linux)]
        public void MeasureProcessTreeOnLinux(string id)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var processIds = Process.GetProcesses().Select(x => x.Id).ToArray();
            var msecIdList = sw.ElapsedTicks * 1000d / Stopwatch.Frequency;
            sw = Stopwatch.StartNew();
            int success = 0;
            foreach (var processId in processIds)
            {
                var stat = LinuxTaskStatsReader.GetByProcess(processId);
                if (stat.HasValue && stat.Value.Pid != processId) success++;
            }

            var msecStat = sw.ElapsedTicks * 1000d / Stopwatch.Frequency;
            Console.WriteLine(@$"Process.GetProcesses() took {msecIdList:n2} milliseconds
LinuxTaskStatsReader.GetByProcess() for each took {msecStat:n2} milliseconds
Success Statistics: {success} of {processIds.Length}");

        }

    }
}