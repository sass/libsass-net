using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class SourceCommentsTests
    {
        [Fact]
        public static void WithIncludeSourceComments_ContainsSourceCommentsInOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                IncludeSourceComments = true
            }
            .Compile();

            Assert.Contains("/* line 1,", result.Output);
        }

        [Fact]
        public static void WithoutIncludeSourceComments_DoesNotContainSourceCommentsInOutput()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                IncludeSourceComments = false
            }
            .Compile();

            Assert.DoesNotContain("/* line 1,", result.Output);
        }
    }
}
