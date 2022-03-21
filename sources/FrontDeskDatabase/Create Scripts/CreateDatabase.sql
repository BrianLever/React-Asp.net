USE [master]
GO
-- CREATE DATABASE
CREATE DATABASE [FrontDesk]
GO

------------------------------------
-- CREATE TABLES and TRIGERS
USE [FrontDesk]
GO

CREATE LOGIN [frontdesk_appuser] WITH PASSWORD = 'Fr0ntdESk2is-%hs' , CHECK_EXPIRATION = OFF;

GO
use [FrontDesk]
GO

CREATE USER [frontdesk_appuser] FOR LOGIN [frontdesk_appuser] WITH DEFAULT_SCHEMA = dbo ;

GO 

