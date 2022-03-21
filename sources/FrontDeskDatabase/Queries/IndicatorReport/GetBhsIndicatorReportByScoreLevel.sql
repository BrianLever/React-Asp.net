declare @StartDate datetime = '2019-07-01',
@EndDate datetime = '2020-06-30',
@LocationID int = 1

;
WITH tblResults(ScreeningSectionID,	ScoreLevel, Name, Indicates, IsPositive) AS
(

SELECT 
		sl.ScreeningSectionID,
		sl.ScoreLevel,
		sl.Name,
        sl.Indicates,
		Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive
	FROM dbo.ScreeningScoreLevel sl 
		LEFT JOIN (
  
SELECT
    sr.ScreeningSectionID,
	sr.ScoreLevel
FROM dbo.ScreeningSectionResult sr 
	INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
 INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID where  r.IsDeleted = 0  and sr.ScreeningSectionID NOT IN('SIH','TCC')  and k.BranchLocationID = @LocationID  and r.CreatedDate >= @StartDate  and r.CreatedDate < @EndDate    
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
	WHERE sl.ScreeningSectionID NOT IN('SIH','TCC') 

)
Select 
	r.ScreeningSectionID,
    r.Name,
    r.Indicates,
   	SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
	COUNT_BIG(*) as Total
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.ScoreLevel, r.Indicates
ORDER BY ScreeningSectionID, ScoreLevel
