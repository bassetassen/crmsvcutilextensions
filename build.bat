@echo off
cls

if not exist tools\nuget.exe (
	echo Downloading 'NuGet'
	mkdir tools
    PowerShell -NoProfile -ExecutionPolicy Bypass -Command "& '.\download-nuget.ps1'"
)

"tools\nuget.exe" "Install" "FAKE" -OutputDirectory "packages" -ExcludeVersion
"packages\FAKE\tools\Fake.exe" build.fsx