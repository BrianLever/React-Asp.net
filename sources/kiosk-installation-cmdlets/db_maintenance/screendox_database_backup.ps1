<#
.SYNOPSIS
    Execute sql database backup action.

.DESCRIPTION
    A longer description of your script.

.REMARKS
    Create Diff database backup
     .\screendox_database_backup.ps1 -DatabaseName "ScreenDox" -BackupType Incremental 

    Create Full backup
    .\screendox_database_backup.ps1 -DatabaseName "ScreenDox" -BackupType Full

    Backup transaction log 
    .\screendox_database_backup.ps1 -DatabaseName "ScreenDox" -BackupType Log

#>
[CmdletBinding()]
param(
    [Parameter(mandatory=$false)]
    [string] 
    $ServerInstance = ".", 

    [Parameter(mandatory=$true)]
    [string] 
    $DatabaseName, 

    [ValidateSet("Full","Log", "Incremental")] 
    [string]
    $BackupType = "Full",

    [string]
    $BackupDir = "d:\SQLDATA\ScreenDox_Backup\$DatabaseName"

)


mkdir $BackupDir -Force
$BackupSetName = $DatabaseName + "_" + $BackupType
$RetainDays = 120
$Incremental=$false
$BackupAction = "Database";

if($BackupType -Contains "Log")
{
    $RetainDays = 30
    $Incremental=$true
    $BackupAction = "Log"

}
elseif($BackupType -Contains "Incremental")
{
    $RetainDays = 60
    $Incremental=$true
}

$BackupFile = "$BackupDir\$BackupSetName.bak" 

Write-Host "Backup database $DatabaseName."
Write-Host "Backup type: $BackupType."
Write-Host "Expired on: $RetainDays days."
Write-Host "Incremental: $Incremental."
Write-Host "Target file: $BackupFile."


Backup-SqlDatabase -ServerInstance $ServerInstance -Database $DatabaseName -BackupAction $BackupAction -Incremental:$Incremental -BackupSetName ($BackupSetName)  -RetainDays $RetainDays -BackupFile $BackupFile 

Write-Host "Backup completed."