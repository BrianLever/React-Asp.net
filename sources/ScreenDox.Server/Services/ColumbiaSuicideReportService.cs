using Common.Logging;

using FrontDesk;
using FrontDesk.Common.Bhservice;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Screening.Models;

using ScreenDox.Server.Data;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ColumbiaReports;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Services
{
    public interface IColumbiaSuicideReportService
    {
        long Add(ColumbiaSuicideReport model, IUserPrincipal userPrincipal);
        long Create(BhsVisit visit, IUserPrincipal userPrincipal);

        void Update(ColumbiaSuicideReport model, IUserPrincipal userPrincipal);

        ColumbiaSuicideReport Get(long id);

        SearchResponse<UniqueColumbiaReportViewModel> GetUniqueReports(PagedFilterModel filter);
        List<ColumbiaSuicideReportSearchResponse> GetRelatedReports(long id, PagedFilterModel filter);
    }

    public class ColumbiaSuicideReportService : IColumbiaSuicideReportService
    {
        private readonly IColumbiaSuicideReportRepository _repository;
        private readonly ITimeService _timeService;
    
        private readonly ILog _logger = LogManager.GetLogger<ColumbiaSuicideReportService>();

        public ColumbiaSuicideReportService(IColumbiaSuicideReportRepository repository,
                                            ITimeService timeService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
        }


        private void SetCreateNewReportProperties(ColumbiaSuicideReport model, ColumbiaSuicideReport previousReport, IUserPrincipal userPrincipal)
        {
            model.CreatedDate = _timeService.GetDateTimeOffsetNow();
            model.BhsStaffNameCompleted = userPrincipal.FullName;

            if(previousReport != null)
            {
                var p = previousReport;
                {
                    // set lifetime values

                    // SUICIDAL IDEATION
                    model.WishToDead.LifetimeMostSucidal = p.WishToDead?.LifetimeMostSucidal;
                    model.NonSpecificActiveSuicidalThoughts.LifetimeMostSucidal = p.NonSpecificActiveSuicidalThoughts?.LifetimeMostSucidal;
                    model.ActiveSuicidalIdeationWithAnyMethods.LifetimeMostSucidal = p.ActiveSuicidalIdeationWithAnyMethods?.LifetimeMostSucidal;
                    model.ActiveSuicidalIdeationWithSomeIntentToAct.LifetimeMostSucidal = p.ActiveSuicidalIdeationWithSomeIntentToAct?.LifetimeMostSucidal;
                    model.ActiveSuicidalIdeationWithSpecificPlanAndIntent.LifetimeMostSucidal = p.ActiveSuicidalIdeationWithSpecificPlanAndIntent?.LifetimeMostSucidal;

                    // INTENSITY OF IDEATION
                    model.Frequency.LifetimeMostSevere = p.Frequency?.LifetimeMostSevere;
                    model.Duration.LifetimeMostSevere = p.Duration?.LifetimeMostSevere;
                    model.Controllability.LifetimeMostSevere = p.Controllability?.LifetimeMostSevere;
                    model.Deterrents.LifetimeMostSevere = p.Deterrents?.LifetimeMostSevere;
                    model.ReasonsForIdeation.LifetimeMostSevere = p.ReasonsForIdeation?.LifetimeMostSevere;

                    model.LifetimeMostSevereIdeationLevel = p.LifetimeMostSevereIdeationLevel;
                    model.LifetimeMostSevereIdeationDescription = p.LifetimeMostSevereIdeationDescription;

                    // SUICIDE BEHAVIOR
                    model.ActualAttempt.LifetimeLevel = p.ActualAttempt?.LifetimeLevel;
                    model.ActualAttempt.LifetimeCount = p.ActualAttempt?.LifetimeCount;

                    model.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior.LifetimeLevel 
                        = p.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?.LifetimeLevel;
                    model.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior.LifetimeCount 
                        = p.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior?.LifetimeCount;

                    model.InterruptedAttempt.LifetimeLevel = p.InterruptedAttempt?.LifetimeLevel;
                    model.InterruptedAttempt.LifetimeCount = p.InterruptedAttempt?.LifetimeCount;

                    model.AbortedAttempt.LifetimeLevel = p.AbortedAttempt?.LifetimeLevel;
                    model.AbortedAttempt.LifetimeCount = p.AbortedAttempt?.LifetimeCount;

                    model.PreparatoryActs.LifetimeLevel = p.PreparatoryActs?.LifetimeLevel;
                    model.PreparatoryActs.LifetimeCount = p.PreparatoryActs?.LifetimeCount;

                    // Lethality
                    model.SuicideMostLethalRecentAttemptDate = p.SuicideMostLethalRecentAttemptDate;
                    model.SuicideFirstAttemptDate = p.SuicideFirstAttemptDate;

                    model.ActualLethality.InitialAttemptCode = p.ActualLethality?.InitialAttemptCode;
                    model.ActualLethality.MostLethalAttemptCode = p.ActualLethality?.MostLethalAttemptCode;

                    model.PotentialLethality.InitialAttemptCode = p.PotentialLethality?.InitialAttemptCode;
                    model.PotentialLethality.MostLethalAttemptCode = p.PotentialLethality?.MostLethalAttemptCode;

                }
                // Risk Assessment Report
                SetCreateNewReportProperties(model.RiskAssessmentReport, previousReport.RiskAssessmentReport);
            }
        }

        private void SetCreateNewReportProperties(ColumbiaRiskAssessmentReport model, ColumbiaRiskAssessmentReport previousReport)
        {
            model.LifetimeActualSuicideAttempt = previousReport.LifetimeActualSuicideAttempt;
            model.LifetimeInterruptedSuicideAttempt = previousReport.LifetimeInterruptedSuicideAttempt;
            model.LifetimeAbortedSuicideAttempt = previousReport.LifetimeAbortedSuicideAttempt;
            model.LifetimeOtherPreparatoryActsToKillSelf = previousReport.LifetimeOtherPreparatoryActsToKillSelf;
            model.LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent = previousReport.LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent;
        }



        private void SetCompleteReportProperties(ColumbiaSuicideReport model, IUserPrincipal userPrincipal)
        {
            model.CompleteDate = _timeService.GetDateTimeOffsetNow();
            model.BhsStaffNameCompleted = userPrincipal.FullName;
        }

        public Int64 Add(ColumbiaSuicideReport model, IUserPrincipal userPrincipal)
        {
           

            try
            {
                // begin transaction and ensure connection will not be closed
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();


                var previousReport = GetLatestReportForPatient(model);

                SetCreateNewReportProperties(model, previousReport, userPrincipal);

                _repository.Add(model);




                _repository.StopConnectionSharing();
                _repository.CommitTransaction();

                _logger.Info("Created new C-SSRS.");

                
            }
            catch (Exception ex)
            {


                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();


                _logger.Error("Failed to create a new C-SSRS.", ex);


                throw;
            }

            return model.ID;
        }

        public long Create(BhsVisit visit, IUserPrincipal userPrincipal)
        {
            if (visit is null)
            {
                throw new ArgumentNullException(nameof(visit));
            }

            var model = new ColumbiaSuicideReport
            {
                BhsVisitID = visit.ID,
                ScreeningResultID = visit.ScreeningResultID,
                CreatedDate = visit.CreatedDate,
                FirstName = visit.Result.FirstName,
                LastName = visit.Result.LastName,
                MiddleName = visit.Result.MiddleName,
                Birthday = visit.Result.Birthday,
                BranchLocationID = visit.LocationID,
                EhrPatientID = visit.Result.ExportedToPatientID,
                EhrPatientHRN = visit.Result.ExportedToHRN,
            };

            return Add(model, userPrincipal);
        }

        public void Update(ColumbiaSuicideReport model, IUserPrincipal userPrincipal)
        {

            SetCompleteReportProperties(model, userPrincipal);

            try
            {
                _repository.Update(model);

                _logger.Info("Completed C-SSRS.");
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to completed a new C-SSRS.", ex);

                throw;
            }
        }


        public ColumbiaSuicideReport Get(long id)
        {
            try
            {
                return _repository.Get(id);
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to read new C-SSRS.", ex);

                throw;
            }
        }

      

        /// <summary>
        /// Get unique visit records that have created in given period of time
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SearchResponse<UniqueColumbiaReportViewModel> GetUniqueReports(PagedFilterModel filter)
        {
            var result = new SearchResponse<UniqueColumbiaReportViewModel>();

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }
            _logger.Trace("[Columbia][GetUniqueReports] Method called.");

            result.TotalCount = _repository.GetLatestReportsForDisplayCount(filter);

            result.Items = _repository.GetLatestReportsForDisplay(filter);

            return result;
        }

        public List<ColumbiaSuicideReportSearchResponse> GetRelatedReports(long id, PagedFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            _logger.Trace("[Columbia][GetRelatedReports] Method called.");

            List<ColumbiaSuicideReportSearchResponse> result = _repository.GetRelatedReports(
                new SearchRelatedItemsFilter
                {
                    MainRowID = id,
                    StartDate = filter.StartDate,
                    EndDate = filter.EndDate,
                    LocationId = filter.Location,
                    ReportType = filter.ReportType,
                }
            );//.Select(x => VisitViewModelFactory.Create((VisitViewModelBase)x)).ToList();

            return result;
        }

        private ColumbiaSuicideReport GetLatestReportForPatient(IScreeningPatientIdentity patientInfo)
        {
            try
            {
                var previousReportId = _repository.GetLatestReport(patientInfo);
                if (!previousReportId.HasValue)
                {
                    return null;
                }

                return Get(previousReportId.Value);

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to read the previous report for the patient.", ex);

                throw;
            }
        }
    }
}
