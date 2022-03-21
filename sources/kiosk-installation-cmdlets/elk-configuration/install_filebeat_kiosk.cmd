@echo off

REM reading kiosk id from the file
set /p kiosk_key=< .\kiosk_key.txt

echo Installing Filebeat for kiosk %kiosk_key%

