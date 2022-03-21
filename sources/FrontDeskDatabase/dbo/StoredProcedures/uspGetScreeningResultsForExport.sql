CREATE PROCEDURE [dbo].[uspGetScreeningResultsForExport]
@BatchSize int = 45
AS
    SELECT TOP(@BatchSize) 
        r.ScreeningResultID, 
        r.PatientName,
        r.Birthday, 
        r.CreatedDate
    FROM dbo.ScreeningResult r
        LEFT JOIN export.SmartExportLog el ON r.ScreeningResultID = el.ScreeningResultID AND el.Completed = 1
    WHERE r.IsDeleted = 0 AND r.ExportDate IS NULL AND el.ScreeningResultID IS NULL
    ORDER BY r.CreatedDate DESC

RETURN 0
GO
