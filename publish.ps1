.\pack.cmd;

[string]$token = Read-Host -Prompt "What is the token";
$token = $token.Trim();

[string]$version = Read-Host -Prompt "What version";
$version = $version.Trim();


$packages = @("libsassnet", "libsassnet.Web");
$packages | %{ 
    $packageName = $_ + "." + $version + ".nupkg";
    .\.nuget\NuGet.exe Push $packageName $token;
};