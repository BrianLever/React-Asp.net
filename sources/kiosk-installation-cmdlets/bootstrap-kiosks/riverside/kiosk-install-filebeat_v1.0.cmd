@echo off

set nwpwd=%1
echo Admin pass: %nwpwd%

set /p key=< .\kiosk_key.txt

set targetPath=C:\ScreenDox_Installation_files

set nwpath=\\192.168.1.11\c$\Screendox_Installer

echo Kiosk Key: %key%


@echo Connecting to network share
net use %nwpath% %nwpwd%  /user:ehr\kgearhardt

@echo Copiying files to local directory %targetPath%

xcopy "%nwpath%\Kiosk\filebeat-distrib" %targetPath% /S /I /G /R /Y /Z /J

@echo File copy completed


@echo installing Kiosk...
%targetPath%\setup.exe

@echo Installing FileBeat
powershell Set-ExecutionPolicy RemoteSigned

install_filebeat_kiosk.cmd
