using System.Linq;
using System.Web.Optimization;
using LibSass.Compiler;
using LibSass.Compiler.Options;

namespace LibSass.Web
{
    public class SassTransform : IBundleTransform
    {
        private readonly string _basePath;

        public SassTransform(string basePath)
        {
            _basePath = basePath;
        }

        public void Process(BundleContext context, BundleResponse response)
        {
            string root = context.HttpContext.Server.MapPath(_basePath);

            // compile all scss files, but not "include" only files (let them be done with @include)
            var results = response.Files.Where(file => file.VirtualFile.Name != null && !file.VirtualFile.Name.StartsWith("_"))
                         .Select(bundleFile => context.HttpContext.Server
                                              .MapPath(bundleFile.IncludedVirtualPath))
                         .Select(filename => new SassOptions
                         {
                             InputPath = filename,
                             IncludePaths = new[] { root }
                         })
                         .Select(sassOptions => new SassCompiler(sassOptions)
                             .Compile()
                             .Output);

            response.Content = string.Join("", results);
            response.ContentType = "text/css";
        }
    }
}
