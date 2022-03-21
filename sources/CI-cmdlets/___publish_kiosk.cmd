@echo off
set mdbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\
set devenvpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\IDE\
set scriptPath=%~dp0
set versionfilename=.\version.txt
set targetDir=%scriptPath%..\publish

set /p assembly_version=< version.txt

echo Building Kiosk Installer ...
echo Version: %assembly_version%
echo ****************************************************************************
echo ****************************************************************************

"%mdbuildpath%msbuild.exe" ..\FrontDeskKiosk\ScreenDoxKiosk.csproj /t:Clean /t:Rebuild /p:Configuration=Release /p:Platform=AnyCPU /p:SolutionDir=%scriptPath%..\

echo Building installation files...
"%devenvpath%devenv.exe" "%scriptPath%..\FrontDesk-vs2013.sln" /build Release /Project "%scriptPath%..\Installations\FrontDeskKioskSetup1\FrontDeskKioskSetup1.vdproj" /projectconfig Release /Out "%scriptPath%..\Installations\FrontDeskKioskSetup1\output.txt"


set sourceDir=%scriptPath%..\Installations\FrontDeskKioskSetup1\Release
set copyTargetDir=%targetDir%\%assembly_version%\kiosk

echo Copy to directory [%copyTargetDir%]
rmdir /S /Q "%copyTargetDir%"
mkdir %copyTargetDir%
xcopy "%sourceDir%" "%copyTargetDir%" /S /E /I /Q /Y


echo Archiving [%copyTargetDir%]...
set archivePath="%copyTargetDir%\ScreenDoxKiosk-%assembly_version%.zip"

del "%archivePath%" /Q /F 

start powershell Compress-Archive -Path "%copyTargetDir%" -DestinationPath %archivePath%
