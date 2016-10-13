$source = "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe"
$destination = ".\tools\nuget.exe"

$wc = New-Object System.Net.WebClient
$wc.DownloadFile($source, $destination)
