CREATE TABLE [dbo].[RpmsCredentials]
(
	[Id] uniqueidentifier NOT NULL ,
	AccessCode NVARCHAR(max),
	VerifyCode NVARCHAR(max),
	ExpireAt datetime NOT NULL,

	CONSTRAINT PK__RpmsCredentials PRIMARY KEY(Id)
);