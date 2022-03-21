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
