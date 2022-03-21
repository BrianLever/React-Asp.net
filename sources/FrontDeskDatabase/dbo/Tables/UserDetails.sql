CREATE TABLE dbo.UserDetails
(
	UserID int NOT NULL,
	FirstName nvarchar(128) NOT NULL,    
	LastName nvarchar(128) NOT NULL,
	MiddleName nvarchar(128) NULL,
	ContactPhone nvarchar(24) NULL,
	StateCode char(2) NULL,
	City nvarchar(128) NULL,
	AddressLine1 nvarchar(128) NULL,
	AddressLine2 nvarchar(128) NULL,	
	PostalCode nvarchar(24) NULL,
	IsBlock bit NOT NULL CONSTRAINT DF_UserDetails_IsBlock Default (0),	
	FullName as CONVERT(nvarchar(255),dbo.fn_GetFullName(LastName, FirstName, MiddleName)),

	CONSTRAINT PK_UserDetails Primary Key CLUSTERED(UserID),
	CONSTRAINT FK_UserDetails_Users FOREIGN KEY (UserID)
	REFERENCES dbo.Users (PKID) ON UPDATE CASCADE ON DELETE	 CASCADE	
);
GO
CREATE INDEX IX_UserDetails_FullName ON dbo.UserDetails(FullName ASC)
GO
