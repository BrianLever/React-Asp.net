@echo Encrypt FD connection string
%WinDir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pe "connectionStrings" -app "/FrontDeskScreener"

@echo Encrypt machine key
%WinDir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pe "system.web/machineKey" -app "/FrontDeskScreener"


@echo Encrypt rpms connection
%WinDir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pef "rpmsCredentials" "C:\inetpub\wwwroot\FrontDeskRPMSInterface"

pause