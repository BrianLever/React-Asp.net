ALTER TABLE dbo.BhsDemographics ALTER COLUMN ScreeningResultID bigint NULL;


GO

ALTER VIEW [dbo].[vBhsVisitsAndDemographics] AS
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

IF OBJECT_ID('[dbo].[PatientAttendedVisit]') IS NOT NULL
SET NOEXEC ON;
GO

CREATE TABLE [dbo].[PatientAttendedVisit]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_PatientAttendedVisit PRIMARY KEY CLUSTERED (ID),
);
GO
SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[FollowUpContactOutcome]') IS NOT NULL
SET NOEXEC ON;
GO

CREATE TABLE [dbo].[FollowUpContactOutcome]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_FollowUpContactOutcome PRIMARY KEY CLUSTERED (ID),
);
GO
SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[BhsFollowUp]') IS NOT NULL
SET NOEXEC ON;
GO

CREATE TABLE [dbo].[BhsFollowUp]
(
    ID bigint NOT NULL IDENTITY(1,1),
	ScreeningResultID bigint NOT NULL, /*foreign key*/
    BhsVisitID bigint NOT NULL, /*foreign key*/
	VisitDate DateTimeOffset NOT NULL,
    CreatedDate DateTimeOffset NOT NULL,
    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,

    PatientAttendedVisitID int NULL, /*foreign key*/
    FollowUpContactDate datetimeoffset NULL,
    FollowUpContactOutcomeID int NULL,/*foreign key*/

    NewVisitReferralRecommendationID int NULL, /*foreign key*/
    NewVisitReferralRecommendationDescription nvarchar(max) NULL,

    NewVisitReferralRecommendationAcceptedID int NULL, /*foreign key*/
    ReasonNewVisitReferralRecommendationNotAcceptedID int NULL, /*foreign key*/

    NewVisitDate DateTimeOffset NULL,
    DischargedID int NULL, /*foreign key*/
    ThirtyDatyFollowUpFlag bit NOT NULL,

    Notes nvarchar(max) NULL,

    CONSTRAINT PK_BhsFollowUp PRIMARY KEY(ID),
	CONSTRAINT FK_BhsFollowUp__ScreeningResult FOREIGN KEY (ScreeningResultID)
		REFERENCES dbo.ScreeningResult(ScreeningResultID) ON UPDATE NO ACTION ON DELETE NO ACTION,
	CONSTRAINT FK_BhsFollowUp__BhsVisit FOREIGN KEY (BhsVisitID)
		REFERENCES dbo.BhsVisit(ID) ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_BhsFollowUp__PatientAttendedVisit FOREIGN KEY (PatientAttendedVisitID)
		REFERENCES dbo.PatientAttendedVisit(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,

     CONSTRAINT FK_BhsFollowUp__FollowUpContactOutcome FOREIGN KEY (FollowUpContactOutcomeID)
		REFERENCES dbo.FollowUpContactOutcome(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_BhsFollowUp__NewVisitReferralRecommendation 
        FOREIGN KEY (NewVisitReferralRecommendationID)
		REFERENCES dbo.NewVisitReferralRecommendation(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
     CONSTRAINT FK_BhsFollowUp__NewVisitReferralRecommendationAccepted 
        FOREIGN KEY (NewVisitReferralRecommendationAcceptedID)
		REFERENCES dbo.NewVisitReferralRecommendationAccepted(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    
     CONSTRAINT FK_BhsFollowUp__ReasonNewVisitReferralRecommendationNotAccepted 
        FOREIGN KEY (ReasonNewVisitReferralRecommendationNotAcceptedID)
		REFERENCES dbo.ReasonNewVisitReferralRecommendationNotAccepted(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsFollowUp__Discharged 
        FOREIGN KEY (DischargedID)
		REFERENCES dbo.Discharged(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION
);
GO


SET NOEXEC OFF
GO
---------------------------------------------


GO
print 'Populating dbo.NewVisitReferralRecommendation...'
GO
MERGE INTO dbo.NewVisitReferralRecommendation as target
USING (VALUES
(1, 'BHS in-medical', 1),
(2, 'BHS dept.', 2),
(3, 'Internal medical provider', 3),
(4, 'Internal psychiatrist', 3),
(5, 'External BHS provider', 4),
(6, 'External psychiatrist', 5),
(7, ' Other', 6),
(8, 'Not indicated/offered', 100)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;

---------------------------------
GO
print 'Populating dbo.[NewVisitReferralRecommendationAccepted]...'
GO
MERGE INTO dbo.[NewVisitReferralRecommendationAccepted] as target
USING (VALUES
(1, 'Yes', 1),
(2, 'No', 2),
(8, 'Not indicated/offered', 100)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

---------------------------------
GO
print 'Populating dbo.[ReasonNewVisitReferralRecommendationNotAccepted]...'
GO
MERGE INTO dbo.[ReasonNewVisitReferralRecommendationNotAccepted] as target
USING (VALUES
(0, 'Accepted', 1),
(1, 'Service perceived as not needed', 3),
(2, 'Has existing provider', 4),
(3, 'Wants other (external) provider', 5),
(4, 'Distance – too far away', 6),
(5, 'Concerned about confidentiality', 7),
(6, 'No transportation', 8),
(7, 'Work', 9),
(8, 'Not indicated/offered', 2),
(9, 'No childcare', 10),
(10, 'Other responsibility', 11),
(11, 'Too ill, elderly, or handicap', 12),
(12, 'Decline to answer', 13)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;

---------------------------------
GO
print 'Populating dbo.[Discharged]...'
GO
MERGE INTO dbo.[Discharged] as target
USING (VALUES
(0, 'No', 1),
(1, 'Service completed', 2),
(2, 'Symptom reduction', 3),
(3, 'Patient requested discontinuation of service', 3),
(4, 'Address changed – out of service area', 4),
(5, 'Could not contact', 5),
(6, 'Transferred to different provider', 6),
(7, 'Deceased', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[Race]...'
GO
MERGE INTO dbo.[Race] as target
USING (VALUES
(1, 'Alaska Native', 2),
(2, 'American Indian', 3),
(3, 'Asian', 3),
(4, 'Black', 4),
(5, 'Hispanic/Latino ', 5),
(6, 'White', 6),
(7, 'Other', 7)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[Gender]...'
GO
MERGE INTO dbo.[Gender] as target
USING (VALUES
(1, 'Male', 2),
(2, 'Female', 3),
(3, 'Transgender', 3),
(4, 'Other', 4)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[SexualOrientation]...'
GO
MERGE INTO dbo.[SexualOrientation] as target
USING (VALUES
(1, 'Heterosexual', 2),
(2, 'Lesbian', 3),
(3, 'Gay', 3),
(4, 'Bisexual', 4),
(5, 'Other', 5)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[MaritalStatus]...'
GO
MERGE INTO dbo.[MaritalStatus] as target
USING (VALUES
(1, 'Married', 2),
(2, 'Single', 3),
(3, 'In Relationship', 3)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
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
(9, 'AA degree', 10),
(10, 'Doctoral degree', 11)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[LivingOnReservation]...'
GO

MERGE INTO dbo.LivingOnReservation as target
USING (VALUES
(1, 'On reservation', 2),
(2, 'Off reservation', 3)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


print 'Populating dbo.[MilitaryExperience]...'
GO

MERGE INTO dbo.MilitaryExperience as target
USING (VALUES
(1, 'None', 2),
(2, 'Active duty', 3),
(3, 'Veteran', 4),
(4, 'Deployed to a combat zone', 5)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
WHEN NOT MATCHED BY SOURCE THEN
    DELETE
;
GO


print 'Populating dbo.[TreatmentAction]...'
GO

MERGE INTO dbo.TreatmentAction as target
USING (VALUES
(1, 'Evaluation', 1),
(2, 'Education', 2),
(3, 'Brief intervention', 3),
(4, 'Counseling', 4),
(5, 'Pharmacotherapy', 5),
(6, 'Other', 6)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[PatientAttendedVisit]...'
GO

MERGE INTO dbo.PatientAttendedVisit as target
USING (VALUES
(1, 'BHS in-medical', 1),
(2, 'BHS dept.', 2),
(3, 'Internal medical provider', 3),
(4, 'Internal psychiatrist', 4),
(5, 'External BHS provider', 5),
(6, 'External psychiatrist', 6),
(7, 'Other', 7),
(8, 'Not indicated/offered', 8),
(9, 'Unknown', 9),
(10, 'No', 10)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO

print 'Populating dbo.[FollowUpContactOutcome]...'
GO
MERGE INTO dbo.FollowUpContactOutcome as target
USING (VALUES
(0, 'None', 1),
(1, 'Talked with patient or parent', 2),
(2, 'Left message', 3),
(3, 'Phone not working', 4),
(4, 'Unable to leave message', 5)
) as source(ID, [Name], OrderIndex) 
    ON target.ID = source.ID
WHEN MATCHED THEN 
    UPDATE SET [Name] = source.[Name], OrderIndex = source.OrderIndex
WHEN NOT MATCHED BY TARGET THEN
    INSERT(ID, [Name], OrderIndex) 
        VALUES (source.ID, source.[Name], source.OrderIndex)
;
GO


------------------------------------------------
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsVisit TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsDemographics TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.VisitSettings TO frontdesk_appuser


GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsFollowUp TO frontdesk_appuser

---------------------------------------------
IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.0.1')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.0.1');