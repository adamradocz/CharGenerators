using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace CharGenerators.Benchmarks;

[CharExtensions(Global = true, OptimizeFor = "0123456789")]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory, BenchmarkLogicalGroupRule.ByParams)]
[MemoryDiagnoser, DisassemblyDiagnoser(printInstructionAddresses: true, printSource: true, exportDiff: true)]
public partial class ToStringBenchmarks
{
    [Params('0')]
    public char Characters { get; set; }

    [Benchmark(Baseline = true)]
    public string CharToString() => Characters.ToString();

    [Benchmark]
    public string StringCreate() => StringCreateReverseSkipLocalsInit(Characters);

    [System.Runtime.CompilerServices.SkipLocalsInit]
    public static string StringCreateReverseSkipLocalsInit(char character) =>
        string.Create(1, character, (buffer, value) =>
        {
            buffer[0] = value;
        });

    [Benchmark]
    public string ToStringFast() => Characters.ToStringFast();
}
