CREATE TABLE dbo.SystemSettings
(
	[Key] [nvarchar](128) NOT NULL,
	[Value] [nvarchar](255) NULL,
	[Name] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[RegExp] [nvarchar](255) NULL,
	[IsExposed] [bit] NULL,
	CONSTRAINT PK_SystemSettinge Primary Key CLUSTERED([Key])
);
GO

