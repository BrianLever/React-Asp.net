set ver=6.3.1.0
set targetPath=C:\ScreenDox_installation_files\release-%ver%
set kioskUserPwd=Fr01ntD3sk31.!
set programFilesPath=%programfiles(x86)%
net use \\ehr.rsbcihi.org\c$ *  /user:ehr\kgearhardt

@echo Copiying files to local directory %targetPath%

mkdir %targetPath% 
xcopy "\\ehr.rsbcihi.org\c$\FrontDesk Installer\%ver%\kiosk" %targetPath% /S /I /G /R /Y /Z /J

set /p=Press ENTER to continue...

@echo Installing SQL Driver...
call %targetPath%\SSCERuntime_x64-ENU.exe

set /p=Press ENTER to continue...


@echo installing Kiosk...
%targetPath%\setup.exe 

set /p=Press ENTER to continue...


@echo address: https://ehr.rsbcihi.org/FrontDeskScreener

@echo Creating Kiosk user account
net user kioskuser %kioskUserPwd% /ADD /FULLNAME:"ScreenDox application account" /ACTIVE:YES 

WMIC USERACCOUNT WHERE "Name='kioskuser'" SET PasswordExpires=FALSE
net localgroup Administrators kioskuser /add
NET LOCALGROUP "Remote Desktop Users" kioskuser /ADD

ICACLS "%programFilesPath%\FrontDesk\Logs" /grant kioskuser:M /T
ICACLS "%programFilesPath%\FrontDesk\Logs" /grant Users:M /T
ICACLS "%programFilesPath%\FrontDesk\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\FrontDesk\Data" /grant seepoint:M /T

@copy Copy config file...
xcopy "%programFilesPath%\FrontDesk\FrontDeskKiosk.exe.config"  %USERPROFILE%\Desktop\FrontDeskKiosk.config /S /G /R /Y /Z /J

REM  <security mode="none">  <security mode="Transport"><transport clientCredentialType="None" />

REM powershell -Command (Get-Content c:\Users\seepoint\Desktop\FrontDeskKiosk.config) | ForEach-Object { $_ -replace "<security mode=`"None`">", "<security mode=`"Transport`"><transport clientCredentialType=`"None`" />" } | Set-Content c:\Users\seepoint\Desktop\FrontDeskKiosk.exe.config

@echo Review and update configuration file %USERPROFILE%\Desktop\FrontDeskKiosk.exe.config
set /p=Change Kiosk ID and then press ENTER to continue...

 

xcopy  %USERPROFILE%\Desktop\FrontDeskKiosk.exe.config "%programFilesPath%\FrontDesk\FrontDeskKiosk.exe.config" /S /G /R /Y /Z /J


net localgroup Administrators kioskuser /delete