﻿CREATE TABLE DbVersion
(
   DbVersion varchar(32) NOT NULL CONSTRAINT PK_DbVersion PRIMARY KEY CLUSTERED,
   UpdatedOnUTC datetime CONSTRAINT DF_DbVersion_UpdatedOn DEFAULT (GETUTCDATE())
)
GO
