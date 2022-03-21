@echo off

@echo Disabling auto-rotation...

REG ADD HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\AutoRotation /v Enable /t REG_DWORD /d 00000000 /f



@echo Configuring Screen Resolution
powershell Set-ExecutionPolicy RemoteSigned

powershell .\set-dpi.ps1

