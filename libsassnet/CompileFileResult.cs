namespace LibSassNet
{
    public struct CompileFileResult
    {
        private readonly string _CSS;
        private readonly string _SourceMap;
        public CompileFileResult(string css, string sourceMap)
        {
            _CSS = css;
            _SourceMap = sourceMap;
        }

        public string CSS { get { return _CSS; } }
        public string SourceMap { get { return _SourceMap; } }
    }
}
