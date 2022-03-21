$perfis = (Get-ChildItem Registry::HKEY_USERS\ | Where-Object {$_.Name -match "S-1"} | ForEach-Object {Get-ItemProperty "Registry::$_\Control Panel\Desktop" -Name "Win8DpiScaling" -ErrorAction SilentlyContinue}).PSPath
foreach ($_ in $monitores) {Set-ItemProperty -Path "Registry::$_" -Name "Win8DpiScaling" -Value 0}

$monitores = (Get-ChildItem Registry::HKEY_USERS\ | Where-Object {$_.Name -match "S-1"} | ForEach-Object {Get-ChildItem "Registry::$_\Control Panel\Desktop\PerMonitorSettings" -ErrorAction SilentlyContinue}).PSPath
foreach ($_ in $monitores) {Set-ItemProperty -Path "Registry::$_" -Name "DpiValue" -Value 0}