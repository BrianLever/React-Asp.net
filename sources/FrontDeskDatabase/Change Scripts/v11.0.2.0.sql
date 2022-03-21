IF OBJECT_ID('dbo.ColumbiaSuicideReport') IS NULL
SET NOEXEC ON
GO

drop table ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors;
drop table ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors;
drop table ColumbiaSuicideRiskAssessmentReport;
drop table ColumbiaSuicideBehaviorAct;
drop table ColumbiaSuicidalIdeation;
drop table ColumbiaIntensityIdeation;
drop table ColumbiaSuicideReport;
drop table ColumbiaSuicideRiskScoreLevel;

SET NOEXEC OFF
GO


--IF OBJECT_ID('[dbo].[ColumbiaSuicideReport]') IS NOT NULL
--    SET NOEXEC ON
--GO


------------------------
CREATE TABLE [dbo].[ColumbiaSuicideRiskScoreLevel]
(
    ScoreLevel int NOT NULL,
    [Name] nvarchar(64) NOT NULL,
    Indicates nvarchar(max) NULL,
    [Label] nvarchar(64) NOT NULL,
    CONSTRAINT PK__ColumbiaSuicideRiskScoreLevel PRIMARY KEY(ScoreLevel)
)

GO

CREATE TABLE [dbo].[ColumbiaSuicideReport]
(
    ID bigint NOT NULL IDENTITY(1,1),
    
    /* patient info */
    FirstName nvarchar(128) NOT NULL,
    LastName nvarchar(128) NOT NULL,
    MiddleName nvarchar(128) NULL,
    Birthday date NOT NULL,
    StreetAddress nvarchar(512) NULL,
    City nvarchar(255)	NULL,
    StateID char(2) NULL,
    ZipCode char(5) NULL,
    Phone char(14) NULL,
    BranchLocationID int NOT NULL, /*foreign key*/
    PatientName as dbo.fn_GetPatientName(LastName, FirstName, MiddleName) PERSISTED,
    
    ScreeningResultID bigint NULL, /*foreign key*/
    BhsVisitID bigint NULL, /*foreign key*/
    EhrPatientID int null,
    EhrPatientHRN nvarchar(255) null,


    CreatedDate DateTimeOffset NOT NULL,
    BhsStaffNameCompleted nvarchar(128) NULL,
    CompleteDate datetimeoffset NULL,


    ScoreLevel int NULL, /* Risk Level */

    /* INTENSITY OF IDEATION */
    LifetimeMostSevereIdeationLevel int NULL, 
    LifetimeMostSevereIdeationDescription nvarchar(max) NULL,
    
    RecentMostSevereIdeationLevel int NULL, 
    RecentMostSevereIdeationDescription nvarchar(max) NULL,
    

    /* SUICIDE BEHAVIOR */

    SuicideMostRecentAttemptDate datetime NULL,
    SuicideMostLethalRecentAttemptDate datetime NULL,
    SuicideFirstAttemptDate datetime NULL, 
    MedicalDamageMostRecentAttemptCode int NULL, /* 0 - 5 */
    MedicalDamageMostLethalAttemptCode int NULL, /* 0 - 5 */
    MedicalDamageFirstAttemptCode int NULL, /* 0 - 5 */

    PotentialLethalityMostRecentAttemptCode int NULL, /* 0 - 2 */
    PotentialLethalityMostLethalAttemptCode int NULL, /* 0 - 2 */
    PotentialLethalityFirstAttemptCode int NULL, /* 0 - 2 */
    
    

    CONSTRAINT PK__ColumbiaSuicideReport PRIMARY KEY CLUSTERED (ID),

    CONSTRAINT FK__ColumbiaSuicideReport__BranchLocation FOREIGN KEY (BranchLocationID)
        REFERENCES dbo.BranchLocation(BranchLocationID) ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK__ColumbiaSuicideReport__ScreeningResult FOREIGN KEY (ScreeningResultID)
        REFERENCES dbo.ScreeningResult(ScreeningResultID) ON UPDATE NO ACTION ON DELETE NO ACTION,
    CONSTRAINT FK__ColumbiaSuicideReport__BhsVisit FOREIGN KEY (BhsVisitID)
        REFERENCES dbo.BhsVisit(ID) ON UPDATE NO ACTION ON DELETE NO ACTION,

    CONSTRAINT FK__ColumbiaSuicideReport__ColumbiaSuicideRiskScoreLevel FOREIGN KEY (ScoreLevel)
        REFERENCES dbo.ColumbiaSuicideRiskScoreLevel(ScoreLevel) ON UPDATE NO ACTION ON DELETE NO ACTION,

);
GO

