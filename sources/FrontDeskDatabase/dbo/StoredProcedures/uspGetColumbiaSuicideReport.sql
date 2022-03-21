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

