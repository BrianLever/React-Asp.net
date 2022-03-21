IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Client')
	BEGIN
		DROP Table dbo.Client
	END
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

INSERT INTO [dbo].[Client]
           ([CompanyName]
           ,[StateCode]
           ,[City]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[PostalCode]
           ,[Email]
           ,[ContactPerson]
           ,[ContactPhone]
           ,[Notes]
           ,[LastModified])
     VALUES
           ('Sample Company'
           ,'CA'
           ,'City'
           ,null
           ,null
           ,null
           ,null
           ,'Contact Person'
           ,''
           ,null
           ,GETDATE())
GO


