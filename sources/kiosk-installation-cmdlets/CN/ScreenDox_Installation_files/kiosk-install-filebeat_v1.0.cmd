@echo off

set nwpwd=%1
echo Admin pass: %nwpwd%

set /p key=< .\kiosk_key.txt

set targetPath=C:\ScreenDox_Installation_files

set nwpath=\\192.168.1.11\c$\Screendox_Installer

echo Kiosk Key: %key%

@echo Installing FileBeat
powershell Set-ExecutionPolicy RemoteSigned

install_filebeat_kiosk.cmd
