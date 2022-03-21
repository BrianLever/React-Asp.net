

declare @StartDate datetime = '2018-10-01',
@EndDate datetime = '2019-09-30',
@LocationID int = 1
;




WITH tblResults(ScreeningSectionID,	ScoreLevel, Name, Indicates, IsPositive, Age) AS
(

SELECT 
        sl.ScreeningSectionID,
        sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        ISNULL(sr.Age, 0) as Age
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
  
SELECT
    sr.ScreeningSectionID,
    MAX(sr.ScoreLevel) as ScoreLevel,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
 INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID where  r.IsDeleted = 0  and sr.ScreeningSectionID NOT IN('SIH','TCC', 'PHQ-9','GAD-7')  and k.BranchLocationID = @LocationID  and r.CreatedDate >= @StartDate  and r.CreatedDate < @EndDate  group by r.FirstName,r.LastName,r.MiddleName,r.Birthday,sr.ScreeningSectionID   
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID NOT IN('SIH','TCC', 'PHQ-9','GAD-7') 

)
Select 
    r.ScreeningSectionID,
    r.Name,
    r.Indicates,
    r.Age,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.ScoreLevel, r.Indicates, r.Age
ORDER BY ScreeningSectionID, ScoreLevel, Age
