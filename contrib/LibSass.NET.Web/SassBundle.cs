using System.Web.Optimization;

namespace LibSass.Web
{
    public class SassBundle : Bundle
    {
        public SassBundle(string virtualPath, string basePath = "~/")
            : base(virtualPath, new SassTransform(basePath), new CssMinify()) { }

        public SassBundle(string virtualPath, string cdnPath, string basePath = "~/")
            : base(virtualPath, cdnPath, new SassTransform(basePath), new CssMinify()) { }
    }
}
