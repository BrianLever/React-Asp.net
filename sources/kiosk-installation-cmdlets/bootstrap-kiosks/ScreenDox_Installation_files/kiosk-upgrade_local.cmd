@echo off


set /p ver=< .\version.txt
set targetPath=C:\ScreenDox_Installation_files\release-%ver%
set kioskUserPwd=%1
set programFilesPath=%programfiles(x86)%
set nwpath=\\tsclient\D\projects\Karsten\Project_Installation\FrontDesk\

echo Installing Version %ver%
echo Saving Kiosk user passwford to file
echo|set /p="%kioskUserPwd%">kioskpwd.txt


@echo Using directory %targetPath%

@echo File copy completed


@echo installing Kiosk...
%targetPath%\setup.exe SERVER_URL=https://ehr.rsbcihi.org/FrontDeskScreener


@echo Kioks installation completed

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