$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
$nugetExePath = ".\nuget.exe"
Write-Output "Downloading nuget.exe from nuget.org"
Invoke-WebRequest "https://nuget.org/nuget.exe" -OutFile $root\Nuget.exe
Write-Output "Creating Nuget Package"
& $root\Nuget.exe pack $root\Nuget\specs.nuspec
