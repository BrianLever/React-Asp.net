CREATE TABLE [dbo].[County]
(
    [Value] nvarchar(128) NOT NULL PRIMARY KEY,
    LastModifiedDateUTC datetime NOT NULL CONSTRAINT DF__County__LastModifiedDateUTC DEFAULT GETUTCDATE(),
)