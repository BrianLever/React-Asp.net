CREATE VIEW [dbo].[vBhsVisitsAndDemographics] AS
WITH tblResult(ID, ScreeningResultID, CreatedDate,ScreeningDate, CompleteDate, HasAddress, IsVisitRecordType, LocationID, Birthday, FullName) AS
(
SELECT
v.ID
,v.ScreeningResultID
,v.CreatedDate
,v.ScreeningDate
,v.CompleteDate
,CASE WHEN r.StreetAddress IS NOT NULL THEN 1 ELSE 0 END AS HasAddress
,1 as IsVisitRecordType
,v.LocationID
,r.Birthday
,dbo.fn_GetPatientName(r.LastName, r.FirstName, r.MiddleName) as FullName

FROM 
	dbo.BhsVisit v 
	INNER JOIN dbo.ScreeningResult r ON v.ScreeningResultID = r.ScreeningResultID
WHERE r.IsDeleted = 0 and r.IsDeleted = 0 
UNION
SELECT
v.ID
,v.ScreeningResultID
,v.CreatedDate
,v.ScreeningDate
,v.CompleteDate
,CASE WHEN v.StreetAddress IS NOT NULL THEN 1 ELSE 0 END AS HasAddress
,0 as IsVisitRecordType
,v.LocationID
,v.Birthday
,dbo.fn_GetPatientName(v.LastName, v.FirstName, v.MiddleName) as FullName
FROM 
	dbo.BhsDemographics v 
)
SELECT t.* 
FROM tblResult t
GO

