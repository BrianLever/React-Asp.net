@echo off

set nwpwd=%1
echo Admin pass: %nwpwd%

set ver=7.0.8.1
set targetPath=C:\ScreenDox_Installation_files\release-%ver%
set kioskUserPwd=%2
set programFilesPath=%programfiles(x86)%
set nwpath=\\192.168.1.11\c$\Screendox_Installer

echo Saving Kiosk user passwford to file
echo|set /p="%kioskUserPwd%">kioskpwd.txt


net use %nwpath% %nwpwd%  /user:ehr\kgearhardt

@echo Creating directory %targetPath%
mkdir %targetPath%



@echo Copiying files to local directory %targetPath%

mkdir %targetPath% 
xcopy "%nwpath%\%ver%\kiosk" %targetPath% /S /I /G /R /Y /Z /J

@echo File copy completed


@echo installing Kiosk...
%targetPath%\setup.exe SERVER_URL=https://ehr.rsbcihi.org/FrontDeskScreener


@echo Kioks installation completed


@echo address: https://ehr.rsbcihi.org/FrontDeskScreener


ICACLS "%programFilesPath%\FrontDesk\Logs" /grant kioskuser:M /T
ICACLS "%programFilesPath%\FrontDesk\Logs" /grant Users:M /T
ICACLS "%programFilesPath%\FrontDesk\Logs" /grant %username%:M /T

ICACLS "%programFilesPath%\FrontDesk\Data" /grant kioskuser:M /T
ICACLS "%programFilesPath%\FrontDesk\Data" /grant %username%:M /T


@copy Copy config file...
xcopy "%programFilesPath%\FrontDesk\FrontDeskKiosk.exe.config"  %USERPROFILE%\Desktop /S /G /R /Y /Z /J


@echo Review and update configuration file %USERPROFILE%\Desktop\FrontDeskKiosk.exe.config
set /p=Change Kiosk ID and then press ENTER to continue...


xcopy  %USERPROFILE%\Desktop\FrontDeskKiosk.exe.config "%programFilesPath%\FrontDesk\FrontDeskKiosk.exe.config" /S /G /R /Y /Z /J