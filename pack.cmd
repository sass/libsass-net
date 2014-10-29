@echo off
@SET Framework40Version=12.0

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

"%PathToMsBuild32%\MSBuild.exe" libsass-net.sln /nologo /v:n /m:4 /p:Configuration=Release
"%PathToMsBuild64%\MSBuild.exe" libsass-net.sln /nologo /v:n /m:4 /p:Configuration=Release /p:Platform=x64

.nuget\NuGet Pack "libsassnet\libsassnet.csproj" -Properties Configuration=Release
.nuget\NuGet Pack "libsassnet.Web\libsassnet.Web.csproj" -Properties Configuration=Release
.nuget\NuGet Pack "libsassnet\libsassnet.x64.csproj" -Properties Configuration=Release;Platform=x64
.nuget\NuGet Pack "libsassnet.Web\libsassnet.Web.x64.csproj" -Properties Configuration=Release;Platform=x64