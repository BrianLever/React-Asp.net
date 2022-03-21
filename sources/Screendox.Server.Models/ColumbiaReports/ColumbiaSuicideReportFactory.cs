using FrontDesk.Common.Extensions;

using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ColumbiaReports
{
    public static class ColumbiaSuicideReportFactory
    {
        public static ColumbiaSuicideReport Create(IDataReader reader)
        {
            var columnNames = reader.GetColumnNames();

            var result = new ColumbiaSuicideReport
            {
                ID = reader.Get<long>("ID"),
                FullName = reader.Get<string>("FullName"),
                LastName = reader.Get<string>("LastName"),
                FirstName = reader.Get<string>("FirstName"),
                MiddleName = reader.Get<string>("MiddleName"),
                Birthday = reader.Get<DateTime>("Birthday"),
                StreetAddress = reader.Get<string>("StreetAddress"),
                City = reader.Get<string>("City"),
                StateID = reader.Get<string>("StateID"),
                StateName = reader.Get<string>("StateName"),
                ZipCode = reader.Get<string>("ZipCode"),
                Phone = reader.Get<string>("Phone"),
                EhrPatientID = reader.Get<int>("EhrPatientID"),
                EhrPatientHRN = reader.Get<string>("EhrPatientHRN"),
                DemographicsID = reader.GetNullable<long>("DemographicsID"),
                CreatedDate = reader.Get<DateTimeOffset>("CreatedDate"),
                CompleteDate = reader.GetNullable<DateTimeOffset>("CompleteDate"),
                BhsStaffNameCompleted = reader.Get<string>("BhsStaffNameCompleted"),
                BranchLocationID = reader.Get<int>("BranchLocationID"),
                BranchLocationName = reader.Get<string>("BranchLocationName"),

                ScreeningResultID = reader.GetNullable<long>("ScreeningResultID"),
                BhsVisitID = reader.GetNullable<int>("BhsVisitID"),
                LifetimeMostSevereIdeationLevel = reader.GetNullable<int>("LifetimeMostSevereIdeationLevel"),
                LifetimeMostSevereIdeationDescription = reader.Get<string>("LifetimeMostSevereIdeationDescription"),

                RecentMostSevereIdeationLevel = reader.GetNullable<int>("RecentMostSevereIdeationLevel"),
                RecentMostSevereIdeationDescription = reader.Get<string>("RecentMostSevereIdeationDescription"),
                SuicideMostRecentAttemptDate = reader.GetNullable<DateTime>("SuicideMostRecentAttemptDate"),
                SuicideMostLethalRecentAttemptDate = reader.GetNullable<DateTime>("SuicideMostLethalRecentAttemptDate"),
                SuicideFirstAttemptDate = reader.GetNullable<DateTime>("SuicideFirstAttemptDate"),

                ActualLethality = new ColumbiaSuicialBehaviorLethality
                {
                    QuestionId = ColumbiaSuicideBehaviorLethalityQuesions.ActualLethality,
                    MostRecentAttemptCode = reader.GetNullable<int>("MedicalDamageMostRecentAttemptCode"),
                    MostLethalAttemptCode = reader.GetNullable<int>("MedicalDamageMostLethalAttemptCode"),
                    InitialAttemptCode = reader.GetNullable<int>("MedicalDamageFirstAttemptCode"),

                },
                PotentialLethality = new ColumbiaSuicialBehaviorLethality
                {
                    QuestionId = ColumbiaSuicideBehaviorLethalityQuesions.PotentialLethality,
                    MostRecentAttemptCode = reader.GetNullable<int>("PotentialLethalityMostRecentAttemptCode"),
                    MostLethalAttemptCode = reader.GetNullable<int>("PotentialLethalityMostLethalAttemptCode"),
                    InitialAttemptCode = reader.GetNullable<int>("PotentialLethalityFirstAttemptCode"),
                },

                RiskAssessmentReport = new ColumbiaRiskAssessmentReport
                {
                    ID = reader.Get<long>("ColumbiaReportID"),
                    ActualSuicideAttempt = reader.GetNullable<bool>("ActualSuicideAttempt") ?? false,
                    LifetimeActualSuicideAttempt = reader.GetNullable<bool>("LifetimeActualSuicideAttempt") ?? false,
                    InterruptedSuicideAttempt = reader.GetNullable<bool>("InterruptedSuicideAttempt") ?? false,
                    LifetimeInterruptedSuicideAttempt = reader.GetNullable<bool>("LifetimeInterruptedSuicideAttempt") ?? false,
                    AbortedSuicideAttempt = reader.GetNullable<bool>("AbortedSuicideAttempt") ?? false,
                    LifetimeAbortedSuicideAttempt = reader.GetNullable<bool>("LifetimeAbortedSuicideAttempt") ?? false,
                    OtherPreparatoryActsToKillSelf = reader.GetNullable<bool>("OtherPreparatoryActsToKillSelf") ?? false,
                    LifetimeOtherPreparatoryActsToKillSelf = reader.GetNullable<bool>("LifetimeOtherPreparatoryActsToKillSelf") ?? false,
                    ActualSelfInjuryBehaviorWithoutSuicideIntent = reader.GetNullable<bool>("ActualSelfInjuryBehaviorWithoutSuicideIntent") ?? false,
                    LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent = reader.GetNullable<bool>("LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent") ?? false,
                    WishToBeDead = reader.GetNullable<bool>("WishToBeDead") ?? false,
                    SuicidalThoughts = reader.GetNullable<bool>("SuicidalThoughts") ?? false,
                    SuicidalIntent = reader.GetNullable<bool>("SuicidalIntent") ?? false,
                    SuicidalIntentWithSpecificPlan = reader.GetNullable<bool>("SuicidalIntentWithSpecificPlan") ?? false,
                    RecentLoss = reader.GetNullable<bool>("RecentLoss") ?? false,
                    DescribeRecentLoss = reader.Get<string>("DescribeRecentLoss"),
                    PendingIncarceration = reader.GetNullable<bool>("PendingIncarceration") ?? false,
                    CurrentOrPendingIsolation = reader.GetNullable<bool>("CurrentOrPendingIsolation") ?? false,
                    Hopelessness = reader.GetNullable<bool>("Hopelessness") ?? false,
                    Helplessness = reader.GetNullable<bool>("Helplessness") ?? false,
                    FeelingTrapped = reader.GetNullable<bool>("FeelingTrapped") ?? false,
                    MajorDepressiveEpisode = reader.GetNullable<bool>("MajorDepressiveEpisode") ?? false,
                    MixedAffectiveEpisode = reader.GetNullable<bool>("MixedAffectiveEpisode") ?? false,
                    CommandHallucinationsToHurtSelf = reader.GetNullable<bool>("CommandHallucinationsToHurtSelf") ?? false,
                    HighlyImpulsiveBehavior = reader.GetNullable<bool>("HighlyImpulsiveBehavior") ?? false,
                    SubstanceAbuseOrDependence = reader.GetNullable<bool>("SubstanceAbuseOrDependence") ?? false,
                    AgitationOrSevereAnxiety = reader.GetNullable<bool>("AgitationOrSevereAnxiety") ?? false,
                    PerceivedBurdenOnFamilyOrOthers = reader.GetNullable<bool>("PerceivedBurdenOnFamilyOrOthers") ?? false,
                    ChronicPhysicalPain = reader.GetNullable<bool>("ChronicPhysicalPain") ?? false,
                    HomicidalIdeation = reader.GetNullable<bool>("HomicidalIdeation") ?? false,
                    AggressiveBehaviorTowardsOthers = reader.GetNullable<bool>("AggressiveBehaviorTowardsOthers") ?? false,
                    MethodForSuicideAvailable = reader.GetNullable<bool>("ActualSuicideAttempt") ?? false,
                    RefusesOrFeelsUnableToAgreeToSafetyPlan = reader.GetNullable<bool>("RefusesOrFeelsUnableToAgreeToSafetyPlan") ?? false,
                    SexualAbuseLifetime = reader.GetNullable<bool>("SexualAbuseLifetime") ?? false,


                    FamilyHistoryOfSuicideLifetime = reader.GetNullable<bool>("FamilyHistoryOfSuicideLifetime") ?? false,
                    PreviousPsychiatricDiagnosesAndTreatments = reader.GetNullable<bool>("PreviousPsychiatricDiagnosesAndTreatments") ?? false,
                    HopelessOrDissatisfiedWithTreatment = reader.GetNullable<bool>("HopelessOrDissatisfiedWithTreatment") ?? false,
                    NonCompliantWithTreatment = reader.GetNullable<bool>("NonCompliantWithTreatment") ?? false,
                    NotReceivingTreatment = reader.GetNullable<bool>("NotReceivingTreatment") ?? false,
                    IdentifiesReasonsForLiving = reader.GetNullable<bool>("IdentifiesReasonsForLiving") ?? false,
                    ResponsibilityToFamilyOrOthers = reader.GetNullable<bool>("ResponsibilityToFamilyOrOthers") ?? false,
                    SupportiveSocialNetworkOrFamily = reader.GetNullable<bool>("SupportiveSocialNetworkOrFamily") ?? false,
                    FearOfDeathOrDyingDueToPainAndSuffering = reader.GetNullable<bool>("FearOfDeathOrDyingDueToPainAndSuffering") ?? false,
                    BeliefThatSuicideIsImmoral = reader.GetNullable<bool>("BeliefThatSuicideIsImmoral") ?? false,
                    EngagedInWorkOrSchool = reader.GetNullable<bool>("EngagedInWorkOrSchool") ?? false,
                    EngagedWithPhoneWorker = reader.GetNullable<bool>("EngagedWithPhoneWorker") ?? false,
                    DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior = reader.Get<string>("DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior"),
                }
            };


            return result;
        }

        public static ColumbiaSuicidalIdeation CreateIdeation(IDataReader reader)
        {
            return new ColumbiaSuicidalIdeation
            {
                QuestionId = reader.Get<int>("QuestionID"),
                LifetimeMostSucidal = reader.GetNullable<bool>("LifetimeMostSucidal"),
                PastLastMonth = reader.GetNullable<bool>("PastLastMonth"),
                Description = reader.Get<string>("Description") ?? string.Empty,
            };

        }

        public static ColumbiaIntensityIdeation CreateIntensity(IDataReader reader)
        {
            return new ColumbiaIntensityIdeation
            {
                QuestionId = reader.Get<int>("QuestionID"),
                LifetimeMostSevere = reader.GetNullable<int>("LifetimeMostSevere"),
                RecentMostSevere = reader.GetNullable<int>("RecentMostSevere"),
            };
        }

        public static ColumbiaSuicialBehaviorAct CreateBehavior(IDataReader reader)
        {
            return new ColumbiaSuicialBehaviorAct
            {
                QuestionId = reader.Get<int>("QuestionID"),
                LifetimeLevel = reader.GetNullable<bool>("LifetimeLevel"),
                LifetimeCount = reader.GetNullable<int>("LifetimeCount"),
                PastThreeMonths = reader.GetNullable<bool>("PastThreeMonths"),
                PastThreeMonthsCount = reader.GetNullable<int>("PastThreeMonthsCount"),
                Description = reader.Get<string>("Description"),
            };
        }


        public static void InitIdeation(ColumbiaSuicideReport report, List<ColumbiaSuicidalIdeation> items)
        {

            report.WishToDead =
                items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicidalIdeationQuesions.WithTobeDead) ??
                new ColumbiaSuicidalIdeation
                {
                    QuestionId = ColumbiaSuicidalIdeationQuesions.WithTobeDead,
                };

            report.NonSpecificActiveSuicidalThoughts =
               items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicidalIdeationQuesions.NonSpecificActiveSuicidalThoughts) ??
               new ColumbiaSuicidalIdeation
               {
                   QuestionId = ColumbiaSuicidalIdeationQuesions.NonSpecificActiveSuicidalThoughts,
               };

            report.ActiveSuicidalIdeationWithAnyMethods =
              items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithAnyMethods) ??
              new ColumbiaSuicidalIdeation
              {
                  QuestionId = ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithAnyMethods,
              };

            report.ActiveSuicidalIdeationWithSomeIntentToAct =
              items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithSomeIntentToAct) ??
              new ColumbiaSuicidalIdeation
              {
                  QuestionId = ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithSomeIntentToAct,
              };

            report.ActiveSuicidalIdeationWithSpecificPlanAndIntent =
              items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithSpecificPlanAndIntent) ??
              new ColumbiaSuicidalIdeation
              {
                  QuestionId = ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithSpecificPlanAndIntent,
              };

        }



        public static void InitIntensity(ColumbiaSuicideReport result, List<ColumbiaIntensityIdeation> items)
        {
            result.Frequency =
               items.FirstOrDefault(x => x.QuestionId == ColumbiaIntensityOfIdeationQuesions.Frequency) ??
               new ColumbiaIntensityIdeation
               {
                   QuestionId = ColumbiaIntensityOfIdeationQuesions.Frequency,
               };

            result.Duration =
                items.FirstOrDefault(x => x.QuestionId == ColumbiaIntensityOfIdeationQuesions.Duration) ??
                new ColumbiaIntensityIdeation
                {
                    QuestionId = ColumbiaIntensityOfIdeationQuesions.Duration,
                };

            result.Controllability =
               items.FirstOrDefault(x => x.QuestionId == ColumbiaIntensityOfIdeationQuesions.Controllability) ??
               new ColumbiaIntensityIdeation
               {
                   QuestionId = ColumbiaIntensityOfIdeationQuesions.Controllability,
               };

            result.Deterrents =
               items.FirstOrDefault(x => x.QuestionId == ColumbiaIntensityOfIdeationQuesions.Deterrents) ??
               new ColumbiaIntensityIdeation
               {
                   QuestionId = ColumbiaIntensityOfIdeationQuesions.Deterrents,
               };

            result.ReasonsForIdeation =
               items.FirstOrDefault(x => x.QuestionId == ColumbiaIntensityOfIdeationQuesions.ReasonsForIdeation) ??
               new ColumbiaIntensityIdeation
               {
                   QuestionId = ColumbiaIntensityOfIdeationQuesions.ReasonsForIdeation,
               };

        }



        public static void InitBehavior(ColumbiaSuicideReport result, List<ColumbiaSuicialBehaviorAct> items)
        {
            result.ActualAttempt =
              items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicideBehaviorActQuesions.ActualAttempt) ??
              new ColumbiaSuicialBehaviorAct
              {
                  QuestionId = ColumbiaSuicideBehaviorActQuesions.ActualAttempt,
              };

            result.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior =
              items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicideBehaviorActQuesions.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior) ??
              new ColumbiaSuicialBehaviorAct
              {
                  QuestionId = ColumbiaSuicideBehaviorActQuesions.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior,
              };

            result.InterruptedAttempt =
                items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicideBehaviorActQuesions.InterruptedAttempt) ??
                new ColumbiaSuicialBehaviorAct
                {
                    QuestionId = ColumbiaSuicideBehaviorActQuesions.InterruptedAttempt,
                };

            result.AbortedAttempt =
              items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicideBehaviorActQuesions.AbortedAttempt) ??
              new ColumbiaSuicialBehaviorAct
              {
                  QuestionId = ColumbiaSuicideBehaviorActQuesions.AbortedAttempt,
              };


            result.PreparatoryActs =
              items.FirstOrDefault(x => x.QuestionId == ColumbiaSuicideBehaviorActQuesions.PreparatoryActs) ??
              new ColumbiaSuicialBehaviorAct
              {
                  QuestionId = ColumbiaSuicideBehaviorActQuesions.PreparatoryActs,
              };
        }

        /// <summary>
        /// Create new Columbia report using patient and address from existing report
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static ColumbiaSuicideReport CopyPatientFrom(ColumbiaSuicideReport source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ColumbiaSuicideReport
            {
                LastName = source.LastName,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                Birthday = source.Birthday,
                EhrPatientHRN = source.EhrPatientHRN,
                EhrPatientID = source.EhrPatientID,
                StreetAddress = source.StreetAddress,
                City = source.City,
                StateID = source.StateID,
                StateName = source.StateName,
                ZipCode = source.ZipCode,
                Phone = source.Phone,
                BranchLocationID = source.BranchLocationID
            };
        }

        public static ColumbiaSuicideReport Create(ColumbiaSuicideCreateRequest source)
        {
            if (source is null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return new ColumbiaSuicideReport
            {
                LastName = source.LastName,
                FirstName = source.FirstName,
                MiddleName = source.MiddleName,
                Birthday = source.Birthday,
                EhrPatientHRN = source.EhrPatientHRN,
                EhrPatientID = source.EhrPatientID,
                StreetAddress = source.StreetAddress,
                City = source.City,
                StateID = source.StateID,
                ZipCode = source.ZipCode,
                Phone = source.Phone,
                BranchLocationID = source.BranchLocationID
            };
        }

    }
}
