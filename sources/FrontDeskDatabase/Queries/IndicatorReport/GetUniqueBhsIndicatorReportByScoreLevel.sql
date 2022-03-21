declare @StartDate datetime = '2019-07-01',
@EndDate datetime = '2020-06-30'

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
  
SELECT sr.ScreeningSectionID
,MAX(sr.ScoreLevel) as ScoreLevel
,r.FirstName
,r.LastName
,r.MiddleName
,r.Birthday
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
  where  r.IsDeleted = 0  and sr.ScreeningSectionID NOT IN('PHQ-9','SIH','TCC','GAD-7')  and r.CreatedDate >= @StartDate  and r.CreatedDate < @EndDate  group by r.FirstName, r.LastName, r.MiddleName, r.Birthday, sr.ScreeningSectionID   
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID NOT IN('PHQ-9','SIH','TCC','GAD-7') 

)
Select 
    r.ScreeningSectionID,
    r.Name,
    r.Indicates,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
    COUNT_BIG(*) as Total,
    ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.ScoreLevel, r.Indicates
ORDER BY ScreeningSectionID, ScoreLevel
