CREATE PROCEDURE [dbo].[uspGetScreeningResults]
@StartDate date,
@BatchSize int
AS
    SELECT TOP (@BatchSize)
        r.ScreeningResultID, 
        r.PatientName,
        r.Birthday
    FROM dbo.ScreeningResult r
    WHERE r.IsDeleted = 0 AND CreatedDate >= @StartDate
    ORDER BY r.CreatedDate ASC

RETURN 0
GO
