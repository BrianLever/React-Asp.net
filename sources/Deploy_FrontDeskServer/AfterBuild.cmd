@echo off
rem	ScreenDox Server
rem !!! NOTE this file is different from License Management Tool: other set of website assemblies.
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


cd %1
cd bin
@echo %CD%

rem include eazfuscator
set EazExePath=..\..\..\packages\eazfuscator.net.3.3\tools\Eazfuscator.NET.exe
@echo Current directory: %CD%
echo Checking obfuscator location: %EazExePath%
%EazExePath% /? >nul 2>nul
if %errorlevel%==0 goto continue3
echo Eazfuscator.NET.exe not found

exit 2
:continue3


rem === Merge assemblies


set targetassembly=%2.dll

set otherassemblies=^
BouncyCastle-1.5-3DESonly.dll ^
FrontDesk.dll ^
FrontDesk.Server.dll ^
FrontDesk.Licensing.dll ^
itextsharp.dll ^
Bootstrapper.dll ^
Bootstrapper.AutoMapper.dll ^
AutoMapper.dll 

rem exclude from otherassemblies: AjaxControlToolkit.dll, fails to correctly merge resources

echo ILMerge: merging assemblies...
ilmerge /ndebug /internalize:..\..\ExcludeFromInternalize.txt /allowDup /copyattrs /allowMultiple ^
/keyfile:%KEYFILE% ^
/out:%targetassembly% %targetassembly% %otherassemblies% ^
/targetplatform:"v4,C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0"

if %errorlevel%==0 goto continue4
echo ilmerge failed to merge assemblies
pause
exit 3
:continue4

del %otherassemblies%

rem === Obfuscate
echo Eazfuscator.NET.exe --key-file %KEYFILE% %targetassembly%
rem %EazExePath%  --key-file %KEYFILE% %targetassembly%

cd ..\..

:end
set PATH=%OLDPATH%