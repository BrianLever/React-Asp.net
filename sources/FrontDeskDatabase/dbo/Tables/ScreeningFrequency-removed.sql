CREATE TABLE dbo.ScreeningFrequency
(
	ID varchar(16) NOT NULL,
	Frequency int NOT NULL,
	LastModifiedDateUTC datetime NOT NULL,
	CONSTRAINT PK_ScreeningFrequency PRIMARY KEY(ID ASC)
)
GO
