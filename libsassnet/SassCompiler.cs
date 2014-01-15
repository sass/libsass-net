//Copyright (C) 2013 by TBAPI-0KA
//
//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies
//of the Software, and to permit persons to whom the Software is furnished to do
//so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//SOFTWARE.

using System;
using System.Collections.Generic;
using System.IO;

namespace LibSassNet
{
    public class SassCompiler : ISassCompiler
    {
        private readonly ISassInterface _sassInterface;

        //static SassCompiler()
        //{
        //    AssemblyResolver.Initialize();
        //}

        public SassCompiler()
        {
            _sassInterface = new SassInterface();
        }

        public SassCompiler(ISassInterface sassInterface)
        {
            _sassInterface = sassInterface;
        }

        public string Compile(string source, OutputStyle outputStyle = OutputStyle.Nested, bool sourceComments = true, IEnumerable<string> includePaths = null)
        {
            if (outputStyle != OutputStyle.Nested && outputStyle != OutputStyle.Compressed)
            {
                throw new ArgumentException("Only nested and compressed output styles are currently supported by libsass.");
            }

            SassContext context = new SassContext
            {
                SourceString = source,
                Options = new SassOptions
                {
                    OutputStyle = (int)outputStyle,
                    SourceComments = sourceComments,
                    IncludePaths = includePaths != null ? String.Join(";", includePaths) : String.Empty,
                    ImagePath = String.Empty
                }
            };

            _sassInterface.Compile(context);

            if (context.ErrorStatus)
            {
                throw new SassCompileException(context.ErrorMessage);
            }

            return context.OutputString;
        }

        public string CompileFile(string inputPath, OutputStyle outputStyle = OutputStyle.Nested, bool sourceComments = true, IEnumerable<string> additionalIncludePaths = null)
        {
            if (outputStyle != OutputStyle.Nested && outputStyle != OutputStyle.Compressed)
            {
                throw new ArgumentException("Only nested and compressed output styles are currently supported by libsass.");
            }

            string directoryName = Path.GetDirectoryName(inputPath);
            List<string> includePaths = new List<string> { directoryName };
            if (additionalIncludePaths != null)
            {
                includePaths.AddRange(additionalIncludePaths);
            }

            SassFileContext context = new SassFileContext
            {
                InputPath = inputPath,
                Options = new SassOptions
                {
                    OutputStyle = (int)outputStyle,
                    SourceComments = sourceComments,
                    IncludePaths = String.Join(";", includePaths),
                    ImagePath = String.Empty
                }
            };

            _sassInterface.Compile(context);

            if (context.ErrorStatus)
            {
                throw new SassCompileException(context.ErrorMessage);
            }

            return context.OutputString;
        }
    }
}
