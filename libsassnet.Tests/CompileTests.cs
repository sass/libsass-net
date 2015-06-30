using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibSassNet.Tests
{
    public class CompileTests
    {
        readonly ISassCompiler Compiler = new SassCompiler();

        [Fact]
        public void can_compile_simple_string()
        {
            var response = Compiler.Compile("body { color: red; }");
            Assert.NotEmpty(response);
        }

        [Fact]
        public void can_compile_file()
        {
            var response = Compiler.CompileFile("example.scss", includeSourceComments: false);
            Assert.NotEmpty(response.CSS);
        }

        [Fact]
        public void when_source_map_file_specfied_should_return_sourcemap_data()
        {
            var response = Compiler.CompileFile("example.scss", sourceMapPath: "example.css.map");
            Assert.NotEmpty(response.SourceMap);
        }
    }
}
