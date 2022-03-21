

declare @StartDate datetime = '2019-10-01',
@EndDate datetime = '2020-09-30',
@LocationID int = 1
;



WITH tblResults(ScreeningSectionID,QuestionsAsked,ScoreLevel, Name, Indicates, IsPositive, Age) AS
(

SELECT 
        sl.ScreeningSectionID,
		sr.QuestionsAsked,
		sl.ScoreLevel,
        sl.Name,
        sl.Indicates,
        Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive,
        ISNULL(sr.Age, 0) as Age
    FROM dbo.ScreeningScoreLevel sl 
        LEFT JOIN (
  
SELECT
    sr.ScreeningSectionID,
	q.QuestionsAsked,
    MAX(sr.ScoreLevel) as ScoreLevel,
    [dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.ScreeningSectionResult sr 
    INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
	CROSS APPLY (SELECT CASE WHEN COUNT(qr.QuestionID) = 2 THEN 2 ELSE 10 END as QuestionsAsked FROM dbo.ScreeningSectionQuestionResult qr 
		WHERE qr.ScreeningResultID = r.ScreeningResultID AND
			qr.ScreeningSectionID = sr.ScreeningSectionID) as q
 INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID 
 where  r.IsDeleted = 0  and sr.ScreeningSectionID IN('PHQ-9')  and k.BranchLocationID = @LocationID  and r.CreatedDate >= @StartDate  and r.CreatedDate < @EndDate  
 group by r.FirstName,r.LastName,r.MiddleName,r.Birthday,sr.ScreeningSectionID, q.QuestionsAsked   
        ) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
    WHERE sl.ScreeningSectionID IN('PHQ-9') 

)
Select 
    r.ScreeningSectionID,
	r.QuestionsAsked,
    r.Name,
    r.Indicates,
    r.Age,
    SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive
FROM tblResults r
GROUP BY r.Name, r.ScreeningSectionID, r.QuestionsAsked, r.ScoreLevel, r.Indicates, r.Age
ORDER BY ScreeningSectionID, QuestionsAsked, ScoreLevel, Age
