using System;
using System.IO;
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

using System.Web;

namespace LibSassNet.Web
{
    public class SassHandler : IHttpHandler
    {
        private readonly ISassCompiler Compiler = new SassCompiler();

        public void ProcessRequest(HttpContext context)
        {
            string path = context.Server.MapPath(String.Format("~{0}", context.Request.Path));
            var file = new FileInfo(path);
            if (!file.Name.StartsWith("_") && string.Equals(file.Extension, ".scss", StringComparison.OrdinalIgnoreCase))
            {
                string output = Compiler.CompileFile(path).CSS;

                context.Response.ContentType = "text/css";
                context.Response.Write(output);
            }
            else if (file.Name.EndsWith(".css", StringComparison.OrdinalIgnoreCase) && string.Equals(file.Extension, ".map", StringComparison.OrdinalIgnoreCase))
            {
                string output = Compiler.CompileFile(path).SourceMap;

                context.Response.Write(output);
            }
        }

        public bool IsReusable { get { return true; } }
    }
}
