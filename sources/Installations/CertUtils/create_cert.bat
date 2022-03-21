
Rem echo off


set pvk=%1
set cer=%2
set pfx=%3
set PoolIdentity=%4


makecert -pe -n "CN=FrontDesk Root Authority" -a sha1 -sky signature -r -sv %pvk%  %cer% -cy authority -ss Root -sr LocalMachine
makecert -pe -n "CN=FrontDesk" -sr LocalMachine -ss My -a sha1 -sky exchange -in "FrontDesk Root Authority" -is Root -ir LocalMachine -sp "Microsoft RSA SChannel Cryptographic Provider" -sy 12
pvk2pfx -pvk %pvk% -spc %cer% -pfx %pfx%

winhttpcertcfg -g -c LOCAL_MACHINE\My -s "FrontDesk" -a aspnet
winhttpcertcfg -g -c LOCAL_MACHINE\My -s "FrontDesk" -a "NETWORK SERVICE"

IF NOT %PoolIdentity%=="" winhttpcertcfg -g -c LOCAL_MACHINE\My -s "FrontDesk" -a %PoolIdentity%

importpfx -f %pfx% -p "" -t MACHINE -s CA

del %pvk%
del %cer%