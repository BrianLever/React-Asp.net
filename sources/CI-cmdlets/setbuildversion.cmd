@echo off
REM This script is designed to be executed from CI pipeline.
REM Script reads branch version from version.txt file in format of 11.00.00.00. 
REM The last segment is replaced with the build_revision parameter.
REM Example: setbuildversion.cmd 1.0.0.0 1

set build_revision=%1
set commit_changes=%2
set scriptPath=%~dp0
set versionfilename=.\version.txt


cd %scriptPath%


set /p assembly_version=< %versionfilename%


if NOT defined assembly_version goto ERROR_VERSION
if NOT defined build_revision goto ERROR

echo Setting version [%assembly_version%] and revision [%build_revision%].


powershell .\Set-AssemblyVersion.ps1 -AssemblyVersion %assembly_version% -Revision %build_revision%

if "%commit_changes%" NEQ "1"  goto :ENDFILE

git add ../*
git commit -m "Changed version to %assembly_version% with revision %build_revision%." 



goto ENDFILE

:ERROR_VERSION

@echo File version.txt is not found or does not have the version.

:ERROR

@echo BuildRevision parameter not found.
@echo Use following format: setbuildversion.cmd BuildRevision


:ENDFILE

@echo on