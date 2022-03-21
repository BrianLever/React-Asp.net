IF OBJECT_ID('[dbo].[VisitSettings]') IS NOT NULL
SET NOEXEC ON;
GO

CREATE TABLE [dbo].[VisitSettings]
(
    [MeasureToolId] char(5) NOT NULL, 
	[Name] varchar(64) NOT NULL,
    IsEnabled bit NOT NULL CONSTRAINT DF_VisitSettings_IsEnabled DEFAULT 1,
    OrderIndex tinyint NOT NULL,
	LastModifiedDateUTC datetime NOT NULL,
	CONSTRAINT PK_VisitSettings PRIMARY KEY CLUSTERED(MeasureToolId ASC)
)
GO
CREATE INDEX IX_VisitSettings_OrderIndex ON dbo.VisitSettings(OrderIndex ASC, [MeasureToolId] ASC, [Name] ASC)
GO

SET NOEXEC OFF
GO

IF NOT EXISTS(SELECT NULL FROM [dbo].[VisitSettings])
BEGIN
INSERT INTO dbo.VisitSettings(MeasureToolId, Name, IsEnabled, OrderIndex, LastModifiedDateUTC) VALUES
('SIH', 'Smoker in the Home', 0, 10, GETUTCDATE()),
('TCC1', 'Tobacco Use (Ceremony)', 0, 20, GETUTCDATE()),
('TCC2', 'Tobacco Use (Smoking)', 1, 30, GETUTCDATE()),
('TCC3', 'Tobacco Use (Smokeless)', 1, 40, GETUTCDATE()),
('CAGE', 'Alcohol Use (CAGE)', 1, 50, GETUTCDATE()),
('DAST', 'Non-Medical Drug Use (DAST-10)', 1, 60, GETUTCDATE()),
('PHQ1', 'Depression (PHQ-9)', 1, 70, GETUTCDATE()),
('PHQ2', 'Suicide Identiation (PHQ-9)', 1, 80, GETUTCDATE()),
('HITS', 'Intimate Partner/Domestic Violence (HITS)', 1, 90, GETUTCDATE())
;

END
GO

IF OBJECT_ID('[dbo].[NewVisitReferralRecommendation]') IS NOT NULL
SET NOEXEC ON
GO
PRINT N'Creating [dbo].[NewVisitReferralRecommendation]...';


GO
CREATE TABLE [dbo].[NewVisitReferralRecommendation] (
    [ID]         INT           NOT NULL,
    [Name]       NVARCHAR (64) NOT NULL,
    [OrderIndex] INT           NOT NULL,
    CONSTRAINT [PK_NewVisitReferralRecommendation] PRIMARY KEY CLUSTERED ([ID] ASC)
);
GO
PRINT N'Creating [dbo].[NewVisitReferralRecommendation].[IX_NewVisitReferralRecommendation_OrderIndex]...';


GO
CREATE NONCLUSTERED INDEX [IX_NewVisitReferralRecommendation_OrderIndex]
    ON [dbo].[NewVisitReferralRecommendation]([OrderIndex] DESC)
    INCLUDE([Name]);

GO
SET NOEXEC OFF
---------------------------
IF OBJECT_ID('[dbo].[NewVisitReferralRecommendationAccepted]') IS NOT NULL
SET NOEXEC ON
GO

print 'Creating [dbo].[NewVisitReferralRecommendationAccepted]...'

CREATE TABLE [dbo].[NewVisitReferralRecommendationAccepted]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_NewVisitReferralRecommendationAccepted PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_NewVisitReferralRecommendationAccepted_OrderIndex 
    ON dbo.NewVisitReferralRecommendationAccepted([OrderIndex] DESC) INCLUDE([Name])
GO

SET NOEXEC OFF
GO
------------
IF OBJECT_ID('[dbo].[ReasonNewVisitReferralRecommendationNotAccepted]') IS NOT NULL
SET NOEXEC ON
GO

print 'Creating [dbo].[ReasonNewVisitReferralRecommendationNotAccepted]...'
GO

CREATE TABLE [dbo].[ReasonNewVisitReferralRecommendationNotAccepted]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_ReasonNewVisitReferralRecommendationNotAccepted PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_ReasonNewVisitReferralRecommendationNotAccepted_OrderIndex 
    ON dbo.ReasonNewVisitReferralRecommendationNotAccepted([OrderIndex] DESC) INCLUDE([Name])
GO

SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[Discharged]') IS NOT NULL
SET NOEXEC ON
GO

print 'Creating [dbo].[Discharged]...'
GO

CREATE TABLE [dbo].[Discharged]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_Discharged PRIMARY KEY CLUSTERED (ID),
);
GO
CREATE INDEX IX_Discharged_OrderIndex 
    ON dbo.[NewVisitReferralRecommendation]([OrderIndex] DESC) INCLUDE([Name])
GO

GO

SET NOEXEC OFF
GO


IF OBJECT_ID('[dbo].[TreatmentAction]') IS NOT NULL
SET NOEXEC ON
GO

