@echo off

set /p ver=< .\version.txt
set /p key=< .\kiosk_key.txt

set targetPath=C:\ScreenDox_Installation_files\release-%ver%
set kioskUserPwd=%1
set programFilesPath=%programfiles(x86)%
set nwpath=\\tsclient\D\projects\Karsten\Project_Installation\Screendox\

echo Saving Kiosk user passwford to file
echo|set /p="%kioskUserPwd%">kioskpwd.txt


@echo Kiosk Key: %key%

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
%targetPath%\setup.exe


@echo Kioks installation completed


@echo Creating Kiosk user account
net user kioskuser %kioskUserPwd% /ADD /Y /FULLNAME:"ScreenDox application account" /ACTIVE:YES 

WMIC USERACCOUNT WHERE "Name='kioskuser'" SET PasswordExpires=FALSE
net localgroup Administrators kioskuser /add
NET LOCALGROUP "Remote Desktop Users" kioskuser /ADD

ICACLS "%programFilesPath%\ScreenDox\Logs" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant Users:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\app" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\app" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\app\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\app\Data" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %username%:M /T

ICACLS "%PROGRAMDATA%\ScreenDox" /grant kioskuser:M /T
ICACLS "%PROGRAMDATA%\ScreenDox" /grant %username%:M /T




@copy Copying configuration file...
xcopy "%programFilesPath%\ScreenDox\Launcher\data\configuration.yaml"  %USERPROFILE%\Desktop /S /G /R /Y /Z /J


@echo Review and update configuration file %USERPROFILE%\Desktop\configuration.yaml
set /p=Change Kiosk ID and then press ENTER to continue...


xcopy  %USERPROFILE%\Desktop\configuration.yaml "%programFilesPath%\ScreenDox\Launcher\data\configuration.yaml" /S /G /R /Y /Z /J


echo Kioskuser's password copied to Buffer
echo|set /p="%kioskUserPwd%"|clip

net localgroup Administrators kioskuser /add


runas /user:kioskuser "powershell -c start-process -FilePath \"'%programFilesPath%\\ScreenDox\\ScreenDoxKioskUILocker.exe'\" -verb runAs"

echo Removing kioskuser from Administrators
net localgroup Administrators kioskuser /delete
