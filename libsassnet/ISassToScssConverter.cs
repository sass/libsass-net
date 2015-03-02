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
