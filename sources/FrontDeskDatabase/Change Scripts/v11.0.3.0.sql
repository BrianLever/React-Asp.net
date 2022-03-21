ALTER VIEW dbo.[vScreeningResults] 
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
-------------------

IF OBJECT_ID('dbo.vPatients') IS NOT NULL
    DROP VIEW dbo.vPatients
GO

CREATE VIEW dbo.[vPatients] 
AS
SELECT 
    patient.ScreeningResultID,
    patient.PatientName,
    patient.Birthday,
    r.LastName,
    r.FirstName,
    r.MiddleName,
    r.ExportedToPatientId,
    r.ExportedToHRN,
    r.StreetAddress,
    r.City,
    r.StateID,
    st.Name as StateName,
    r.ZipCode,
    d.ID as DemographicsID
FROM 
(
    SELECT 
        MAX(r.ScreeningResultID) as ScreeningResultID,
        r.PatientName, 
        r.Birthday
    FROM dbo.ScreeningResult r
    WHERE r.IsDeleted = 0
    GROUP BY r.Birthday, r.PatientName 
) as patient
    INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = patient.ScreeningResultID
    LEFT JOIN dbo.State st ON st.StateCode = r.StateID
    LEFT JOIN dbo.BhsDemographics d ON d.PatientName = r.PatientName AND d.Birthday = r.Birthday
GO
;

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '11.0.3.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('11.0.3.0');