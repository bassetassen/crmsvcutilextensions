$source = "http://nuget.org/nuget.exe"
$destination = ".\tools\nuget.exe"

$wc = New-Object System.Net.WebClient
$wc.DownloadFile($source, $destination)