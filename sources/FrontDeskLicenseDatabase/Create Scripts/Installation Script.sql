﻿ USE [master]
GO
-- CREATE DATABASE
CREATE DATABASE [FrontDeskLicense]
GO

------------------------------------
-- CREATE LOGIN and USER
USE [FrontDeskLicense]
GO

CREATE LOGIN [fdlicense_appuser] WITH PASSWORD = 'F1dL1ce0ns-^3k2is-%hs' , CHECK_EXPIRATION = OFF;

GO
use [FrontDeskLicense]
GO

CREATE USER [fdlicense_appuser] FOR LOGIN [fdlicense_appuser] WITH DEFAULT_SCHEMA = dbo ;

GO 
EXEC sp_addrolemember 'db_owner', @membername='fdlicense_appuser';
GO

--------------------------------------------------------

----- DROP TABLES ----

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'dbo.State')
	BEGIN
		DROP  Table dbo.State
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Activation')
	BEGIN
		DROP Table dbo.Activation
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'License')
	BEGIN
		DROP Table dbo.License
	END
GO

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Client')
	BEGIN
		DROP Table dbo.Client
	END
GO
-- CREATE TABLES ----------------------
CREATE TABLE dbo.State
(
	StateCode char(2) NOT NULL,
	CountryCode varchar(2) NOT NULL,
	Name nvarchar(128) NOT NULL,
	CONSTRAINT PK_State PRIMARY KEY (StateCode, CountryCode),
	CONSTRAINT UQ_State UNIQUE (StateCode) 
)
GO

CREATE INDEX IX_State_Name ON dbo.State(Name);
GO


CREATE TABLE dbo.Client
(
	ClientID int NOT NULL IDENTITY(1, 1),
	
	CompanyName nvarchar(128) NOT NULL,    
	StateCode char(2) NULL,
	City nvarchar(128) NULL,
	AddressLine1 nvarchar(128) NULL,
	AddressLine2 nvarchar(128) NULL,	
	PostalCode nvarchar(24) NULL,
	
	Email nvarchar(128) NULL,
	ContactPerson nvarchar(128) NULL,    		
	ContactPhone nvarchar(24) NULL,
	
	Notes nvarchar(max) NULL,
	LastModified datetimeoffset NOT NULL

	CONSTRAINT PK_Client Primary Key CLUSTERED(ClientID)
)
GO



CREATE TABLE dbo.License
(
	LicenseID int NOT NULL IDENTITY(1, 1),
	ClientID int NULL,
	
	SerialNumber int NOT NULL,
	Years int NOT NULL,
	MaxKiosks int NOT NULL,
	MaxBranchLocations int NOT NULL,
	
	LicenseString varchar(128) NULL,	-- calculated and stored for convenience
	
	Issued datetimeoffset NOT NULL,

	CONSTRAINT PK_License Primary Key CLUSTERED(LicenseID),
	CONSTRAINT UQ_License_SerialNumber UNIQUE(SerialNumber),
	CONSTRAINT FK_License_Client FOREIGN KEY (ClientID) 
		REFERENCES dbo.Client(ClientID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO


CREATE TABLE dbo.Activation
(
	ActivationID int NOT NULL IDENTITY(1, 1),
	
	ActivationRequest varchar(128) NOT NULL,
	
	LicenseID int NOT NULL,					
	ProductId varchar(128) NOT NULL,	-- Windows product id from customer
	ExpirationDate datetime NOT NULL,		-- calculated using license Years
	ActivationKey varchar(128) NULL,	-- calculated and stored for convenience
	
	Issued datetimeoffset NOT NULL,

	CONSTRAINT PK_Activation Primary Key CLUSTERED(ActivationID),	
	CONSTRAINT UQ_Activation_LicenseID UNIQUE(LicenseID),	-- only one activation per license
	CONSTRAINT FK_Activation_License FOREIGN KEY (LicenseID) 
		REFERENCES dbo.License(LicenseID) ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO


-- Error Log

IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'ErrorLog')
	BEGIN
		DROP  Table ErrorLog
	END
GO

CREATE TABLE dbo.ErrorLog
(
   ErrorLogID bigint NOT NULL IDENTITY(1,1) NOT FOR REPLICATION,
   KioskID smallint NULL,
   ErrorMessage nvarchar(max) ,
   ErrorTraceLog nvarchar(max),
   CreatedDate datetimeoffset NOT NULL,
   CONSTRAINT PK_ErrorLog PRIMARY KEY(ErrorLogID)

)
GO
CREATE INDEX IX_ErrorLog_CreatedDate ON dbo.ErrorLog(CreatedDate DESC) INCLUDE(KioskID,ErrorMessage);
GO
-------------------------
---- Insert States
DELETE FROM State;

INSERT INTO State(StateCode, Name, CountryCode)
VALUES
('AK', 'Alaska', 'US'),
('AL', 'Alabama', 'US'),
('AR', 'Arkansas', 'US'),
('AZ', 'Arizona', 'US'),
('CA', 'California', 'US'),
('CO', 'Colorado', 'US'),
('CT', 'Connecticut', 'US'),
('DC', 'District of Columbia', 'US'),
('DE', 'Delaware', 'US'),
('FL', 'Florida', 'US'),
('GA', 'Georgia', 'US'),
('GU', 'Guam', 'US'),
('HI', 'Hawaii', 'US'),
('IA', 'Iowa', 'US'),
('ID', 'Idaho', 'US'),
('IL', 'Illinois', 'US'),
('IN', 'Indiana', 'US'),
('KS', 'Kansas', 'US'),
('KY', 'Kentucky', 'US'),
('LA', 'Louisiana', 'US'),
('MA', 'Massachusetts', 'US'),
('MD', 'Maryland', 'US'),
('ME', 'Maine', 'US'),
('MI', 'Michigan', 'US'),
('MN', 'Minnesota', 'US'),
('MO', 'Missouri', 'US'),
('MS', 'Mississippi', 'US'),
('MT', 'Montana', 'US'),
('NC', 'North Carolina', 'US'),
('ND', 'North Dakota', 'US'),
('NE', 'Nebraska', 'US'),
('NH', 'New Hampshire', 'US'),
('NJ', 'New Jersey', 'US'),
('NM', 'New Mexico', 'US'),
('NV', 'Nevada', 'US'),
('NY', 'New York', 'US'),
('OH', 'Ohio', 'US'),
('OK', 'Oklahoma', 'US'),
('OR', 'Oregon', 'US'),
('PA', 'Pennsylvania', 'US'),
('PR', 'Puerto Rico', 'US'),
('RI', 'Rhode Island', 'US'),
('SC', 'South Carolina', 'US'),
('SD', 'South Dakota', 'US'),
('TN', 'Tennessee', 'US'),
('TX', 'Texas', 'US'),
('UT', 'Utah', 'US'),
('VA', 'Virginia', 'US'),
('VT', 'Vermont', 'US'),
('WA', 'Washington', 'US'),
('WI', 'Wisconsin', 'US'),
('WV', 'West Virginia', 'US'),
('WY', 'Wyoming', 'US');
 
GO
