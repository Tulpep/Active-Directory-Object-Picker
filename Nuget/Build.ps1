$root = (split-path -parent $MyInvocation.MyCommand.Definition) + '\..'
Write-Output "Downloading nuget.exe from nuget.org"
Invoke-WebRequest "https://nuget.org/nuget.exe" -OutFile $root\Nuget.exe


$version = [System.Reflection.Assembly]::LoadFile("$root\CubicOrange.Windows.Forms.ActiveDirectory\bin\Release\CubicOrange.Windows.Forms.ActiveDirectory.dll").GetName().Version
$versionStr = "{0}.{1}.{2}" -f ($version.Major, $version.Minor, $version.Build)
Write-Host "Setting .nuspec version tag to $versionStr"
$content = (Get-Content $root\NuGet\MarkdownLog.nuspec)
$content = $content -replace '\$version\$',$versionStr

$content | Out-File $root\nuget\MarkdownLog.compiled.nuspec


Write-Output "Creating Nuget Package"
& $root\Nuget.exe pack $root\Nuget\specs.nuspec
