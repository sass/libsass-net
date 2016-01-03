using System;
using System.Text;
using LibSass.Compiler.Options;
using Xunit;
using static LibSass.Tests.TestCommonsAndExtensions;

namespace LibSass.Tests.Unit
{
    public static class EmbedSourceMapTests
    {
        [Fact]
        public static void WithEmbedSourceMapUrl_ContainOutputPathInSourceMap()
        {
            var result = new SassOptions
            {
                InputPath = GetFixtureRelativePath("basic/basic1.scss"),
                OutputPath = GetFixtureRelativePath("output/example.css"),
                SourceMapFile = GetFixtureRelativePath("output/example.css.map"),
                EmbedSourceMap = true
            }
            .Compile();

            var startKey = "base64,";
            var endKey = " */";
            var indexStart = result.Output.IndexOf(startKey, StringComparison.Ordinal) + startKey.Length;
            var indexEnd = result.Output.IndexOf(endKey, StringComparison.Ordinal);
            var encodedString = result.Output.Substring(indexStart, indexEnd - indexStart);

            byte[] data = Convert.FromBase64String(encodedString);
            var json = Encoding.UTF8.GetString(data);

            Assert.Contains("\"example.css\"", json);
        }

        [Fact]
        public static void WithEmbedSourceMapUrl_ContainInputPathInSourceMap()
        {
            var inputPath = GetFixtureAbsolutePath("basic", "basic1.scss");
            var outputPath = GetFixtureAbsolutePath("output", "example.css");
            var mapPath = GetFixtureAbsolutePath("output", "example.css.map");
            var result = new SassOptions
            {
                InputPath = inputPath,
                OutputPath = outputPath,
                SourceMapFile = mapPath,
                EmbedSourceMap = true
            }
            .Compile();

            var startKey = "base64,";
            var endKey = " */";
            var indexStart = result.Output.IndexOf(startKey, StringComparison.Ordinal) + startKey.Length;
            var indexEnd = result.Output.IndexOf(endKey, StringComparison.Ordinal);
            var encodedString = result.Output.Substring(indexStart, indexEnd - indexStart);

            byte[] data = Convert.FromBase64String(encodedString);
            var json = Encoding.UTF8.GetString(data);

            var uri = new Uri(outputPath);
            var relativePath = uri.MakeRelativeUri(new Uri(inputPath)).ToString();

            Assert.Contains(relativePath, json);
        }
    }
}
