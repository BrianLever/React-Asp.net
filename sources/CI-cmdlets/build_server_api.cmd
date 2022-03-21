@echo off
REM This script is designed to be executed from Azure DevOps Agent
REM Use publish_server_api.cmd for manual execution

set localmsbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\
set agentmsbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Enterprise\MSBuild\Current\Bin\

set scriptPath=%~dp0
set versionfilename=%scriptPath%version.txt
set publishDir=%~1


set solutionDir=%scriptPath%..
set slnPublishTargetDir=%solutionDir%\publish


set /p assembly_version=< %versionfilename%

echo "Script path: %scriptPath%."
echo "Version file path: %versionfilename%."
echo "Publish target dir: %publishDir%."

echo "Building and Publishing Screendox Server API..."
echo "Version: %assembly_version%"


REM find path to msbuild

IF exist "%localmsbuildpath%" set msbuildpath=%localmsbuildpath%
IF exist "%agentmsbuildpath%" set msbuildpath=%agentmsbuildpath%

echo "MS Build Path: %msbuildpath%"


echo ****************************************************************************
echo "Building and Publishing Screendox Server API..."
echo ****************************************************************************

"%msbuildpath%msbuild.exe" %solutionDir%\ScreenDox.Server.Api\ScreenDox.Server.Api.csproj /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /t:Restore;Build /p:Configuration=Release /p:Platform=AnyCPU

echo "Build completed."

echo ****************************************************************************
echo "Building Tests Files..."
echo ****************************************************************************

"%msbuildpath%msbuild.exe" %solutionDir%\ScreenDox.Server.Api.Test\ScreenDox.Server.Api.Tests.csproj /t:Restore;Build /p:Configuration=Release /p:Platform=AnyCPU
"%msbuildpath%msbuild.exe" %solutionDir%\FrontDEsk.Server.Tests\FrontDEsk.Server.Tests.csproj /t:Restore;Build /p:Configuration=Release /p:Platform=AnyCPU
"%msbuildpath%msbuild.exe" %solutionDir%\FrontDEsk.Common.Tests\FrontDEsk.Common.Tests.csproj /t:Restore;Build /p:Configuration=Release /p:Platform=AnyCPU

echo "Building Tests Files completed."


dir %slnPublishTargetDir%
dir %slnPublishTargetDir%\ScreendoxServerApi


set serverTargetDir=%publishDir%\ScreendoxServerApi

echo Copy to directory [%serverTargetDir%]

if exist "%serverTargetDir%" rmdir /S /Q "%serverTargetDir%"
mkdir %serverTargetDir%


xcopy "%slnPublishTargetDir%\ScreendoxServerApi" "%serverTargetDir%" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt