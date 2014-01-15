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
#include "StringToANSI.hpp"
#include "SassInterface.hpp"

using namespace std;

namespace LibSassNet
{
	int SassInterface::Compile(SassContext^ sassContext)
	{
		sass_context* ctx = sass_new_context();
		try
		{
			// Copy fields from managed structure to unmanaged
			ctx -> source_string = MarshalConstString(sassContext -> SourceString);
			if (sassContext -> Options)
			{
				ctx -> options.output_style = sassContext -> Options -> OutputStyle;
				ctx -> options.source_comments = sassContext -> Options -> SourceComments;
				ctx -> options.include_paths = MarshalString(sassContext -> Options -> IncludePaths);
				ctx -> options.image_path = MarshalString(sassContext -> Options -> ImagePath);
			}

			// Compile SASS using context provided
			int result = sass_compile(ctx);

			// Copy resulting fields from unmanaged structure to managed
			sassContext -> OutputString = gcnew String(ctx -> output_string);
			sassContext -> ErrorStatus = !!ctx -> error_status;
			sassContext -> ErrorMessage = gcnew String(ctx -> error_message);

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
			FreeConstString(ctx -> source_string);
			sass_free_context(ctx);
		}
	}

	int SassInterface::Compile(SassFileContext^ sassFileContext)
	{
		sass_file_context* ctx = sass_new_file_context();
		try
		{
			// Copy fields from managed structure to unmanaged
			ctx -> input_path = MarshalString(sassFileContext -> InputPath);
			if (sassFileContext -> Options)
			{
				ctx -> options.output_style = sassFileContext -> Options -> OutputStyle;
				ctx -> options.source_comments = sassFileContext -> Options -> SourceComments;
				ctx -> options.include_paths = MarshalString(sassFileContext -> Options -> IncludePaths);
				ctx -> options.image_path = MarshalString(sassFileContext -> Options -> ImagePath);
			}

			// Compile SASS using context provided
			int result = sass_compile_file(ctx);

			// Copy resulting fields from unmanaged structure to managed
			sassFileContext -> OutputString = gcnew String(ctx -> output_string);
			sassFileContext -> ErrorStatus = !!ctx -> error_status;
			sassFileContext -> ErrorMessage = gcnew String(ctx -> error_message);

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
			FreeString(ctx -> input_path);
			sass_free_file_context(ctx);
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