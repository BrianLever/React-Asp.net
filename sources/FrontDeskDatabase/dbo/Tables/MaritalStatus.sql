CREATE TABLE [dbo].[MaritalStatus]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__MaritalStatus__LastModifiedDateUTC DEFAULT GETUTCDATE(),
    CONSTRAINT PK_MaritalStatus PRIMARY KEY CLUSTERED (ID),
);
GO
