using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibSassNet
{
    /// <summary>
    /// Converts SASS to SCSS
    /// </summary>
    public interface ISassToScssConverter
    {
        string Convert(string source);
    }
}
