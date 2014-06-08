using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibSassNet
{
    public class SassToScssConverter : ISassToScssConverter
    {
        readonly ISassInterface _Sass;
        public SassToScssConverter()
        {
            _Sass = new SassInterface();
        }

        public string Convert(string source)
        {
            var context = new SassToScssConversionContext { SourceText = source };
            _Sass.Convert(context);

            return context.OutputText;
        }
    }
}
