IF OBJECT_ID('[dbo].[RpmsCredentials]') IS NULL
CREATE TABLE [dbo].[RpmsCredentials]
(
	[Id] uniqueidentifier NOT NULL ,
	AccessCode NVARCHAR(max),
	VerifyCode NVARCHAR(max),
	ExpireAt datetime NOT NULL,

	CONSTRAINT PK__RpmsCredentials PRIMARY KEY(Id)
);
	 
GO 

GO 
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.RpmsCredentials TO frontdesk_appuser
-----------
INSERT INTO dbo.DbVersion(DbVersion) VALUES('4.0.8.0');

