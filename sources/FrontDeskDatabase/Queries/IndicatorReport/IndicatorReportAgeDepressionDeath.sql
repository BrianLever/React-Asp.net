declare @StartDate datetime = '7/1/2015',
@EndDate datetime = '7/1/2016'
;

WITH tblResults(AnswerScaleID, AnswerScaleOptionID, OptionValue, Name, IsPositive, Age) AS
(

SELECT 
	ao.AnswerScaleID,
	ao.AnswerScaleOptionID,
	ao.OptionValue,
	ao.OptionText,
	Convert(int, (CASE WHEN sr.AnswerValue = ao.OptionValue THEN 1 ELSE 0 END)) as IsPositive,
	sr.Age
	FROM dbo.ScreeningSectionQuestion question 
		INNER JOIN dbo.AnswerScale sc ON question.AnswerScaleID = sc.AnswerScaleID 
		INNER JOIN dbo.AnswerScaleOption ao ON sc.AnswerScaleID = ao.AnswerScaleID 
	LEFT JOIN (
  
		SELECT
			qr.AnswerValue
			,q.AnswerScaleID
			,[dbo].[fn_GetAge](r.Birthday) as Age
		FROM dbo.ScreeningSectionQuestionResult qr
			INNER JOIN dbo.ScreeningSectionQuestion q ON q.ScreeningSectionID = qr.ScreeningSectionID AND q.QuestionID = qr.QuestionID 
			INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = qr.ScreeningResultID 
		 where  r.IsDeleted = 0  AND qr.ScreeningSectionID = 'PHQ-9' AND qr.QuestionID = 9  AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate   
				) sr ON sr.AnswerScaleID = ao.AnswerScaleID

		WHERE question.ScreeningSectionID = 'PHQ-9' AND question.QuestionID = 9 
)
Select 
	r.AnswerScaleID
    ,r.Name
	,r.Age
   	,SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive
	,COUNT_BIG(*) as Total
FROM tblResults r
GROUP BY r.Name, r.AnswerScaleID, r.AnswerScaleOptionID, r.OptionValue, r.Age
ORDER BY AnswerScaleOptionID


