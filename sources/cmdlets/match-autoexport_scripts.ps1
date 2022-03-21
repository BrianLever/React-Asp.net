Get-ChildItem "." -Filter *_Failures.txt | Select-String -Pattern 'does not match'| Select -ExpandProperty line | Set-Content "Exports_no_matches.txt"

Get-ChildItem "." -Filter *_Failures.txt | Select-String -Pattern 'aborted'| Select -ExpandProperty line | Set-Content "Exports_aborted.txt"



Get-ChildItem "." -Filter 2020-02-21_screendox_autoexport_rpms_console.txt | Select-String -Pattern 'Succeed'| Select -ExpandProperty line | Set-Content "Exports_succeed.txt"

Get-ChildItem "." -Filter 2020-02-21_screendox_autoexport_rpms_console.txt | Select-String -Pattern 'Matched'| Select -ExpandProperty line | Set-Content "Exports_matched.txt"
