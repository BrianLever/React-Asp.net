@echo Decrypt FD connection string
%WinDir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pd "connectionStrings" -app "/FrontDeskScreener"

@echo Decrypt machine key
%WinDir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pd "system.web/machineKey" -app "/FrontDeskScreener"


@echo Decrypt rpms connection
%WinDir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -pdf "rpmsCredentials" "C:\inetpub\wwwroot\FrontDeskRPMSInterface"
pause