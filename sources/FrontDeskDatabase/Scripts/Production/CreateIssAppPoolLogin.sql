CREATE LOGIN [IIS AppPool\FrontDesk-Server] FROM WINDOWS

CREATE USER [frontdesk_apppool] FOR LOGIN [IIS AppPool\FrontDesk-Server] WITH DEFAULT_SCHEMA = dbo ;

EXEC sp_addrolemember @rolename ='frontdesk_appaccount', @membername = 'frontdesk_apppool'