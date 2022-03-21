IF OBJECT_ID('[dbo].[LookupValuesDeleteLog]') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [dbo].[LookupValuesDeleteLog]
(
    [TableName]  NVARCHAR(64) NOT NULL,
    [ID] INT NULL,
    [Name] NVARCHAR(128) NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__LookupValuesDeleteLog__LastModifiedDateUTC DEFAULT GETUTCDATE(),
);
GO

SET NOEXEC OFF
GO


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.0.6')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.0.6');