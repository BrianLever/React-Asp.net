CREATE VIEW dbo.[vScreeningResults] 
AS
SELECT 
r.ScreeningResultID, 
r.PatientName, 
r.FirstName,
r.LastName,
r.MiddleName,
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
GO