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
using System.Text;

namespace LibSassNet
{
    public class SassCompiler : ISassCompiler
    {
        private readonly ISassInterface _sassInterface;

        public SassCompiler()
        {
            _sassInterface = new SassInterface();
        }

        public SassCompiler(ISassInterface sassInterface)
        {
            _sassInterface = sassInterface;
        }

        public string Compile(string source, OutputStyle outputStyle = OutputStyle.Nested, bool includeSourceComments = true, int precision = 5, IEnumerable<string> includePaths = null)
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
                    IncludeSourceComments = includeSourceComments,
                    IncludePaths = includePaths != null ? String.Join(";", includePaths) : String.Empty,
                    ImagePath = string.Empty,
                    Precision = precision
                }
            };

            _sassInterface.Compile(context);

            if (context.ErrorStatus)
            {
                throw new SassCompileException(context.ErrorMessage);
            }

            return context.OutputString;
        }

        public CompileFileResult CompileFile(string inputPath, OutputStyle outputStyle = OutputStyle.Nested,  string sourceMapPath = null, bool includeSourceComments = true, int precision = 5, IEnumerable<string> additionalIncludePaths = null)
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
                // libsass 3.0 expects utf8 path string, but strings in .NET are utf16, so we need to convert it
                InputPath = Utf16ToUtf8(inputPath),
                Options = new SassOptions
                {
                    OutputStyle = (int)outputStyle,
                    IncludeSourceComments = includeSourceComments,
                    IncludePaths = String.Join(";", includePaths),
                    ImagePath = string.Empty,
                    Precision = precision
                },
                OutputSourceMapFile = sourceMapPath
            };

            _sassInterface.Compile(context);

            if (context.ErrorStatus)
            {
                throw new SassCompileException(context.ErrorMessage);
            }

            return new CompileFileResult(context.OutputString, context.OutputSourceMap);
        }

        /// <summary>
        /// Converts utf16 string to utf8
        /// </summary>
        /// <param name="utf16String"></param>
        /// <returns></returns>
        private static string Utf16ToUtf8(string utf16String)
        {
            // Get UTF16 bytes and convert UTF16 bytes to UTF8 bytes
            var utf16Bytes = Encoding.Unicode.GetBytes(utf16String);
            var utf8Bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, utf16Bytes);

            // Return UTF8 bytes as ANSI string
            return Encoding.Default.GetString(utf8Bytes);
        }
    }
}
