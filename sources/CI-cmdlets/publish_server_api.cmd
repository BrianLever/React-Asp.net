@echo off
set msbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2019\Professional\MSBuild\Current\Bin\
set scriptPath=%~dp0
set versionfilename=.\version.txt
set targetDir=%scriptPath%..\publish
set solutionDir=%scriptPath%..

set /p assembly_version=< %versionfilename%

echo "Building and Publishing Screendox Server API..."
echo "Version: %assembly_version%"



echo ****************************************************************************
echo "Building and Publishing Screendox Server API..."
echo ****************************************************************************

"%msbuildpath%msbuild.exe" %solutionDir%\ScreenDox.Server.Api\ScreenDox.Server.Api.csproj /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /t:Build /p:Configuration=Release /p:Platform=AnyCPU

set serverTargetDir=%targetDir%\%assembly_version%\ScreendoxServerApi


echo ****************************************************************************
echo "Building Tests Files..."
echo ****************************************************************************

"%msbuildpath%msbuild.exe" %solutionDir%\ScreenDox.Server.Api.Tests\ScreenDox.Server.Api.Tests.csproj /t:Restore;Build /p:Configuration=Release /p:Platform=AnyCPU
"%msbuildpath%msbuild.exe" %solutionDir%\FrontDEsk.Server.Tests\FrontDEsk.Server.Tests.csproj /t:Restore;Build /p:Configuration=Release /p:Platform=AnyCPU
"%msbuildpath%msbuild.exe" %solutionDir%\FrontDEsk.Common.Tests\FrontDEsk.Common.Tests.csproj /t:Restore;Build /p:Configuration=Release /p:Platform=AnyCPU

echo "Building Tests Files completed."



echo Copy to directory [%serverTargetDir%]
rmdir /S /Q "%serverTargetDir%"
mkdir %serverTargetDir%
xcopy "%targetDir%\ScreendoxServerApi" "%serverTargetDir%" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt

echo Archiving [%serverTargetDir%]...
set archivePath="%targetDir%\%assembly_version%\ScreendoxServerApi-%assembly_version%.zip"

del %archivePath% /Q 

powershell Compress-Archive -Path "%serverTargetDir%" -DestinationPath %archivePath%