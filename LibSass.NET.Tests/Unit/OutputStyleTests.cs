using LibSass.Compiler.Options;
using Xunit;

namespace LibSass.Tests.Unit
{
    public static class OutputStyleTests
    {
        [Fact]
        public static void WithCompact_ProcudecsCompactOutput()
        {
            var result = new SassOptions
            {
                Data = "a{b:c}",
                OutputStyle = SassOutputStyle.Compact
            }
            .Compile();

            Assert.Equal("a { b: c; }\n", result.Output);
        }

        [Fact]
        public static void WithCompressed_ProcudecsCompressedOutput()
        {
            var result = new SassOptions
            {
                Data = "a{b:c}",
                OutputStyle = SassOutputStyle.Compressed
            }
            .Compile();

            Assert.Equal("a{b:c}\n", result.Output);
        }

        [Fact]
        public static void WithExpanded_ProcudecsExpandedOutput()
        {
            var result = new SassOptions
            {
                Data = "a{b:c}",
                OutputStyle = SassOutputStyle.Expanded
            }
            .Compile();

            Assert.Equal("a {\n  b: c;\n}\n", result.Output);
        }

        [Fact]
        public static void WithNested_ProcudecsNestedOutput()
        {
            var result = new SassOptions
            {
                Data = "a{b:c}",
                OutputStyle = SassOutputStyle.Nested
            }
            .Compile();

            Assert.Equal("a {\n  b: c; }\n", result.Output);
        }

        [Fact]
        public static void WithoutOutputStyle_ProcudecsOutputWithDefaultStyle() // default is 'Nested'
        {
            var result = new SassOptions
            {
                Data = "a{b:c}"
            }
            .Compile();

            Assert.Equal("a {\n  b: c; }\n", result.Output);
        }
    }
}
