@echo off
set mdbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\
set scriptPath=%~dp0
set versionfilename=.\version.txt
set targetDir=%scriptPath%..\publish

set /p assembly_version=< version.txt

echo "Building and Publishing Server..."
echo "Version: %assembly_version%"


"%mdbuildpath%msbuild.exe" ..\FrontDeskServer\website.publishproj /p:DeployOnBuild=true /t:Build /p:Configuration=Release /p:Platform=AnyCPU /p:AssemblyFileVersion=%assembly_version%  /p:AssemblyVersion=%assembly_version% /p:PublishProfile=ToFolder

set serverTargetDir=%targetDir%\%assembly_version%\webserver

echo Copy to directory [%serverTargetDir%]
rmdir /S /Q "%serverTargetDir%"
mkdir %serverTargetDir%
xcopy "%targetDir%\server" "%serverTargetDir%" /S /E /I /Q /Y /EXCLUDE:%scriptPath%publish_exclude.txt

echo Archiving [%serverTargetDir%]...
set archivePath="%targetDir%\%assembly_version%\webserver-%assembly_version%.zip"

del %archivePath% /Q 

start powershell Compress-Archive -Path "%serverTargetDir%" -DestinationPath %archivePath%


echo Copy SQL incremental scripts...

set sqlSourcePath=%scriptPath%..\FrontDeskDatabase\Change Scripts
set sqlTargetPath=%targetDir%\%assembly_version%\sql_upgrage

del %sqlTargetPath% /Q 
mkdir %sqlTargetPath%

echo Source folder for SQL scripts: "%sqlSourcePath%"

xcopy "%sqlSourcePath%" "%sqlTargetPath%" /Y 

set sqlArchFile="%sqlTargetPath%-%assembly_version%.zip"

del "%sqlTargetPath%-%assembly_version%.zip" /Q
powershell Compress-Archive -Path "%sqlTargetPath%" -DestinationPath "%sqlTargetPath%-%assembly_version%.zip"
