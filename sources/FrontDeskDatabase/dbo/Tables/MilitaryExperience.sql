CREATE TABLE [dbo].[MilitaryExperience]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__MilitaryExperience__LastModifiedDateUTC DEFAULT GETUTCDATE(),
    CONSTRAINT PK_MilitaryExperience PRIMARY KEY CLUSTERED (ID),
);
GO
