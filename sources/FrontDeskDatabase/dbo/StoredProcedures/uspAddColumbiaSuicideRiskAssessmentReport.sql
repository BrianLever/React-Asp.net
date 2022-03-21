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

