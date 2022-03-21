# Performs post build file processing and replaces ../asserts/... links to relative ./asserts/

	
# Enable -Verbose option
[CmdletBinding()]
param
(
	[Parameter(Mandatory=$true)]
    [string]
	$RootFolder
)
# Regular expression pattern to find the version in the build number 
# and then apply it to the assemblies
$assetUrlSourcePattern= "(.*)(\.\.\/\/)+assets\/(.*)"
	
	
function Fix-AssetUrl
{
    param(
    [string]$FilePath
    )
    
    Write-Host "Transforming file:  $FilePath"

    (Get-Content $FilePath) | ForEach-Object{
        $_ -replace "../assets/", "./assets/"
        
    } | Set-Content $FilePath

}

Write-Host "Root folder: $RootFolder"
	
$sourceFolder = $RootFolder
	

# Apply the version to the assembly property files
$files = gci $sourceFolder -recurse -include *.js  

if($files)
{
	Write-Host "Will apply transformation to $($files.count) files."
	
	foreach ($file in $files) {
		
        attrib $file -r
		
		Fix-AssetUrl -FilePath $file.FullName
        
		Write-Host "$file - transformation applied"
	}
}
else
{
	Write-Warning "Found no files."
}