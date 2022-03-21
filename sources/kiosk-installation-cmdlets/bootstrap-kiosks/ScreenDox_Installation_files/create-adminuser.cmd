@echo off

set password=%1
echo Admin pass: %password%


@echo Creating Kiosk user account
net user Admin3 %password% /ADD /Y /ACTIVE:YES 

WMIC USERACCOUNT WHERE "Name='Admin3'" SET PasswordExpires=FALSE
net localgroup Administrators Admin3 /add
NET LOCALGROUP "Remote Desktop Users" Admin3 /ADD