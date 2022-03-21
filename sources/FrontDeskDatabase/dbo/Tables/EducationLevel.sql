CREATE TABLE [dbo].[EducationLevel]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__EducationLevel__LastModifiedDateUTC DEFAULT GETUTCDATE(),
    CONSTRAINT PK_EducationLevel PRIMARY KEY CLUSTERED (ID),
);
GO
