
update dbo.SystemSettings set Value = 100000 where [KEY] = 'ExportedSecurityReportMaximumLength';
GO

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.1.0.5')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.1.0.5');