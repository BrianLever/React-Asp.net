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