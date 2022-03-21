@echo off
set mdbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\
set scriptPath=%~dp0
set versionfilename=.\version.txt
set targetDir=%scriptPath%..\publish

set /p assembly_version=< version.txt

echo "Building and Publishing RPMS Interfance, Kiosk Installation API and Smart Export Services..."
echo "Version: %assembly_version%"



echo ****************************************************************************
echo "Building Kiosk Installation API..."
echo ****************************************************************************

"%mdbuildpath%msbuild.exe" ..\ScreenDoxKioskInstallApi\ScreenDoxKioskInstallApi.csproj /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /t:Build /p:Configuration=Release /p:Platform=AnyCPU

set serverTargetDir=%targetDir%\%assembly_version%\ScreenDoxKioskInstallApi

echo Copy to directory [%serverTargetDir%]
rmdir /S /Q "%serverTargetDir%"
mkdir %serverTargetDir%
xcopy "%targetDir%\ScreenDoxKioskInstallApi" "%serverTargetDir%" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt

echo Archiving [%serverTargetDir%]...
set archivePath="%targetDir%\%assembly_version%\ScreenDoxKioskInstallApi-%assembly_version%.zip"

del %archivePath% /Q 

powershell Compress-Archive -Path "%serverTargetDir%" -DestinationPath %archivePath%

echo ****************************************************************************
echo "Building RPMS Interface..."
echo ****************************************************************************

"%mdbuildpath%msbuild.exe" ..\FrontDeskRpmsService\ScreenDoxEhrService.csproj /p:DeployOnBuild=true /p:PublishProfile=LocalFolder /t:Build /p:Configuration=Release /p:Platform=AnyCPU /p:SolutionDir=%scriptPath%..\

set serverTargetDir=%targetDir%\%assembly_version%\ScreenDoxEhrService

echo Copy to directory [%serverTargetDir%]
rmdir /S /Q "%serverTargetDir%"
mkdir %serverTargetDir%
xcopy "%targetDir%\ScreenDoxEhrService" "%serverTargetDir%" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt

echo Archiving [%serverTargetDir%]...
set archivePath="%targetDir%\%assembly_version%\ScreenDoxEhrService-%assembly_version%.zip"

del %archivePath% /Q 

start powershell Compress-Archive -Path "%serverTargetDir%" -DestinationPath %archivePath%





echo ****************************************************************************
echo "Building SmartExport Service..."
echo ****************************************************************************

"%mdbuildpath%msbuild.exe" ..\SmartExport\ScreenDox.Rpms.SmartExport.App\ScreenDox.Rpms.SmartExport.App.csproj /p:DeployOnBuild=true /p:PublishProfile=FolderProfile /t:Build /p:Configuration=Release /p:Platform=AnyCPU

set serverTargetDir=%targetDir%\%assembly_version%\SmartExportApp

echo Copy to directory [%serverTargetDir%]
rmdir /S /Q "%serverTargetDir%"
mkdir %serverTargetDir%
xcopy "%targetDir%\SmartExportApp" "%serverTargetDir%" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt

echo Archiving [%serverTargetDir%]...
set archivePath="%targetDir%\%assembly_version%\SmartExportApp-%assembly_version%.zip"

del %archivePath% /Q 

powershell Compress-Archive -Path "%serverTargetDir%" -DestinationPath %archivePath%
