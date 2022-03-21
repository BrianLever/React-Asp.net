@echo off
set mdbuildpath=%ProgramFiles(x86)%\Microsoft Visual Studio\2017\Professional\MSBuild\15.0\Bin\
set scriptPath=%~dp0
set versionfilename=.\version.txt


echo Publishing Installation Package...

set /p assembly_version=< version.txt

set sourcetDir=%scriptPath%..\publish\%assembly_version%
set sqlDriveInstallerPath=%scriptPath%..\!!!Required Components\SSCERuntime_x64-ENU.exe
set targetDir=D:\projects\Karsten\Project_Installation\Screendox\%assembly_version%\
set backupPublishDir=\\192.168.100.101\Files\ScreenDox\Publish\%assembly_version%\

echo Version: %assembly_version%
echo Source: %sourcetDir%
echo Target Location: %targetDir%
echo Backup Location: %backupPublishDir%
echo SQL Compact Path: %sqlDriveInstallerPath%




REM "%mdbuildpath%msbuild.exe" ..\FrontDeskServer\website.publishproj /p:DeployOnBuild=true /t:Build /p:Configuration=Release /p:Platform=AnyCPU /p:PublishProfile=ToFolder

@echo Copy Web server installation


echo Copy to directory [%targetDir%]
echo Copy to directory [%backupPublishDir%]

mkdir %targetDir%
mkdir %backupPublishDir%

xcopy "%sourcetDir%\*.zip" "%targetDir%"  /Q /Y
xcopy "%sourcetDir%\*.zip" "%backupPublishDir%" /Q /Y


echo Copy Kiosk Installations...
xcopy "%sourcetDir%\kiosk\*.*" "%targetDir%kiosk\"  /Q /Y
xcopy "%sourcetDir%\kiosk-package\*.*" "%targetDir%kiosk-package\"  /Q /Y

xcopy "%sqlDriveInstallerPath%" "%targetDir%kiosk\"  /Q /Y
xcopy "%sourcetDir%\kiosk\*.msi" "%backupPublishDir%kiosk\"  /Q /Y
xcopy "%sourcetDir%\kiosk\*.exe" "%backupPublishDir%kiosk\"  /Q /Y


