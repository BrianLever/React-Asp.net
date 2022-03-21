param(
    
)


function Get-Action (
    [string]$ServiceUrl
)
{
    $command = "Invoke-WebRequest $ServiceUrl"
    $arg="-executionpolicy bypass -noprofile -NonInteractive -NoLogo -WindowStyle Hidden -command $command"
    
    Write-Host "Action command: $arg"
    return  New-ScheduledTaskAction -Execute 'Powershell.exe' -Argument $arg
}

function Create-Job(
    [string]$ServiceUrl
)
{

    $trigger =  New-ScheduledTaskTrigger -Daily -At 1:01am -DaysInterval 1
   
    $principal =  (New-ScheduledTaskPrincipal -UserId "NT AUTHORITY\System" -LogonType S4U -RunLevel Limited -Id "Author")

    Write-Host "Creating Jobs for starting Auto-export job ..."
    $action= (Get-Action -ServiceUrl $ServiceUrl)
    
    Unregister-ScheduledTask -TaskName "Warm up ScreenDox Auto Export: $ServiceUrl" -Confirm -ErrorAction "Ignore" 

    $task = Register-ScheduledTask -Action $action -Trigger $trigger -TaskName "Warm up ScreenDox Auto Export: $ServiceUrl" -Principal $principal 
    $task.Triggers.repetition.Duration = 'PT24H'
    $task.Triggers.repetition.Interval = 'PT02H'
    $task | Set-ScheduledTask
    
 
    
    Write-Host "Jobs for Warming up ScreenDox Auto Export service is created."
}


Create-Job -ServiceUrl "https://localhost/ScreenDoxSmartExport/jobs"

