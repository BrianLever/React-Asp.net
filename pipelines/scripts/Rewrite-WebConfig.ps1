# If found use it to version the assemblies.
#
# For example, if the 'Build number format' build pipeline parameter 
# $(BuildDefinitionName)_$(Year:yyyy).$(Month).$(DayOfMonth)$(Rev:.r)
# then your build numbers come out like this:
# "Build HelloWorld_2013.07.19.1"
# This script would then apply version 2013.07.19.1 to your assemblies.
	
# Enable -Verbose option
[CmdletBinding()]
	
# Regular expression pattern to find the version in the build number 
# and then apply it to the assemblies
$VariableSubstitutePattern= "(.*?)\{\{([A-Za-z_]+)\}\}(.*)"
$VariableExtendedSubstitutePattern= "(.*)\{\{([A-Za-z_]+)\}\}(.*)"
	
	
function Set-Variables
{
    param(
    [string]$FilePath
    )
    
    Write-Host "Transforming file:  $FilePath"

    (Get-Content $FilePath) | ForEach-Object{

        $replaced = $false;

        if($_ -match $VariableSubstitutePattern){
            # We have found the matching line
            # Edit the variable  number and put back.
           
            $varName = $Matches[2];

            Write-Host "Found variable: $varName"

            $varValue = ('$env:'+"$($varName)" | Invoke-Expression) 

            if (-not $varValue)
            {
                Write-Error ("$varName pipeline variable is not defined.")
	            exit 1
            }

            Write-Host "Found variable value. $varName : $varValue"
            
            '{1}{0}{2}' -f  $varValue, $Matches[1], $Matches[3]

            $replaced = $true;
        
        } 
        if($_ -match $VariableExtendedSubstitutePattern){ #replace second variable in a row
            # We have found the matching line
            # Edit the variable  number and put back.
           
            $varName = $Matches[2];

            Write-Host "Found variable: $varName"

            $varValue = ('$env:'+"$($varName)" | Invoke-Expression) 

            if (-not $varValue)
            {
                Write-Error ("$varName pipeline variable is not defined.")
	            exit 1
            }

            Write-Host "Found variable value. $varName : $varValue"
            
            '{1}{0}{2}' -f  $varValue, $Matches[1], $Matches[3]

            $replaced = $true;
        } 
        if( -not $replaced) {
            # Output line as is
            $_
        }
    } | Set-Content $FilePath

}

# Make sure path to source code directory is available
if (-not $Env:SYSTEM_DEFAULTWORKINGDIRECTORY)
{
	Write-Error ("SYSTEM_DEFAULTWORKINGDIRECTORY environment variable is missing.")
	exit 1
}

Write-Host "SYSTEM_DEFAULTWORKINGDIRECTORY: $Env:SYSTEM_DEFAULTWORKINGDIRECTORY"
	
$sourceFolder = $Env:SYSTEM_DEFAULTWORKINGDIRECTORY
	

# Apply the version to the assembly property files
$files = gci $sourceFolder -recurse -include web_setup.config  

if($files)
{
	Write-Host "Will apply transformation to $($files.count) files."
	
	foreach ($file in $files) {
		
        attrib $file -r
		
        Set-Variables -FilePath $file.FullName
        
		Write-Host "$file - transformation applied"
      
        # rename to web.config-
        $NewName = $file.Name -replace "_setup",''
        $Destination = Join-Path -Path $file.Directory.FullName -ChildPath $NewName
        Move-Item -Path $file.FullName -Destination $Destination -Force

        Write-Host "$Destination - file generation completed"
	}
}
else
{
	Write-Warning "Found no files."
}