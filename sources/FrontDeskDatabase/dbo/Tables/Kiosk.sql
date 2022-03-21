CREATE TABLE dbo.Kiosk
(
	KioskID smallint identity(1000,1) NOT NULL,
	KioskName nvarchar(255) NOT NULL,
	Description nvarchar(max) NULL,
	CreatedDate dateTimeoffset NOT NULL,
	LastActivityDate dateTimeoffset NULL,
	BranchLocationID int NOT NULL,
	Disabled bit NOT NULL CONSTRAINT DF_Kiosk_Disabled DEFAULT(0),
    IpAddress varchar(45) NULL,
    KioskAppVersion varchar(16) NULL,
    SecretKey varchar(64) NULL,
	CONSTRAINT PK_Kiosk PRIMARY KEY (KioskID),
	CONSTRAINT FK_Kiosk_BranchLocation FOREIGN KEY (BranchLocationID) 
		REFERENCES BranchLocation(BranchLocationID) ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT [UQ__Kiosk] UNIQUE ([KioskName] ASC)	
);
GO
GRANT SELECT, INSERT, UPDATE, DELETE ON dbo.Kiosk TO frontdesk_appuser