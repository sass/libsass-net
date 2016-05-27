using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class LinefeedTests
    {
        [Fact]
        public static void WithUserDefinedLinefeed_ContiansUserDefinedLinefeedInOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic", "basic1.scss"),
                Linefeed = "\r"
            }
            .Compile();

            var expected = GetCssString("basic", "basic1.css").Replace("\n", "\r");

            Assert.Equal(expected, result.Output);
        }

        [Fact]
        public static void WithoutUserDefinedLinefeed_ContainsDefaultLinefeedInOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic", "basic1.scss")
            }
            .Compile();

            var expected = GetCssString("basic", "basic1.css");

            Assert.Equal(expected, result.Output);
        }
    }
}
