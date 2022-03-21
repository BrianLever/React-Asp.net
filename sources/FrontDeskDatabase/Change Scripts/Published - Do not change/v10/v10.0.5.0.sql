-- Improvement to DB performance - adding indecies

IF NOT EXISTS(
SELECT 1 
FROM sys.indexes 
WHERE name='IX__ScreeningResult_Birthday' AND object_id = OBJECT_ID('dbo.[ScreeningResult]')
)

CREATE NONCLUSTERED INDEX IX__ScreeningResult_Birthday
ON [dbo].[ScreeningResult] ([Birthday])

GO



IF NOT EXISTS(
SELECT 1 
FROM sys.indexes 
WHERE name='IX__License_ActivateDate' AND object_id = OBJECT_ID('dbo.[License]')
)

CREATE NONCLUSTERED INDEX IX__License_ActivateDate
ON [dbo].[License] (ActivatedDate Desc, CreatedDate DESC)

GO

IF NOT EXISTS(
SELECT 1 
FROM sys.indexes 
WHERE name='IX__ScreeningSectionResult_ScreeningSectionID' AND object_id = OBJECT_ID('dbo.[ScreeningSectionResult]')
)

CREATE NONCLUSTERED INDEX IX__ScreeningSectionResult_ScreeningSectionID
ON [dbo].[ScreeningSectionResult] ([ScreeningSectionID])
INCLUDE ([ScreeningResultID],[ScoreLevel])

GO



IF NOT EXISTS(
SELECT 1 
FROM sys.indexes 
WHERE name='IX__ScreeningResult_ExportDate' AND object_id = OBJECT_ID('dbo.[ScreeningResult]')
)
CREATE NONCLUSTERED INDEX IX__ScreeningResult_ExportDate
ON [dbo].[ScreeningResult] ([IsDeleted],[ExportDate])

GO
---------------------------

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '10.0.5.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('10.0.5.0');