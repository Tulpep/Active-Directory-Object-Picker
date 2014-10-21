$nugetExePath = ".\nuget.exe"
Invoke-WebRequest "https://nuget.org/nuget.exe" -OutFile $nugetExePath
Start-Process -FilePath $nugetExePath  -ArgumentList "pack specs.nuspec"
