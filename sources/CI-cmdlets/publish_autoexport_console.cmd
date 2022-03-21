@echo off
set mdbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\
set scriptPath=%~dp0
set versionfilename=.\version.txt
set targetDir=%scriptPath%..\publish

set /p assembly_version=< version.txt

echo "Building and Publishing RPMS Interfance and Smart Export Services..."
echo "Version: %assembly_version%"



echo ****************************************************************************
echo "Building SmartExport Console App..."
echo ****************************************************************************

set sourceSolutionPath=..\ScreenDox.Rpms.SmartExport.Console

"%mdbuildpath%msbuild.exe" %sourceSolutionPath%\ScreenDox.Rpms.SmartExport.Console.csproj /p:DeployOnBuild=false /t:Build /p:Configuration=Release /p:Platform=AnyCPU

set serverTargetDir=%targetDir%\%assembly_version%\SmartExportConsole

echo Copy to directory [%serverTargetDir%]
rmdir /S /Q "%serverTargetDir%"
mkdir %serverTargetDir%

xcopy "%sourceSolutionPath%\bin\Release" "%targetDir%\SmartExportConsole" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt

xcopy "%targetDir%\SmartExportConsole" "%serverTargetDir%" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt

echo Archiving [%serverTargetDir%]...
set archivePath="%targetDir%\%assembly_version%\SmartExportConsole-%assembly_version%.zip"

del %archivePath% /Q 

powershell Compress-Archive -Path "%serverTargetDir%" -DestinationPath %archivePath%


