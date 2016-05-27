@echo off
@SET Framework40Version=14.0

@if "%PathToMsBuild32%"=="" (
  @for /F "tokens=1,2*" %%i in ('reg query "HKLM\SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions\%Framework40Version%" /v "MSBuildToolsPath"') DO (
    if "%%i" == "MSBuildToolsPath" (
      @SET "PathToMsBuild32=%%k"
    )
  )
)

@if "%PathToMsBuild64%"=="" (
  @for /F "tokens=1,2*" %%i in ('reg query "HKLM\SOFTWARE\Microsoft\MSBuild\ToolsVersions\%Framework40Version%" /v "MSBuildToolsPath"') DO (
    if "%%i" == "MSBuildToolsPath" (
      @SET "PathToMsBuild64=%%k"
    )
  )
)

"%PathToMsBuild32%\MSBuild.exe" LibSass.NET.sln /nologo /v:n /m:4 /p:Configuration=Release

.nuget\NuGet Pack "LibSass.NET\LibSass.NET.csproj" 					-Properties Configuration=Release
.nuget\NuGet Pack "contrib\LibSass.NET.Web\LibSass.NET.Web.csproj" 	-Properties Configuration=Release
