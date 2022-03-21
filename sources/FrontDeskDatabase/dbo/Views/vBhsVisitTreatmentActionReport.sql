CREATE VIEW [dbo].[vBhsVisitTreatmentActionReport]
AS
WITH tblTable(CategoryID, TreatmentID, Age)
AS(
SELECT 
	'TreatmentAction1' as CategoryID
	,t.ID
	,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = v.ScreeningResultID
	INNER JOIN dbo.TreatmentAction t ON t.ID = v.TreatmentAction1ID 
UNION ALL
SELECT 
	'TreatmentAction2' as CategoryID
	,t.ID
	,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = v.ScreeningResultID
	INNER JOIN dbo.TreatmentAction t ON t.ID = v.TreatmentAction2ID 
UNION ALL
SELECT 
	'TreatmentAction3' as CategoryID
	,t.ID
	,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = v.ScreeningResultID
	INNER JOIN dbo.TreatmentAction t ON t.ID = v.TreatmentAction3ID 
UNION ALL
SELECT 
	'TreatmentAction4' as CategoryID
	,t.ID
	,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = v.ScreeningResultID
	INNER JOIN dbo.TreatmentAction t ON t.ID = v.TreatmentAction4ID 
UNION ALL
SELECT 
	'TreatmentAction5' as CategoryID
	,t.ID
	,[dbo].[fn_GetAge](r.Birthday) as Age
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult r ON r.ScreeningResultID = v.ScreeningResultID
	INNER JOIN dbo.TreatmentAction t ON t.ID = v.TreatmentAction5ID 
)
SELECT* FROM tblTable t1
WHERE t1.CompleteDate IS NOT NULL
