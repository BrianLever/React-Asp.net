
--- Frontdesk Server installation script - v1.0.0.0

USE [master]
GO

IF DB_ID('FrontDesk') IS NOT NULL
BEGIN
	ALTER DATABASE [FrontDesk] SET OFFLINE WITH ROLLBACK IMMEDIATE
	ALTER DATABASE [FrontDesk] SET ONLINE
	DROP DATABASE [FrontDesk];
END
GO

CREATE DATABASE [FrontDesk]
GO

------------------------------------
-- CREATE TABLES and TRIGERS
USE [FrontDesk]
GO

IF NOT EXISTS(SELECT name FROM master.dbo.syslogins WHERE name = 'frontdesk_appuser')
BEGIN
	--The following statement was imported into the database project as a schema object and named frontdesk_appuser.
--CREATE LOGIN [frontdesk_appuser] WITH PASSWORD = '{PASSWORD}' , CHECK_EXPIRATION = OFF;

END
GO

ALTER LOGIN [frontdesk_appuser] WITH  PASSWORD = '{PASSWORD}';
GO

IF NOT EXISTS(SELECT name FROM sys.database_principals WHERE name = 'frontdesk_appuser')
BEGIN
	--The following statement was imported into the database project as a schema object and named frontdesk_appuser.
--CREATE USER [frontdesk_appuser] FOR LOGIN [frontdesk_appuser] WITH DEFAULT_SCHEMA = dbo ;

END
GO


--=========================================================================== 
--=========================================================================== 
-- MEMBERSHIP
--=========================================================================== 
--------------------------------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Users')
BEGIN

DBCC CHECKIDENT (Users, RESEED, 1);

INSERT INTO [dbo].[Users]
           (
			[Username]
           ,[Email]
           ,[Comment]
           ,[Password]
           ,[PasswordQuestion]
           ,[PasswordAnswer]	
           ,[IsApproved]
           ,[IsLockedOut]
           ,[FailedPasswordAttemptCount]
           ,[FailedPasswordAttemptWindowStart]
           ,[FailedPasswordAnswerAttemptCount]
           ,[FailedPasswordAnswerAttemptWindowStart]
			,[CreationDate]
		   ,[LastActivityDate])
     VALUES
           (
			'su',
			'testuser@3si2.com',
			'Built-in Master Admin Account',
			'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
			NULL,
			NULL,
			1,
			0,
			5,
			GETDATE(),
			0,
			GETDATE(),
			GETDATE(),
			GETDATE());

END
GO

INSERT INTO [dbo].[Users]
           (
			[Username]
           ,[Email]
           ,[Comment]
           ,[Password]
           ,[PasswordQuestion]
           ,[PasswordAnswer]	
           ,[IsApproved]
           ,[IsLockedOut]
           ,[FailedPasswordAttemptCount]
           ,[FailedPasswordAttemptWindowStart]
           ,[FailedPasswordAnswerAttemptCount]
           ,[FailedPasswordAnswerAttemptWindowStart]
			,[CreationDate]
		   ,[LastActivityDate])
     VALUES
           (
			'technical_support',
			'',
			'Tech Support Admin Account',
			'Sxuu1WEDNtOlDrmyf8uVqGjwZUw=',
			NULL,
			NULL,
			1,
			0,
			2,
			GETDATE(),
			0,
			GETDATE(),
			GETDATE(),
			GETDATE()
	);
GO

INSERT INTO [Roles] ([Rolename]) VALUES('Super Administrator');
GO

INSERT INTO [Roles] ([Rolename]) VALUES('Branch Administrator');
GO

INSERT INTO [Roles] ([Rolename]) VALUES('Staff');
GO

INSERT INTO [Roles] ([Rolename]) VALUES('Medical Professionals');
GO

INSERT INTO [Roles] ([Rolename]) VALUES('Lead Medical Professionals');
GO

----------------------
IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'UsersInRoles')
BEGIN

--The following statement was imported into the database project as a schema object and named dbo.UsersInRoles.
--CREATE TABLE [dbo].[UsersInRoles](
--	[Username] [nvarchar](255) NOT NULL,
--	[Rolename] [varchar](255) NOT NULL,
--	CONSTRAINT [PKUsersInRoles] PRIMARY KEY CLUSTERED ([Username] ASC,[Rolename] ASC),
--	CONSTRAINT [FK_UsersInRoles_Roles] FOREIGN KEY([Rolename]) REFERENCES [dbo].[Roles] ([Rolename])
--		ON UPDATE CASCADE ON DELETE CASCADE,
--	CONSTRAINT FK_UsersInRoles_Users FOREIGN KEY([Username]) REFERENCES [dbo].[Users] ([Username])
--		ON UPDATE CASCADE ON DELETE CASCADE
--) ;



INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('su', 'Super Administrator');
INSERT INTO UsersInRoles ([UserName], [RoleName]) VALUES ('technical_support', 'Super Administrator');

END
GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'UserDetails')
BEGIN
--The following statement was imported into the database project as a schema object and named dbo.UserDetails.
--CREATE TABLE dbo.UserDetails
--(
--	UserID int NOT NULL,
--	FirstName nvarchar(128) NOT NULL,    
--	LastName nvarchar(128) NOT NULL,
--	MiddleName nvarchar(128) NULL,
--	ContactPhone nvarchar(24) NULL,
--	StateCode char(2) NULL,
--	City nvarchar(128) NULL,
--	AddressLine1 nvarchar(128) NULL,
--	AddressLine2 nvarchar(128) NULL,	
--	PostalCode nvarchar(24) NULL,
--	IsBlock bit NOT NULL CONSTRAINT DF_UserDetails_IsBlock Default (0),	
--	FullName as CONVERT(nvarchar(255),dbo.fn_GetFullName(LastName, FirstName, MiddleName)),
--
--	CONSTRAINT PK_UserDetails Primary Key CLUSTERED(UserID),
--	CONSTRAINT FK_UserDetails_Users FOREIGN KEY (UserID)
--	REFERENCES dbo.Users (PKID) ON UPDATE CASCADE ON DELETE	 CASCADE	
--);


INSERT INTO dbo.UserDetails 
(
	UserID,
	FirstName,    
	LastName,
	MiddleName,
	StateCode,
	City,
	ContactPhone,
	AddressLine1,
	AddressLine2,
	PostalCode	
)
VALUES
(
	1,
	'Built-in',    
	'Superuser',
	null,
	null,
	null,
	null,
	null,
	null,
	null	
);


INSERT INTO dbo.UserDetails 
(
	UserID,
	FirstName,    
	LastName,
	MiddleName,
	StateCode,
	City,
	ContactPhone,
	AddressLine1,
	AddressLine2,
	PostalCode	
)
VALUES
(
	2,
	'Technical',    
	'Support',
	null,
	null,
	null,
	null,
	null,
	null,
	null	
);




END
GO

