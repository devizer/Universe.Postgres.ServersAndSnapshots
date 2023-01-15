using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using BenchmarkDotNet.Attributes;

namespace Universe.Postgres.ServersAndSnapshots.Benchmark
{
    public class ServerBenchmark
    {
        private ServerBinaries _server;
        List<string> _directories = new List<string>(10000);

        [GlobalSetup]
        public void GlobalSetup()
        {
            _server = PostgresServerDiscovery.GetServers().OrderByDescending(x => x.Version).FirstOrDefault();
            Console.WriteLine($"PostgreSQL Server: {_server}");
            InitDbImplementation(true);
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
}