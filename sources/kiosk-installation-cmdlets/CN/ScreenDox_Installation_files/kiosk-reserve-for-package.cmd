@echo off

set ScreenDoxServerDns=%1
set programFilesPath=%programfiles(x86)%


@echo Target server name/ %ScreenDoxServerDns%


@echo Copying kiosk app config to Desktop...
xcopy "%programFilesPath%\ScreenDox\app\ScreenDoxKiosk.exe.config"  %USERPROFILE%\Desktop /S /G /R /Y /Z /J

@copy Copying configuration file...
xcopy "%programFilesPath%\ScreenDox\Launcher\data\configuration.yaml"  %USERPROFILE%\Desktop /S /G /R /Y /Z /J

@echo Replacing "screendox.3sicorp.com" to %ScreenDoxServerDns%


powershell Set-ExecutionPolicy RemoteSigned
powershell .\ReplaceScreenDoxAddress.ps1 -TargetScreenDoxAppUri %ScreenDoxServerDns%


@copy Copying configuration files back to installation folder...

xcopy  %USERPROFILE%\Desktop\configuration.yaml "%programFilesPath%\ScreenDox\Launcher\data\configuration.yaml" /S /G /R /Y /Z /J
xcopy  %USERPROFILE%\Desktop\ScreenDoxKiosk.exe.config "%programFilesPath%\ScreenDox\app\ScreenDoxKiosk.exe.config" /S /G /R /Y /Z /J



@echo Granting Permissions...

ICACLS "%programFilesPath%\ScreenDox\Logs" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant Users:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\app" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\app" /grant %username%:M /T


ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %username%:M /T




