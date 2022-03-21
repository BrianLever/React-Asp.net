param(
    $InstallElasticsearch = $false,

    $InstallKibana = $false,
    
    $InstallLogstash = $false,

    $InstallFilebeats = $false
)

$currentPath = Get-Location

$env:IIS_SCREENDOX_HOME = "C:\inetpub\wwwroot\screendox"



<# 1. Get the latest version and create a new copy. #>