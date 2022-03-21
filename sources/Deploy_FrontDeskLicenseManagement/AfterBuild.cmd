@echo off
rem	ScreenDox License Management Tools
rem !!! NOTE this file is different from FD Server: other set of website assemblies.
rem 
rem %1 == $(Configuration)
rem %2 == $(SingleAssemblyName)
rem

set KEYFILE=..\..\..\FrontDesk.snk

rem === Check for required tools and add them to path

set OLDPATH=%PATH%

rem include ilmerge
set PATH=C:\Program Files\Microsoft\ILMerge;%PATH%
set PATH=C:\Program Files (x86)\Microsoft\ILMerge;%PATH%

ilmerge /? >nul
if %errorlevel%==0 goto continue2
echo ilmerge not found
pause
exit 1
:continue2

rem include eazfuscator
set PATH=C:\Program Files\Eazfuscator.NET;%PATH%
Eazfuscator.NET.exe /? >nul 2>nul
if %errorlevel%==0 goto continue3
echo Eazfuscator.NET.exe not found
pause
exit 2
:continue3


rem === Merge assemblies

cd %1
cd bin

set targetassembly=%2.dll

set otherassemblies=^
BouncyCastle-1.5-3DESonly.dll ^
FrontDesk.dll ^
FrontDesk.Licensing.dll ^
FrontDesk.Server.dll ^
FrontDesk.Server.Licensing.Management.dll ^
itextsharp.dll

rem exclude from otherassemblies: AjaxControlToolkit.dll, fails to correctly merge resources:
rem "Binary format of the specified custom attribute was invalid"

echo ILMerge: merge assemblies...
ilmerge /ndebug /internalize /allowDup /copyattrs /allowMultiple ^
/keyfile:%KEYFILE% ^
/out:%targetassembly% %targetassembly% %otherassemblies%
del %otherassemblies%

rem === Obfuscate
Eazfuscator.NET.exe --key-file %KEYFILE% %targetassembly%

cd ..\..

:end
set PATH=%OLDPATH%