
CREATE PROCEDURE [dbo].[uspGetHistoryOfVisitsAndFollowUps]
    @LastName varchar(128),
    @FirstName varchar(128),
    @MiddleName varchar(128),
    @Birthday date,
    @StartDate datetimeoffset,
    @EndDate datetimeoffset
AS
SELECT 
v.ID
,v.CompleteDate
,v.DischargedID
,v.NewVisitReferralRecommendationAcceptedID
,'Visit' as ItemType
FROM dbo.BhsVisit v
	INNER JOIN dbo.ScreeningResult sr ON sr.ScreeningResultID = v.ScreeningResultID
WHERE 
sr.IsDeleted = 0 AND v.CompleteDate > @StartDate  AND v.CompleteDate < @EndDate AND
dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName) = dbo.fn_GetPatientName(sr.LastName, sr.FirstName, sr.MiddleName) AND @Birthday = sr.Birthday
UNION ALL
SELECT 
f.ID
,f.CompleteDate
,f.DischargedID
,f.NewVisitReferralRecommendationAcceptedID
,'Follow-Up' as ItemType
FROM dbo.BhsFollowUp f
	INNER JOIN dbo.ScreeningResult sr ON sr.ScreeningResultID = f.ScreeningResultID
WHERE 
sr.IsDeleted = 0 AND f.CompleteDate > @StartDate AND f.CompleteDate < @EndDate AND
dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName) = dbo.fn_GetPatientName(sr.LastName, sr.FirstName, sr.MiddleName) AND @Birthday = sr.Birthday
ORDER BY CompleteDate DESC
