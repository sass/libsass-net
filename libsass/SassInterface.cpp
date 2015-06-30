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

#include <exception>
#include "native\sass_interface.h"
#include "native\sass2scss.h"
#include "StringToANSI.hpp"
#include "SassInterface.hpp"
#include "native\sass_context.h"

using namespace std;

namespace LibSassNet
{
    int SassInterface::Compile(SassContext^ sassContext)
    {
        char* includePaths = MarshalString(sassContext->Options->IncludePaths);
        char* sourceString = MarshalString(sassContext->SourceString);
        struct Sass_Data_Context* ctx;

        try
        {
            ctx = sass_make_data_context(sourceString);
            struct Sass_Options* options = sass_data_context_get_options(ctx);
            struct Sass_Context* ctx_out = sass_data_context_get_context(ctx);

            // copy options around
            sass_option_set_output_style(options, GetOutputStyle(sassContext->Options->OutputStyle));
            sass_option_set_source_comments(options, sassContext->Options->IncludeSourceComments);
            sass_option_set_precision(options, sassContext->Options->Precision);
            sass_option_set_include_path(options, includePaths);
            sass_option_set_omit_source_map_url(options, true);

            sass_compile_data_context(ctx);

            sassContext->ErrorStatus = sass_context_get_error_status(ctx_out);
            sassContext->ErrorMessage = gcnew String(sass_context_get_error_message(ctx_out));
            sassContext->OutputString = gcnew String(sass_context_get_output_string(ctx_out));

            return 0;
        }
        catch (exception& e)
        {
            throw gcnew Exception(gcnew String(e.what()));
        }
        catch (...)
        {
            throw gcnew Exception("Unhandled exception in native code");
        }
        finally
        {
            // Free resources
            FreeString(includePaths);
            FreeString(sourceString);
            sass_delete_data_context(ctx);
        }
    }

    Sass_Output_Style SassInterface::GetOutputStyle(int raw)
    {
        switch (raw)
        {
        case 0: return SASS_STYLE_NESTED;
        case 1: return SASS_STYLE_EXPANDED;
        case 2: return SASS_STYLE_COMPACT;
        case 3: return SASS_STYLE_COMPRESSED;
        default: return SASS_STYLE_NESTED;
        }
    }

    int SassInterface::Compile(SassFileContext^ sassFileContext)
    {
        char* includePaths = MarshalString(sassFileContext->Options->IncludePaths);
        char* mapFile = MarshalString(sassFileContext->OutputSourceMapFile);
        char* inputPath = MarshalString(sassFileContext->InputPath);

        struct Sass_File_Context* ctx;
        try
        {
            ctx = sass_make_file_context(inputPath);
            struct Sass_Options* options = sass_file_context_get_options(ctx);
            struct Sass_Context* ctx_out = sass_file_context_get_context(ctx);

            // copy options around
            //sass_option_set_input_path(options, inputPath);
            sass_option_set_output_style(options, GetOutputStyle(sassFileContext->Options->OutputStyle));
            sass_option_set_source_comments(options, sassFileContext->Options->IncludeSourceComments);
            sass_option_set_precision(options, sassFileContext->Options->Precision);
            sass_option_set_include_path(options, includePaths);
            sass_option_set_omit_source_map_url(options, String::IsNullOrEmpty(sassFileContext->OutputSourceMapFile));
            sass_option_set_source_map_file(options, mapFile);

            sass_compile_file_context(ctx);

            sassFileContext->ErrorStatus = sass_context_get_error_status(ctx_out);
            sassFileContext->ErrorMessage = gcnew String(sass_context_get_error_message(ctx_out));
            sassFileContext->OutputString = gcnew String(sass_context_get_output_string(ctx_out));
            sassFileContext->OutputSourceMap = gcnew String(sass_context_get_source_map_string(ctx_out));

            return 0;
        }
        catch (exception& e)
        {
            throw gcnew Exception(gcnew String(e.what()));
        }
        catch (...)
        {
            throw gcnew Exception("Unhandled exception in native code");
        }
        finally
        {
            // Free resources
            FreeString(includePaths);
            FreeString(inputPath);
            FreeString(mapFile);
            sass_delete_file_context(ctx);
        }
    }

    void SassInterface::Convert(SassToScssConversionContext^ context)
    {
        char* sourceText;
        try
        {
            sourceText = MarshalString(context->SourceText);

            char* result = sass2scss(sourceText, 128);
            context->OutputText = gcnew String(result);

            FreeString(result);
        }
        catch (exception& e)
        {
            throw gcnew Exception(gcnew String(e.what()));
        }
        catch (...)
        {
            throw gcnew Exception("Unhandled exception in native code");
        }
        finally
        {
            FreeString(sourceText);
        }
    }

    // Folder context isn't implemented in core libsass library now
    /*int SassInterface::Compile(SassFolderContext^ sassFolderContext)
    {
    sass_folder_context* ctx = sass_new_folder_context();
    try
    {
    // Copy fields from managed structure to unmanaged
    ctx -> search_path = MarshalString(sassFolderContext -> SearchPath);
    //ctx -> output_path = MarshalString(sassFolderContext -> OutputPath);
    if (sassFolderContext -> Options)
    {
    ctx -> options.output_style = sassFolderContext -> Options -> OutputStyle;
    ctx -> options.source_comments = sassFolderContext -> Options -> SourceComments;
    ctx -> options.include_paths = MarshalString(sassFolderContext -> Options -> IncludePaths);
    ctx -> options.image_path = MarshalString(sassFolderContext -> Options -> ImagePath);
    }

    // Compile SASS using context provided
    int result = sass_compile_folder(ctx);

    // Copy resulting fields from unmanaged structure to managed
    //sassFolderContext -> OutputPath = gcnew String(ctx -> output_path);
    sassFolderContext -> ErrorStatus = !!ctx -> error_status;
    sassFolderContext -> ErrorMessage = gcnew String(ctx -> error_message);

    return result;
    }
    catch (exception& e)
    {
    throw gcnew Exception(gcnew String(e.what()));
    }
    catch (...)
    {
    throw gcnew Exception("Unhandled exception in native code");
    }
    finally
    {
    // Free resources
    FreeString(ctx -> options.include_paths);
    FreeString(ctx -> options.image_path);
    //FreeString(ctx -> output_path);
    FreeString(ctx -> search_path);
    sass_free_folder_context(ctx);
    }
    }*/
}