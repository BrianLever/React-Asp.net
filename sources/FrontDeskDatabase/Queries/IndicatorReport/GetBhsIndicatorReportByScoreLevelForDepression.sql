declare @StartDate datetime = '2018-10-01',
@EndDate datetime = '2019-09-30',
@LocationID int = 1

;
WITH tblResults(ScreeningSectionID, QuestionsAsked, ScoreLevel, Name, Indicates, IsPositive, TotalCountItem) AS
(

SELECT 
		sl.ScreeningSectionID,
		sr.QuestionsAsked,
		sl.ScoreLevel,
		sl.Name,
        sl.Indicates,
		Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
		Convert(int, (CASE WHEN sr.ScoreLevel IS NOT NULL THEN 1 ELSE 0 END)) as TotalCountItem
	FROM dbo.ScreeningScoreLevel sl 
		LEFT JOIN (
  
SELECT
    sr.ScreeningSectionID,
	q.QuestionsAsked,
	sr.ScoreLevel
FROM dbo.ScreeningSectionResult sr 
	INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
	INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
	CROSS APPLY (SELECT CASE WHEN COUNT(qr.QuestionID) = 2 THEN 2 ELSE 10 END as QuestionsAsked FROM dbo.ScreeningSectionQuestionResult qr 
		WHERE qr.ScreeningResultID = r.ScreeningResultID AND
			qr.ScreeningSectionID = sr.ScreeningSectionID) as q
	WHERE  r.IsDeleted = 0  and sr.ScreeningSectionID IN('PHQ-9')  
	AND k.BranchLocationID = @LocationID  AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate    
        
) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
	
	WHERE sl.ScreeningSectionID IN('PHQ-9') 

)
Select 
	r.ScreeningSectionID,
	r.QuestionsAsked,
    r.Name,
    r.Indicates,
   	SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
	SUM(TotalCountItem) as Total,
    ScoreLevel
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.QuestionsAsked, r.ScoreLevel, r.Indicates
ORDER BY ScreeningSectionID, QuestionsAsked, ScoreLevel
