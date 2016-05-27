using System.IO;
using System.Text;
using LibSass.Compiler;
using LibSass.Compiler.Options;
using LibSass.Types;
using static System.Console;

namespace LibSass.Console
{
    public class Program
    {
        public static void Main()
        {
            OutputEncoding = Encoding.Unicode;

            const string inputPath = "test.scss";

            if (!File.Exists(inputPath))
            {
                WriteLine($"Input file '{inputPath}' does not exist.");
                ReadKey();
                return;
            }

            // this can be a custom type implementing ISassOptions interface.
            SassOptions sassOptions = new SassOptions
            {
                Linefeed = "\r\n",
                Indent = "\t\t",
                OutputStyle = SassOutputStyle.Compressed,
                InputPath = inputPath,
                OutputPath = "test.css",
                SourceMapFile = "test.css.map",
                IncludePaths = new[] { "subdir3", "/temp/subdir2" },
                Data = "",
                //
                // Note: Custom import is an array of delegates, each returning array of SassImport.
                //
                Importers = new CustomImportDelegate[]
                {
                    (currentImport, parentImport, options) =>
                    {
                        // Note 1: this importer will cause the next importer to be called by LibSass.
                        //
                        // Note 2: this will cause the next importer to be called by LibSass.
                        //
                        return null;
                    },
                    (currentImport, parentImport, options) =>
                    {
                        WriteLine(currentImport);
                        WriteLine(parentImport);
                        // Note 1: this returns all-valid SassImport array object.
                        //
                        // Note 2: this importer will be called, as the previous one returned null.
                        //
                        // Note 3: this shows that you can return an array of SassImport objects
                        // for "one" import. All the files/contents will be interpolated
                        // in the result at the call-site of import.
                        //
                        // Note 4: This follows the same precedence as the main SassOption, that is;
                        // if Path and Data are provided, Data will be used as content and Path will
                        // be used for source-map only. If only Path is provided,
                        // then LibSass will read it as file from file-system "only" and use its contents.
                        // If Path contains a URL string, LibSass will throw the same error as if the
                        // file on file-system not found.
                        // If you want to import Sass from URL, then create a custom importer for it
                        // which downloads the content or file from the network and then return that
                        // downloaded Path /Data.
                        // This is the reason of providing multiple importer support, so
                        // single-responsibility methods can be declarated. To bail-out to the next importer
                        // (ex. an importer can say, I don't want to handle 'ftp' requests), return null.
                        //
                        return new []
                        {
                            new SassImport { Path = "\\temp\\blah2.scss" },
                            new SassImport { Path = "/my/path/foo.scss", Data = "a{b:c;}.mango{yellow:kiwi;}" },
                            new SassImport
                            {
                                Path = "http://url/blah/foo.scss",
                                Data = ".X1{Y1:Z1;}",
                                Map = "{\"version\": 3,\"file\": \"index.css\",\"sources\": [\"index.scss\"],\"mappings\": \"AAAA,OAAO,CAAC;EACN,KAAK,EAAE,GAAI\",\"names\": []}"
                            }
                        };
                    },
                    (currentImport, parentImport, options) =>
                    {
                        // Note 1: this is how to throw error.
                        // if you are providing plugin for LibSass,
                        // or you want to handle some invalid case at
                        // runtime and want the error returned as
                        // a standard compiler error, use this approach.
                        // In this case, the returned ErrorMessage and ErrorJson
                        // will contain "My Custom Error".
                        //
                        // Note 2: this importer will not be called, as the previous one
                        // has already returned value.
                        //
                        return new [] { new SassImport { Error = "My Custom Error" } };
                    },
                    (currentImport, parentImport, options) =>
                    {
                        // Note 1: the syntax error (a missing '}').
                        //
                        // Note 2: this importer will not called, as the previous one returned.
                        //
                        return new [] { new SassImport { Path = "blah", Data = "x{y:z;" } };
                    }
                    //
                    // Note 1: if some importer throws exception, it will be runtime exception.
                    // so catch the exceptions in your code.
                    //
                    // Note 2: these lambdas can be defined separately as methods in
                    // same class or even in different class/assembly, with same signature
                    // as CustomImportDelegate.
                    //
                },
                Headers = new CustomImportDelegate[]
                {
                    // Note: all headers will be executed exactly once, before any other compile artifact.
                    (currentImport, parentImport, options) =>
                    {
                        return new [] { new SassImport { Path = "blah", Data = "x{y:z;}" } };
                    },
                    (currentImport, parentImport, options) =>
                    {
                        // Note: since this is header, unlike custom importer,
                        //       this method will be executed even though the
                        //       previous one returned a non-null value.
                        //
                        return new [] { new SassImport { Path = "blah2", Data = "more {display:inline-block;}" } };
                    }
                }
            };

            sassOptions.Functions = new SassFunctionCollection
            {
                ["foo($foo, $bar)"] = (sassOpions, signature, values) =>
                {
                    SassString foo = (SassString)values[0];
                    SassNumber bar = (SassNumber)values[1];

                    WriteLine($"foo: {foo}");
                    WriteLine($"bar: {bar}");

                    return new SassString("Trouble here!");
                },
                ["tada($list, $map)"] = GetTada
            };

            var sass = new SassCompiler(sassOptions);
            var result = sass.Compile();
            WriteLine(result.ToString());
            ReadKey();
        }

        private static SassList GetTada(ISassOptions sassOptions, string signature, params ISassType[] sassValues)
        {
            SassList list = (SassList)sassValues[0];
            SassMap map = (SassMap)sassValues[1];

            WriteLine($"list: {list}");
            WriteLine($"map: {map}");

            list.Separator = SassListSeparator.Comma;

            return list;
        }
    }
}
