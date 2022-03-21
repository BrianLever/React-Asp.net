'
'   Fix for WCF service in precompiled non-updatable website
'   http://connect.microsoft.com/VisualStudio/feedback/details/367241/precompiled-web-deployment-projects-fail-when-deploying-wcf-service-in-aspnet-website
'
'   Arguments
'   0 == $(Configuration)


if WScript.Arguments.Count = 0 then
    WScript.Echo "Argument required: Release or Debug."
    WScript.Quit(1)    
end if

dim fileName, searchFor, replaceTo
fileName = "kioskendpoint.svc."  ' may be only part of actual file name: searched as substring.
searchFor = "customString=""/FrontDeskServer/endpoint"
replaceTo = "customString=""~/endpoint"

Dim pattern4RemovalArr(10)

pattern4RemovalArr(0) = "|App_global.asax, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
pattern4RemovalArr(1) = "|App_GlobalResources, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
pattern4RemovalArr(2) = "|App_Code, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
pattern4RemovalArr(3) = "|App_WebReferences, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null"
pattern4RemovalArr(4) = "|BouncyCastle-1.5-3DESonly, Version=1.5.0.0, Culture=neutral, PublicKeyToken=bde4cfb7f85a7a02"
pattern4RemovalArr(5) = "|FrontDesk, Version=1.0.0.0, Culture=neutral, PublicKeyToken=bde4cfb7f85a7a02"
pattern4RemovalArr(6) = "|itextsharp, Version=4.0.6.0, Culture=neutral, PublicKeyToken=8354ae6d2174ddca"
pattern4RemovalArr(7) = "|FrontDesk.Licensing, Version=0.1.0.0, Culture=neutral, PublicKeyToken=bde4cfb7f85a7a02"
pattern4RemovalArr(8) = "|FrontDesk.Server, Version=4.0.0.0, Culture=neutral, PublicKeyToken=bde4cfb7f85a7a02"
pattern4RemovalArr(9) = "|Microsoft.VisualStudio.Web.PageInspector.Loader, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a"
pattern4RemovalArr(10) = "|System.Web.WebPages.Deployment, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"


set fso = CreateObject("Scripting.FileSystemObject")


' find file for editing
dim srcFileName
srcFileName = ""

dim folder
folder = fso.GetAbsolutePathName(".") & "\" &  WScript.Arguments(0) & "\bin"
WScript.Echo "Directory: " & dir 

set dir = fso.GetFolder(folder)


set files = dir.Files
for each file in files
    if InStr(file.Name, fileName) > 0 then
        srcFileName = file.Path
        exit for    ' 1st found file is used
    end if
next

if srcFileName = "" then
    ' file not found
    WScript.Quit(2)
end if

' use temp file for changes
dim tmpFileName
tmpFileName = fso.GetTempName()

WScript.Echo "Editing file: " & srcFileName 
WScript.Echo "Temp file:    " & tmpFileName
set src = fso.OpenTextFile(srcFileName, 1)  ' 1 == ForReading
set dst = fso.OpenTextFile(tmpFileName, 2, true)  ' 2 == ForWriting

' TODO: below, probably better error handling is required to not to kill source file without replacement?

' edit every line
do while not src.AtEndOfStream
    line = src.ReadLine()
    
    For Each pattern In pattern4RemovalArr
        edited = Replace(line, searchFor, replaceTo)
        edited = Replace(edited, pattern, "")
    Next

    dst.WriteLine(edited) ' note this will append newline at EOF if source has no one
loop

dst.Close
src.Close

' replace source file with edited version
fso.DeleteFile srcFileName 
fso.MoveFile tmpFileName, srcFileName
