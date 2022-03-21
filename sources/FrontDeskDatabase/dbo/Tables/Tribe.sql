CREATE TABLE [dbo].[Tribe]
(
    [Value] nvarchar(128) NOT NULL PRIMARY KEY,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__Tribe__LastModifiedDateUTC DEFAULT GETUTCDATE(),
)
