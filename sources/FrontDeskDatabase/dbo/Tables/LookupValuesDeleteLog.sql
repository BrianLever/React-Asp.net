CREATE TABLE [dbo].[LookupValuesDeleteLog]
(
    [TableName]  NVARCHAR(64) NOT NULL,
    [ID] INT NULL,
    [Name] NVARCHAR(128) NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__LookupValuesDeleteLog__LastModifiedDateUTC DEFAULT GETUTCDATE(),
);
GO
