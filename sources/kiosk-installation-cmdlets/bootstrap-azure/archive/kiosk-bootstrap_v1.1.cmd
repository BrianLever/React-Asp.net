@echo off

set nwpwd=%1
echo Admin pass: %nwpwd%

set /p ver=< .\version.txt
set targetPath=C:\ScreenDox_Installation_files\release-%ver%
set kioskUserPwd=%2
set programFilesPath=%programfiles(x86)%
set nwpath=\\tsclient\D\projects\Karsten\Project_Installation\ScreenDox\

echo Saving Kiosk user passwford to file
echo|set /p="%kioskUserPwd%">kioskpwd.txt


net use %nwpath% %nwpwd%  /user:kioskinstallation

@echo Creating directory %targetPath%
mkdir %targetPath%



@echo Copiying files to local directory %targetPath%

mkdir %targetPath% 
xcopy "%nwpath%\%ver%\kiosk" %targetPath% /S /I /G /R /Y /Z /J

@echo File copy completed

@echo Installing SQL Driver...
call %targetPath%\SSCERuntime_x64-ENU.exe

@echo Drive installation completed

@echo installing Kiosk...
%targetPath%\setup.exe SERVER_URL=https://jlwa.3sicorp.com:8443/frontdeskscreener"


@echo Kioks installation completed


@echo address: https://jlwa.3sicorp.com:8443/frontdeskscreener

@echo Creating Kiosk user account
net user kioskuser %kioskUserPwd% /ADD /Y /FULLNAME:"ScreenDox application account" /ACTIVE:YES 

WMIC USERACCOUNT WHERE "Name='kioskuser'" SET PasswordExpires=FALSE
net localgroup Administrators kioskuser /add
NET LOCALGROUP "Remote Desktop Users" kioskuser /ADD

ICACLS "%programFilesPath%\ScreenDox\Logs" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant Users:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\app\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\app\Data" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %username%:M /T

ICACLS "%PROGRAMDATA%\ScreenDox\" /grant kioskuser:M /T
ICACLS "%PROGRAMDATA%\ScreenDox\" /grant %username%:M /T




@copy Copy config file...
xcopy "%programFilesPath%\ScreenDox\ScreenDoxKiosk.exe.config"  %USERPROFILE%\Desktop /S /G /R /Y /Z /J


@echo Review and update configuration file %USERPROFILE%\Desktop\ScreenDoxKiosk.exe.config
set /p=Change Kiosk ID and then press ENTER to continue...


xcopy  %USERPROFILE%\Desktop\ScreenDoxKiosk.exe.config "%programFilesPath%\ScreenDox\ScreenDoxKiosk.exe.config" /S /G /R /Y /Z /J

echo Kioskuser's password copied to Buffer
echo|set /p="%kioskUserPwd%"|clip

runas /user:kioskuser "powershell -c start-process -FilePath \"'%programFilesPath%\\ScreenDox\\ScreenDoxKioskUILocker.exe'\" -verb runAs"




REM net localgroup Administrators kioskuser /delete
