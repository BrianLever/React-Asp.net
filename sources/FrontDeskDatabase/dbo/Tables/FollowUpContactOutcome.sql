CREATE TABLE [dbo].[FollowUpContactOutcome]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_FollowUpContactOutcome PRIMARY KEY CLUSTERED (ID),
);
GO
