using Xunit;
using static LibSass.Compiler.SassCompiler;

namespace LibSass.Tests.Unit
{
    public static class VersionInfoTests
    {
        [Fact]
        public static void WithStaticSassCompiler_ContainsAllVersionInfos()
        {
            Assert.NotEmpty(SassInfo.LibSassNetVersion);
            Assert.NotEmpty(SassInfo.LibSassVersion);
            Assert.NotEmpty(SassInfo.SassLanguageVersion);
        }
    }
}
