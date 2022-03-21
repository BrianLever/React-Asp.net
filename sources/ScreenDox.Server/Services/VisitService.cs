using Common.Logging;

using FrontDesk.Common.Bhservice;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Services;

using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.Server.Screening.Services
{
    public class VisitService : BhsVisitService, IVisitService
    {

        private readonly ILog _logger = LogManager.GetLogger<VisitService>();
        private readonly ISecurityLogService _securityLogService;
        private readonly IScreeningDefinitionService _screeningDefinitionService;

        public VisitService(
            IBhsVisitRepository bhsVisitRepository,
            IBhsVisitFactory bhsVisitFactory,
            IVisitCreator visitCreator,
            IBhsDemographicsService bhsDemographicsService,
            IBhsFollowUpService bhsFollowUpService,
            IBhsHistoryRepository bhsHistoryRepository,
            IScreenerResultReadRepository screeningResultRepository,
            ISecurityLogService securityLogService,
            IScreeningDefinitionService screeningDefinitionService) : base(
            bhsVisitRepository,
            bhsVisitFactory,
            visitCreator,
            bhsDemographicsService,
            bhsFollowUpService,
            bhsHistoryRepository,
            screeningResultRepository
        )
        {
            _securityLogService = securityLogService ?? throw new System.ArgumentNullException(nameof(securityLogService));
            _screeningDefinitionService = screeningDefinitionService ?? throw new System.ArgumentNullException(nameof(screeningDefinitionService));
        }

        /// <summary>
        /// Get unique visit records that have created in given period of time
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SearchResponse<UniqueVisitViewModel> GetUniqueVisits(PagedVisitFilterModel filter)
        {
            var result = new SearchResponse<UniqueVisitViewModel>();

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }
            _logger.Trace("[VisitService][GetUniqueVisits] Method called.");

            result.TotalCount = _bhsVisitRepository.GetLatestVisitsForDisplayCount(filter);

            result.Items = _bhsVisitRepository.GetLatestVisitsForDisplay(filter);

            return result;
        }

        /// <summary>
        /// Get Visits and Patient Demographics records for specific patient
        /// </summary>
        /// <param name="id">Screendox ID of the Screening Result which is used to get patient identity.</param>
        /// <param name="filter">Filter condition.</param>
        /// <returns>>List of visits and demographics.</returns>
        public List<VisitViewModel> GetRelatedVisits(long id, VisitFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            _logger.Trace("[ScreeningResult][GetRelatedPatientScreenings] Method called.");

            List<VisitViewModel> result = _bhsVisitRepository.GetRelatedReports(
                new BhsSearchRelatedItemsFilter
                {
                    MainRowID = id,
                    ScreeningResultID = filter.ScreeningResultID,
                    StartDate = filter.StartDate,
                    EndDate = filter.EndDate,
                    LocationId = filter.Location,
                    ReportType = filter.ReportType,
                }
            ).Select(x => VisitViewModelFactory.Create((VisitViewModelBase)x)).ToList();

            return result;
        }

        /// <summary>
        /// Updates Drugs selections in Screening Result
        /// </summary>
        /// <param name="model"></param>
        public void UpdateDrugOfChoiceInResult(BhsVisit model)
        {

            //track if there are changes

            var dbResult = new DrugOfChoiceModel(_screeningResultRepository.GetScreeningResult(model.ScreeningResultID));
            var updateModel = new DrugOfChoiceModel(model.Result);

            if (dbResult.Primary != updateModel.Primary ||
                dbResult.Secondary != updateModel.Secondary ||
                dbResult.Tertiary != updateModel.Tertiary)
            {
                //there are changes from Visit page
                //need update screening results
                _bhsVisitRepository.UpdateDrugOfChoice(model.ScreeningResultID, updateModel);

                _securityLogService.Add(
                    new SecurityLog(
                        SecurityEvents.UpdateDrugOfChoice,
                        string.Join("~", model.ScreeningResultID, model.ID,
                        dbResult.Primary, dbResult.Secondary, dbResult.Tertiary,
                        updateModel.Primary, updateModel.Secondary, updateModel.Tertiary
                        )
                    ));
            }

        }



        public BhsVisit CreateFromScreen(long screeningResultId)
        {
            //begin transation
            try
            {
                _bhsVisitRepository.BeginTransaction();
                _bhsVisitRepository.StartConnectionSharing();

                var id = _bhsVisitRepository.FindByScreeningResultId(screeningResultId);

                if (id.HasValue)
                {
                    return Get(id.Value);
                }

                var screeningResult = _screeningResultRepository.GetScreeningResult(screeningResultId);
                var info = _screeningDefinitionService.Get();

                var model = _bhsVisitFactory.Create(screeningResult, info);
                if (model != null)
                {
                    _bhsVisitRepository.Add(model);
                }


                EnsureDemographicsExists(screeningResult);


                _bhsVisitRepository.StopConnectionSharing();
                _bhsVisitRepository.CommitTransaction();


                // add record to audit log
                _securityLogService.Add(new SecurityLog(SecurityEvents.ManuallyCreateBhsVisitInformation,
                        String.Format("{0}~{1}", screeningResult.FullName, screeningResult.Birthday.FormatAsDate()),
                        screeningResult.LocationID));

                return model;

            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException("Failed to create Visit in database.", ex);
                _bhsVisitRepository.StopConnectionSharing();
                _bhsVisitRepository.RollbackTransaction();

                throw;
            }
            finally
            {
                _bhsVisitRepository.Disconnect();
            }
        }
    }
}

