declare @StartDate datetimeoffset(7),
@EndDate datetimeoffset(7);

set @StartDate = '2017-07-01';
set @EndDate = '2018-06-30';
;
SELECT	sr.ScreeningSectionID
	            ,qr.QuestionID
	            ,MAX(qr.AnswerValue) AS AnswerValue
	            ,r.FirstName
	            ,r.LastName
	            ,r.MiddleName
	            ,r.Birthday

			FROM dbo.ScreeningSectionResult sr 
				INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
				LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
				INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID 
			WHERE  sr.ScreeningSectionID = 'TCC' AND r.IsDeleted = 0  
			AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate  
			group by 
				sr.ScreeningSectionID,qr.QuestionID,r.FirstName,r.LastName,r.MiddleName,r.Birthday 
			order by LastName, FIRSTName, QuestionID;
			

WITH tblResults(ScreeningSectionID,	QuestionID, QuestionText, IsPositive) AS
(
		SELECT 
		sl.ScreeningSectionID,
		sl.QuestionID,
		sl.QuestionText,
		ISNULL(sr.AnswerValue, 0) as IsPositive
	FROM dbo.ScreeningSectionQuestion sl 
		LEFT JOIN (
			SELECT	sr.ScreeningSectionID
	            ,qr.QuestionID
	            ,MAX(qr.AnswerValue) AS AnswerValue
	            ,r.FirstName
	            ,r.LastName
	            ,r.MiddleName
	            ,r.Birthday

			FROM dbo.ScreeningSectionResult sr 
				INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
				LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
				INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID 
			WHERE  sr.ScreeningSectionID = 'TCC' AND r.IsDeleted = 0  
			AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate  
			group by 
				sr.ScreeningSectionID,qr.QuestionID,r.FirstName,r.LastName,r.MiddleName,r.Birthday 
		) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID AND (sr.QuestionID IS NULL OR sl.QuestionID = sr.QuestionID)
	WHERE sl.ScreeningSectionID = 'TCC' 
)
SELECT
	r.ScreeningSectionID,
	r.QuestionText,
	r.QuestionID,
	SUM(Convert(bigint,ISNULL(IsPositive, 0))) as TotalPositive,
	COUNT_BIG(*) as Total
FROM tblResults r
	
GROUP BY r.QuestionText, r.ScreeningSectionID, r.QuestionID
ORDER BY ScreeningSectionID, QuestionID
