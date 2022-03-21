
Rem echo off

set pfx=%1
set pass=%2

importpfx -f %pfx% -p %pass% -t MACHINE -s ROOT
makecert -pe -n "CN=FrontDesk" -ss My -sr LocalMachine -a sha1 -sky exchange -in "FrontDesk Root Authority" -is Root -ir LocalMachine -sp "Microsoft RSA SChannel Cryptographic Provider" -sy 12

