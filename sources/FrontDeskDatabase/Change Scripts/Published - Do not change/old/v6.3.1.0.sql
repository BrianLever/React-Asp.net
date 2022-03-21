IF OBJECT_ID('dbo.[vScreeningResults]') IS NOT NULL
DROP VIEW dbo.[vScreeningResults] 
GO
;
CREATE VIEW dbo.[vScreeningResults] 
AS
SELECT 
r.ScreeningResultID, 
r.PatientName, 
r.FirstName,
r.LastName,
r.Birthday, 
r.KioskID,
k.BranchLocationID,
r.CreatedDate,
ssr.ScreeningSectionID, 
ssr.Score, 
ssr.ScoreLevel, 
ssr.AnswerValue as SectionAnswerValue, 
qr.QuestionID, 
qr.AnswerValue
FROM dbo.ScreeningResult r
    INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
	INNER JOIN dbo.ScreeningSectionResult ssr 
        ON r.ScreeningResultID = ssr.ScreeningResultID
	INNER JOIN dbo.ScreeningSectionQuestionResult qr 
        ON qr.ScreeningResultID = ssr.ScreeningResultID 
            AND qr.ScreeningSectionID = ssr.ScreeningSectionID
WHERE r.IsDeleted = 0
;
GO

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.3.1.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.3.1.0');