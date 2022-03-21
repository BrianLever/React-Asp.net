param(
    [string]
    $IpAddress = "192.168.100.115",
    $elasticSearchPath = "c:\Program Files\Elastic\Elasticsearch",
    $targetCertDirectory = "c:\elastic\certs",
    $pathToOpenSslExe = "C:\Program Files\OpenSSL-Win64\bin\openssl.exe"
)

if(-not (Test-Path $targetCertDirectory -PathType Container))
{
    mkdir $targetCertDirectory - | Out-Null
}
else{
    Remove-Item $targetCertDirectory\*.* -Recurse
}

Write-Host "Remember current path."
$currentPath = Get-Location

cd $elasticSearchPath

Write-Host "Creating Root CA certificate..."

bin/elasticsearch-certutil ca -out "$targetCertDirectory\ca-bundle.zip" -pem --days 20000
Expand-Archive "$targetCertDirectory\ca-bundle.zip" -DestinationPath "$targetCertDirectory"
Copy-Item -Path "$targetCertDirectory\ca\*.*" -Destination "$targetCertDirectory"

Remove-Item -Path "$targetCertDirectory\ca" -Recurse -Force
Remove-Item -Path "$targetCertDirectory\ca-bundle.zip" -Force



Write-Host "Creating Server certificate..."

bin/elasticsearch-certutil cert --ca-cert "$targetCertDirectory\ca.crt" -ca-key "$targetCertDirectory\ca.key" -out "$targetCertDirectory\server-bundle.zip" -pem -ip $IpAddress,127.0.0.1 --days 20000

Expand-Archive "$targetCertDirectory\server-bundle.zip" -DestinationPath "$targetCertDirectory"
Copy-Item -Path "$targetCertDirectory\instance\*.*" -Destination "$targetCertDirectory"

Rename-Item -Path "$targetCertDirectory\instance.crt" -NewName "server.crt"
Rename-Item -Path "$targetCertDirectory\instance.key" -NewName "server.key"

Remove-Item -Path "$targetCertDirectory\instance" -Recurse -Force
Remove-Item -Path "$targetCertDirectory\server-bundle.zip" -Force



Write-Host "Creating Client certificate..."

bin/elasticsearch-certutil cert --ca-cert "$targetCertDirectory\ca.crt" -ca-key "$targetCertDirectory\ca.key" -out "$targetCertDirectory\client-bundle.zip" -pem -ip $IpAddress,127.0.0.1 --days 20000

Expand-Archive "$targetCertDirectory\client-bundle.zip" -DestinationPath "$targetCertDirectory"
Copy-Item -Path "$targetCertDirectory\instance\*.*" -Destination "$targetCertDirectory"

Rename-Item -Path "$targetCertDirectory\instance.crt" -NewName "client.crt"
Rename-Item -Path "$targetCertDirectory\instance.key" -NewName "client.key"

Remove-Item -Path "$targetCertDirectory\instance" -Recurse -Force
Remove-Item -Path "$targetCertDirectory\client-bundle.zip" -Force


Write-Host "Creating pkcs8 certificates..."

. "$pathToOpenSslExe" pkcs8 -in "$targetCertDirectory\server.key" -topk8 -out "$targetCertDirectory\server-pkcs8.key" -nocrypt
. "$pathToOpenSslExe" pkcs8 -in "$targetCertDirectory\client.key" -topk8 -out "$targetCertDirectory\client-pkcs8.key" -nocrypt
