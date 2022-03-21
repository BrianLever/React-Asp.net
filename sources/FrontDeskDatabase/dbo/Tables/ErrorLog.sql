CREATE TABLE dbo.ErrorLog
	(
	   ErrorLogID bigint NOT NULL IDENTITY(1,1) NOT FOR REPLICATION,
	   KioskID smallint NULL,
	   ErrorMessage nvarchar(max) ,
	   ErrorTraceLog nvarchar(max),
	   CreatedDate datetimeoffset NOT NULL,
	   CONSTRAINT PK_ErrorLog PRIMARY KEY(ErrorLogID)

	)
GO
CREATE INDEX IX_ErrorLog_CreatedDate ON dbo.ErrorLog(CreatedDate DESC) INCLUDE(KioskID,ErrorMessage);
GO
---
GRANT SELECT,INSERT,DELETE,UPDATE ON ErrorLog TO frontdesk_appuser