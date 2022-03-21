---- BhsFollowUp

IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BhsFollowUp') AND name = 'NewFollowUpDate')
SET NOEXEC ON
GO

ALTER TABLE [dbo].BhsFollowUp ADD NewFollowUpDate DateTimeOffset NULL;
GO

UPDATE [dbo].BhsFollowUp SET NewFollowUpDate = DATEADD(d, 30, NewVisitDate) WHERE NewVisitDate IS NOT NULL;

GO
SET NOEXEC OFF
GO


---- BhsVisit

IF EXISTS(SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('BhsVisit') AND name = 'FollowUpDate')
SET NOEXEC ON
GO

ALTER TABLE [dbo].BhsVisit ADD FollowUpDate DateTimeOffset NULL;
GO

UPDATE [dbo].BhsVisit SET FollowUpDate = DATEADD(d, 30, NewVisitDate) WHERE NewVisitDate IS NOT NULL;

GO
SET NOEXEC OFF
GO


IF OBJECT_ID('dbo.vBhsVisitsWithDemographics') IS NOT NULL
DROP VIEW dbo.vBhsVisitsWithDemographics
GO


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
,r.Birthday
,r.PatientName as FullName
,pd.ID as DemographicsID
,pd.ScreeningDate as DemographicsScreeningDate
,pd.CreatedDate as DemographicsCreateDate
,pd.CompleteDate as  DemographicsCompleteDate
FROM 
	dbo.BhsVisit v 
	INNER JOIN dbo.ScreeningResult r ON v.ScreeningResultID = r.ScreeningResultID
	LEFT JOIN dbo.BhsDemographics pd ON pd.Birthday = r.Birthday AND pd.PatientName = r.PatientName

WHERE r.IsDeleted = 0 and r.IsDeleted = 0 
GO


print 'Populating dbo.[EducationLevel]...'
GO

MERGE INTO dbo.[EducationLevel] as target
USING (VALUES
(1, 'Elementary school', 2),
(2, 'Some high school', 3),
(3, 'Graduated high school', 4),
(4, 'Some college', 5),
(5, 'Technical school', 6),
(6, 'AA degree', 7),
(7, 'Bachelor''s degree', 8),
(8, 'Master''s degree', 9),
(10, 'Doctoral degree', 11)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE ;
GO





-------------------------------
IF OBJECT_ID('dbo.Tribe') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [dbo].[Tribe]
(
    [Value] nvarchar(128) NOT NULL PRIMARY KEY
)
GO

SET NOEXEC OFF
GO

IF OBJECT_ID('dbo.County') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [dbo].[County]
(
    [Value] nvarchar(128) NOT NULL PRIMARY KEY
)
GO

SET NOEXEC OFF
GO





---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.1.30')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.1.30');