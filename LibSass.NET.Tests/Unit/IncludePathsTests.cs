using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class IncludePathsTests // IncludePaths (plural) an array of paths provided in OS-agnostic way
    {
        [Fact]
        public static void WithDisjointImports_OutputMatchesCssFile()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureAbsolutePath("dependencies (disjoint)", "dirA", "dirA1", "dirA11", "disjoint1.scss"),
                IncludePaths = new[]
                {
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirA"),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirA", "dirA2", "dirA21"),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirA", "dirA3")
                }
            }
            .Compile();

            var expectedOutput = GetCssString("dependencies (disjoint)", "dirA", "dirA1", "dirA11", "disjoint1.css");

            Assert.Equal(expectedOutput, result.Output);
        }

        [Fact]
        public static void WithSemiDisjointImports_OutputMatchesCssFile()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureAbsolutePath("dependencies (disjoint)", "dirB", "disjoint2.scss"),
                IncludePaths = new[]
                {
                    GetFixtureRelativePath(),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirB", "dirB2")
                }
            }
            .Compile();

            var expectedOutput = GetCssString("dependencies (disjoint)", "dirB", "disjoint2.css");

            Assert.Equal(expectedOutput, result.Output);
        }
    }
}
