@echo off


set ver=6.3.3.0
set targetPath=C:\ScreenDox_Installation_files\release-%ver%
set kioskUserPwd=%2
set programFilesPath=%programfiles(x86)%
set nwpath=\\192.168.1.11\installation

echo Uninstalling Screendox...

%targetPath%\setup.exe /uninst


@echo Kioks unistalled

