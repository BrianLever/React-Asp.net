CREATE TABLE [dbo].[ScreeningProfile]
(
    ID int NOT NULL IDENTITY(1, 1),
	[Name] nvarchar(128) NOT NULL,
	[Description] nvarchar(max) NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__ScreeningProfile__LastModifiedDateUTC DEFAULT GETUTCDATE(),
   	CONSTRAINT PK_ScreeningProfile PRIMARY KEY (ID) 
)
GO
