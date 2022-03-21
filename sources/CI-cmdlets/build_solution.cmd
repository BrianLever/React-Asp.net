@echo off
set mdbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\
set scriptPath=%~dp0
set versionfilename=.\version.txt
set targetDir=%scriptPath%..\publish\%assembly_version%

set /p assembly_version=< %versionfilename%

echo "Building and Publishing Server..."
echo "Version: %assembly_version%"

echo "Deleting folder %targetDir%..."

rmdir /S /Q "%targetDir%"
mkdir "%targetDir%\sql_upgrage"

"%mdbuildpath%msbuild.exe" ..\FrontDesk-vs2013.sln /t:Build /p:Configuration=Release /p:Platform="Mixed Platforms"
