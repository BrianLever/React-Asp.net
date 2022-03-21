
CREATE VIEW [dbo].[vBhsVisitsWithDemographics] AS
SELECT
v.ID
,v.ScreeningResultID
,v.CreatedDate
,v.ScreeningDate
,v.CompleteDate
,CASE WHEN r.StreetAddress IS NOT NULL THEN 1 ELSE 0 END AS HasAddress
,1 as IsVisitRecordType
,v.LocationID
,l.Name as LocationName
,r.Birthday
,r.PatientName as FullName
,pd.ID as DemographicsID
,pd.ScreeningDate as DemographicsScreeningDate
,pd.CreatedDate as DemographicsCreateDate
,pd.CompleteDate as  DemographicsCompleteDate
FROM 
	dbo.BhsVisit v 
	INNER JOIN dbo.ScreeningResult r ON v.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = v.LocationID
	LEFT JOIN dbo.BhsDemographics pd ON pd.Birthday = r.Birthday AND pd.PatientName = r.PatientName

WHERE r.IsDeleted = 0 and r.IsDeleted = 0 
UNION ALL
SELECT
NULL AS ID
,pd.ScreeningResultID
,r.CreatedDate
,pd.ScreeningDate as ScreeningDate
,pd.CompleteDate
,0 As HasAddress
,0 as IsVisitRecordType
,pd.LocationID
,l.Name as LocationName
,r.Birthday
,r.PatientName as FullName
,pd.ID as DemographicsID
,pd.ScreeningDate as DemographicsScreeningDate
,pd.CreatedDate as DemographicsCreateDate
,pd.CompleteDate as  DemographicsCompleteDate
FROM 
	dbo.BhsDemographics pd
	INNER JOIN dbo.ScreeningResult r ON pd.ScreeningResultID = r.ScreeningResultID
    INNER JOIN dbo.BranchLocation l ON l.BranchLocationID = pd.LocationID
WHERE r.IsDeleted = 0 and r.IsDeleted = 0 AND
NOT EXISTS( 
SELECT 1 FROM dbo.BhsVisit v 
INNER JOIN dbo.ScreeningResult r2 ON v.ScreeningResultID = r2.ScreeningResultID 
WHERE r2.PatientName = pd.PatientName AND r2.Birthday = pd.Birthday)
GO
