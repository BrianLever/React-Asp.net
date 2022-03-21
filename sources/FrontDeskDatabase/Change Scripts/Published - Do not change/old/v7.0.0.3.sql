ALTER VIEW [dbo].[vBhsVisitsWithDemographics] AS
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
GO




---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '7.0.0.3')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('7.0.0.3');