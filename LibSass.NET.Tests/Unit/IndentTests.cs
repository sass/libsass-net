using LibSass.Compiler.Options;
using Xunit;

namespace LibSass.Tests.Unit
{
    public static class IndentTests
    {
        [Fact]
        public static void WithUserDefinedIndentation_ContiansUserDefinedIndentationInOutput()
        {
            var result = new SassOptions
            {
                Data = "a{b:c}",
                Indent = "\t\t\t"
            }
            .Compile();

            Assert.Equal("a {\n\t\t\tb: c; }\n", result.Output);
        }

        [Fact]
        public static void WithoutUserDefinedIndentation_ContainsDefaultIndentationInOutput()
        {
            var result = new SassOptions
            {
                Data = "a{b:c}"
            }
            .Compile();

            Assert.Equal("a {\n  b: c; }\n", result.Output);
        }
    }
}
