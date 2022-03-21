    CREATE TABLE [dbo].[SexualOrientation]
    (
        [ID] INT NOT NULL,
        [Name] NVARCHAR(64) NOT NULL,
        [OrderIndex] INT NOT NULL,
        LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__SexualOrientation__LastModifiedDateUTC DEFAULT GETUTCDATE(),
        CONSTRAINT PK_SexualOrientation PRIMARY KEY CLUSTERED (ID),
    );
    GO
