using System.Reflection;
using LibSass.Compiler;
using LibSass.Compiler.Options;
using LibSass.Tests.Spec;
using static System.IO.File;
using static System.IO.Path;

namespace LibSass.Tests
{
    public static class TestCommonsAndExtensions
    {
        private static readonly string FixturesAbsoluteDirectory;

        private const string FixturesRelativeDirectory = "../../Fixtures";

        static TestCommonsAndExtensions()
        {
            var assembly = typeof(SpecTests).GetTypeInfo().Assembly;
            var codebase = assembly.CodeBase.Replace("file:///", "");
            var baseDir = GetDirectoryName(
                              GetDirectoryName(
                                  GetDirectoryName(codebase)));

            FixturesAbsoluteDirectory = baseDir != null ?
                                    Combine(baseDir, "Fixtures") :
                                    string.Empty;
        }

        public static SassResult Compile(this ISassOptions options)
        {
            var sass = new SassCompiler(options);
            return sass.Compile();
        }

        public static string GetCssString(params string[] fragments)
        {
            var cssPath = Combine(FixturesAbsoluteDirectory, Combine(fragments));
            return ReadAllText(cssPath).Replace("\r\n", "\n");
        }

        public static string GetFixtureAbsolutePath(params string[] fragments)
        {
            return fragments == null ?
                FixturesAbsoluteDirectory :
                Combine(FixturesAbsoluteDirectory, Combine(fragments));
        }

        public static string GetFixtureRelativePath(params string[] fragments)
        {
            return fragments == null ?
                FixturesRelativeDirectory :
                Combine(FixturesRelativeDirectory, Combine(fragments));
        }
    }
}
