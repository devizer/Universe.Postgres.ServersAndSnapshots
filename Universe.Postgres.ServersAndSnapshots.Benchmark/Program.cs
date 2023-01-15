using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace Universe.Postgres.ServersAndSnapshots.Benchmark
{
    public class Program
    {
        static void Main(string[] args)
        {
            Summary summary = BenchmarkRunner.Run<ServerBenchmark>();
        }
    }

    public class ServerBenchmark
    {
        private ServerBinaries _server;
        List<string> _directories = new List<string>(10000);

        [GlobalSetup]
        public void GlobalSetup()
        {
            _server = PostgresServerDiscovery.GetServers().OrderByDescending(x => x.Version).FirstOrDefault();
            Console.WriteLine($"PostgreSQL Server: {_server}");
            InitDbImplementation(false);
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            foreach (var directory in _directories)
            {
                try
                {
                    Directory.Delete(directory, true);
                }
                catch
                {
                }
            }
        }

        [Benchmark]
        public void InitDb()
        {
            InitDbImplementation(false);
        }

        private void InitDbImplementation(bool debug)
        {
            var dataPath = Path.Combine(TestUtils.RootWorkFolder, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ffff"));
            _directories.Add(dataPath);

            PostgresInstanceOptions options = new PostgresInstanceOptions()
            {
                DataPath = dataPath,
                ServerPort = Interlocked.Increment(ref TestUtils.Port),
            };

            var result = PostgresServerManager.CreateServerInstance(_server, options);
            if (debug)
                Console.WriteLine($"INITDB OUTPUT: {Environment.NewLine}{result.OutputText}");
        }

    }

    internal class TestUtils
    {
        public static int Port = 5432;

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
