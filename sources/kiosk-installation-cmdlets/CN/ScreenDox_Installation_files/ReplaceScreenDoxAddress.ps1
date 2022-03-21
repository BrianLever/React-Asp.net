[CmdletBinding()]
param(
# Screendox server DNS name and application name, for example: screendox.3sicorp.com/screendox
[Parameter(Mandatory=$true)]
[String]$TargetScreenDoxAppUri,

[Parameter(Mandatory=$false)]
[String]$SourceScreenDoxAppUri = "screendox.3sicorp.com"

)

$pathDesktop =  [Environment]::GetFolderPath("Desktop");



function Rename-StringInFile
{
    param(
    [string]$FilePath,
    [Parameter(Mandatory=$true)]
    [string]$Source,
    [Parameter(Mandatory=$true)]
    [string]$Target

    )
    
    $patternAssembly = "(.*)($Source)(.*)"

    (Get-Content $path) | ForEach-Object{
        if($_ -match $patternAssembly){
            # We have found the matching line
            # Edit the version number and put back.
            
            '{1}{0}{2}' -f $Target, $Matches[1], $Matches[3]
        } 
      
        else {
            # Output line as is
            $_
        }
    } | Set-Content $path

}


Get-ChildItem $pathDesktop -Recurse | ForEach-Object {


    $path = $_.FullName;

    if($path -like '*configuration.yaml' -or  $path -like '*ScreenDoxKiosk.exe.config')
    {
        Rename-StringInFile -FilePath $path -Source $SourceScreenDoxAppUri -Target $TargetScreenDoxAppUri

         Write-Host "Modified file: $path " 

    }
}