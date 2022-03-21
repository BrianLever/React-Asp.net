declare @StartDate datetime = '2018-10-01',
@EndDate datetime = '2019-09-30',
@LocationID int = 1
;

WITH tblResults(ScreeningSectionID,	QuestionID, QuestionText, IsPositive,TotalCountItem) AS
(

SELECT 
	sl.ScreeningSectionID,
	sl.QuestionID,
	sl.QuestionText,
	ISNULL(sr.AnswerValue, 0) as IsPositive,
    Convert(int, (CASE WHEN sr.AnswerValue IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
FROM dbo.ScreeningSectionQuestion sl 
	LEFT JOIN (
  
SELECT
	sr.ScreeningSectionID,
	qr.QuestionID,
	qr.AnswerValue
FROM dbo.ScreeningSectionResult sr 
	INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
	LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
	LEFT JOIN dbo.ScreeningSectionQuestion q ON qr.ScreeningSectionID = q.ScreeningSectionID AND qr.QuestionID = q.QuestionID

  where  sr.ScreeningSectionID = 'TCC'  and q.IsMainQuestion = 0  and r.IsDeleted = 0  and r.CreatedDate >= @StartDate  and r.CreatedDate < @EndDate    
    ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID AND (sr.QuestionID IS NULL OR sl.QuestionID = sr.QuestionID)
WHERE sl.ScreeningSectionID = 'TCC' and sl.IsMainQuestion=0

)
SELECT 
	r.ScreeningSectionID,
	r.QuestionText,
	SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
	SUM(TotalCountItem) as Total,
	r.QuestionID
FROM tblResults r
GROUP BY r.QuestionText, r.ScreeningSectionID, r.QuestionID
ORDER BY ScreeningSectionID, QuestionID
