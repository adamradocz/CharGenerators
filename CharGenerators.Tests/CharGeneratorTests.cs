using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;

namespace CharGenerators.Tests;

[UsesVerify] // 👈 Adds hooks for Verify into XUnit
public class CharGeneratorTests
{
    private const string _snapshotsDirectory = "Snapshots";
    private readonly ITestOutputHelper _output;

    public CharGeneratorTests(ITestOutputHelper output)
    {
        _output = output;
    }

    [Fact]
    public Task CanGenerateCharExtensionsInGlobalNamespace()
    {
        // The source code to test.
        const string input = @"
using CharGenerators;

[CharExtensions(Global = true)]
public class MyTestClass
{
}";

        // Pass the source code to our helper and snapshot test the output.
        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).UseDirectory(_snapshotsDirectory);
    }

    [Fact]
    public Task CanGenerateCharExtensionsInChildNamespace()
    {
        const string input = @"
using CharGenerators;

namespace MyTestNamespace
{
    [CharExtensions(Global = true)]
    public class MyTestClass
    {
    }
}";

        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).UseDirectory(_snapshotsDirectory);
    }

    [Fact]
    public Task CanGenerateCharExtensionsInSubChildNamespace()
    {
        const string input = @"
using CharGenerators;

namespace MyTestNamespace.Sub;

[CharExtensions(Global = true, OptimizeFor = ""*"")]
public class MyTestClass
{
}
";

        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).UseDirectory(_snapshotsDirectory);
    }

    [Fact]
    public Task CanGeneratePrivateHelperMethods()
    {
        const string input = @"
using CharGenerators;

namespace MyTestNamespace;

[CharExtensions]
public partial class MyTestClass
{
}";

        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).UseDirectory(_snapshotsDirectory);
    }

    [Fact]
    public Task CanGeneratePrivateHelperMethodsWithCustomOptimization()
    {
        const string input = @"
using CharGenerators;

namespace MyTestNamespace;

[CharExtensions(OptimizeFor = ""0123456789"")]
public partial class MyTestClass
{
}";

        var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

        Assert.Empty(diagnostics);
        return Verifier.Verify(output).UseDirectory(_snapshotsDirectory);
    }
}
