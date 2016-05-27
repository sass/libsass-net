using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class SourceMapRootTests
    {
        [Fact]
        public static void WithSourceRoot_PassesThroughSourceRootInSourceMap()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                SourceMapFile = GetFixtureRelativePath("output/example.css.map"),
                SourceMapRoot = "../webservice/api/"
            }
            .Compile();

            Assert.Contains("\"sourceRoot\": \"../webservice/api/\",", result.SourceMap);
        }

        [Fact]
        public static void WithSourceRoot_ContainsEmptySourceRootInSourceMap()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                SourceMapFile = GetFixtureRelativePath("output/example.css.map")
            }
            .Compile();

            Assert.DoesNotContain("\"sourceRoot\"", result.SourceMap);
        }
    }
}
