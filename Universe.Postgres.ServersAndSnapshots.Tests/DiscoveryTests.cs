using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Universe.NUnitTests;

namespace Universe.Postgres.ServersAndSnapshots.Tests
{
    public class DiscoveryTests : NUnitTestsBase
    {

        [Test, Explicit]
        public void TestEscape()
        {
            var fileName = $"C:\\Temp\\postgres processes {DateTime.Now:yyyy-MM-dd-HH-mm-ss}.log";
            var args = $"-c \"(Get-WmiObject Win32_Process -Filter \"\"\"name like '%postgres%'\"\"\") | ft Handle,Name,CommandLine > \"\"\"{fileName}\"\"\"\"";
            var result = ExecProcessHelper.HiddenExec("powershell", args);
            Console.WriteLine(result.OutputText);

        }
        [Test]
        public void TestNothing()
        {
            Console.WriteLine($"ArtifactsUtility.Can7z: [{ArtifactsUtility.Can7z}]");
            Console.WriteLine($"ArtifactsUtility.Directory: [{ArtifactsUtility.Directory}]");

            Version.TryParse("16.99999", out var v);
            TestContext.Progress.WriteLine($"Version: {v}");

            TestContext.Progress.WriteLine(Path.GetPathRoot(Environment.SystemDirectory));
            TestContext.Progress.WriteLine(new DirectoryInfo("C:\\X").FullName);
            TestContext.Progress.WriteLine(new DirectoryInfo("C:\\X\\").FullName);
        }

        [Test]
        [TestCase("First")]
        [TestCase("Next")]
        public void TestDiscovery(string id)
        {
            ServerBinaries[] servers = PostgresServerDiscovery.GetServers();
            TestContext.Progress.WriteLine($"Servers: {servers.Length}");
            foreach (var server in servers)
            {
                TestContext.Progress.WriteLine($"• {server}");
            }
        }
    }
}