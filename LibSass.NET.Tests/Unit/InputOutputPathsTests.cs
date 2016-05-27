using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class InputOutputPathsTests
    {
        [Fact]
        public static void WithRelativeInputFile_ProducesOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss")
            }
            .Compile();

            Assert.Contains("color: crimson", result.Output);
        }

        [Fact]
        public static void WithAbsoluteInputFile_ProducesOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureAbsolutePath("basic", "basic1.scss")
            }
            .Compile();

            Assert.Contains("color: crimson", result.Output);
        }

        [Fact]
        public static void WithAbsoluteInputFile_OutputMatchesCssFile()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureAbsolutePath("basic", "basic1.scss")
            }
            .Compile();

            var expectedOutput = GetCssString("basic", "basic1.css");

            Assert.Equal(expectedOutput, result.Output);
        }

        [Fact]
        public static void WithImportsInInputFile_OutputMatchesCssFile()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureAbsolutePath("dependencies (structured)", "structured1.scss")
            }
            .Compile();

            var expectedOutput = GetCssString("dependencies (structured)", "structured1.css");

            Assert.Equal(expectedOutput, result.Output);
        }
    }
}
