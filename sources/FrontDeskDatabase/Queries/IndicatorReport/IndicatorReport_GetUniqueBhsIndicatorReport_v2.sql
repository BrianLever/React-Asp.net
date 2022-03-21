declare @StartDate datetimeoffset(7),
@EndDate datetimeoffset(7);

set @StartDate = '2017-07-01';
set @EndDate = '2018-06-30';
;


WITH tblResults(ScreeningSectionID, QuestionID, AnswerValue, PositiveScore, FirstName, LastName, MiddleName, Birthday) AS
(  
SELECT 
 sr.ScreeningSectionID
	,q.QuestionID
    ,Convert(int, (CASE WHEN MAX(qr.AnswerValue) > 0 THEN 1 ELSE 0 END)) as AnswerValue
    ,Convert(int, (CASE WHEN MAX(qr.AnswerValue) > 0 THEN 1 ELSE 0 END)) as PositiveScore
	,r.FirstName
	,r.LastName
	,r.MiddleName
	,r.Birthday
FROM 
	dbo.ScreeningSectionQuestionResult qr 
	INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID
	INNER JOIN dbo.ScreeningSectionResult sr ON sr.ScreeningResultID = qr.ScreeningResultID AND sr.ScreeningSectionID = qr.ScreeningSectionID
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID
 where  q.IsMainQuestion = 1  AND r.IsDeleted = 0  AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate 
 group by sr.ScreeningSectionID,q.QuestionID,r.FirstName,r.LastName,r.MiddleName,r.Birthday
 )
SELECT q.ScreeningSectionID
      ,q.QuestionID
      ,q.QuestionText
      ,ISNULL(t1.AnswerValue,0) as AnswerValue
      ,ISNULL(t1.AnswerCount, 0) as AnswerCount
      ,ISNULL(t2.PositiveScore, 0) as PositiveScore
      ,ISNULL(t2.PositiveScoreCount, 0) as PositiveScoreCount
      ,q.PreambleText
FROM dbo.ScreeningSectionQuestion q
	INNER JOIN dbo.ScreeningSection s ON s.ScreeningSectionID = q.ScreeningSectionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
        ,r.QuestionID
		,r.AnswerValue
        ,COUNT_BIG(r.AnswerValue) as AnswerCount
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionID, r.AnswerValue
) t1 ON q.ScreeningSectionID = t1.ScreeningSectionID AND q.QuestionID = t1.QuestionID
LEFT JOIN (
    SELECT 
        r.ScreeningSectionID
		,r.QuestionID
        ,r.PositiveScore
        ,COUNT_BIG(r.PositiveScore) as PositiveScoreCount
    FROM tblResults r
    GROUP BY r.ScreeningSectionID, r.QuestionID, r.PositiveScore
) t2 ON q.ScreeningSectionID = t2.ScreeningSectionID AND q.QuestionID = t2.QuestionID 

WHERE s.ScreeningID = 'BHS' AND q.IsMainQuestion = 1
ORDER BY s.OrderIndex ASC, q.OrderIndex ASC, t1.AnswerValue ASC, t2.PositiveScore ASC
