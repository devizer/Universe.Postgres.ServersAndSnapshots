using System;
using NUnit.Framework;
using Universe.LinuxTaskStats;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class LinuxProcessTreeTests : NUnitTestsBase
    {
        [Test]
        public void ShowSupportedFeatures()
        {
            Console.WriteLine($"IsGetTidSupported: {LinuxTaskStatsReader.IsGetTidSupported}");
            Console.WriteLine($"IsGetPidSupported: {LinuxTaskStatsReader.IsGetPidSupported}");
            Console.WriteLine($"IsGetTaskStatByProcessSupported: {LinuxTaskStatsReader.IsGetTaskStatByProcessSupported}");
            Console.WriteLine($"IsGetTaskStatByThreadSupported: {LinuxTaskStatsReader.IsGetTaskStatByThreadSupported}");
            if (LinuxTaskStatsReader.IsGetTaskStatByProcessSupported)
                Console.WriteLine($"GeTaskStatsVersion(): {LinuxTaskStatsReader.GeTaskStatsVersion()}");
        }
    }
}