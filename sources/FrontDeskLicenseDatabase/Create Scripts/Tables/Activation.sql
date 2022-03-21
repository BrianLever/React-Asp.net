IF EXISTS (SELECT * FROM sysobjects WHERE type = 'U' AND name = 'Activation')
	BEGIN
		DROP Table dbo.Activation
	END
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
 