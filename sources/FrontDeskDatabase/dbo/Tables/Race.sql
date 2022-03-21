CREATE TABLE [dbo].[Race]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__Race__LastModifiedDateUTC DEFAULT GETUTCDATE(),
    CONSTRAINT PK_Race PRIMARY KEY CLUSTERED (ID),
);
GO
