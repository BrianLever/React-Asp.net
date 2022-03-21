-- =============================================
-- KSA, July 8 2012
-- Script returns actuall screening resuilts by Section Severity Level to test the Indicator Report
-- =============================================

declare @StartDate datetimeoffset(7),
@EndDate datetimeoffset(7);

set @StartDate = '2012-07-01';
set @EndDate = '2013-06-30';

SELECT 
		sl.ScreeningSectionID,
		sl.Name as ScoreName,
		sr.FirstName,
		sr.LastName,
		Convert(int, (CASE WHEN sr.ScoreLevel = sl.ScoreLevel THEN 1 ELSE 0 END)) as IsPositive
	FROM dbo.ScreeningScoreLevel sl 
		LEFT JOIN (
			SELECT
				sr.ScreeningSectionID,
				sr.ScoreLevel,
				r.FirstName,
				r.LastName
			FROM dbo.ScreeningSectionResult sr 
				INNER JOIN dbo.ScreeningResult r ON sr.ScreeningResultID = r.ScreeningResultID 
				INNER JOIN dbo.Kiosk k ON r.KioskID = k.KioskID 
			WHERE  r.IsDeleted = 0  
			AND r.CreatedDate >= @StartDate  AND r.CreatedDate < @EndDate   
		) sr ON sl.ScreeningSectionID = sr.ScreeningSectionID 
	--WHERE sl.ScreeningSectionID NOT IN('SIH','TCC') 
	order by ScreeningSectionID, ScoreName
