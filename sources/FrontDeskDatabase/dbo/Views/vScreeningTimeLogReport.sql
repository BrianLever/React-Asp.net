CREATE VIEW [dbo].[vScreeningTimeLogReport]
AS 
SELECT
s.ScreeningSectionName,
s.ScreeningSectionID,
s.OrderIndex,
tlog.DurationInSeconds,
r.ScreeningResultID,
r.CreatedDate,
r.PatientName,
r.Birthday,
k.BranchLocationID
FROM dbo.ScreeningTimeLog tlog
	INNER JOIN dbo.ScreeningResult r ON tlog.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.Kiosk k ON k.KioskID = r.KioskID
	LEFT JOIN dbo.ScreeningSection s ON tlog.ScreeningSectionID = s.ScreeningSectionID    
;
GO