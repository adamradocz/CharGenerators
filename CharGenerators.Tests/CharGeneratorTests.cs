using System.Threading.Tasks;
using VerifyXunit;
using Xunit;
using Xunit.Abstractions;


namespace CharGenerators.Tests
{
    [UsesVerify]
    public class CharGeneratorTests
    {
        private readonly ITestOutputHelper _output;

        public CharGeneratorTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public Task CanGenerateGlobalCharExtensions()
        {
            const string input = @"using Generator.CharGenerators;

[CharExtensions(Global = true, OptimizeFor = ""0123456789"")]
public partial class GlobalTestClass
    {

    }";

            var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

            Assert.Empty(diagnostics);
            return Verifier.Verify(output).UseDirectory("Snapshots");
        }

        [Fact]
        public Task CanGeneratePrivateCharHelperMethods()
        {
            const string input = @"using Generator.CharGenerators;

[CharExtensions(OptimizeFor = ""0123456789"")]
public partial class PrivateTestClass
    {

    }";

            var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

            Assert.Empty(diagnostics);
            return Verifier.Verify(output).UseDirectory("Snapshots");
        }

        [Fact]
        public Task CanGenerateGlobalCharExtensionsWithNamespace()
        {
            const string input = @"using Generator.CharGenerators;

namespace TestNamespace
{
    [CharExtensions(Global = true, OptimizeFor = ""0123456789"")]
    public partial class GlobalTestClass
        {
    
        }
}";

            var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

            Assert.Empty(diagnostics);
            return Verifier.Verify(output).UseDirectory("Snapshots");
        }

        [Fact]
        public Task CanGeneratePrivateCharHelperMethodsWithNamespace()
        {
            const string input = @"using Generator.CharGenerators;

namespace TestNamespace
{
    [CharExtensions(OptimizeFor = ""0123456789"")]
    public partial class PrivateTestClass
        {
    
        }
}";

            var (diagnostics, output) = TestHelpers.GetGeneratedOutput<CharExtensionsSourceGenerator>(input);

            Assert.Empty(diagnostics);
            return Verifier.Verify(output).UseDirectory("Snapshots");
        }
    }
}