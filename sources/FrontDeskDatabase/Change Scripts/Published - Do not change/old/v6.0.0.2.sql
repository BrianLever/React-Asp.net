IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BhsFollowUp') AND name = 'ParentFollowUpID')
SET NOEXEC ON
GO

ALTER TABLE [dbo].[BhsFollowUp] ADD ParentFollowUpID bigint NULL;

ALTER TABLE [dbo].[BhsFollowUp] ADD CONSTRAINT FK_BhsFollowUp__BhsFollowUp 
        FOREIGN KEY (ParentFollowUpID)
		REFERENCES dbo.BhsFollowUp(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION




SET NOEXEC OFF
GO


IF OBJECT_ID('[dbo].[uspGetHistoryOfPreviousNotEmptyScreenings]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetHistoryOfPreviousNotEmptyScreenings]
GO

CREATE PROCEDURE [dbo].[uspGetHistoryOfPreviousNotEmptyScreenings]
    @LastName varchar(128),
    @FirstName varchar(128),
    @MiddleName varchar(128),
    @Birthday date,
    @EndDate datetimeoffset,
    @Limit int
AS
SELECT TOP (@Limit)
sr.ScreeningResultID
,sr.CreatedDate
,CASE WHEN SUM(ISNULL(ssr.ScoreLevel,0)) > 0 THEN 1 ELSE 0 END AS Positive
,CASE WHEN COUNT(ssr.ScoreLevel) > 0 THEN 1 ELSE 0 END AS HasAnyScreening
FROM dbo.ScreeningResult sr
    LEFT JOIN dbo.ScreeningSectionResult ssr ON sr.ScreeningResultID = ssr.ScreeningResultID
WHERE 
sr.IsDeleted = 0 AND sr.CreatedDate < @EndDate AND
dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName) = dbo.fn_GetPatientName(sr.LastName, sr.FirstName, sr.MiddleName) AND @Birthday = sr.Birthday
GROUP BY sr.ScreeningResultID, sr.CreatedDate, sr.ExportDate, sr.ExportedToHRN
HAVING (CASE WHEN COUNT(ssr.ScoreLevel) > 0 THEN 1 ELSE 0 END) > 0
ORDER BY sr.CreatedDate DESC;

RETURN 0
GO

IF OBJECT_ID('[dbo].[uspGetHistoryOfVisitsAndFollowUps]') IS NOT NULL
DROP PROCEDURE [dbo].[uspGetHistoryOfVisitsAndFollowUps]
GO

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
GO


------------------------------------------------

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsFollowUp TO frontdesk_appuser
GRANT EXECUTE ON  [dbo].[uspGetHistoryOfPreviousNotEmptyScreenings] TO frontdesk_appuser
GRANT EXECUTE ON  [dbo].[uspGetHistoryOfVisitsAndFollowUps] TO frontdesk_appuser



---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.0.2')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.0.2');