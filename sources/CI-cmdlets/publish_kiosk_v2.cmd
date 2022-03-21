@echo off
set mdbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\
set devenvpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\Common7\IDE\
set scriptPath=%~dp0
set versionfilename=.\version.txt
set targetDir=%scriptPath%..\publish


set /p assembly_version=< version.txt


echo Building Kiosk App ...

echo ****************************************************************************
echo Version: %assembly_version%
echo ****************************************************************************

set sourceBinDir=%scriptPath%..\FrontDeskKiosk\bin\x86\Release
set copyTargetBinDir=%targetDir%\%assembly_version%\kiosk\bin
set zipTargetDir=%targetDir%\%assembly_version%\kiosk-package

"%mdbuildpath%msbuild.exe" ..\FrontDeskKiosk\ScreenDoxKiosk.csproj -verbosity:m ^
/t:Clean /t:Rebuild /p:Configuration=Release /p:Platform="x86" ^
/p:OutDir="%sourceBinDir%" /p:OutputPath="%sourceBinDir%" /p:SolutionDir=%scriptPath%..\ 



echo Package Output files...


echo Copy to directory [%copyTargetBinDir%]
rmdir /S /Q "%copyTargetBinDir%"
mkdir %copyTargetBinDir%

xcopy "%sourceBinDir%" "%copyTargetBinDir%" /S /E /I /Q /Y
echo Removing ScreenDoxKiosk.exe.config
del "%copyTargetBinDir%\ScreenDoxKiosk.exe.config" /F /Q


echo Archiving [%copyTargetBinDir%]...
set archivePath="%zipTargetDir%\ScreenDoxKiosk-package-%assembly_version%.zip"

del "%archivePath%" /Q /F 
mkdir "%zipTargetDir%" /Q
start powershell Compress-Archive -Path "%copyTargetBinDir%\*" -DestinationPath %archivePath%


echo ****************************************************************************
echo Building MSI installation file...
echo ****************************************************************************
"%devenvpath%devenv.exe" "%scriptPath%..\FrontDesk-vs2013.sln" /build Release /Project "%scriptPath%..\Installations\FrontDeskKioskSetup1\FrontDeskKioskSetup1.vdproj" /projectconfig Release /Out "%scriptPath%..\Installations\FrontDeskKioskSetup1\output.txt"


set sourceDir=%scriptPath%..\Installations\FrontDeskKioskSetup1\Release
set copyTargetDir=%targetDir%\%assembly_version%\kiosk

echo Copy to directory [%copyTargetDir%]

xcopy "%sourceDir%" "%copyTargetDir%" /S /E /I /Q /Y
