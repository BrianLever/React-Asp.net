declare @StartDate datetimeoffset(7),
@EndDate datetimeoffset(7);

set @StartDate = '2017-07-01';
set @EndDate = '2018-06-30';
;

  
SELECT 
    MAX(qr.AnswerValue) as AnswerValue
	,q.AnswerScaleID
    ,r.FirstName
	,r.LastName
	,r.MiddleName
	,r.Birthday
FROM dbo.ScreeningSectionQuestionResult qr
	INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID 
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = qr.ScreeningResultID 
 where  r.IsDeleted = 0  AND qr.ScreeningSectionID = 'PHQ-9' AND qr.QuestionID = 9  
 AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate   
 GROUP BY r.FirstName,r.LastName, r.MiddleName, r.Birthday,q.AnswerScaleID
 ;

WITH tblResults(AnswerScaleID,	AnswerScaleOptionID, OptionValue, Name, IsPositive) AS
(

SELECT 
	ao.AnswerScaleID,
	ao.AnswerScaleOptionID,
	ao.OptionValue,
	ao.OptionText,
	Convert(int, (CASE WHEN sr.AnswerValue = ao.OptionValue THEN 1 ELSE 0 END)) as IsPositive
	FROM dbo.ScreeningSectionQuestion question 
		INNER JOIN dbo.AnswerScale sc ON question.AnswerScaleID = sc.AnswerScaleID 
		INNER JOIN dbo.AnswerScaleOption ao ON sc.AnswerScaleID = ao.AnswerScaleID 
	LEFT JOIN (
  
SELECT 
    MAX(qr.AnswerValue) as AnswerValue
	,q.AnswerScaleID
    ,r.FirstName
	,r.LastName
	,r.MiddleName
	,r.Birthday
FROM dbo.ScreeningSectionQuestionResult qr
	INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID 
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = qr.ScreeningResultID 
 where  r.IsDeleted = 0  AND qr.ScreeningSectionID = 'PHQ-9' AND qr.QuestionID = 9  
 AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate   
 GROUP BY r.FirstName,r.LastName, r.MiddleName, r.Birthday,q.AnswerScaleID
        ) sr ON sr.AnswerScaleID = ao.AnswerScaleID

WHERE question.ScreeningSectionID = 'PHQ-9' AND question.QuestionID = 9 
)
Select 
	r.AnswerScaleID,
    r.Name,
   	SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
	COUNT_BIG(*) as Total
FROM tblResults r
GROUP BY r.Name, r.AnswerScaleID, r.AnswerScaleOptionID, r.OptionValue
ORDER BY AnswerScaleOptionID


