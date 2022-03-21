---- BhsFollowUp

IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BhsFollowUp') AND name = 'FollowUpDate')
SET NOEXEC ON
GO

ALTER TABLE [dbo].BhsFollowUp ADD  FollowUpDate DateTimeOffset NULL;
GO
UPDATE [dbo].BhsFollowUp SET FollowUpDate = VisitDate WHERE FollowUpDate IS NULL;

ALTER TABLE [dbo].BhsFollowUp ALTER COLUMN FollowUpDate DateTimeOffset NOT NULL;

GO
SET NOEXEC OFF
GO


---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.0.4')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.0.4');