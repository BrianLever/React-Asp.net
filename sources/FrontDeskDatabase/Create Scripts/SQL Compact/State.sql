CREATE TABLE State
(
	StateCode nvarchar(2) NOT NULL,
	CountryCode nvarchar(2) NOT NULL,
	Name nvarchar(128) NOT NULL,
	CONSTRAINT PK_State PRIMARY KEY (StateCode, CountryCode),
	CONSTRAINT UQ_State UNIQUE (StateCode) 
)
GO