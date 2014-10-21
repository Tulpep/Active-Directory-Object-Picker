$nugetExePath = ".\nuget.exe"
Write-Output "Downloading nuget.exe from nuget.org"
Invoke-WebRequest "https://nuget.org/nuget.exe" -OutFile $nugetExePath
Write-Output "Creating Nuget Package"
& $nugetExePath  "pack specs.nuspec"
pwd
