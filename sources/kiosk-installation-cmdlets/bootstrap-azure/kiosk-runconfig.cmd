@echo off


set kioskUserPwd=%1
set programFilesPath=%programfiles(x86)%

echo Kioskuser's password copied to Buffer
echo|set /p="%kioskUserPwd%"|clip

runas /user:kioskuser "powershell -c start-process -FilePath \"'%programFilesPath%\\ScreenDox\\ScreenDoxKioskUILocker.exe'\" -verb runAs"


