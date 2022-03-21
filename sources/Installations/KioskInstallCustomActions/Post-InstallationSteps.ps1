param(
[string]$Password
)

net user kioskuser $Password /Add
net localgroup "Remote Desktop Users" "kioskuser" /add

net localgroup "Administrators" "kioskuser" /add



icacls "c:\program files\frontdesk\data" /grant kioskuser:F /T
icacls "c:\program files\frontdesk\logs" /grant kioskuser:F /T
