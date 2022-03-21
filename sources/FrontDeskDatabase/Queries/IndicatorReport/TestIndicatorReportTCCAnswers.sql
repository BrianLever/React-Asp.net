-- =============================================
-- Script Template
-- =============================================
declare @StartDate datetimeoffset(7),
@EndDate datetimeoffset(7);

set @StartDate = '2012-07-01';
set @EndDate = '2013-06-30';


			SELECT
				sr.ScreeningSectionID,
				qr.QuestionID,
				qr.AnswerValue,
				r.LastName,
				r.FirstName,
				r.CreatedDate
			FROM dbo.ScreeningSectionResult sr 
				INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
				LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
				INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID 
			WHERE sr.ScreeningSectionID = 'TCC' AND  r.IsDeleted = 0  
			AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate   
;			
		SELECT 
		sl.ScreeningSectionID,
		sl.QuestionID,
		sl.QuestionText,
		ISNULL(sr.AnswerValue,0) as IsPositive,
		sr.FirstName,
		sr.LastName,
		sr.CreatedDate
	FROM dbo.ScreeningSectionQuestion sl 
		LEFT JOIN (
			SELECT
				sr.ScreeningSectionID,
				qr.QuestionID,
				qr.AnswerValue,
				r.LastName,
				r.FirstName,
				r.CreatedDate
			FROM dbo.ScreeningSectionResult sr 
				INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
				LEFT JOIN dbo.ScreeningSectionQuestionResult qr ON qr.ScreeningResultID = sr.ScreeningResultID AND qr.ScreeningSectionID = sr.ScreeningSectionID
				INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID 
			WHERE sr.ScreeningSectionID = 'TCC' AND r.IsDeleted = 0  
			AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate   
		) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID and (sr.QuestionID IS NULL OR sl.QuestionID = sr.QuestionID)
	WHERE sl.ScreeningSectionID = 'TCC'