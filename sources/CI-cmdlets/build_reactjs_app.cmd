@echo off
REM This script is designed to be executed from Azure DevOps Agent
REM it prepare publish artifacts for screendox.ui.app

set scriptPath=%~dp0
set versionfilename=%scriptPath%version.txt

set solutionDir=%scriptPath%..
set appDir=%solutionDir%\screen.dox.ui.app
set publishDir=%appDir%\build


set /p assembly_version=< %versionfilename%


echo "Script path: %scriptPath%."
echo "Version file path: %versionfilename%."
echo "App dir: %appDir%"
echo "Publish target dir: %publishDir%."

echo "Building and Publishing Screendox UI App..."
echo "Version: %assembly_version%"

cd %appDir%

REM echo ****************************************************************************
REM echo "NPM Install..."
REM echo ****************************************************************************
REM call npm install -f

echo ****************************************************************************
echo "Build React App..."
echo ****************************************************************************


call npm run build

cd %scriptPath%

echo ****************************************************************************
echo "Applying fix for assets urls..."
echo ****************************************************************************
powershell %scriptPath%\Fix-AssetUrls.ps1 -RootFolder "%publishDir%"


echo ****************************************************************************
echo "Copy web.config file..."
echo ****************************************************************************

echo Copy to directory [%publishDir%]

copy  /v /y "%appDir%\web.config" "%publishDir%\web.config"


echo ****************************************************************************
echo "Completed Build React App..."
echo ****************************************************************************
