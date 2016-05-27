using System;
using System.IO;
using LibSass.Compiler.Options;
using Xunit;

namespace LibSass.Tests.Unit
{
    public static class DataInputTests
    {
        [Fact]
        public static void WithData_ProducesOutput()
        {
            var result = new SassOptions
            {
                Data = "tr { color: red; }"
            }
            .Compile();

            Assert.Contains("color: red", result.Output);
        }

        [Fact]
        public static void WithDataAndInputPath_ProducesOutputForData()
        {
            var result = new SassOptions
            {
                Data = "div { color: lightgoldenrodyellow; }",
                InputPath = "Fixtures/basic/basic1.scss"
            }
            .Compile();

            Assert.Contains("color: lightgoldenrodyellow", result.Output);
        }

        [Fact]
        public static void WithRelativeOutputPathAndSourceMap_ProducesSourceMapWithRelativeOutputPath()
        {
            var result = new SassOptions
            {
                Data = "div { color: lightgoldenrodyellow; }",
                InputPath = "Fixtures/basic/basic1.scss",
                SourceMapFile = "color.css"
            }
            .Compile();

            Assert.Contains("\"Fixtures/basic/basic1.scss\"", result.SourceMap);
        }

        [Fact]
        public static void WithAbsolutOutputPathAndSourceMap_ProducesSourceMapWithRelativeOutputPath()
        {
            var result = new SassOptions
            {
                Data = "div { color: lightgoldenrodyellow; }",
                InputPath = Path.Combine(Environment.CurrentDirectory, "Fixtures/basic/basic1.scss"),
                SourceMapFile = "color.css"
            }
            .Compile();

            Assert.Contains("\"Fixtures/basic/basic1.scss\"", result.SourceMap);
        }
    }
}
