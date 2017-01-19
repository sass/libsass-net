using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LibSass.Compiler.Options;
using Xunit;
using static System.IO.File;
using static System.IO.Path;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Spec
{
    public static class SpecTests
    {
        [Theory, MemberData(nameof(GetSassSpecDataSuites))]
        public static void Sass_Specs_Run(string source, string expected, bool error, string[] paths)
        {
            var result = new SassOptions
            {
                InputPath = source,
                IncludePaths = paths
            }
            .Compile();

            if (!string.IsNullOrEmpty(result.ErrorMessage))
            {
                Assert.True(error);
            }
            else
            {
                Assert.False(error);
            }

            if (!string.IsNullOrEmpty(expected))
            {
                Assert.Equal(
                    expected.SpecNormalize(),
                    result.Output.SpecNormalize());
            }
        }

        const string SpecInputFile = "input.scss";
        const string SpecExpectedFile = "expected_output.css";
        const string SpecErrorFile = "error";
        const string SpecSubDirectory = "sub";

        private static IEnumerable<object> GetSassSpecDataSuites()
        {
            string spec = GetFixtureAbsolutePath("sass-spec", "spec");
            string[] ignoreSuites =
            {
                "libsass-todo-issues",
                "libsass-todo-tests"
            };

            var directories = Directory.GetDirectories(spec, "*", SearchOption.TopDirectoryOnly)
                             .Select(d => new DirectoryInfo(d))
                             .Where(d => ignoreSuites.All(s => d.Name != s));

            foreach (var directory in directories)
            {
                var testDirectories =
                    Directory.GetDirectories(directory.FullName, "*", SearchOption.TopDirectoryOnly)
                   .Select(d => new DirectoryInfo(d));

                foreach (var testDirectory in testDirectories)
                {
                    var testPath = testDirectory.FullName;
                    var hasErrorFile = Exists(Combine(testPath, SpecErrorFile));
                    var hasError = false;

                    if (hasErrorFile)
                    {
                        var errorFileContents = ReadAllText(Combine(testPath, SpecErrorFile));
                        hasError = !(errorFileContents.StartsWith("DEPRECATION WARNING") ||
                                     errorFileContents.StartsWith("WARNING:") ||
                                     Regex.IsMatch(errorFileContents, @"^.*?\/input.scss:\d+ DEBUG:"));
                    }

                    var inputFile = Combine(testDirectory.FullName, SpecInputFile);

                    if (!Exists(inputFile))
                        continue;

                    yield return new object[]
                    {
                        inputFile,
                        ReadAllText(Combine(testDirectory.FullName, SpecExpectedFile)),
                        hasErrorFile && hasError,
                        new[] { testPath, Combine(testPath, SpecSubDirectory) }
                    };
                }
            }
        }

        private static string SpecNormalize(this string input)
        {
            return Regex.Replace(input, @"\s+", string.Empty)
                        .Replace("{\r", "{")
                        .Replace("{", "{\n")
                        .Replace(";", ";\n");
        }
    }
}
