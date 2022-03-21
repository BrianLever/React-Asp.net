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

