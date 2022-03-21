[CmdletBinding()]
param(
# Assembly version in format 1.0.0.0
[Parameter(Mandatory=$true)]
[System.Version]$AssemblyVersion,
# Build ID in format of 1
[Parameter(Mandatory=$true)]
[System.Int32]$Revision
)

$CopyrightString = "Copyright © 2021, J.L. Ward Associates, Inc. All rights reserved."


function Format-AssemblyVersion
{
    param(
        [Parameter(Mandatory=$true)]
        [System.Version]$AssemblyVersion,
        [Parameter(Mandatory=$true)]
        [int]$Revision
    )

    return [System.Version]::new($AssemblyVersion.Major, $AssemblyVersion.Minor, $AssemblyVersion.Build, $Revision);
}


function Replace-AssemblyInfoString
{
    param(
    [string]$FilePath,
    [Parameter(Mandatory=$true)]
    [string]$AssemblyVersion,
    [Parameter(Mandatory=$true)]
    [string]$FileVersion

    )
    
    $patternAssembly = '(.*)\[assembly: AssemblyVersion\("(.*)"\)\]'
    $patternFile = '(.*)\[assembly: AssemblyFileVersion\("(.*)"\)\]'

    (Get-Content $path) | ForEach-Object{
        if($_ -match $patternAssembly){
            # We have found the matching line
            # Edit the version number and put back.
            
            '{1}[assembly: AssemblyVersion("{0}")]' -f $AssemblyVersion, $Matches[1]
        } 
        elseif($_ -match $patternFile){
            
            '{1}[assembly: AssemblyFileVersion("{0}")]' -f $FileVersion, $Matches[1]
        } 
        
        else {
            # Output line as is
            $_
        }
    } | Set-Content $path

}

function Replace-NetStandardAssemblyInfoString
{
    param(
    [string]$FilePath,
    [Parameter(Mandatory=$true)]
    [string]$AssemblyVersion,
    [Parameter(Mandatory=$true)]
    [string]$FileVersion

    )
    
    $patternAssembly = '(.*)<AssemblyVersion>(.*)<\/AssemblyVersion>'
    $patternFile = '(.*)<FileVersion>(.*)<\/FileVersion>'
    $patternVersion = '(.*)<Version>(.*)"<\/Version>'

    (Get-Content $path) | ForEach-Object{
        if($_ -match $patternAssembly){
            # We have found the matching line
            # Edit the version number and put back.
            
            '{1}<AssemblyVersion>{0}</AssemblyVersion>' -f $AssemblyVersion, $Matches[1]
        } 
        elseif($_ -match $patternFile){
            
            '{1}<FileVersion>{0}</FileVersion>' -f $FileVersion, $Matches[1]
        } 
        elseif($_ -match $patternVersion){
            
            '{1}<Version>{0}</Version>' -f $FileVersion, $Matches[1]
        } 
        
        else {
            # Output line as is
            $_
        }
    } | Set-Content $path

}


function Replace-CopyrightString
{
    param(
    [string]$FilePath,
    [Parameter(Mandatory=$true)]
    [string]$Copyright

    )
    
    $patternCopyrightXml = '(.*)<Copyright>(.*)<\/Copyright>'
    $patternCopyrightCsharp = '(.*)\[assembly: AssemblyCopyright\("(.*)"\)\]'

    (Get-Content $path) | ForEach-Object{
        if($_ -match $patternCopyrightXml){
            # We have found the matching line
            # Edit the version number and put back.
            
            '{1}<Copyright>{0}</Copyright>' -f $Copyright, $Matches[1]
        } 
        elseif($_ -match $patternCopyrightCsharp){
            
            '{1}[assembly: AssemblyCopyright("{0}")]' -f $Copyright, $Matches[1]
        } 
        else {
            # Output line as is
            $_
        }
    } | Set-Content $path

}

# format assembly version

$AssemblyVersion = Format-AssemblyVersion -AssemblyVersion $AssemblyVersion -Revision $Revision

[System.Version]$AssemblyFileVersion = $AssemblyVersion

Write-Host "Assembly version: $AssemblyVersion"

if($AssemblyVersion -eq ([System.Version]::new(0,0,0,0)) )
{
    Write-Error "Invalid assembly version: $AssemblyVersion"
    
    break Script
}


$excludeFilter =  "*BouncyCastle-1.5*"

Get-ChildItem .\.. -Recurse -Exclude $excludeFilter | where {$_.Directory -notlike $excludeFilter} | ForEach-Object {


    $path = $_.FullName;

    if($path -like '*AssemblyInfo.cs')
    {
         Replace-AssemblyInfoString -FilePath $path -AssemblyVersion $AssemblyVersion -FileVersion $AssemblyFileVersion

         Replace-CopyrightString -FilePath $path -Copyright $CopyrightString

         Write-Host "Modified file: $path." 

    }
    elseif($path -like '*.csproj')
    {
        # Process .NET standard  properties
        Replace-NetStandardAssemblyInfoString -FilePath $path -AssemblyVersion $AssemblyVersion -FileVersion $AssemblyFileVersion

        Replace-CopyrightString -FilePath $path -Copyright $CopyrightString

        Write-Host "Modified file: $path." 

    }


}