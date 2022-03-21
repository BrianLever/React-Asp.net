[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string]
    $KioskId = "",
    [string]
    $ScreenDoxIpAddress = "52.156.104.145"
)

function Create-Directory {

    param(
        [string]$Path
    )

    if(-not (Test-Path $path))
    {
        Write-Host "Creating directory [$path]..."
          
        New-Item -Path $path -ItemType Directory -ErrorAction Stop | Out-Null #-Force
    }

}

function Replace-StringInFile
{
    param(
    [string]$FilePath,
    [Parameter(Mandatory=$true)]
    [string]$OldValue,
    [Parameter(Mandatory=$true)]
    [string]$NewValue

)

if( [string]::IsNullOrWhiteSpace($KioskId)) { Throw “You must supply a value for -KioskID” }

    
$pattern = '(.*)' + $OldValue + '(.*)'


(Get-Content $FilePath) | ForEach-Object{
    if($_ -match $pattern){
        
        '{0}{1}{2}' -f $Matches[1], $NewValue, $Matches[2]    
    } 
    else {
        # Output line as is
        $_
    }
} | Set-Content $FilePath

}


$currentPath = Get-Location

$env:ELOG_HOME = "C:\elastic"
$env:Path += ";$env:ES_HOME\jdk\bin;"

$filebeatName = "filebeat-7.7.0"



$targetRootPath = "C:\elastic"
$targetFilebeatPath = "$targetRootPath\filebeat"

Write-Host Creating target folders ...
 
Create-Directory -Path $targetRootPath
Create-Directory -Path "$targetRootPath\certs"
Create-Directory -Path $targetFilebeatPath


Write-Host Installing Filebeat...

Write-Host "1. Copying certificates..."

Copy-Item -Path .\certs\* -Destination "$targetRootPath\certs" -Recurse -Force


Write-Host "2. Copying Filebeat files to the destination..."

Copy-Item -Path .\$filebeatName\* -Destination "$targetFilebeatPath" -Recurse -Force


Write-Host "3. Configuring Filebeat..." 


Replace-StringInFile -FilePath "$targetFilebeatPath\filebeat.yml" -OldValue "KIOSK-KEY-XXXX" -NewValue $KioskId
Replace-StringInFile -FilePath "$targetFilebeatPath\filebeat.yml" -OldValue "SCREENDOX-SERVER-IP" -NewValue $ScreenDoxIpAddress



Write-Host "3. Install service Filebeat..." 
. "$targetFilebeatPath\install-service-filebeat.ps1"



Write-Host "3. Start filebeat service..." 
Start-Service "filebeat"


