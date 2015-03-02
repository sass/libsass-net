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

using System.Linq;
using System.Text;
using System.Web.Optimization;

namespace LibSassNet.Web
{
    public class SassTransform : IBundleTransform
    {
        private readonly ISassCompiler Compiler = new SassCompiler();
        readonly string _BasePath;
        public SassTransform(string basePath)
        {
            _BasePath = basePath;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            StringBuilder responseContent = new StringBuilder();
            string root = context.HttpContext.Server.MapPath(_BasePath);

            // compile all scss files, but not "include" only files (let them be done with @include)
            foreach (BundleFile bundleFile in response.Files.Where(x => !x.VirtualFile.Name.StartsWith("_")))
            {
                string filename = context.HttpContext.Server.MapPath(bundleFile.IncludedVirtualPath);
                string output = Compiler.CompileFile(filename, additionalIncludePaths: new[] { root }).CSS;
                responseContent.Append(output);
            }

            response.Content = responseContent.ToString();
            response.ContentType = "text/css";
        }
    }
}
