CREATE TABLE LookupValue
(
   Screen nvarchar(32) NOT NULL,
   ID INT NOT NULL,
   Name NVARCHAR(64) NOT NULL,
   OrderIndex INT NOT NULL,
   LastModifiedDateUTC datetime NOT NULL,
   CONSTRAINT PK_LookupValue PRIMARY KEY (Screen, ID)
)
GO
CREATE INDEX IX_LookupValue_OrderIndex ON LookupValue(Screen ASC, OrderIndex ASC)
GO
CREATE INDEX IX_LookupValue_LastModifiedDateUTC ON LookupValue(LastModifiedDateUTC DESC)
GO