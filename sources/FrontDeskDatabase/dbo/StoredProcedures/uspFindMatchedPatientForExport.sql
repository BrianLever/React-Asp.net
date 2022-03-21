CREATE PROCEDURE [dbo].[uspFindMatchedPatientForExport]
    @LastName nvarchar(128),
    @FirstName nvarchar(128),
    @MiddleName nvarchar(128),
    @Birthday date
AS
BEGIN
    SELECT DISTINCT
        r.LastName,
        r.FirstName,
        r.MiddleName,
        r.Birthday,
        DIFFERENCE(ISNULL(r.MiddleName,''), dbo.ufnMapPatientName(@MiddleName)) AS MiddleNameDiff,
        r.ExportedToPatientID,
        r.ExportedToHRN
    FROM dbo.ScreeningResult r
        INNER JOIN export.SmartExportLog l ON r.ScreeningResultID = l.ScreeningResultID 
            AND l.Succeed = 1 and l.Completed = 1
    WHERE 
        l.ExportDate > '2020-10-01' /* ignore previous exports in DB where patient name might be wrong */
        AND r.Birthday = @Birthday
        AND DIFFERENCE(r.LastName, dbo.ufnMapPatientName(@LastName)) = 4
        AND DIFFERENCE(r.FirstName, dbo.ufnMapPatientName(@FirstName)) = 4
    ORDER BY DIFFERENCE(ISNULL(r.MiddleName,''), dbo.ufnMapPatientName(@MiddleName)) DESC
END
GO

