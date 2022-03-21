ALTER TABLE [export].[SmartExportLog] ALTER COLUMN FailedReason nvarchar(256) NULL;
------------------------------------------------------------------------------

GRANT DELETE ON [export].SmartExportLog  TO frontdesk_appuser;


IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '9.0.0.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('9.0.0.1');