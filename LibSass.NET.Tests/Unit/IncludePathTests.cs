using LibSass.Compiler.Options;
using Xunit;
using static System.IO.Path;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class IncludePathTests // IncludePath (singular) a string of paths provided in OS-aware way (joined by PathSeparator)
    {
        [Fact]
        public static void WithDisjointImports_OutputMatchesCssFile()
        {
            var result = new SassOptions
            {

                InputPath = GetFixtureAbsolutePath("dependencies (disjoint)", "dirA", "dirA1", "dirA11", "disjoint1.scss"),
                IncludePath = string.Join(PathSeparator.ToString(),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirA"),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirA", "dirA2", "dirA21"),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirA", "dirA3")
                )
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
                IncludePath = string.Join(PathSeparator.ToString(),

                    GetFixtureRelativePath(),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirB", "dirB2")
                )
            }
            .Compile();

            var expectedOutput = GetCssString("dependencies (disjoint)", "dirB", "disjoint2.css");

            Assert.Equal(expectedOutput, result.Output);
        }

        [Fact]
        public static void WithIncorrectPathSeparator_GivesError()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureAbsolutePath("dependencies (disjoint)", "dirB", "disjoint2.scss"),
                IncludePath = string.Join(PathSeparator == ';' ? ":" : ";",
                    GetFixtureRelativePath(),
                    GetFixtureAbsolutePath("dependencies (disjoint)", "dirB", "dirB2")
                )
            }
            .Compile();

            Assert.NotEmpty(result.ErrorMessage);
        }
    }
}