-- Suicidal Ideation
CREATE TABLE [dbo].[ColumbiaSuicidalIdeation]
(
    ColumbiaReportID bigint NOT NULL, /*foreign key*/
    QuestionID int NOT NULL, /* unique question number */
    LifetimeMostSucidal int NULL, /* 0 - No, 1 - Yes */
    PastLastMonth int NULL, /* 0 - No, 1 - Yes */
    [Description] nvarchar(max) NULL,
    
    CONSTRAINT PK__ColumbiaSuicidalIdeation PRIMARY KEY CLUSTERED (ColumbiaReportID, QuestionID),
     
    CONSTRAINT FK__ColumbiaSuicidalIdeation__ColumbiaSuicideReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideReport (ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO

CREATE TABLE [dbo].[ColumbiaIntensityIdeation]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    QuestionID int NOT NULL, /* unique question number */
    LifetimeMostSevere int NULL,
    RecentMostSevere int NULL,

    CONSTRAINT PK__ColumbiaIntensityIdeation PRIMARY KEY CLUSTERED (ColumbiaReportID, QuestionID),
     
    CONSTRAINT FK__ColumbiaIntensityIdeation__ColumbiaSuicideReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideReport (ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO


CREATE TABLE [dbo].[ColumbiaSuicideBehaviorAct]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    QuestionID int NOT NULL, /* unique question number */
    LifetimeLevel int NULL, /* 0 - No, 1 - Yes */
    LifetimeCount int NULL,

    PastThreeMonths int NULL, /* 0 - No, 1 - Yes */
    PastThreeMonthsCount int NULL,

    [Description] nvarchar(max) NULL,


    CONSTRAINT PK_ColumbiaSuicideBehaviorAct PRIMARY KEY CLUSTERED (ColumbiaReportID, QuestionID),
     
    CONSTRAINT FK__ColumbiaSuicideBehaviorAct__ColumbiaSuicideReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideReport (ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)
GO


CREATE TABLE [dbo].[ColumbiaSuicideRiskAssessmentReport]
(
    ColumbiaReportID bigint NOT NULL, /*foreign key*/

    /* SUICIDAL AND SELF-INJURY BEHAVIOR (PAST WEEK) */
    ActualSuicideAttempt bit NOT NULL,
    LifetimeActualSuicideAttempt bit NOT NULL,

    InterruptedSuicideAttempt bit NOT NULL,
    LifetimeInterruptedSuicideAttempt bit NOT NULL,

    AbortedSuicideAttempt bit NOT NULL,
    LifetimeAbortedSuicideAttempt bit NOT NULL,

    OtherPreparatoryActsToKillSelf bit NOT NULL,
    LifetimeOtherPreparatoryActsToKillSelf bit NOT NULL,

    ActualSelfInjuryBehaviorWithoutSuicideIntent bit NOT NULL,
    LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent bit NOT NULL,

    /* SUICIDAL IDEATION (MOST SEVERE IN PAST WEEK) */
    WishToBeDead bit NOT NULL,
    SuicidalThoughts bit NOT NULL,
    SuicidalThoughtsWithMethod bit NOT NULL,
    SuicidalIntent bit NOT NULL,
    SuicidalIntentWithSpecificPlan bit NOT NULL,

    /* ACTIVATING EVENTS (RECENT) */
    RecentLoss bit NOT NULL,
    DescribeRecentLoss nvarchar(max) NULL,
    PendingIncarceration bit NOT NULL,
    CurrentOrPendingIsolation bit NOT NULL,

    /* CLINICAL STATUS (RECENT) */
    Hopelessness bit NOT NULL,
    Helplessness bit NOT NULL,

    FeelingTrapped bit NOT NULL,

    MajorDepressiveEpisode bit NOT NULL,

    MixedAffectiveEpisode bit NOT NULL,
    CommandHallucinationsToHurtSelf bit NOT NULL,
    HighlyImpulsiveBehavior bit NOT NULL,
    SubstanceAbuseOrDependence bit NOT NULL,

    AgitationOrSevereAnxiety bit NOT NULL,
    PerceivedBurdenOnFamilyOrOthers bit NOT NULL,
    ChronicPhysicalPain bit NOT NULL,
    HomicidalIdeation bit NOT NULL,
    AggressiveBehaviorTowardsOthers bit NOT NULL,
    MethodForSuicideAvailable bit NOT NULL,
    RefusesOrFeelsUnableToAgreeToSafetyPlan bit NOT NULL,
    SexualAbuseLifetime bit NOT NULL,
    FamilyHistoryOfSuicideLifetime bit NOT NULL,

    /* TREATMENT HISTORY */
    PreviousPsychiatricDiagnosesAndTreatments bit NOT NULL,
    HopelessOrDissatisfiedWithTreatment bit NOT NULL,
    NonCompliantWithTreatment bit NOT NULL,
    NotReceivingTreatment bit NOT NULL,

    /* PROTECTIVE FACTORS (RECENT) */
    IdentifiesReasonsForLiving bit NOT NULL,
    ResponsibilityToFamilyOrOthers bit NOT NULL,
    SupportiveSocialNetworkOrFamily bit NOT NULL,

    FearOfDeathOrDyingDueToPainAndSuffering bit NOT NULL,
    BeliefThatSuicideIsImmoral bit NOT NULL,
    EngagedInWorkOrSchool bit NOT NULL,
    EngagedWithPhoneWorker bit NOT NULL,


    DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior nvarchar(max),

    CONSTRAINT PK__ColumbiaSuicideRiskAssessmentReport PRIMARY KEY CLUSTERED (ColumbiaReportID),
     
    CONSTRAINT FK__ColumbiaSuicideRiskAssessmentReport__ColumbiaSuicideReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideReport (ID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO




/* OTHER PROTECTIVE FACTORS */
CREATE TABLE [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    ItemID int NOT NULL, /* unique question number */
    ProtectiveFactor nvarchar (max),


    CONSTRAINT PK__ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors 
        PRIMARY KEY CLUSTERED (ColumbiaReportID, ItemID),
     
    CONSTRAINT FK__ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors__ColumbiaSuicideRiskAssessmentReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideRiskAssessmentReport (ColumbiaReportID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO


/* OTHER RISK FACTORS */
CREATE TABLE [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors]
(
    ColumbiaReportID bigint NOT NULL /*foreign key*/,
    ItemID int NOT NULL, /* unique question number */
    RiskFactor nvarchar (max),


    CONSTRAINT PK__ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors 
        PRIMARY KEY CLUSTERED (ColumbiaReportID, ItemID),
     
    CONSTRAINT FK__ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors__ColumbiaSuicideRiskAssessmentReport 
        FOREIGN KEY (ColumbiaReportID)
        REFERENCES dbo.ColumbiaSuicideRiskAssessmentReport (ColumbiaReportID)
        ON UPDATE NO ACTION ON DELETE NO ACTION
)

GO


------------------------
SET NOEXEC OFF
GO




IF OBJECT_ID('dbo.uspAddColumbiaSuicideReport') IS NOT NULL
    DROP PROC dbo.uspAddColumbiaSuicideReport
GO

CREATE PROCEDURE [dbo].[uspAddColumbiaSuicideReport]
    @ID bigint OUTPUT 
    ,@FirstName nvarchar(128)
    ,@LastName nvarchar(128)
    ,@MiddleName nvarchar(128)
    ,@Birthday date
    ,@StreetAddress nvarchar(512)
    ,@City nvarchar(255)
    ,@StateID char(2)
    ,@ZipCode char(5)
    ,@Phone char(14)
    ,@BranchLocationID int
    ,@EhrPatientID int
    ,@EhrPatientHRN nvarchar(255)

    ,@ScreeningResultID bigint
    ,@BhsVisitID bigint
    ,@CreatedDate datetimeoffset(7)
    ,@BhsStaffNameCompleted nvarchar(128)
    ,@CompleteDate datetimeoffset(7)
    ,@ScoreLevel int
    ,@LifetimeMostSevereIdeationLevel int
    ,@LifetimeMostSevereIdeationDescription nvarchar(max)
    ,@RecentMostSevereIdeationLevel int
    ,@RecentMostSevereIdeationDescription nvarchar(max)
    ,@SuicideMostRecentAttemptDate datetime
    ,@SuicideMostLethalRecentAttemptDate datetime
    ,@SuicideFirstAttemptDate datetime
    ,@MedicalDamageMostRecentAttemptCode int
    ,@MedicalDamageMostLethalAttemptCode int
    ,@MedicalDamageFirstAttemptCode int
    ,@PotentialLethalityMostRecentAttemptCode int
    ,@PotentialLethalityMostLethalAttemptCode int
    ,@PotentialLethalityFirstAttemptCode int
AS
    INSERT dbo.ColumbiaSuicideReport (
    FirstName
    ,LastName
    ,MiddleName
    ,Birthday
    ,StreetAddress
    ,City
    ,StateID
    ,ZipCode
    ,Phone
    ,BranchLocationID
    ,EhrPatientID
    ,EhrPatientHRN
    ,[ScreeningResultID]
    ,[BhsVisitID]
    ,[CreatedDate]
    ,[BhsStaffNameCompleted]
    ,[CompleteDate]
    ,[ScoreLevel]
    ,[LifetimeMostSevereIdeationLevel]
    ,[LifetimeMostSevereIdeationDescription]
    ,[RecentMostSevereIdeationLevel]
    ,[RecentMostSevereIdeationDescription]
    ,[SuicideMostRecentAttemptDate]
    ,[SuicideMostLethalRecentAttemptDate]
    ,[SuicideFirstAttemptDate]
    ,[MedicalDamageMostRecentAttemptCode]
    ,[MedicalDamageMostLethalAttemptCode]
    ,[MedicalDamageFirstAttemptCode]
    ,[PotentialLethalityMostRecentAttemptCode]
    ,[PotentialLethalityMostLethalAttemptCode]
    ,[PotentialLethalityFirstAttemptCode]

    ) VALUES (
     @FirstName
    ,@LastName
    ,@MiddleName
    ,@Birthday
    ,@StreetAddress
    ,@City
    ,@StateID
    ,@ZipCode
    ,@Phone
    ,@BranchLocationID
    ,@EhrPatientID
    ,@EhrPatientHRN
    ,@ScreeningResultID
    ,@BhsVisitID
    ,@CreatedDate
    ,@BhsStaffNameCompleted
    ,@CompleteDate
    ,@ScoreLevel
    ,@LifetimeMostSevereIdeationLevel
    ,@LifetimeMostSevereIdeationDescription
    ,@RecentMostSevereIdeationLevel
    ,@RecentMostSevereIdeationDescription
    ,@SuicideMostRecentAttemptDate
    ,@SuicideMostLethalRecentAttemptDate
    ,@SuicideFirstAttemptDate
    ,@MedicalDamageMostRecentAttemptCode
    ,@MedicalDamageMostLethalAttemptCode
    ,@MedicalDamageFirstAttemptCode
    ,@PotentialLethalityMostRecentAttemptCode
    ,@PotentialLethalityMostLethalAttemptCode
    ,@PotentialLethalityFirstAttemptCode
)


    SET @ID = SCOPE_IDENTITY();

RETURN 0
GO


--------------------
IF OBJECT_ID('dbo.uspAddColumbiaIntensityIdeation') IS NOT NULL
    DROP PROC dbo.uspAddColumbiaIntensityIdeation
GO

CREATE PROCEDURE dbo.uspAddColumbiaIntensityIdeation
    @ColumbiaReportID bigint
    ,@QuestionID int
    ,@LifetimeMostSevere int
    ,@RecentMostSevere int
AS
INSERT INTO [dbo].[ColumbiaIntensityIdeation] (
    [ColumbiaReportID]
    ,[QuestionID]
    ,[LifetimeMostSevere]
    ,[RecentMostSevere]
)
VALUES (
    @ColumbiaReportID
    ,@QuestionID
    ,@LifetimeMostSevere
    ,@RecentMostSevere
)

RETURN 0
GO


--------------------
IF OBJECT_ID('dbo.uspAddColumbiaSuicidalIdeation') IS NOT NULL
    DROP PROC dbo.uspAddColumbiaSuicidalIdeation
GO

CREATE PROCEDURE dbo.uspAddColumbiaSuicidalIdeation
    @ColumbiaReportID bigint
    ,@QuestionID int
    ,@LifetimeMostSucidal int
    ,@PastLastMonth int
    ,@Description nvarchar(max)
AS
    INSERT dbo.ColumbiaSuicidalIdeation (
        ColumbiaReportID
        ,QuestionID
        ,LifetimeMostSucidal
        ,PastLastMonth
        ,[Description]
    ) VALUES (
         @ColumbiaReportID
        ,@QuestionID
        ,@LifetimeMostSucidal
        ,@PastLastMonth
        ,@Description
    )

RETURN 0
GO


--------------------
IF OBJECT_ID('dbo.uspAddColumbiaSuicideBehaviorAct') IS NOT NULL
    DROP PROC dbo.uspAddColumbiaSuicideBehaviorAct
GO

CREATE PROCEDURE dbo.uspAddColumbiaSuicideBehaviorAct
    @ColumbiaReportID bigint
    ,@QuestionID int
    ,@LifetimeLevel int
    ,@LifetimeCount int
    ,@PastThreeMonths int
    ,@PastThreeMonthsCount int
    ,@Description nvarchar(max)
AS
INSERT INTO [dbo].[ColumbiaSuicideBehaviorAct] (
    [ColumbiaReportID]
    ,[QuestionID]
    ,[LifetimeLevel]
    ,[LifetimeCount]
    ,[PastThreeMonths]
    ,[PastThreeMonthsCount]
    ,[Description]
)
VALUES (
    @ColumbiaReportID
    ,@QuestionID
    ,@LifetimeLevel
    ,@LifetimeCount
    ,@PastThreeMonths
    ,@PastThreeMonthsCount
    ,@Description
)

RETURN 0
GO

--------------------
IF OBJECT_ID('dbo.uspAddColumbiaSuicideRiskAssessmentReport') IS NOT NULL
    DROP PROC dbo.uspAddColumbiaSuicideRiskAssessmentReport
GO

CREATE PROCEDURE dbo.uspAddColumbiaSuicideRiskAssessmentReport
    @ColumbiaReportID bigint
    ,@ActualSuicideAttempt bit
    ,@LifetimeActualSuicideAttempt bit
    ,@InterruptedSuicideAttempt bit
    ,@LifetimeInterruptedSuicideAttempt bit
    ,@AbortedSuicideAttempt bit
    ,@LifetimeAbortedSuicideAttempt bit
    ,@OtherPreparatoryActsToKillSelf bit
    ,@LifetimeOtherPreparatoryActsToKillSelf bit
    ,@ActualSelfInjuryBehaviorWithoutSuicideIntent bit
    ,@LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent bit
    ,@WishToBeDead bit
    ,@SuicidalThoughts bit
    ,@SuicidalThoughtsWithMethod bit
    ,@SuicidalIntent bit
    ,@SuicidalIntentWithSpecificPlan bit
    ,@RecentLoss bit
    ,@DescribeRecentLoss nvarchar(max)
    ,@PendingIncarceration bit
    ,@CurrentOrPendingIsolation bit
    ,@Hopelessness bit
    ,@Helplessness bit
    ,@FeelingTrapped bit
    ,@MajorDepressiveEpisode bit
    ,@MixedAffectiveEpisode bit
    ,@CommandHallucinationsToHurtSelf bit
    ,@HighlyImpulsiveBehavior bit
    ,@SubstanceAbuseOrDependence bit
    ,@AgitationOrSevereAnxiety bit
    ,@PerceivedBurdenOnFamilyOrOthers bit
    ,@ChronicPhysicalPain bit
    ,@HomicidalIdeation bit
    ,@AggressiveBehaviorTowardsOthers bit
    ,@MethodForSuicideAvailable bit
    ,@RefusesOrFeelsUnableToAgreeToSafetyPlan bit
    ,@SexualAbuseLifetime bit
    ,@FamilyHistoryOfSuicideLifetime bit
    ,@PreviousPsychiatricDiagnosesAndTreatments bit
    ,@HopelessOrDissatisfiedWithTreatment bit
    ,@NonCompliantWithTreatment bit
    ,@NotReceivingTreatment bit
    ,@IdentifiesReasonsForLiving bit
    ,@ResponsibilityToFamilyOrOthers bit
    ,@SupportiveSocialNetworkOrFamily bit
    ,@FearOfDeathOrDyingDueToPainAndSuffering bit
    ,@BeliefThatSuicideIsImmoral bit
    ,@EngagedInWorkOrSchool bit
    ,@EngagedWithPhoneWorker bit
    ,@DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior nvarchar(max)
AS
INSERT INTO [dbo].[ColumbiaSuicideRiskAssessmentReport] (
    [ColumbiaReportID]
    ,[ActualSuicideAttempt]
    ,[LifetimeActualSuicideAttempt]
    ,[InterruptedSuicideAttempt]
    ,[LifetimeInterruptedSuicideAttempt]
    ,[AbortedSuicideAttempt]
    ,[LifetimeAbortedSuicideAttempt]
    ,[OtherPreparatoryActsToKillSelf]
    ,[LifetimeOtherPreparatoryActsToKillSelf]
    ,[ActualSelfInjuryBehaviorWithoutSuicideIntent]
    ,[LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent]
    ,[WishToBeDead]
    ,[SuicidalThoughts]
    ,[SuicidalThoughtsWithMethod]
    ,[SuicidalIntent]
    ,[SuicidalIntentWithSpecificPlan]
    ,[RecentLoss]
    ,[DescribeRecentLoss]
    ,[PendingIncarceration]
    ,[CurrentOrPendingIsolation]
    ,[Hopelessness]
    ,[Helplessness]
    ,[FeelingTrapped]
    ,[MajorDepressiveEpisode]
    ,[MixedAffectiveEpisode]
    ,[CommandHallucinationsToHurtSelf]
    ,[HighlyImpulsiveBehavior]
    ,[SubstanceAbuseOrDependence]
    ,[AgitationOrSevereAnxiety]
    ,[PerceivedBurdenOnFamilyOrOthers]
    ,[ChronicPhysicalPain]
    ,[HomicidalIdeation]
    ,[AggressiveBehaviorTowardsOthers]
    ,[MethodForSuicideAvailable]
    ,[RefusesOrFeelsUnableToAgreeToSafetyPlan]
    ,[SexualAbuseLifetime]
    ,[FamilyHistoryOfSuicideLifetime]
    ,[PreviousPsychiatricDiagnosesAndTreatments]
    ,[HopelessOrDissatisfiedWithTreatment]
    ,[NonCompliantWithTreatment]
    ,[NotReceivingTreatment]
    ,[IdentifiesReasonsForLiving]
    ,[ResponsibilityToFamilyOrOthers]
    ,[SupportiveSocialNetworkOrFamily]
    ,[FearOfDeathOrDyingDueToPainAndSuffering]
    ,[BeliefThatSuicideIsImmoral]
    ,[EngagedInWorkOrSchool]
    ,[EngagedWithPhoneWorker]
,[DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior]
)
VALUES (
    @ColumbiaReportID
    ,@ActualSuicideAttempt
    ,@LifetimeActualSuicideAttempt
    ,@InterruptedSuicideAttempt
    ,@LifetimeInterruptedSuicideAttempt
    ,@AbortedSuicideAttempt
    ,@LifetimeAbortedSuicideAttempt
    ,@OtherPreparatoryActsToKillSelf
    ,@LifetimeOtherPreparatoryActsToKillSelf
    ,@ActualSelfInjuryBehaviorWithoutSuicideIntent
    ,@LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent
    ,@WishToBeDead
    ,@SuicidalThoughts
    ,@SuicidalThoughtsWithMethod
    ,@SuicidalIntent
    ,@SuicidalIntentWithSpecificPlan
    ,@RecentLoss
    ,@DescribeRecentLoss
    ,@PendingIncarceration
    ,@CurrentOrPendingIsolation
    ,@Hopelessness
    ,@Helplessness
    ,@FeelingTrapped
    ,@MajorDepressiveEpisode
    ,@MixedAffectiveEpisode
    ,@CommandHallucinationsToHurtSelf
    ,@HighlyImpulsiveBehavior
    ,@SubstanceAbuseOrDependence
    ,@AgitationOrSevereAnxiety
    ,@PerceivedBurdenOnFamilyOrOthers
    ,@ChronicPhysicalPain
    ,@HomicidalIdeation
    ,@AggressiveBehaviorTowardsOthers
    ,@MethodForSuicideAvailable
    ,@RefusesOrFeelsUnableToAgreeToSafetyPlan
    ,@SexualAbuseLifetime
    ,@FamilyHistoryOfSuicideLifetime
    ,@PreviousPsychiatricDiagnosesAndTreatments
    ,@HopelessOrDissatisfiedWithTreatment
    ,@NonCompliantWithTreatment
    ,@NotReceivingTreatment
    ,@IdentifiesReasonsForLiving
    ,@ResponsibilityToFamilyOrOthers
    ,@SupportiveSocialNetworkOrFamily
    ,@FearOfDeathOrDyingDueToPainAndSuffering
    ,@BeliefThatSuicideIsImmoral
    ,@EngagedInWorkOrSchool
    ,@EngagedWithPhoneWorker
    ,@DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior
)

RETURN 0
GO




--------------------
IF OBJECT_ID('dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherProtectiveFactors') IS NOT NULL
    DROP PROC dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherProtectiveFactors
GO

CREATE PROCEDURE dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherProtectiveFactors
    @ColumbiaReportID bigint
    ,@ItemID int
    ,@ProtectiveFactor nvarchar(max)
AS
INSERT INTO [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors] (
    [ColumbiaReportID]
    ,[ItemID]
    ,[ProtectiveFactor]
)
VALUES (
    @ColumbiaReportID
    ,@ItemID
    ,@ProtectiveFactor
)

RETURN 0
GO

--------------------
IF OBJECT_ID('dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherRiskFactors') IS NOT NULL
    DROP PROC dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherRiskFactors
GO

CREATE PROCEDURE dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherRiskFactors
    @ColumbiaReportID bigint
    ,@ItemID int
    ,@RiskFactor nvarchar(max)
AS
INSERT INTO [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors] (
    [ColumbiaReportID]
    ,[ItemID]
    ,[RiskFactor]
)
VALUES (
    @ColumbiaReportID
    ,@ItemID
    ,@RiskFactor
)

RETURN 0
GO

--------------------
IF OBJECT_ID('dbo.uspDeleteColumbiaSuicideReportItems') IS NOT NULL
    DROP PROC dbo.uspDeleteColumbiaSuicideReportItems
GO

CREATE PROCEDURE [dbo].[uspDeleteColumbiaSuicideReportItems]
    @ColumbiaReportID bigint
AS
BEGIN
    DELETE FROM [dbo].[ColumbiaSuicidalIdeation] WHERE ColumbiaReportID = @ColumbiaReportID;
    DELETE FROM [dbo].[ColumbiaIntensityIdeation] WHERE ColumbiaReportID = @ColumbiaReportID;
    DELETE FROM [dbo].[ColumbiaSuicideBehaviorAct] WHERE ColumbiaReportID = @ColumbiaReportID;

END
RETURN 0
GO


--------------------

-- Score Levels
MERGE INTO dbo.ColumbiaSuicideRiskScoreLevel as target
USING (
VALUES
( 1, 'NO RISK Identified', 'No response required', 'No Risk'),
( 2, 'LOW RISK Identified', 'Provide education and resources', 'Low'),
( 3, 'MODERATE RISK Identified', 'Provider to assess and determine disposition. Provider has option to provide safe environment, per clinical judgement and assessment. Obtain or complete further assessment. Behavioral health consultation, if available', 'Moderate'),
( 4, 'HIGH RISK Identified', 'Immediate notification to provider to assess and determine ultimate disposition. Provide safe environment', 'High')
) AS source(ScoreLevel, [Name], Indicates, [Label]) ON target.ScoreLevel = source.ScoreLevel
WHEN MATCHED THEN  
    UPDATE SET [Name] = source.[Name], Indicates = source.Indicates, [Label] = source.[Label]
WHEN NOT MATCHED BY TARGET THEN 
    INSERT (ScoreLevel, [Name], Indicates, [Label]) 
    VALUES (source.ScoreLevel, source.[Name], source.Indicates, [Label])  
;
--------------------
IF OBJECT_ID('dbo.uspUpdateColumbiaSuicideReport') IS NOT NULL
    DROP PROC dbo.uspUpdateColumbiaSuicideReport
GO


CREATE PROCEDURE dbo.uspUpdateColumbiaSuicideReport
    @ID bigint
    ,@FirstName nvarchar(128)
    ,@LastName nvarchar(128)
    ,@MiddleName nvarchar(128)
    ,@Birthday date
    ,@StreetAddress nvarchar(512)
    ,@City nvarchar(255)
    ,@StateID char(2)
    ,@ZipCode char(5)
    ,@Phone char(14)
    ,@BranchLocationID int
    ,@EhrPatientID int
    ,@EhrPatientHRN nvarchar(255)

    ,@BhsStaffNameCompleted nvarchar(128)
    ,@CompleteDate datetimeoffset(7)
    ,@ScoreLevel int
    ,@LifetimeMostSevereIdeationLevel int
    ,@LifetimeMostSevereIdeationDescription nvarchar(max)
    ,@RecentMostSevereIdeationLevel int
    ,@RecentMostSevereIdeationDescription nvarchar(max)
    ,@SuicideMostRecentAttemptDate datetime
    ,@SuicideMostLethalRecentAttemptDate datetime
    ,@SuicideFirstAttemptDate datetime
    ,@MedicalDamageMostRecentAttemptCode int
    ,@MedicalDamageMostLethalAttemptCode int
    ,@MedicalDamageFirstAttemptCode int
    ,@PotentialLethalityMostRecentAttemptCode int
    ,@PotentialLethalityMostLethalAttemptCode int
    ,@PotentialLethalityFirstAttemptCode int
AS
    UPDATE dbo.ColumbiaSuicideReport SET
  
         FirstName = @FirstName
        ,LastName = @LastName
        ,MiddleName = @MiddleName
        ,Birthday = @Birthday
        ,StreetAddress = @StreetAddress
        ,City = @City
        ,StateID = @StateID
        ,ZipCode = @ZipCode
        ,Phone = @Phone
        ,BranchLocationID = @BranchLocationID
        ,EhrPatientID = @EhrPatientID
        ,EhrPatientHRN = @EhrPatientHRN

        ,BhsStaffNameCompleted = @BhsStaffNameCompleted
        ,CompleteDate = @CompleteDate
        ,ScoreLevel = @ScoreLevel
        ,LifetimeMostSevereIdeationLevel = @LifetimeMostSevereIdeationLevel
        ,LifetimeMostSevereIdeationDescription = @LifetimeMostSevereIdeationDescription
        ,RecentMostSevereIdeationLevel = @RecentMostSevereIdeationLevel
        ,RecentMostSevereIdeationDescription = @RecentMostSevereIdeationDescription
        ,SuicideMostRecentAttemptDate = @SuicideMostRecentAttemptDate
        ,SuicideMostLethalRecentAttemptDate = @SuicideMostLethalRecentAttemptDate
        ,SuicideFirstAttemptDate = @SuicideFirstAttemptDate
        ,MedicalDamageMostRecentAttemptCode = @MedicalDamageMostRecentAttemptCode
        ,MedicalDamageMostLethalAttemptCode = @MedicalDamageMostLethalAttemptCode
        ,MedicalDamageFirstAttemptCode = @MedicalDamageFirstAttemptCode
        ,PotentialLethalityMostRecentAttemptCode = @PotentialLethalityMostRecentAttemptCode
        ,PotentialLethalityMostLethalAttemptCode = @PotentialLethalityMostLethalAttemptCode
        ,PotentialLethalityFirstAttemptCode = @PotentialLethalityFirstAttemptCode
    WHERE ID = @ID
GO

-------------------
IF OBJECT_ID('dbo.uspUpdateColumbiaSuicideRiskAssessmentReport') IS NOT NULL
    DROP PROC dbo.uspUpdateColumbiaSuicideRiskAssessmentReport
GO

CREATE PROCEDURE dbo.uspUpdateColumbiaSuicideRiskAssessmentReport
    @ColumbiaReportID bigint
    ,@ActualSuicideAttempt bit
    ,@LifetimeActualSuicideAttempt bit
    ,@InterruptedSuicideAttempt bit
    ,@LifetimeInterruptedSuicideAttempt bit
    ,@AbortedSuicideAttempt bit
    ,@LifetimeAbortedSuicideAttempt bit
    ,@OtherPreparatoryActsToKillSelf bit
    ,@LifetimeOtherPreparatoryActsToKillSelf bit
    ,@ActualSelfInjuryBehaviorWithoutSuicideIntent bit
    ,@LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent bit
    ,@WishToBeDead bit
    ,@SuicidalThoughts bit
    ,@SuicidalThoughtsWithMethod bit
    ,@SuicidalIntent bit
    ,@SuicidalIntentWithSpecificPlan bit
    ,@RecentLoss bit
    ,@DescribeRecentLoss nvarchar(max)
    ,@PendingIncarceration bit
    ,@CurrentOrPendingIsolation bit
    ,@Hopelessness bit
    ,@Helplessness bit
    ,@FeelingTrapped bit
    ,@MajorDepressiveEpisode bit
    ,@MixedAffectiveEpisode bit
    ,@CommandHallucinationsToHurtSelf bit
    ,@HighlyImpulsiveBehavior bit
    ,@SubstanceAbuseOrDependence bit
    ,@AgitationOrSevereAnxiety bit
    ,@PerceivedBurdenOnFamilyOrOthers bit
    ,@ChronicPhysicalPain bit
    ,@HomicidalIdeation bit
    ,@AggressiveBehaviorTowardsOthers bit
    ,@MethodForSuicideAvailable bit
    ,@RefusesOrFeelsUnableToAgreeToSafetyPlan bit
    ,@SexualAbuseLifetime bit
    ,@FamilyHistoryOfSuicideLifetime bit
    ,@PreviousPsychiatricDiagnosesAndTreatments bit
    ,@HopelessOrDissatisfiedWithTreatment bit
    ,@NonCompliantWithTreatment bit
    ,@NotReceivingTreatment bit
    ,@IdentifiesReasonsForLiving bit
    ,@ResponsibilityToFamilyOrOthers bit
    ,@SupportiveSocialNetworkOrFamily bit
    ,@FearOfDeathOrDyingDueToPainAndSuffering bit
    ,@BeliefThatSuicideIsImmoral bit
    ,@EngagedInWorkOrSchool bit
    ,@EngagedWithPhoneWorker bit
    ,@DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior nvarchar(max)
AS
UPDATE [dbo].[ColumbiaSuicideRiskAssessmentReport] SET
    ActualSuicideAttempt= @ActualSuicideAttempt
    ,LifetimeActualSuicideAttempt= @LifetimeActualSuicideAttempt
    ,InterruptedSuicideAttempt= @InterruptedSuicideAttempt
    ,LifetimeInterruptedSuicideAttempt= @LifetimeInterruptedSuicideAttempt
    ,AbortedSuicideAttempt= @AbortedSuicideAttempt
    ,LifetimeAbortedSuicideAttempt= @LifetimeAbortedSuicideAttempt
    ,OtherPreparatoryActsToKillSelf= @OtherPreparatoryActsToKillSelf
    ,LifetimeOtherPreparatoryActsToKillSelf= @LifetimeOtherPreparatoryActsToKillSelf
    ,ActualSelfInjuryBehaviorWithoutSuicideIntent= @ActualSelfInjuryBehaviorWithoutSuicideIntent
    ,LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent= @LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent
    ,WishToBeDead= @WishToBeDead
    ,SuicidalThoughts= @SuicidalThoughts
    ,SuicidalThoughtsWithMethod= @SuicidalThoughtsWithMethod
    ,SuicidalIntent= @SuicidalIntent
    ,SuicidalIntentWithSpecificPlan= @SuicidalIntentWithSpecificPlan
    ,RecentLoss= @RecentLoss
    ,DescribeRecentLoss= @DescribeRecentLoss
    ,PendingIncarceration= @PendingIncarceration
    ,CurrentOrPendingIsolation= @CurrentOrPendingIsolation
    ,Hopelessness= @Hopelessness
    ,Helplessness= @Helplessness
    ,FeelingTrapped= @FeelingTrapped
    ,MajorDepressiveEpisode= @MajorDepressiveEpisode
    ,MixedAffectiveEpisode= @MixedAffectiveEpisode
    ,CommandHallucinationsToHurtSelf= @CommandHallucinationsToHurtSelf
    ,HighlyImpulsiveBehavior= @HighlyImpulsiveBehavior
    ,SubstanceAbuseOrDependence= @SubstanceAbuseOrDependence
    ,AgitationOrSevereAnxiety= @AgitationOrSevereAnxiety
    ,PerceivedBurdenOnFamilyOrOthers= @PerceivedBurdenOnFamilyOrOthers
    ,ChronicPhysicalPain= @ChronicPhysicalPain
    ,HomicidalIdeation= @HomicidalIdeation
    ,AggressiveBehaviorTowardsOthers= @AggressiveBehaviorTowardsOthers
    ,MethodForSuicideAvailable= @MethodForSuicideAvailable
    ,RefusesOrFeelsUnableToAgreeToSafetyPlan= @RefusesOrFeelsUnableToAgreeToSafetyPlan
    ,SexualAbuseLifetime= @SexualAbuseLifetime
    ,FamilyHistoryOfSuicideLifetime= @FamilyHistoryOfSuicideLifetime
    ,PreviousPsychiatricDiagnosesAndTreatments= @PreviousPsychiatricDiagnosesAndTreatments
    ,HopelessOrDissatisfiedWithTreatment= @HopelessOrDissatisfiedWithTreatment
    ,NonCompliantWithTreatment= @NonCompliantWithTreatment
    ,NotReceivingTreatment= @NotReceivingTreatment
    ,IdentifiesReasonsForLiving= @IdentifiesReasonsForLiving
    ,ResponsibilityToFamilyOrOthers= @ResponsibilityToFamilyOrOthers
    ,SupportiveSocialNetworkOrFamily= @SupportiveSocialNetworkOrFamily
    ,FearOfDeathOrDyingDueToPainAndSuffering= @FearOfDeathOrDyingDueToPainAndSuffering
    ,BeliefThatSuicideIsImmoral= @BeliefThatSuicideIsImmoral
    ,EngagedInWorkOrSchool= @EngagedInWorkOrSchool
    ,EngagedWithPhoneWorker= @EngagedWithPhoneWorker
    ,DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior= @DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior
WHERE ColumbiaReportID = @ColumbiaReportID

GO

-------------------
IF OBJECT_ID('dbo.[vColumbiaSuicideReport]') IS NOT NULL
    DROP VIEW [dbo].[vColumbiaSuicideReport]
GO
;

CREATE VIEW [dbo].[vColumbiaSuicideReport]
AS SELECT 
v.ID
,v.PatientName as FullName
,v.LastName
,v.FirstName
,v.MiddleName
,v.Birthday
,v.StreetAddress
,v.City
,v.StateID
,state.Name as StateName
,v.ZipCode
,v.Phone
,v.EhrPatientID
,v.EhrPatientHRN
,pd.ID as DemographicsID

,v.CreatedDate
,v.CompleteDate
,v.[BhsStaffNameCompleted]
,v.BranchLocationID as BranchLocationID
,l.Name as BranchLocationName

,v.ScreeningResultID
,v.BhsVisitID

,v.[LifetimeMostSevereIdeationLevel]
,v.[LifetimeMostSevereIdeationDescription]
,v.[RecentMostSevereIdeationLevel]
,v.[RecentMostSevereIdeationDescription]
,v.[SuicideMostRecentAttemptDate]
,v.[SuicideMostLethalRecentAttemptDate]
,v.[SuicideFirstAttemptDate]
,v.[MedicalDamageMostRecentAttemptCode]
,v.[MedicalDamageMostLethalAttemptCode]
,v.[MedicalDamageFirstAttemptCode]
,v.[PotentialLethalityMostRecentAttemptCode]
,v.[PotentialLethalityMostLethalAttemptCode]
,v.[PotentialLethalityFirstAttemptCode]

,ssrsRiskAsmt.[ColumbiaReportID]
,ssrsRiskAsmt.[ActualSuicideAttempt]
,ssrsRiskAsmt.[LifetimeActualSuicideAttempt]
,ssrsRiskAsmt.[InterruptedSuicideAttempt]
,ssrsRiskAsmt.[LifetimeInterruptedSuicideAttempt]
,ssrsRiskAsmt.[AbortedSuicideAttempt]
,ssrsRiskAsmt.[LifetimeAbortedSuicideAttempt]
,ssrsRiskAsmt.[OtherPreparatoryActsToKillSelf]
,ssrsRiskAsmt.[LifetimeOtherPreparatoryActsToKillSelf]
,ssrsRiskAsmt.[ActualSelfInjuryBehaviorWithoutSuicideIntent]
,ssrsRiskAsmt.[LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent]
,ssrsRiskAsmt.[WishToBeDead]
,ssrsRiskAsmt.[SuicidalThoughts]
,ssrsRiskAsmt.[SuicidalThoughtsWithMethod]
,ssrsRiskAsmt.[SuicidalIntent]
,ssrsRiskAsmt.[SuicidalIntentWithSpecificPlan]
,ssrsRiskAsmt.[RecentLoss]
,ssrsRiskAsmt.[DescribeRecentLoss]
,ssrsRiskAsmt.[PendingIncarceration]
,ssrsRiskAsmt.[CurrentOrPendingIsolation]
,ssrsRiskAsmt.[Hopelessness]
,ssrsRiskAsmt.[Helplessness]
,ssrsRiskAsmt.[FeelingTrapped]
,ssrsRiskAsmt.[MajorDepressiveEpisode]
,ssrsRiskAsmt.[MixedAffectiveEpisode]
,ssrsRiskAsmt.[CommandHallucinationsToHurtSelf]
,ssrsRiskAsmt.[HighlyImpulsiveBehavior]
,ssrsRiskAsmt.[SubstanceAbuseOrDependence]
,ssrsRiskAsmt.[AgitationOrSevereAnxiety]
,ssrsRiskAsmt.[PerceivedBurdenOnFamilyOrOthers]
,ssrsRiskAsmt.[ChronicPhysicalPain]
,ssrsRiskAsmt.[HomicidalIdeation]
,ssrsRiskAsmt.[AggressiveBehaviorTowardsOthers]
,ssrsRiskAsmt.[MethodForSuicideAvailable]
,ssrsRiskAsmt.[RefusesOrFeelsUnableToAgreeToSafetyPlan]
,ssrsRiskAsmt.[SexualAbuseLifetime]
,ssrsRiskAsmt.[FamilyHistoryOfSuicideLifetime]
,ssrsRiskAsmt.[PreviousPsychiatricDiagnosesAndTreatments]
,ssrsRiskAsmt.[HopelessOrDissatisfiedWithTreatment]
,ssrsRiskAsmt.[NonCompliantWithTreatment]
,ssrsRiskAsmt.[NotReceivingTreatment]
,ssrsRiskAsmt.[IdentifiesReasonsForLiving]
,ssrsRiskAsmt.[ResponsibilityToFamilyOrOthers]
,ssrsRiskAsmt.[SupportiveSocialNetworkOrFamily]
,ssrsRiskAsmt.[FearOfDeathOrDyingDueToPainAndSuffering]
,ssrsRiskAsmt.[BeliefThatSuicideIsImmoral]
,ssrsRiskAsmt.[EngagedInWorkOrSchool]
,ssrsRiskAsmt.[EngagedWithPhoneWorker]
,ssrsRiskAsmt.[DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior]

FROM 
    dbo.ColumbiaSuicideReport v
    LEFT JOIN dbo.BranchLocation l ON v.BranchLocationID = l.BranchLocationID
  
    LEFT JOIN dbo.ColumbiaSuicideRiskScoreLevel ssrsLevel ON ssrsLevel.ScoreLevel = v.ScoreLevel
    LEFT JOIN dbo.ColumbiaSuicideRiskAssessmentReport ssrsRiskAsmt ON v.ID = ssrsRiskAsmt.ColumbiaReportID
    LEFT JOIN dbo.BhsDemographics pd ON pd.Birthday = v.Birthday AND pd.PatientName = v.PatientName
    LEFT JOIN dbo.State state ON v.StateID = state.StateCode 
;
GO


---------------------

IF OBJECT_ID('dbo.[uspGetColumbiaSuicideReport]') IS NOT NULL
    DROP PROC [dbo].uspGetColumbiaSuicideReport
GO
;


CREATE PROCEDURE [dbo].[uspGetColumbiaSuicideReport]
    @ID bigint
AS
    SELECT [ID]
      ,[FullName]
      ,[LastName]
      ,[FirstName]
      ,[MiddleName]
      ,[Birthday]
      ,[StreetAddress]
      ,[City]
      ,[StateID]
      ,[StateName]
      ,[ZipCode]
      ,[Phone]
      ,[EhrPatientID]
      ,[EhrPatientHRN]
      ,[DemographicsID]
      ,[CreatedDate]
      ,[CompleteDate]
      ,[BhsStaffNameCompleted]
      ,[BranchLocationID]
      ,[BranchLocationName]
      ,[ScreeningResultID]
      ,[BhsVisitID]
      ,[LifetimeMostSevereIdeationLevel]
      ,[LifetimeMostSevereIdeationDescription]
      ,[RecentMostSevereIdeationLevel]
      ,[RecentMostSevereIdeationDescription]
      ,[SuicideMostRecentAttemptDate]
      ,[SuicideMostLethalRecentAttemptDate]
      ,[SuicideFirstAttemptDate]
      ,[MedicalDamageMostRecentAttemptCode]
      ,[MedicalDamageMostLethalAttemptCode]
      ,[MedicalDamageFirstAttemptCode]
      ,[PotentialLethalityMostRecentAttemptCode]
      ,[PotentialLethalityMostLethalAttemptCode]
      ,[PotentialLethalityFirstAttemptCode]
      ,[ColumbiaReportID]
      ,[ActualSuicideAttempt]
      ,[LifetimeActualSuicideAttempt]
      ,[InterruptedSuicideAttempt]
      ,[LifetimeInterruptedSuicideAttempt]
      ,[AbortedSuicideAttempt]
      ,[LifetimeAbortedSuicideAttempt]
      ,[OtherPreparatoryActsToKillSelf]
      ,[LifetimeOtherPreparatoryActsToKillSelf]
      ,[ActualSelfInjuryBehaviorWithoutSuicideIntent]
      ,[LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent]
      ,[WishToBeDead]
      ,[SuicidalThoughts]
      ,[SuicidalThoughtsWithMethod]
      ,[SuicidalIntent]
      ,[SuicidalIntentWithSpecificPlan]
      ,[RecentLoss]
      ,[DescribeRecentLoss]
      ,[PendingIncarceration]
      ,[CurrentOrPendingIsolation]
      ,[Hopelessness]
      ,[Helplessness]
      ,[FeelingTrapped]
      ,[MajorDepressiveEpisode]
      ,[MixedAffectiveEpisode]
      ,[CommandHallucinationsToHurtSelf]
      ,[HighlyImpulsiveBehavior]
      ,[SubstanceAbuseOrDependence]
      ,[AgitationOrSevereAnxiety]
      ,[PerceivedBurdenOnFamilyOrOthers]
      ,[ChronicPhysicalPain]
      ,[HomicidalIdeation]
      ,[AggressiveBehaviorTowardsOthers]
      ,[MethodForSuicideAvailable]
      ,[RefusesOrFeelsUnableToAgreeToSafetyPlan]
      ,[SexualAbuseLifetime]
      ,[FamilyHistoryOfSuicideLifetime]
      ,[PreviousPsychiatricDiagnosesAndTreatments]
      ,[HopelessOrDissatisfiedWithTreatment]
      ,[NonCompliantWithTreatment]
      ,[NotReceivingTreatment]
      ,[IdentifiesReasonsForLiving]
      ,[ResponsibilityToFamilyOrOthers]
      ,[SupportiveSocialNetworkOrFamily]
      ,[FearOfDeathOrDyingDueToPainAndSuffering]
      ,[BeliefThatSuicideIsImmoral]
      ,[EngagedInWorkOrSchool]
      ,[EngagedWithPhoneWorker]
      ,[DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior]
  FROM [dbo].[vColumbiaSuicideReport]
  WHERE ID = @ID

  -- select ideation
  SELECT [ColumbiaReportID]
      ,[QuestionID]
      ,[LifetimeMostSucidal]
      ,[PastLastMonth]
      ,[Description]
  FROM [dbo].[ColumbiaSuicidalIdeation]
  WHERE ColumbiaReportID = @ID
  ORDER BY QuestionID ASC


    -- select intensivity
  SELECT [ColumbiaReportID]
      ,[QuestionID]
      ,[LifetimeMostSevere]
      ,[RecentMostSevere]
  FROM [dbo].[ColumbiaIntensityIdeation]
  WHERE ColumbiaReportID = @ID
  ORDER BY QuestionID ASC

  -- behavior
  SELECT [ColumbiaReportID]
      ,[QuestionID]
      ,[LifetimeLevel]
      ,[LifetimeCount]
      ,[PastThreeMonths]
      ,[PastThreeMonthsCount]
      ,[Description]
  FROM [dbo].[ColumbiaSuicideBehaviorAct]
  WHERE ColumbiaReportID = @ID
  ORDER BY QuestionID ASC


  -- other protectve behavior
  SELECT [ColumbiaReportID]
      ,[ItemID]
      ,[ProtectiveFactor]
  FROM [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors]
  WHERE ColumbiaReportID = @ID
  ORDER BY ItemID ASC


  -- Other Risk factors
  SELECT [ColumbiaReportID]
      ,[ItemID]
      ,[RiskFactor]
  FROM [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors]
  WHERE ColumbiaReportID = @ID
  ORDER BY ItemID ASC

GO

---- permissions
GRANT UPDATE, INSERT ON dbo.ColumbiaSuicideReport TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaIntensityIdeation TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicidalIdeation TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideBehaviorAct TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideRiskAssessmentReport TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors TO [screendox_appaccount];
GRANT DELETE, UPDATE, INSERT ON dbo.ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors TO [screendox_appaccount];

GO


-------------------

IF NOT EXISTS(SELECT 1 FROM dbo.DbVersion WHERE DbVersion =  '11.0.2.0')
INSERT INTO dbo.DbVersion(DbVersion) VALUES('11.0.2.0');