print 'Creating [dbo].[TreatmentAction]...'
GO
CREATE TABLE [dbo].[TreatmentAction]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_TreatmentAction PRIMARY KEY CLUSTERED (ID),
);
GO

SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[BhsVisit]') IS NOT NULL
SET NOEXEC ON
GO

print 'Creating [dbo].[BhsVisit]...'
GO

CREATE TABLE [dbo].[BhsVisit]
(
    ID bigint NOT NULL IDENTITY(1,1),
	ScreeningResultID bigint NOT NULL, /*foreign key*/
	LocationID int NOT NULL, /*foreign key*/
    CreatedDate DateTimeOffset NOT NULL,
    ScreeningDate DateTimeOffset NOT NULL,
	TobacoExposureSmokerInHomeFlag bit NOT NULL,
    TobacoExposureCeremonyUseFlag bit NOT NULL,
    TobacoExposureSmokingFlag bit NOT NULL,
    TobacoExposureSmoklessFlag bit NOT NULL,
    
    AlcoholUseFlagScoreLevel int NULL,
    AlcoholUseFlagScoreLevelLabel nvarchar(64) NULL,

    SubstanceAbuseFlagScoreLevel int NULL,
    SubstanceAbuseFlagScoreLevelLabel nvarchar(64) NULL,

    DepressionFlagScoreLevel int NULL,
    DepressionFlagScoreLevelLabel nvarchar(64) NULL,
    DepressionThinkOfDeathAnswer nvarchar(64) NULL,

    PartnerViolenceFlagScoreLevel int NULL,
    PartnerViolenceFlagScoreLevelLabel nvarchar(64) NULL,

    NewVisitReferralRecommendationID int NULL, /*foreign key*/
    NewVisitReferralRecommendationDescription nvarchar(max) NULL,

    NewVisitReferralRecommendationAcceptedID int NULL, /*foreign key*/

    ReasonNewVisitReferralRecommendationNotAcceptedID int NULL, /*foreign key*/

    NewVisitDate DateTimeOffset NULL,

    DischargedID int NULL, /*foreign key*/

    ThirtyDatyFollowUpFlag bit NOT NULL,

    Notes nvarchar(max) NULL,

    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,

    TreatmentAction1ID int NULL,
    TreatmentAction1Description nvarchar(max) NULL,
    TreatmentAction2ID int NULL,
    TreatmentAction2Description nvarchar(max) NULL,
    TreatmentAction3ID int NULL,
    TreatmentAction3Description nvarchar(max) NULL,
    TreatmentAction4ID int NULL,
    TreatmentAction4Description nvarchar(max) NULL,
    TreatmentAction5ID int NULL,
    TreatmentAction5Description nvarchar(max) NULL,

    OtherScreeningTools xml NULL,

    CONSTRAINT PK_BhsVisit PRIMARY KEY(ID),
	CONSTRAINT FK_BhsVisit__ScreeningResult FOREIGN KEY (ScreeningResultID)
		REFERENCES dbo.ScreeningResult(ScreeningResultID),
    CONSTRAINT FK_BhsVisit__BranchLocation FOREIGN KEY (LocationID)
		REFERENCES dbo.BranchLocation(BranchLocationID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__NewVisitReferralRecommendation 
        FOREIGN KEY (NewVisitReferralRecommendationID)
		REFERENCES dbo.NewVisitReferralRecommendation(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
     CONSTRAINT FK_BhsVisit__NewVisitReferralRecommendationAccepted 
        FOREIGN KEY (NewVisitReferralRecommendationAcceptedID)
		REFERENCES dbo.NewVisitReferralRecommendationAccepted(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    
     CONSTRAINT FK_BhsVisit__ReasonNewVisitReferralRecommendationNotAccepted 
        FOREIGN KEY (ReasonNewVisitReferralRecommendationNotAcceptedID)
		REFERENCES dbo.ReasonNewVisitReferralRecommendationNotAccepted(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__Discharged 
        FOREIGN KEY (DischargedID)
		REFERENCES dbo.Discharged(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK_BhsVisit__TreatmentAction1 
        FOREIGN KEY (TreatmentAction1ID)
		REFERENCES dbo.TreatmentAction(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction2 
        FOREIGN KEY (TreatmentAction2ID)
		REFERENCES dbo.TreatmentAction(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction3 
        FOREIGN KEY (TreatmentAction3ID)
		REFERENCES dbo.TreatmentAction(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction4
        FOREIGN KEY (TreatmentAction4ID)
		REFERENCES dbo.TreatmentAction(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsVisit__TreatmentAction5
        FOREIGN KEY (TreatmentAction5ID)
		REFERENCES dbo.TreatmentAction(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION
);
GO



SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[EducationLevel]') IS NOT NULL
SET NOEXEC ON
GO


CREATE TABLE [dbo].[EducationLevel]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_EducationLevel PRIMARY KEY CLUSTERED (ID),
);
GO

SET NOEXEC OFF
GO


IF OBJECT_ID('[dbo].[Gender]') IS NOT NULL
SET NOEXEC ON
GO


CREATE TABLE [dbo].[Gender]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_Gender PRIMARY KEY CLUSTERED (ID),
);
GO


SET NOEXEC OFF
GO



IF OBJECT_ID('[dbo].[Race]') IS NOT NULL
SET NOEXEC ON
GO


CREATE TABLE [dbo].[Race]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_Race PRIMARY KEY CLUSTERED (ID),
);
GO


SET NOEXEC OFF
GO


IF OBJECT_ID('[dbo].[MaritalStatus]') IS NOT NULL
SET NOEXEC ON
GO


CREATE TABLE [dbo].[MaritalStatus]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_MaritalStatus PRIMARY KEY CLUSTERED (ID),
);
GO


SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[MilitaryExperience]') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [dbo].[MilitaryExperience]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_MilitaryExperience PRIMARY KEY CLUSTERED (ID),
);
GO


SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[SexualOrientation]') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [dbo].[SexualOrientation]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_SexualOrientation PRIMARY KEY CLUSTERED (ID),
);
GO

SET NOEXEC OFF
GO


IF OBJECT_ID('[dbo].[LivingOnReservation]') IS NOT NULL
SET NOEXEC ON
GO

CREATE TABLE [dbo].[LivingOnReservation]
(
    [ID] INT NOT NULL,
    [Name] NVARCHAR(64) NOT NULL,
    [OrderIndex] INT NOT NULL
    CONSTRAINT PK_LivingOnReservation PRIMARY KEY CLUSTERED (ID),
);
GO


SET NOEXEC OFF
GO



IF OBJECT_ID('[dbo].[BhsDemographics]') IS NOT NULL
SET NOEXEC ON
GO


CREATE TABLE [dbo].[BhsDemographics]
(
    ID bigint NOT NULL IDENTITY(1,1),
    ScreeningResultID bigint NOT NULL, /*foreign key*/
    LocationID int NOT NULL, /*foreign key*/
    CreatedDate DateTimeOffset NOT NULL,
    ScreeningDate DateTimeOffset NOT NULL,
    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,

    FirstName nvarchar(128) NOT NULL,
	LastName nvarchar(128) NOT NULL,
	MiddleName nvarchar(128) NULL,
	Birthday date NOT NULL,
	StreetAddress nvarchar(512) NULL,
	City nvarchar(255)	NULL,
	StateID char(2) NULL,
	ZipCode char(5) NULL,
	Phone char(14) NULL,
    RaceID int NULL,
    GenderID int NULL,
    SexualOrientationID int NULL,
    TribalAffiliation nvarchar(128) NULL,
    MaritalStatusID int NULL,
    EducationLevelID int NULL,
    LivingOnReservationID int NULL,
    CountyOfResidence nvarchar(128) NULL,
    MilitaryExperienceID int NULL,
    
    CONSTRAINT PK_BhsDemographics PRIMARY KEY(ID),
	CONSTRAINT FK_BhsDemographics__ScreeningResult FOREIGN KEY (ScreeningResultID)
		REFERENCES dbo.ScreeningResult(ScreeningResultID)
        ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__BranchLocation FOREIGN KEY (LocationID)
		REFERENCES dbo.BranchLocation(BranchLocationID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__Race FOREIGN KEY (RaceID)
		REFERENCES dbo.Race(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__Gender FOREIGN KEY (GenderID)
		REFERENCES dbo.Gender(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__SexualOrientation FOREIGN KEY (SexualOrientationID)
		REFERENCES dbo.SexualOrientation(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__MaritalStatus FOREIGN KEY (MaritalStatusID)
		REFERENCES dbo.MaritalStatus(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__EducationLevel FOREIGN KEY (EducationLevelID)
		REFERENCES dbo.EducationLevel(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__LivingOnReservation FOREIGN KEY (LivingOnReservationID)
		REFERENCES dbo.LivingOnReservation(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK_BhsDemographics__MilitaryExperience FOREIGN KEY (MilitaryExperienceID)
		REFERENCES dbo.MilitaryExperience(ID)
		ON UPDATE NO ACTION ON DELETE NO ACTION
)


SET NOEXEC OFF
GO

IF OBJECT_ID('[dbo].[vBhsVisitsAndDemographics]') IS NOT NULL
SET NOEXEC ON
GO

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
,CASE WHEN r.StreetAddress IS NOT NULL THEN 1 ELSE 0 END AS HasAddress
,0 as IsVisitRecordType
,v.LocationID
,v.Birthday
,dbo.fn_GetPatientName(v.LastName, v.FirstName, v.MiddleName) as FullName
FROM 
	dbo.BhsDemographics v 
	INNER JOIN dbo.ScreeningResult r ON v.ScreeningResultID = r.ScreeningResultID
)
SELECT t.* 
FROM tblResult t
GO

SET NOEXEC OFF
GO

-------------------------------------------
------------------------------------
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
;
GO
-----

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

-----

GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsVisit TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.BhsDemographics TO frontdesk_appuser
GRANT SELECT,INSERT,DELETE,UPDATE ON dbo.VisitSettings TO frontdesk_appuser

GRANT SELECT, EXECUTE ON SCHEMA :: dbo TO frontdesk_appuser


IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '6.0.0.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('6.0.0.0');