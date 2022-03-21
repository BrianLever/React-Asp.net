param(
    $ScriptRootDir = (Resolve-Path .\).Path
)


function Get-Action (
    [string]$DbName,
    [string]$BackupType
)
{
    $command = "$ScriptRootDir\screendox_database_backup.ps1 -DatabaseName $DbName -BackupType $BackupType"
    $arg="-executionpolicy bypass -noprofile -NonInteractive -NoLogo -WindowStyle Hidden -file $command"
    
    Write-Host "Action command: $arg"
    return  New-ScheduledTaskAction -Execute 'Powershell.exe' -Argument $arg
}

function Create-Job(
    [string]$DbName
)
{

    $triggerLog =  New-ScheduledTaskTrigger -Daily -At 12am
    $triggerDiff =  New-ScheduledTaskTrigger -Daily -At 1am -DaysInterval 1
    $triggerFull =  New-ScheduledTaskTrigger -Weekly -DaysOfWeek Sunday -At 3am

    $principal =  (New-ScheduledTaskPrincipal -UserId "NT AUTHORITY\System" -LogonType S4U -RunLevel Limited -Id "Author")

    Write-Host "Creating Jobs for $DbName database ..."
    $action= (Get-Action -DbName $DbName -BackupType "Log")
    
    Unregister-ScheduledTask -TaskName "SQL DB Log Backup - $DbName" -Confirm -ErrorAction "Ignore" 
    Unregister-ScheduledTask -TaskName "SQL DB Diff Backup - $DbName" -Confirm -ErrorAction "Ignore" 
    Unregister-ScheduledTask -TaskName "SQL DB Full Backup - $DbName" -Confirm -ErrorAction "Ignore"

    $task = Register-ScheduledTask -Action $action -Trigger $triggerLog -TaskName "SQL DB Log Backup - $DbName" -Principal $principal 
    $task.Triggers.repetition.Duration = 'PT24H'
    $task.Triggers.repetition.Interval = 'PT02H'
    $task | Set-ScheduledTask
    
 
    $action= (Get-Action -DbName $DbName -BackupType "Incremental")

    Register-ScheduledTask -Action $action -Trigger $triggerDiff -TaskName "SQL DB Diff Backup - $DbName" -Principal $principal

    $action= (Get-Action -DbName $DbName -BackupType "Full")
    Register-ScheduledTask -Action $action -Trigger $triggerFull -TaskName "SQL DB Full Backup - $DbName" -Principal $principal

    Write-Host "Jobs for $DbName database created."
}


Create-Job -DbName "ScreenDox"

Create-Job -DbName "master"

