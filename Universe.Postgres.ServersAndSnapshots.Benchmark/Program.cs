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
}
