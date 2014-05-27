.\pack.cmd;

[string]$token = Read-Host -Prompt "What is the token";
$token = $token.Trim();


$packages = @("libsassnet", "libsassnet.Web");
$packages %{ 
    $packageName = $_ + ".nupkg";
    & '.\.nuget\NuGet.exe' $packageName $token
};