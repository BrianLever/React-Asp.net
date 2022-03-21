
declare @StartDate datetime = '2019-07-01',
@EndDate datetime = '2020-06-30',
@LocationID int = 1

;


WITH tblResults(ScreeningSectionID, QuestionId, AnswerValue, Age, PositiveScore) AS
(  
SELECT 
    qr.ScreeningSectionID
    ,qr.QuestionID
    ,MAX(qr.AnswerValue) as AnswerValue 
    ,[dbo].[fn_GetAge](r.Birthday) as Age
    ,1 as PositiveScore
FROM dbo.ScreeningSectionQuestionResult qr
    INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID
    INNER JOIN dbo.ScreeningResult r ON qr.ScreeningResultID = r.ScreeningResultID
 INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID where  q.IsMainQuestion = 1  and r.IsDeleted = 0  and qr.AnswerValue > 0  and k.BranchLocationID = @LocationID  and r.CreatedDate >= @StartDate  and r.CreatedDate < @EndDate  
 group by  r.FirstName,r.LastName,r.MiddleName,r.Birthday,qr.ScreeningSectionID,qr.QuestionID   
 )
SELECT q.ScreeningSectionID
      ,q.QuestionId
      ,q.QuestionText
       ,ISNULL(t2.PositiveScoreCount, 0) as PositiveScoreCount
       ,ISNULL(t2.Age, 0) as Age
      ,q.PreambleText
FROM dbo.ScreeningSectionQuestion q
    INNER JOIN dbo.ScreeningSection s ON s.ScreeningSectionID = q.ScreeningSectionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionId
        ,r.PositiveScore
        ,COUNT_BIG(r.PositiveScore) as PositiveScoreCount
        ,r.Age
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionId, r.PositiveScore, r.Age 
) t2 ON q.ScreeningSectionID = t2.ScreeningSectionID AND q.QuestionID = t2.QuestionId 

WHERE s.ScreeningID = 'BHS' AND q.IsMainQuestion = 1 AND q.ScreeningSectionID NOT IN ('PHQ-9','GAD-7')
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, t2.Age ASC
