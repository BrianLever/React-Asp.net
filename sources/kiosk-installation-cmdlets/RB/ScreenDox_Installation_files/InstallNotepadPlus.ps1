function Install-7zipNpp ($source = ($env:TEMP + "\SW"))
{    
    If (!(Test-Path -Path $source -PathType Container)) {New-Item -Path $source -ItemType Directory | Out-Null}
    
    $packages = @(
    @{title='7zip Extractor';url='https://sourceforge.net/projects/sevenzip/files/7-Zip/17.01/7z1701-x64.msi';Arguments=' /qn';Destination=$source},
    @{title='Notepad++ 6.7.5';url='http://download.tuxfamily.org/notepadplus/archive/6.7.5/npp.6.7.5.Installer.exe';Arguments=' /Q /S';Destination=$source}
    )
    
    foreach ($package in $packages) {
            $packageName = $package.title
            $fileName = Split-Path $package.url -Leaf
            $destinationPath = $package.Destination + "\" + $fileName
    
    If (!(Test-Path -Path $destinationPath -PathType Leaf)) {
    
        Write-Host "Downloading $packageName"
        $webClient = New-Object System.Net.WebClient
        $webClient.DownloadFile($package.url,$destinationPath)
        }
        }
    
    foreach ($package in $packages) {
        $packageName = $package.title
        $fileName = Split-Path $package.url -Leaf
        $destinationPath = $package.Destination + "\" + $fileName
        $Arguments = $package.Arguments
        Write-Output "Installing $packageName"
    
    
    Invoke-Expression -Command "$destinationPath $Arguments"
    }
}
Install-7zipNpp