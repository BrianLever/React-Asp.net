using Common.Logging;

using FrontDesk;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ColumbiaReports;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Data
{
    public interface IColumbiaSuicideReportRepository: ITransactionalDatabase
    {
        long Add(ColumbiaSuicideReport model);
        void Update(ColumbiaSuicideReport model);
        ColumbiaSuicideReport Get(long id);
        long GetLatestReportsForDisplayCount(PagedFilterModel filter);
        List<UniqueColumbiaReportViewModel> GetLatestReportsForDisplay(PagedFilterModel filter);
        List<ColumbiaSuicideReportSearchResponse> GetRelatedReports(SearchRelatedItemsFilter searchRelatedItemsFilter);
        long? GetLatestReport(IScreeningPatientIdentity columbiaReport);
    }

    public class ColumbiaSuicideReportDb : DBDatabase, IColumbiaSuicideReportRepository
    {

        #region Constructors
        public ColumbiaSuicideReportDb() : base(0)
        {

        }
        #endregion


        public long Add(ColumbiaSuicideReport model)
        {

            // add main record

            ClearParameters();

            AddParameter("@FirstName", DbType.String, 128).Value = model.FirstName.AsSqlParameter();
            AddParameter("@LastName", DbType.String, 128).Value = model.LastName.AsSqlParameter();
            AddParameter("@MiddleName", DbType.String, 128).Value = model.MiddleName.AsSqlParameter();
            AddParameter("@Birthday", DbType.Date).Value = model.Birthday.Date;
            AddParameter("@StreetAddress", DbType.String, 512).Value = model.StreetAddress.AsSqlParameter();
            AddParameter("@City", DbType.String, 255).Value = model.City.AsSqlParameter();
            AddParameter("@StateID", DbType.AnsiString, 2).Value = model.StateID.AsSqlParameter();
            AddParameter("@ZipCode", DbType.AnsiString, 5).Value = model.ZipCode.AsSqlParameter();
            AddParameter("@Phone", DbType.AnsiString, 14).Value = model.Phone.AsSqlParameter();
            AddParameter("@BranchLocationID", DbType.Int32).Value = model.BranchLocationID.AsSqlParameter();

            AddParameter("@EhrPatientID", DbType.Int32).Value = model.EhrPatientID.AsSqlParameter();
            AddParameter("@EhrPatientHRN ", DbType.String, 255).Value = model.EhrPatientHRN.AsSqlParameter();

            AddParameter("@ScreeningResultID", DbType.Int64).Value = model.ScreeningResultID.AsSqlParameter();
            AddParameter("@BhsVisitID", DbType.Int64).Value = model.BhsVisitID.AsSqlParameter();
            AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = model.CreatedDate;
            AddParameter("@BhsStaffNameCompleted", DbType.String, 128).Value = model.BhsStaffNameCompleted.AsSqlParameter();
            AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = model.CompleteDate.AsSqlParameter();

            AddParameter("@ScoreLevel", DbType.Int32).Value = model.ScoreLevel.AsSqlParameter();

            AddParameter("@LifetimeMostSevereIdeationLevel", DbType.Int32).Value = model.LifetimeMostSevereIdeationLevel.AsSqlParameter();
            AddParameter("@LifetimeMostSevereIdeationDescription", DbType.String).Value = model.LifetimeMostSevereIdeationDescription.AsSqlParameter();

            AddParameter("@RecentMostSevereIdeationLevel", DbType.Int32).Value = model.RecentMostSevereIdeationLevel.AsSqlParameter();
            AddParameter("@RecentMostSevereIdeationDescription", DbType.String).Value = model.RecentMostSevereIdeationDescription.AsSqlParameter();

            AddParameter("@SuicideMostRecentAttemptDate", DbType.Date).Value = model.SuicideMostRecentAttemptDate.AsSqlParameter();
            AddParameter("@SuicideMostLethalRecentAttemptDate", DbType.Date).Value = model.SuicideMostLethalRecentAttemptDate.AsSqlParameter();
            AddParameter("@SuicideFirstAttemptDate", DbType.Date).Value = model.SuicideFirstAttemptDate.AsSqlParameter();

            AddParameter("@MedicalDamageMostRecentAttemptCode", DbType.Int32).Value = model.ActualLethality.MostRecentAttemptCode.AsSqlParameter();
            AddParameter("@MedicalDamageMostLethalAttemptCode", DbType.Int32).Value = model.ActualLethality.MostLethalAttemptCode.AsSqlParameter();
            AddParameter("@MedicalDamageFirstAttemptCode", DbType.Int32).Value = model.ActualLethality.InitialAttemptCode.AsSqlParameter();

            AddParameter("@PotentialLethalityMostRecentAttemptCode", DbType.Int32).Value = model.PotentialLethality.MostRecentAttemptCode.AsSqlParameter();
            AddParameter("@PotentialLethalityMostLethalAttemptCode", DbType.Int32).Value = model.PotentialLethality.MostLethalAttemptCode.AsSqlParameter();
            AddParameter("@PotentialLethalityFirstAttemptCode", DbType.Int32).Value = model.PotentialLethality.InitialAttemptCode.AsSqlParameter();


            var IdParam = AddParameter("@ID", Type: DbType.Int64);
            IdParam.Direction = ParameterDirection.Output;


            try
            {
                Connect();
                BeginTransaction();

                // add columbia assessment report
                RunProcedureNonSelectQuery("[dbo].[uspAddColumbiaSuicideReport]");

                model.ID = (long)IdParam.Value;

                CleanupSuicidalItems(model);

                AddSuicidalItems(model);
                AddIntensityItems(model);
                AddSuicideBehaviorItems(model);


                // Add risk report
                AddColumbiaRiskAssessmentReport(model.ID, model.RiskAssessmentReport);


                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
            return model.ID;
        }


        private void CleanupSuicidalItems(ColumbiaSuicideReport model)
        {
            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = model.ID;
            RunProcedureNonSelectQuery("dbo.uspDeleteColumbiaSuicideReportItems");

        }

        private void AddSuicidalItems(ColumbiaSuicideReport model)
        {
            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = model.ID;

            var questionIDParam = AddParameter("@QuestionID", DbType.Int32);
            var lifetimeMostSucidalParam = AddParameter("@LifetimeMostSucidal", DbType.Int32);
            var pastLastMonthParam = AddParameter("@PastLastMonth", DbType.Int32);
            var descriptionParam = AddParameter("@Description", DbType.String);


            var ideationList = new[] {
                model.WishToDead,
                model.NonSpecificActiveSuicidalThoughts,
                model.ActiveSuicidalIdeationWithSomeIntentToAct,
                model.ActiveSuicidalIdeationWithAnyMethods,
                model.ActiveSuicidalIdeationWithSpecificPlanAndIntent
            };

            foreach (var item in ideationList)
            {
                questionIDParam.Value = item.QuestionId;
                lifetimeMostSucidalParam.Value = item.LifetimeMostSucidal.AsSafeIntParameter();
                pastLastMonthParam.Value = item.PastLastMonth.AsSafeIntParameter();
                descriptionParam.Value = item.Description.AsSqlParameter();

                RunProcedureNonSelectQuery("dbo.uspAddColumbiaSuicidalIdeation");
            }
        }

        private void AddIntensityItems(ColumbiaSuicideReport model)
        {
            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = model.ID;

            var questionIDParam = AddParameter("@QuestionID", DbType.Int32);
            var lifetimeMostSevereParam = AddParameter("@LifetimeMostSevere", DbType.Int32);
            var recentMostSevereParam = AddParameter("@RecentMostSevere", DbType.Int32);


            var itemList = new[] {
                model.Frequency,
                model.Duration,
                model.Controllability,
                model.Deterrents,
                model.ReasonsForIdeation
            };

            foreach (var item in itemList)
            {
                questionIDParam.Value = item.QuestionId;
                lifetimeMostSevereParam.Value = item.LifetimeMostSevere.AsSqlParameter();
                recentMostSevereParam.Value = item.RecentMostSevere.AsSqlParameter();

                RunProcedureNonSelectQuery("dbo.uspAddColumbiaIntensityIdeation");
            }
        }

        private void AddSuicideBehaviorItems(ColumbiaSuicideReport model)
        {
            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = model.ID;

            var questionIDParam = AddParameter("@QuestionID", DbType.Int32);
            var lifetimeLevelParam = AddParameter("@LifetimeLevel", DbType.Int32);
            var lifetimeCountParam = AddParameter("@LifetimeCount", DbType.Int32);
            var pastThreeMonthsParam = AddParameter("@PastThreeMonths", DbType.Int32);
            var pastThreeMonthsCountParam = AddParameter("@PastThreeMonthsCount", DbType.Int32);
            var descriptionParam = AddParameter("@Description", DbType.String);


            var itemList = new[] {
                model.ActualAttempt,
                model.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior,
                model.InterruptedAttempt,
                model.AbortedAttempt,
                model.PreparatoryActs
            };

            foreach (var item in itemList)
            {
                questionIDParam.Value = item.QuestionId;
                lifetimeLevelParam.Value = item.LifetimeLevel.AsSafeIntParameter();
                lifetimeCountParam.Value = item.LifetimeCount.AsSqlParameter();
                pastThreeMonthsParam.Value = item.PastThreeMonths.AsSafeIntParameter();
                pastThreeMonthsCountParam.Value = item.PastThreeMonthsCount.AsSqlParameter();
                descriptionParam.Value = item.Description.AsSqlParameter();

                RunProcedureNonSelectQuery("dbo.uspAddColumbiaSuicideBehaviorAct");
            }
        }

        private void AddColumbiaRiskAssessmentReport(long columbiaReportId, ColumbiaRiskAssessmentReport model)
        {
            // add main record


            model.ID = columbiaReportId;

            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = columbiaReportId;
            AddParameter("@ActualSuicideAttempt", DbType.Boolean).Value = model.ActualSuicideAttempt;
            AddParameter("@LifetimeActualSuicideAttempt", DbType.Boolean).Value = model.LifetimeActualSuicideAttempt;
            AddParameter("@InterruptedSuicideAttempt", DbType.Boolean).Value = model.InterruptedSuicideAttempt;
            AddParameter("@LifetimeInterruptedSuicideAttempt", DbType.Boolean).Value = model.LifetimeInterruptedSuicideAttempt;
            AddParameter("@AbortedSuicideAttempt", DbType.Boolean).Value = model.AbortedSuicideAttempt;
            AddParameter("@LifetimeAbortedSuicideAttempt", DbType.Boolean).Value = model.LifetimeAbortedSuicideAttempt;
            AddParameter("@OtherPreparatoryActsToKillSelf", DbType.Boolean).Value = model.OtherPreparatoryActsToKillSelf;
            AddParameter("@LifetimeOtherPreparatoryActsToKillSelf", DbType.Boolean).Value = model.LifetimeOtherPreparatoryActsToKillSelf;
            AddParameter("@ActualSelfInjuryBehaviorWithoutSuicideIntent", DbType.Boolean).Value = model.ActualSelfInjuryBehaviorWithoutSuicideIntent;
            AddParameter("@LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent", DbType.Boolean).Value = model.LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent;
            AddParameter("@WishToBeDead", DbType.Boolean).Value = model.WishToBeDead;
            AddParameter("@SuicidalThoughts", DbType.Boolean).Value = model.SuicidalThoughts;
            AddParameter("@SuicidalThoughtsWithMethod", DbType.Boolean).Value = model.SuicidalThoughtsWithMethod;
            AddParameter("@SuicidalIntent", DbType.Boolean).Value = model.SuicidalIntent;
            AddParameter("@SuicidalIntentWithSpecificPlan", DbType.Boolean).Value = model.SuicidalIntentWithSpecificPlan;
            AddParameter("@RecentLoss", DbType.Boolean).Value = model.RecentLoss;
            AddParameter("@DescribeRecentLoss", DbType.String).Value = model.DescribeRecentLoss.AsSqlParameter();
            AddParameter("@PendingIncarceration", DbType.Boolean).Value = model.PendingIncarceration;
            AddParameter("@CurrentOrPendingIsolation", DbType.Boolean).Value = model.CurrentOrPendingIsolation;
            AddParameter("@Hopelessness", DbType.Boolean).Value = model.Hopelessness;
            AddParameter("@Helplessness", DbType.Boolean).Value = model.Helplessness;
            AddParameter("@FeelingTrapped", DbType.Boolean).Value = model.FeelingTrapped;
            AddParameter("@MajorDepressiveEpisode", DbType.Boolean).Value = model.MajorDepressiveEpisode;
            AddParameter("@MixedAffectiveEpisode", DbType.Boolean).Value = model.MixedAffectiveEpisode;
            AddParameter("@CommandHallucinationsToHurtSelf ", DbType.Boolean).Value = model.CommandHallucinationsToHurtSelf;
            AddParameter("@HighlyImpulsiveBehavior", DbType.Boolean).Value = model.HighlyImpulsiveBehavior;
            AddParameter("@SubstanceAbuseOrDependence", DbType.Boolean).Value = model.SubstanceAbuseOrDependence;
            AddParameter("@AgitationOrSevereAnxiety", DbType.Boolean).Value = model.AgitationOrSevereAnxiety;
            AddParameter("@PerceivedBurdenOnFamilyOrOthers", DbType.Boolean).Value = model.PerceivedBurdenOnFamilyOrOthers;
            AddParameter("@ChronicPhysicalPain", DbType.Boolean).Value = model.ChronicPhysicalPain;
            AddParameter("@HomicidalIdeation", DbType.Boolean).Value = model.HomicidalIdeation;
            AddParameter("@AggressiveBehaviorTowardsOthers", DbType.Boolean).Value = model.AggressiveBehaviorTowardsOthers;
            AddParameter("@MethodForSuicideAvailable", DbType.Boolean).Value = model.MethodForSuicideAvailable;
            AddParameter("@RefusesOrFeelsUnableToAgreeToSafetyPlan", DbType.Boolean).Value = model.RefusesOrFeelsUnableToAgreeToSafetyPlan;
            AddParameter("@SexualAbuseLifetime", DbType.Boolean).Value = model.SexualAbuseLifetime;
            AddParameter("@FamilyHistoryOfSuicideLifetime", DbType.Boolean).Value = model.FamilyHistoryOfSuicideLifetime;
            AddParameter("@PreviousPsychiatricDiagnosesAndTreatments", DbType.Boolean).Value = model.PreviousPsychiatricDiagnosesAndTreatments;
            AddParameter("@HopelessOrDissatisfiedWithTreatment", DbType.Boolean).Value = model.HopelessOrDissatisfiedWithTreatment;
            AddParameter("@NonCompliantWithTreatment", DbType.Boolean).Value = model.NonCompliantWithTreatment;
            AddParameter("@NotReceivingTreatment", DbType.Boolean).Value = model.NotReceivingTreatment;
            AddParameter("@IdentifiesReasonsForLiving", DbType.Boolean).Value = model.IdentifiesReasonsForLiving;
            AddParameter("@ResponsibilityToFamilyOrOthers", DbType.Boolean).Value = model.ResponsibilityToFamilyOrOthers;
            AddParameter("@SupportiveSocialNetworkOrFamily", DbType.Boolean).Value = model.SupportiveSocialNetworkOrFamily;
            AddParameter("@FearOfDeathOrDyingDueToPainAndSuffering", DbType.Boolean).Value = model.FearOfDeathOrDyingDueToPainAndSuffering;
            AddParameter("@BeliefThatSuicideIsImmoral", DbType.Boolean).Value = model.BeliefThatSuicideIsImmoral;
            AddParameter("@EngagedInWorkOrSchool", DbType.Boolean).Value = model.EngagedInWorkOrSchool;
            AddParameter("@EngagedWithPhoneWorker", DbType.Boolean).Value = model.EngagedWithPhoneWorker;
            AddParameter("@DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior", DbType.String).Value = model.DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior.AsSqlParameter();


            RunProcedureNonSelectQuery("[dbo].[uspAddColumbiaSuicideRiskAssessmentReport]");


            UpdateSuicidalRiskFactors(model);
            UpdateSuicidalProtectiveFactors(model);
        }

        private void UpdateSuicidalRiskFactors(ColumbiaRiskAssessmentReport model)
        {
            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = model.ID;


            //clean up
            RunNonSelectQuery(@"DELETE from [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherRiskFactors] 
WHERE ColumbiaReportID = @ColumbiaReportID");

            var itemIdParam = AddParameter("@ItemID", DbType.Int32);
            var riskFactorParam = AddParameter("@RiskFactor", DbType.String);

            // add
            var index = 1;
            foreach (var item in model.OtherRisksFactors)
            {
                itemIdParam.Value = index++;
                riskFactorParam.Value = item?.Trim().AsSqlParameter();

                RunProcedureNonSelectQuery("dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherRiskFactors");
            }
        }

        private void UpdateSuicidalProtectiveFactors(ColumbiaRiskAssessmentReport model)
        {
            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = model.ID;

            //clean up
            RunNonSelectQuery(@"DELETE from [dbo].[ColumbiaSuicideRiskAssessmentReport_OtherProtectiveFactors] 
WHERE ColumbiaReportID = @ColumbiaReportID");


            var itemIdParam = AddParameter("@ItemID", DbType.Int32);
            var riskFactorParam = AddParameter("@RiskFactor", DbType.String);



            // add
            var index = 1;
            foreach (var item in model.OtherProtectiveFactors)
            {
                itemIdParam.Value = index++;
                riskFactorParam.Value = item?.Trim().AsSqlParameter();

                RunProcedureNonSelectQuery("dbo.uspAddColumbiaSuicideRiskAssessmentReportOtherProtectiveFactors");
            }
        }

        public void Update(ColumbiaSuicideReport model)
        {
            // add main record

            ClearParameters();

            AddParameter("@ID", Type: DbType.Int64).Value = model.ID;

            AddParameter("@FirstName", DbType.String, 128).Value = model.FirstName.AsSqlParameter();
            AddParameter("@LastName", DbType.String, 128).Value = model.LastName.AsSqlParameter();
            AddParameter("@MiddleName", DbType.String, 128).Value = model.MiddleName.AsSqlParameter();
            AddParameter("@Birthday", DbType.Date).Value = model.Birthday.Date;
            AddParameter("@StreetAddress", DbType.String, 512).Value = model.StreetAddress.AsSqlParameter();
            AddParameter("@City", DbType.String, 255).Value = model.City.AsSqlParameter();
            AddParameter("@StateID", DbType.AnsiString, 2).Value = model.StateID.AsSqlParameter();
            AddParameter("@ZipCode", DbType.AnsiString, 5).Value = model.ZipCode.AsSqlParameter();
            AddParameter("@Phone", DbType.AnsiString, 14).Value = model.Phone.AsSqlParameter();
            AddParameter("@BranchLocationID", DbType.Int32).Value = model.BranchLocationID.AsSqlParameter();

            AddParameter("@EhrPatientID", DbType.Int32).Value = model.EhrPatientID.AsSqlParameter();
            AddParameter("@EhrPatientHRN ", DbType.String, 255).Value = model.EhrPatientHRN.AsSqlParameter();

            AddParameter("@BhsStaffNameCompleted", DbType.String, 128).Value = SqlParameterSafe(model.BhsStaffNameCompleted);
            AddParameter("@CompleteDate", DbType.DateTimeOffset).Value = SqlParameterSafe(model.CompleteDate);

            AddParameter("@ScoreLevel", DbType.Int32).Value = model.ScoreLevel.AsSqlParameter();

            AddParameter("@LifetimeMostSevereIdeationLevel", DbType.Int32).Value = model.LifetimeMostSevereIdeationLevel.AsSqlParameter();
            AddParameter("@LifetimeMostSevereIdeationDescription", DbType.String).Value = model.LifetimeMostSevereIdeationDescription.AsSqlParameter();

            AddParameter("@RecentMostSevereIdeationLevel", DbType.Int32).Value = model.RecentMostSevereIdeationLevel.AsSqlParameter();
            AddParameter("@RecentMostSevereIdeationDescription", DbType.String).Value = model.RecentMostSevereIdeationDescription.AsSqlParameter();

            AddParameter("@SuicideMostRecentAttemptDate", DbType.Date).Value = model.SuicideMostRecentAttemptDate.AsSqlParameter();
            AddParameter("@SuicideMostLethalRecentAttemptDate", DbType.Date).Value = model.SuicideMostLethalRecentAttemptDate.AsSqlParameter();
            AddParameter("@SuicideFirstAttemptDate", DbType.Date).Value = model.SuicideFirstAttemptDate.AsSqlParameter();

            AddParameter("@MedicalDamageMostRecentAttemptCode", DbType.Int32).Value = model.ActualLethality.MostRecentAttemptCode.AsSqlParameter();
            AddParameter("@MedicalDamageMostLethalAttemptCode", DbType.Int32).Value = model.ActualLethality.MostLethalAttemptCode.AsSqlParameter();
            AddParameter("@MedicalDamageFirstAttemptCode", DbType.Int32).Value = model.ActualLethality.InitialAttemptCode.AsSqlParameter();

            AddParameter("@PotentialLethalityMostRecentAttemptCode", DbType.Int32).Value = model.PotentialLethality.MostRecentAttemptCode.AsSqlParameter();
            AddParameter("@PotentialLethalityMostLethalAttemptCode", DbType.Int32).Value = model.PotentialLethality.MostLethalAttemptCode.AsSqlParameter();
            AddParameter("@PotentialLethalityFirstAttemptCode", DbType.Int32).Value = model.PotentialLethality.InitialAttemptCode.AsSqlParameter();

            try
            {
                Connect();
                BeginTransaction();

                // add columbia assessment report
                RunProcedureNonSelectQuery("[dbo].[uspUpdateColumbiaSuicideReport]");

                CleanupSuicidalItems(model);

                AddSuicidalItems(model);
                AddIntensityItems(model);
                AddSuicideBehaviorItems(model);


                // Add risk report
                UpdateColumbiaRiskAssessmentReport(model.ID, model.RiskAssessmentReport);


                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        private void UpdateColumbiaRiskAssessmentReport(long columbiaReportId, ColumbiaRiskAssessmentReport model)
        {
            model.ID = columbiaReportId;

            ClearParameters();

            AddParameter("@ColumbiaReportID", DbType.Int64).Value = columbiaReportId;
            AddParameter("@ActualSuicideAttempt", DbType.Boolean).Value = model.ActualSuicideAttempt;
            AddParameter("@LifetimeActualSuicideAttempt", DbType.Boolean).Value = model.LifetimeActualSuicideAttempt;
            AddParameter("@InterruptedSuicideAttempt", DbType.Boolean).Value = model.InterruptedSuicideAttempt;
            AddParameter("@LifetimeInterruptedSuicideAttempt", DbType.Boolean).Value = model.LifetimeInterruptedSuicideAttempt;
            AddParameter("@AbortedSuicideAttempt", DbType.Boolean).Value = model.AbortedSuicideAttempt;
            AddParameter("@LifetimeAbortedSuicideAttempt", DbType.Boolean).Value = model.LifetimeAbortedSuicideAttempt;
            AddParameter("@OtherPreparatoryActsToKillSelf", DbType.Boolean).Value = model.OtherPreparatoryActsToKillSelf;
            AddParameter("@LifetimeOtherPreparatoryActsToKillSelf", DbType.Boolean).Value = model.LifetimeOtherPreparatoryActsToKillSelf;
            AddParameter("@ActualSelfInjuryBehaviorWithoutSuicideIntent", DbType.Boolean).Value = model.ActualSelfInjuryBehaviorWithoutSuicideIntent;
            AddParameter("@LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent", DbType.Boolean).Value = model.LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent;
            AddParameter("@WishToBeDead", DbType.Boolean).Value = model.WishToBeDead;
            AddParameter("@SuicidalThoughts", DbType.Boolean).Value = model.SuicidalThoughts;
            AddParameter("@SuicidalThoughtsWithMethod", DbType.Boolean).Value = model.SuicidalThoughtsWithMethod;
            AddParameter("@SuicidalIntent", DbType.Boolean).Value = model.SuicidalIntent;
            AddParameter("@SuicidalIntentWithSpecificPlan", DbType.Boolean).Value = model.SuicidalIntentWithSpecificPlan;
            AddParameter("@RecentLoss", DbType.Boolean).Value = model.RecentLoss;
            AddParameter("@DescribeRecentLoss", DbType.String).Value = model.DescribeRecentLoss.AsSqlParameter();
            AddParameter("@PendingIncarceration", DbType.Boolean).Value = model.PendingIncarceration;
            AddParameter("@CurrentOrPendingIsolation", DbType.Boolean).Value = model.CurrentOrPendingIsolation;
            AddParameter("@Hopelessness", DbType.Boolean).Value = model.Hopelessness;
            AddParameter("@Helplessness", DbType.Boolean).Value = model.Helplessness;
            AddParameter("@FeelingTrapped", DbType.Boolean).Value = model.FeelingTrapped;
            AddParameter("@MajorDepressiveEpisode", DbType.Boolean).Value = model.MajorDepressiveEpisode;
            AddParameter("@MixedAffectiveEpisode", DbType.Boolean).Value = model.MixedAffectiveEpisode;
            AddParameter("@CommandHallucinationsToHurtSelf ", DbType.Boolean).Value = model.CommandHallucinationsToHurtSelf;
            AddParameter("@HighlyImpulsiveBehavior", DbType.Boolean).Value = model.HighlyImpulsiveBehavior;
            AddParameter("@SubstanceAbuseOrDependence", DbType.Boolean).Value = model.SubstanceAbuseOrDependence;
            AddParameter("@AgitationOrSevereAnxiety", DbType.Boolean).Value = model.AgitationOrSevereAnxiety;
            AddParameter("@PerceivedBurdenOnFamilyOrOthers", DbType.Boolean).Value = model.PerceivedBurdenOnFamilyOrOthers;
            AddParameter("@ChronicPhysicalPain", DbType.Boolean).Value = model.ChronicPhysicalPain;
            AddParameter("@HomicidalIdeation", DbType.Boolean).Value = model.HomicidalIdeation;
            AddParameter("@AggressiveBehaviorTowardsOthers", DbType.Boolean).Value = model.AggressiveBehaviorTowardsOthers;
            AddParameter("@MethodForSuicideAvailable", DbType.Boolean).Value = model.MethodForSuicideAvailable;
            AddParameter("@RefusesOrFeelsUnableToAgreeToSafetyPlan", DbType.Boolean).Value = model.RefusesOrFeelsUnableToAgreeToSafetyPlan;
            AddParameter("@SexualAbuseLifetime", DbType.Boolean).Value = model.SexualAbuseLifetime;
            AddParameter("@FamilyHistoryOfSuicideLifetime", DbType.Boolean).Value = model.FamilyHistoryOfSuicideLifetime;
            AddParameter("@PreviousPsychiatricDiagnosesAndTreatments", DbType.Boolean).Value = model.PreviousPsychiatricDiagnosesAndTreatments;
            AddParameter("@HopelessOrDissatisfiedWithTreatment", DbType.Boolean).Value = model.HopelessOrDissatisfiedWithTreatment;
            AddParameter("@NonCompliantWithTreatment", DbType.Boolean).Value = model.NonCompliantWithTreatment;
            AddParameter("@NotReceivingTreatment", DbType.Boolean).Value = model.NotReceivingTreatment;
            AddParameter("@IdentifiesReasonsForLiving", DbType.Boolean).Value = model.IdentifiesReasonsForLiving;
            AddParameter("@ResponsibilityToFamilyOrOthers", DbType.Boolean).Value = model.ResponsibilityToFamilyOrOthers;
            AddParameter("@SupportiveSocialNetworkOrFamily", DbType.Boolean).Value = model.SupportiveSocialNetworkOrFamily;
            AddParameter("@FearOfDeathOrDyingDueToPainAndSuffering", DbType.Boolean).Value = model.FearOfDeathOrDyingDueToPainAndSuffering;
            AddParameter("@BeliefThatSuicideIsImmoral", DbType.Boolean).Value = model.BeliefThatSuicideIsImmoral;
            AddParameter("@EngagedInWorkOrSchool", DbType.Boolean).Value = model.EngagedInWorkOrSchool;
            AddParameter("@EngagedWithPhoneWorker", DbType.Boolean).Value = model.EngagedWithPhoneWorker;
            AddParameter("@DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior", DbType.String).Value = model.DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior.AsSqlParameter();


            RunProcedureNonSelectQuery("[dbo].[uspUpdateColumbiaSuicideRiskAssessmentReport]");


            UpdateSuicidalRiskFactors(model);
            UpdateSuicidalProtectiveFactors(model);
        }

        public ColumbiaSuicideReport Get(long id)
        {
            ColumbiaSuicideReport result = null;

            var sql = "[dbo].[uspGetColumbiaSuicideReport]";

            ClearParameters();

            AddParameter("@ID", DbType.Int64).Value = id;

            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        // init report + risk assessment
                        result = ColumbiaSuicideReportFactory.Create(reader);
                    }

                    //  select ideation
                    if (reader.NextResult())
                    {
                        var items = new List<ColumbiaSuicidalIdeation>(5);

                        while (reader.Read())
                        {
                            items.Add(ColumbiaSuicideReportFactory.CreateIdeation(reader));
                        }

                        ColumbiaSuicideReportFactory.InitIdeation(result, items);
                    }

                    //  select intensity
                    if (reader.NextResult())
                    {
                        var items = new List<ColumbiaIntensityIdeation>(5);

                        while (reader.Read())
                        {
                            items.Add(ColumbiaSuicideReportFactory.CreateIntensity(reader));
                        }

                        ColumbiaSuicideReportFactory.InitIntensity(result, items);
                    }

                    //  select behavior
                    if (reader.NextResult())
                    {
                        var items = new List<ColumbiaSuicialBehaviorAct>(5);

                        while (reader.Read())
                        {
                            items.Add(ColumbiaSuicideReportFactory.CreateBehavior(reader));
                        }

                        ColumbiaSuicideReportFactory.InitBehavior(result, items);
                    }

                    //  select other protective factors
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            result.RiskAssessmentReport.OtherProtectiveFactors.Add(
                                reader.Get<string>("ProtectiveFactor"));
                        }
                    }

                    //  select other risks factors
                    if (reader.NextResult())
                    {
                        while (reader.Read())
                        {
                            result.RiskAssessmentReport.OtherRisksFactors.Add(
                                reader.Get<string>("RiskFactor"));
                        }
                    }
                }

                return result;
            }
            finally
            {
                Disconnect();
            }
        }


        // SEARCH

        /// <summary>
        /// Get c-ssrs results grouped by patient name
        /// </summary>
        public List<UniqueColumbiaReportViewModel> GetLatestReportsForDisplay(PagedFilterModel filter)
        {

            var result = new List<UniqueColumbiaReportViewModel>();

            var orderBy = (filter.OrderBy ?? "lastcreateddate").ToLowerInvariant();


            string innerOrderBy = string.Empty;
            string totalOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy)) orderBy = "lastcreateddate DESC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("lastcreateddate"))
            {
                innerOrderBy = orderBy.Replace("lastcreateddate", "MAX(v.CreatedDate)");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("fullname"))
            {
                innerOrderBy = orderBy.Replace("fullname", "v.FullName");
                totalOrderBy = orderBy;
            }
            else if (orderBy.Contains("birthday"))
            {
                innerOrderBy = orderBy.Replace("birthday", "v.Birthday");
                totalOrderBy = orderBy;
            }

            QueryBuilder qbInnerSql = new QueryBuilder(string.Format(@"
SELECT TOP(@startRowIndex + @maxRows) 
    ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
    MAX(v.ID) as ID
    , v.FullName
    , v.Birthday
    , MAX(v.CreatedDate) AS LastCreatedDate
    , MAX(v.CompleteDate) as LastCompleteDate
FROM dbo.vColumbiaSuicideReport v
", innerOrderBy));

            string outerSql = @"
WITH tblCheckIns(RowNumber, ID, FullName, Birthday, LastCreatedDate, LastCompleteDate)
AS ({0})
SELECT t.ID, FullName, Birthday, LastCreatedDate, LastCompleteDate,
( SELECT TOP (1) Name 
    FROM dbo.BranchLocation l 
        INNER JOIN dbo.ColumbiaSuicideReport r ON r.BranchLocationID = l.BranchLocationID
        WHERE r.ID = t.ID
) as LocationName
FROM tblCheckIns t
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY {1} ";


            CommandObject.Parameters.Clear();
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = filter.StartRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = filter.MaximumRows - 1;

            qbInnerSql.AppendWhereCondition("(v.ID IS NOT NULL)", ClauseType.And);

            qbInnerSql.AppendGroupCondition("v.FullName, v.Birthday");
            qbInnerSql.AppendOrderCondition(innerOrderBy);



            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                qbInnerSql.AppendWhereCondition("(v.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                qbInnerSql.AppendWhereCondition("(v.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }
            if (filter.Location.HasValue)
            {
                // TODO: add join to kiosks table, choose kiosks by location
                qbInnerSql.AppendWhereCondition("(v.BranchLocationID = @LocationID)", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("v.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("v.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NULL)", ClauseType.And);
                }
            }
            try
            {

                Connect();
                var sql = string.Format(outerSql, qbInnerSql.ToString(), totalOrderBy);


                Logger.DebugFormat("GetLatestReportsForDisplay SQL:\r\n{0}", sql);

                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new UniqueColumbiaReportViewModel
                        {
                            //t.ScreeningResultID, FullName, Birthday, LastCreatedDate, LastCompleteDate, LocationName, LocationId
                            ID = reader.Get<long>(0) ?? 0, //ID
                            PatientName = reader.GetString(1), //FullName
                            Birthday = reader.Get<DateTime>(2), //Birthday
                            LastCreatedDate = reader.Get<DateTimeOffset>(3), // LastCreatedDate
                            LastCompleteDate = reader.Get<DateTimeOffset>(4), // LastCompleteDate
                            LocationName = reader.GetString(5) // LocationName
                        });
                    }
                }
            }

            finally
            {
                this.Disconnect();
            }

            return result;
        }


        /// <summary>
        /// Get count of reports for Search
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public long GetLatestReportsForDisplayCount(PagedFilterModel filter)
        {
            int rowCount = 0;
            //WITH statement
            QueryBuilder qbInnerSql = new QueryBuilder(@"
SELECT 
    MAX(v.ID) as ID
    , v.FullName
    , v.Birthday
    , MAX(v.CreatedDate) AS LastCreatedDate
FROM dbo.vColumbiaSuicideReport v
");

            string outerSql = @"
WITH tblCheckIns(ID, FullName, Birthday, LastCreatedDate)
AS ({0})
SELECT count(*)
FROM tblCheckIns t
";

            CommandObject.Parameters.Clear();

            qbInnerSql.AppendWhereCondition("(v.ID IS NOT NULL)", ClauseType.And);
            qbInnerSql.AppendGroupCondition("v.FullName, v.Birthday");

            if (!string.IsNullOrEmpty(filter.FirstName))
            {
                qbInnerSql.AppendWhereCondition("(v.FirstName LIKE @FirstName)", ClauseType.And);
                AddParameter("@FirstName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.FirstName, LikeCondition.StartsWith);
            }
            if (!string.IsNullOrEmpty(filter.LastName))
            {
                qbInnerSql.AppendWhereCondition("(v.LastName LIKE @LastName)", ClauseType.And);
                AddParameter("@LastName", DbType.String, 128).Value = SqlLikeStringPrepeare(filter.LastName, LikeCondition.StartsWith);
            }
            if (filter.Location.HasValue)
            {
                // TODO: add join to kiosks table, choose kiosks by location
                qbInnerSql.AppendWhereCondition("v.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.Location.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("v.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qbInnerSql.AppendWhereCondition("v.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }

            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NOT NULL)", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qbInnerSql.AppendWhereCondition("(v.CompleteDate IS NULL)", ClauseType.And);
                }
            }

            try
            {
                Connect();
                rowCount = Convert.ToInt32(this.RunScalarQuery(string.Format(outerSql, qbInnerSql.ToString())));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }
            return rowCount;
        }

        public List<ColumbiaSuicideReportSearchResponse> GetRelatedReports(
            SearchRelatedItemsFilter filter)
        {

            var result = new List<ColumbiaSuicideReportSearchResponse>();

            string sql = @"
SELECT 
t.ID, 
t.CreatedDate,
t.CompleteDate, 
t.BranchLocationName,
t.BhsStaffNameCompleted
FROM vColumbiaSuicideReport t 
INNER JOIN dbo.vColumbiaSuicideReport main 
        ON t.ID = main.ID 
            OR (main.FullName = t.FullName AND main.Birthday = t.Birthday)
";
            CommandObject.Parameters.Clear();

            QueryBuilder qb = new QueryBuilder(sql);

            qb.AppendOrderCondition("t.CreatedDate", OrderType.Desc);

            qb.AppendWhereCondition("main.ID = @ID", ClauseType.And);

            AddParameter("@ID", DbType.Int64).Value = filter.MainRowID;


            if (filter.LocationId.HasValue)
            {
                qb.AppendWhereCondition("t.BranchLocationID = @LocationID", ClauseType.And);

                AddParameter("@LocationID", DbType.Int32).Value = filter.LocationId.Value;
            }
            if (filter.StartDate.HasValue)
            {
                qb.AppendWhereCondition("t.CreatedDate >= @StartDate", ClauseType.And);
                AddParameter("@StartDate", DbType.DateTimeOffset).Value = filter.StartDate.Value;
            }
            if (filter.EndDate.HasValue)
            {
                qb.AppendWhereCondition("t.CreatedDate < @EndDate", ClauseType.And);
                AddParameter("@EndDate", DbType.DateTimeOffset).Value = filter.EndDate.Value;
            }
            if (filter.ReportType != BhsReportType.AllReports)
            {
                if (filter.ReportType == BhsReportType.CompletedReports)
                {
                    qb.AppendWhereCondition("t.CompleteDate IS NOT NULL", ClauseType.And);
                }
                else if (filter.ReportType == BhsReportType.IncompleteReports)
                {
                    qb.AppendWhereCondition("t.CompleteDate IS NULL", ClauseType.And);
                }
            }


            try
            {
                using (var reader = RunSelectQuery(qb.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new ColumbiaSuicideReportSearchResponse
                        {
                            ID = reader.Get<long>(0) ?? 0 ,
                            CreatedDate = reader.Get<DateTimeOffset>(1).Value,
                            CompletedDate = reader.GetNullable<DateTimeOffset>(2),
                            LocationName = reader.Get<string>("BranchLocationName"),
                            StaffNameCompleted = reader.Get<string>("BhsStaffNameCompleted"),
                        });
                    }

                }
            }
            finally
            {
                Disconnect();
            }
            return result;

        }
       
        public long? GetLatestReport(IScreeningPatientIdentity patientInfo)
        {
            long? previousReportId = null;

            string sql = @"
SELECT TOP 1
    t.ID
FROM dbo.ColumbiaSuicideReport t 
WHERE 
    t.Birthday = @Birthday
    AND t.PatientName = dbo.fn_GetPatientName(@LastName, @FirstName, @MiddleName)
    AND t.CompleteDate IS NOT NULL
ORDER BY t.CompleteDate DESC
";
            ClearParameters();

            AddParameter("@Birthday", DbType.Date).Value = patientInfo.Birthday.AsSqlParameter();
            AddParameter("@LastName", DbType.AnsiString, 255).Value = patientInfo.LastName.AsSqlParameter();
            AddParameter("@FirstName", DbType.AnsiString, 255).Value = patientInfo.FirstName.AsSqlParameter();
            AddParameter("@MiddleName", DbType.AnsiString, 255).Value = patientInfo.MiddleName.AsSqlParameter();

            try
            {
                previousReportId = RunScalarQuery<long>(sql);
            }
            finally
            {
                Disconnect();
            }
            return previousReportId;
        }
    }
}
