using BenchmarkDotNet.Running;
using ConsoleApp;

BenchmarkSwitcher benchmarkSwitcher = new(
    new[]
    {
        typeof(Benchy),
        typeof(PrivateBenchy)
    });

benchmarkSwitcher.Run(args);
