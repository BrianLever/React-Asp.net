declare @StartDate datetime = '2019-07-01',
@EndDate datetime = '2020-06-30',
@LocationID int = 1

;
WITH tblResults(Cnt, Age) AS
(  
SELECT 
	(CASE WHEN d.ThirtyDatyFollowUpFlag = 0 THEN 0 ELSE 1 END) as ThirtyDatyFollowUpFlag
    ,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit d
    INNER JOIN dbo.ScreeningResult r ON d.ScreeningResultID=r.ScreeningResultID
  where  d.CompleteDate IS NOT NULL  and d.CreatedDate >= @StartDate  and d.CreatedDate < @EndDate  group by d.ID, r.PatientName, r.Birthday, d.ThirtyDatyFollowUpFlag   )
SELECT 
    ISNULL(t2.Cnt, 0) as TotalCount
    ,ISNULL(t2.Age, 0) as Age
FROM tblResults t2
ORDER BY t2.Age ASC