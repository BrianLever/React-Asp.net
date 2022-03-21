CREATE PROCEDURE [dbo].[uspGetHistoryOfPreviousNotEmptyScreenings]
    @LastName varchar(128),
    @FirstName varchar(128),
    @MiddleName varchar(128),
    @Birthday date,
    @EndDate datetimeoffset,
    @Limit int
AS
SELECT TOP (@Limit)
sr.ScreeningResultID
,sr.CreatedDate
,CASE WHEN SUM(ISNULL(ssr.ScoreLevel,0)) > 0 THEN 1 ELSE 0 END AS Positive
,CASE WHEN COUNT(ssr.ScoreLevel) > 0 THEN 1 ELSE 0 END AS HasAnyScreening
FROM dbo.ScreeningResult sr
    LEFT JOIN dbo.ScreeningSectionResult ssr ON sr.ScreeningResultID = ssr.ScreeningResultID
WHERE 
sr.IsDeleted = 0 AND sr.CreatedDate < @EndDate AND
dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName) = dbo.fn_GetPatientName(sr.LastName, sr.FirstName, sr.MiddleName) AND @Birthday = sr.Birthday
GROUP BY sr.ScreeningResultID, sr.CreatedDate, sr.ExportDate, sr.ExportedToHRN
HAVING (CASE WHEN COUNT(ssr.ScoreLevel) > 0 THEN 1 ELSE 0 END) > 0
ORDER BY sr.CreatedDate DESC;

RETURN 0
