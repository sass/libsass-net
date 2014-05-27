# remove old packages
dir *.nupkg | remove-item;

# generate packages
.\pack.cmd;

# get the token
[string]$token = Read-Host -Prompt "What is the token";
$token = $token.Trim();

$packages = dir *.nupkg | %{ $_.Name };
$packages | %{ 
    .\.nuget\NuGet.exe Push $_ $token;
};