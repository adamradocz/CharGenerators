using CharGenerators;
using Xunit;

namespace CharGenerators.Tests.IntegrationTests
{
    //[CharExtensions(Global = true, OptimizeFor = "0123456789")]
    public partial class GlobalTestClass
    {

    }

    public class CharGeneratorTests
    {
        [Fact]
        public void ToStringFast_IncludedCharsGiven_ReturnsCorrectData()
        {
            //Arrange
            char testChar = '0';

            //Act
            //string result = testChar
        }
    }
}
