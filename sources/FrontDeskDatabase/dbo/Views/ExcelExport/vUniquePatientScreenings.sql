CREATE VIEW [dbo].[vUniquePatientScreenings]
AS
SELECT
    MAX(r.ScreeningResultID) as ScreeningResultID,
    MAX(r.CreatedDate) as CreatedDate,
    r.PatientName,
    r.LastName,
    r.FirstName,
    ISNULL(r.MiddleName,'') as MiddleName,
    r.Birthday
FROM dbo.ScreeningResult r
WHERE r.IsDeleted = 0
GROUP BY r.Birthday, r.PatientName, r.LastName, r.FirstName, r.MiddleName
