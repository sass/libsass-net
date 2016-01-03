using LibSass.Compiler.Options;
using Xunit;

namespace LibSass.Tests.Unit
{
    public static class PrecisionTests
    {
        [Fact]
        public static void WithUserDefinedPrecision_AppliesUserDefinedPrecision()
        {
            var result = new SassOptions
            {
                Data = "$my-width: (20*100)/29; a{width:$my-width}",
                Precision = 14
            }
            .Compile();

            Assert.Equal("a {\n  width: 68.9655172414; }\n", result.Output);
        }

        [Fact]
        public static void WithoutUserDefinedPrecision_AppliesDefaultPrecision() // default is 5
        {
            var result = new SassOptions
            {
                Data = "$my-width: (20*100)/29; a{width:$my-width}"
            }
            .Compile();

            Assert.Equal("a {\n  width: 68.96552; }\n", result.Output);
        }
    }
}
