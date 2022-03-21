IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'License')
	BEGIN
		DROP Table dbo.License
	END
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
 
/*
INSERT INTO [dbo].[License]
           ([ClientID]
           ,[SerialNumber]
           ,[Years]
           ,[MaxKiosks]
           ,[MaxBranchLocations]
           ,[LicenseString]
           ,[Issued]
           )
     VALUES
           (1
           ,100
           ,1
           ,50
           ,50
           ,'license1'
           ,GETDATE()
           )
GO
INSERT INTO [dbo].[License]
           ([ClientID]
           ,[SerialNumber]
           ,[Years]
           ,[MaxKiosks]
           ,[MaxBranchLocations]
           ,[LicenseString]
           ,[Issued]
           )
     VALUES
           (1
           ,110
           ,2
           ,100
           ,150
           ,'license2'
           ,GETDATE()
           )
GO
*/