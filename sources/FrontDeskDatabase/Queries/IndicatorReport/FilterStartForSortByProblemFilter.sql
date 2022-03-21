declare @startRowIndex int = 1, 
@maxRows int = 100,
@StartDate date = '07/01/2017',
@EndDate date = '07/01/2018',
@MinScoreLevel int = 1


;
SELECT 
	ssr.ScreeningSectionID,
	ssr.ScoreLevel,
	COUNT(*) as score_count
FROM ScreeningResult r 
	INNER JOIN dbo.ScreeningSectionResult ssr ON ssr.ScreeningResultID = r.ScreeningResultID 
	INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
 where r.IsDeleted = 0 AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate and ssr.ScoreLevel > 0 
 group by ssr.ScreeningSectionID, ssr.ScoreLevel 
  UNION ALL
 SELECT 
	'TCC_',
	q.QuestionID,
	COUNT(*) as score_count
FROM ScreeningResult r 
	INNER JOIN dbo.ScreeningSectionResult ssr ON ssr.ScreeningResultID = r.ScreeningResultID
	INNER JOIN dbo.ScreeningSectionQuestionResult q ON ssr.ScreeningResultID = q.ScreeningResultID and ssr.ScreeningSectionID = q.ScreeningSectionID 
	INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID
where r.IsDeleted = 0 AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate and q.AnswerValue > 0  and ssr.ScreeningSectionID = 'TCC'
group by q.ScreeningSectionID, q.QuestionID 
 order by ssr.ScreeningSectionID, ssr.ScoreLevel
 