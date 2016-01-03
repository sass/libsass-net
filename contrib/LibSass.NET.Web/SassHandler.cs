using System;
using System.IO;
using System.Web;
using LibSass.Compiler;
using LibSass.Compiler.Options;

namespace LibSass.Web
{
    public class SassHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            string path = context.Server.MapPath(context.Request.AppRelativeCurrentExecutionFilePath);
            var file = new FileInfo(path);
            SassCompiler compiler = new SassCompiler(new SassOptions { InputPath = path });
            SassResult result = compiler.Compile();

            if (!file.Name.StartsWith("_") && string.Equals(file.Extension, ".scss", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.ContentType = "text/css";
                context.Response.Write(result.Output);
            }
            else if (file.Name.EndsWith(".css", StringComparison.OrdinalIgnoreCase) && string.Equals(file.Extension, ".map", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.Write(result.Output);
            }
        }

        public bool IsReusable { get { return true; } }
    }
}
