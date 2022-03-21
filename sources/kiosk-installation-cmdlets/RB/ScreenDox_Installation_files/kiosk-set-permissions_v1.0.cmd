@echo off

set programFilesPath=%programfiles(x86)%
set kioskuser=kioskuser


ICACLS "%programFilesPath%\ScreenDox\Logs" /grant %kioskuser%:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant Users:M /T
ICACLS "%programFilesPath%\ScreenDox\Logs" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\app" /grant %kioskuser%:M /T
ICACLS "%programFilesPath%\ScreenDox\app" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\app\Data" /grant %kioskuser%:M /T
ICACLS "%programFilesPath%\ScreenDox\app\Data" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %kioskuser%:M /T
ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %username%:M /T

ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %kioskuser%:M /T
ICACLS "%programFilesPath%\ScreenDox\Launcher\Data" /grant %username%:M /T

ICACLS "%PROGRAMDATA%\ScreenDox" /grant %kioskuser%:M /T
ICACLS "%PROGRAMDATA%\ScreenDox" /grant %username%:M /T