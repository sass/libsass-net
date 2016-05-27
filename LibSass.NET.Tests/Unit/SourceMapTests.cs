using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class SourceMapTests
    {
        [Fact]
        public static void WithOutputPath_ProducesSourceMap()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                OutputPath = GetFixtureRelativePath("output/example.css"),
                SourceMapFile = GetFixtureRelativePath("output/example.css.map")
            }
            .Compile();

            Assert.Contains("example.css", result.SourceMap);
        }

        [Fact]
        public static void WithOutputPath_ContainsSourceMappingURLInOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                OutputPath = GetFixtureRelativePath("output/example.css"),
                SourceMapFile = GetFixtureRelativePath("output/example.css.map")
            }
            .Compile();

            Assert.Contains("sourceMappingURL=", result.Output);
        }
    }
}
