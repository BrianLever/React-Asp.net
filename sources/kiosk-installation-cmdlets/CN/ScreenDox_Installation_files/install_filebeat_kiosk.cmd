@echo off

REM reading kiosk id from the file
set /p kiosk_key=< .\kiosk_key.txt

echo Installing Filebeat for kiosk %kiosk_key%

powershell .\install-filebeat.ps1 -KioskId %kiosk_key%  -ScreenDoxIpAddress "172.17.78.203"
