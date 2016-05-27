using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class OmitSourceMapUrlTests
    {
        [Fact]
        public static void WithOmitSourceMapUrl_DoesNotContainSourceMappingURLInOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                OutputPath = GetFixtureRelativePath("output/example.css"),
                SourceMapFile = GetFixtureRelativePath("output/example.css.map"),
                OmitSourceMapUrl = true
            }
            .Compile();

            Assert.DoesNotContain("sourceMappingURL=", result.Output);
        }

        [Fact]
        public static void WithoutOmitSourceMapUrl_ContainSourceMappingURLInOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                OutputPath = GetFixtureRelativePath("output/example.css"),
                SourceMapFile = GetFixtureRelativePath("output/example.css.map"),
                OmitSourceMapUrl = false
            }
            .Compile();

            Assert.Contains("sourceMappingURL=", result.Output);
        }
    }
}
