declare @StartDate datetimeoffset(7),
@EndDate datetimeoffset(7);

set @StartDate = '2012-07-01';
set @EndDate = '2013-06-30';

			

	


WITH tblResults(ScreeningSectionID,	ScoreLevel, ScoreName, IsPositive) AS
(
	SELECT 
		sl.ScreeningSectionID,
		sl.ScoreLevel,
		sl.Name as ScoreName,
		Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive
	FROM dbo.ScreeningScoreLevel sl 
		LEFT JOIN (
			SELECT
				sr.ScreeningSectionID,
				sr.ScoreLevel
			FROM dbo.ScreeningSectionResult sr 
				INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
				INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID 
			WHERE  r.IsDeleted = 0  
			AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate   
		) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
	WHERE sl.ScreeningSectionID NOT IN('SIH','TCC') 
)
Select 
	r.ScreeningSectionID,
	r.ScoreName,
	r.ScoreLevel,
	SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
	COUNT_BIG(*) as Total
FROM tblResults r
	
GROUP BY r.ScoreName, r.ScreeningSectionID, r.ScoreLevel
ORDER BY ScreeningSectionID, ScoreLevel