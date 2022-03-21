param(
    $InstallElasticsearch = $false,

    $InstallKibana = $false,
    
    $InstallLogstash = $false,

    $InstallFilebeats = $false
)

$currentPath = Get-Location

$env:ES_HOME = "C:\Program Files\Elastic\Elasticsearch"
$env:ES_DATA = "C:\Program Files\Elastic\Elasticsearch\Data"
$env:ELK_HOME = "C:\Program Files\Elastic"
$env:ELOG_HOME = "C:\elastic\logstash"
$env:Path += ";$env:ES_HOME\jdk\bin;"
$env:JAVA_HOME = "$env:ES_HOME\jdk\"

$elasticsearchName = "elasticsearch-7.7.0"
$kibanaName = "kibana-7.7.0-windows-x86_64"
$filebeatName = "filebeat-7.7.0-windows-x86"
$logstashName = "logstash-7.7.0"


<#
    Replace IP address in elasticsearch.yml and kibana.yml
#>

if($InstallElasticsearch)
{

    if(-not (Test-Path $env:ES_HOME))
    {
        Write-Host "Creating folder in program files.."
        # create folder and copy files
        New-Item -Path $env:ES_HOME -ItemType Directory -ErrorAction Stop | Out-Null #-Force
    }


    Write-Host "Copying elastic search files to the destination.."
    Copy-Item -Path .\$elasticsearchName\* -Destination "$env:ES_HOME" -Recurse -Force

    Write-Host "Changing directory."
    cd $env:ES_HOME

    .\bin\elasticsearch-setup-passwords.bat auto

    .\bin\elasticsearch-service.bat install

    .\bin\elasticsearch-service.bat start


    Write-Host "Configure firewall"
    netsh advfirewall firewall add rule name="Elasticsearch TCP Port 9200, 9300" dir=in action=allow protocol=TCP localport=9200,9300


    Write-Host "Set JAVA_HOME"

    if( -not $env:JAVA_HOME  )
    {
        [Environment]::SetEnvironmentVariable("JAVA_HOME", "$env:ES_HOME\jdk", "Machine")
    }
}

cd $currentPath


if( $InstallKibana )
{
    Write-Host Installing Kibana...

    if(-not (Test-Path $env:ELK_HOME\kibana))
    {
        Write-Host "Creating folder in program files..."
        # create folder and copy files
        New-Item -Path $env:ELK_HOME\kibana -ItemType Directory -ErrorAction Stop | Out-Null #-Force
    }

    Write-Host "Copying kibana files to the destination.."
    Copy-Item -Path .\$kibanaName\* -Destination "$env:ELK_HOME\Kibana" -Recurse -Force

    if( -not (Test-Path "$env:ELK_HOME\nssm.exe"))
    {
        Copy-Item -Path nssm.exe -Destination "$env:ELK_HOME" -Force
    }

    . "$env:ELK_HOME\nssm.exe" install "Kibana" "$env:ELK_HOME\Kibana\bin\kibana.bat" 

    . "$env:ELK_HOME\nssm.exe" set "Kibana" DependOnService "elasticsearch-service-x64"

 
    cd "$env:ELK_HOME\Kibana"

    .\bin\kibana.bat

    . "$env:ELK_HOME\nssm.exe" start "Kibana"

    
    Write-Host "Configure firewall"
    netsh advfirewall firewall add rule name="Elasticsearch Kibana TCP Port 8802" dir=in action=allow protocol=TCP localport=8802

}

cd $currentPath

if( $InstallLogstash)
{
    Write-Host Installing Log Stash...

    if(-not (Test-Path $env:ELOG_HOME))
    {
        Write-Host "Creating folder in program files..."
        # create folder and copy files
        New-Item -Path $env:ELOG_HOME -ItemType Directory -ErrorAction Stop | Out-Null #-Force
    }

    Write-Host "Copying Logstash files to the destination.."
    Copy-Item -Path .\$logstashName\* -Destination "$env:ELOG_HOME" -Recurse -Force

    Write-Host "Creating lostash win service..."

     if( -not (Test-Path "$env:ELK_HOME\nssm.exe"))
    {
        Copy-Item -Path nssm.exe -Destination "$env:ELK_HOME" -Force
    }

   . "$env:ELK_HOME\nssm.exe" install "Logstash" "$env:ELOG_HOME\logstash-start.bat"
   . "$env:ELK_HOME\nssm.exe" set "Logstash" DependOnService "elasticsearch-service-x64"
    
    
    cd "$env:ELOG_HOME"

    .\logstash-start.bat


  
    Write-Host "Configure firewall"
    netsh advfirewall firewall add rule name="Elasticsearch Loststach TCP Port 9600" dir=in action=allow protocol=TCP localport=9600
    netsh advfirewall firewall add rule name="Elasticsearch Loststach TCP Port 8804" dir=in action=allow protocol=TCP localport=8804
	
	
	 . "$env:ELK_HOME\nssm.exe" start "Logstash"

}

cd $currentPath

if($InstallFilebeats)
{
    Write-Host Installing Filebeat...

    if(-not (Test-Path $env:ELK_HOME\filebeat))
    {
        Write-Host "Creating folder in program files..."
        # create folder and copy filesNew-NewName 
        New-Item -Path $env:ELK_HOME\filebeat -ItemType Directory -ErrorAction Stop | Out-Null #-Force
    }

    Write-Host "Copying Filebeat files to the destination..."
    Copy-Item -Path .\$filebeatName\* -Destination "$env:ELK_HOME\Filebeat" -Recurse -Force

    Write-Host "Enabling IIS Logs" 

    
    cd $env:ELK_HOME\filebeat

    Rename-Item -Path .\modules.d\iis.yml.disabled -NewName iis.yml

    .\install-service-filebeat.ps1
}