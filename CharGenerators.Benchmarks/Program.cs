using BenchmarkDotNet.Running;
using CharGenerators.Benchmarks;

BenchmarkSwitcher benchmarkSwitcher = new(
    new[]
    {
        typeof(ToStringBenchmarks)
    });

benchmarkSwitcher.Run(args);
