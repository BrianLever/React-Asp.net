declare @StartDate datetimeoffset(7),
@EndDate datetimeoffset(7);

set @StartDate = '2017-07-01';
set @EndDate = '2018-06-30';
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
 where  q.IsMainQuestion = 1  AND r.IsDeleted = 0  AND qr.AnswerValue > 0  AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate  
 group by r.FirstName,r.LastName,r.MiddleName,r.Birthday,qr.ScreeningSectionID,qr.QuestionID

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

WHERE s.ScreeningID = 'BHS' AND q.IsMainQuestion = 1
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, t2.Age ASC