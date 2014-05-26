@echo off
@SET Framework40Version=4.0

@if "%PathToMsBuild%"=="" (
  @for /F "tokens=1,2*" %%i in ('reg query "HKLM\SOFTWARE\Wow6432Node\Microsoft\MSBuild\ToolsVersions\%Framework40Version%" /v "MSBuildToolsPath"') DO (
    if "%%i" == "MSBuildToolsPath" (
      @SET "PathToMsBuild=%%k"
    )
  )
)

%PathToMsBuild%\MSBuild.exe libsass-net.sln /nologo /v:n /m:4 /p:Configuration=Release

.nuget\NuGet Pack "libsassnet\libsassnet.csproj" -Properties Configuration=Release
.nuget\NuGet Pack "libsassnet.Web\libsassnet.Web.csproj" -Properties Configuration=Release