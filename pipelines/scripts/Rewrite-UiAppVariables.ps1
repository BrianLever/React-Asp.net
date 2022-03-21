# This scripts overides optimized react-script file and 
#  replaces API and virtual folder environment variables.
#
# For example, 
#   .\Rewrite-UiAppVariables.ps1 -SourcePath -ApiUrl "https://screendox-dev.3sicorp.com/ScreendoxServerApi_Dev/api" -VirtualFolderName "/screendox-ui-dev"

	
# Enable -Verbose option
[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [string] $SourcePath,

    [Parameter(Mandatory=$true)]
    [string] $ApiUrl,

    [Parameter(Mandatory=$true)]
    [string] $VirtualFolderName
    
)

Write-Host "Api Url: $ApiUrl"
Write-Host "Virtual Folder Name: $VirtualFolderName"
Write-Host "Source path: $SourcePath"


# Regular expression to replace environment configurations
$apiUrlPattern = "https?:\/\/screendox-dev\.3sicorp\.com\/ScreendoxServerApi_Dev\/api"
$virtualFolderPattern = "\/screendox-ui-dev"
	
function Set-Values
{
    param(
        [string] $FilePath
    )
    
    Write-Host "Transforming file:  $FilePath"

    (Get-Content  $FilePath) -replace $apiUrlPattern, $ApiUrl `
        -replace $virtualFolderPattern, $VirtualFolderName |
        Set-Content $FilePath

}

function Set-BaseTag
{
    param(
        [string] $FilePath
    )
    
    Write-Host "Base Tag. Transforming file:  $FilePath"

    $baseTagSource = "<base\s+href=""[^""]*""\/>"
    $baseTagOutput = "<base href=""$VirtualFolderName/"" />"
    

    (Get-Content  $FilePath) -replace $baseTagSource, $baseTagOutput |
        Set-Content $FilePath

}



Write-Output "Step 1: Replace asset references in js files."
$files = gci $SourcePath -recurse -include *.chunk.js 

if($files)
{
	Write-Host "Will apply transformation to $($files.count) files."
	
	foreach ($file in $files) {
		
        attrib $file -r
		
        Set-Values -FilePath $file.FullName

		Write-Host "$file - transformation applied"
	}
}
else
{
	Write-Warning "Found no files."
}

Write-Output "Step 2: Replacing Base URL tag in index.html"

# Apply the version to the assembly property files
$files = gci $SourcePath index.htm*

if($files)
{
	Write-Host "Will apply Base Tag change transformation to $($files.count) files."
	
	foreach ($file in $files) {
		
        attrib $file.FullName -r
		
        Set-BaseTag -FilePath $file.FullName
        
		Write-Host "$($file.FullName) - transformation applied"
	}
}
else
{
	Write-Warning "Found no files."
}