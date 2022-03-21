@echo off
set kioskuser=cnhsa\sdkiosk

set /p kioskUserPwd=< .\kioskpwd.txt
echo Password: %kioskUserPwd%

set programFilesPath=%programfiles(x86)%

echo Saving Kiosk user passwford to file
echo|set /p="%kioskUserPwd%">kioskpwd.txt


echo Kioskuser's password copied to Buffer
echo|set /p="%kioskUserPwd%"|clip

net localgroup Administrators %kioskuser% /add


runas /user:%kioskuser% "powershell -c start-process -FilePath \"'%programFilesPath%\\ScreenDox\\ScreenDoxKioskUILocker.exe'\" -verb runAs"

echo Removing %kioskuser% from Administrators
net localgroup Administrators %kioskuser% /delete
