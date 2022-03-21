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

