CREATE TABLE [dbo].[Gender]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__Gender__LastModifiedDateUTC DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Gender PRIMARY KEY CLUSTERED (ID),
);
GO
