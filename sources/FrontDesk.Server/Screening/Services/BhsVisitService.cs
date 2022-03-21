using Common.Logging;

using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Controllers;
using FrontDesk.Server.Data;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Mappers;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Services;

using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace FrontDesk.Server.Screening.Services
{
    public class BhsVisitService : IBhsVisitService
    {
        protected readonly IBhsVisitRepository _bhsVisitRepository;
        protected readonly IBhsVisitFactory _bhsVisitFactory;


        protected readonly IVisitCreator _visitCreator;
        protected readonly IBhsDemographicsService _bhsDemographicsService;
        protected readonly IBhsFollowUpService _bhsFollowUpService;
        protected readonly IBhsHistoryRepository _bhsHistoryRepository;
        protected readonly IScreenerResultReadRepository _screeningResultRepository;


        private readonly ILog _logger = LogManager.GetLogger<BhsVisitService>();

        public BhsVisitService() : this(new BhsVisitDb(), new BhsVisitFactory(), new VisitCreator(),
            new BhsDemographicsService(),
            new BhsFollowUpService(),
            new BhsHistoryDb(),
            new ScreeningResultDb()
            )
        {
        }

        public BhsVisitService(
            IBhsVisitRepository bhsVisitRepository,
            IBhsVisitFactory bhsVisitFactory,
            IVisitCreator visitCreator,
            IBhsDemographicsService bhsDemographicsService,
            IBhsFollowUpService bhsFollowUpService,
            IBhsHistoryRepository bhsHistoryRepository,
            IScreenerResultReadRepository screeningResultRepository)
        {

            _bhsVisitRepository = bhsVisitRepository ?? throw new ArgumentNullException(nameof(bhsVisitRepository));
            _bhsVisitFactory = bhsVisitFactory ?? throw new ArgumentNullException(nameof(bhsVisitFactory));
            _visitCreator = visitCreator ?? throw new ArgumentNullException(nameof(visitCreator));
            _bhsDemographicsService = bhsDemographicsService ?? throw new ArgumentNullException(nameof(bhsDemographicsService));
            _bhsFollowUpService = bhsFollowUpService ?? throw new ArgumentNullException(nameof(bhsFollowUpService));
            _bhsHistoryRepository = bhsHistoryRepository ?? throw new ArgumentNullException(nameof(bhsHistoryRepository));
            _screeningResultRepository = screeningResultRepository ?? throw new ArgumentNullException(nameof(screeningResultRepository));
        }

        public BhsVisit Get(long id)
        {
            var visit = _bhsVisitRepository.Get(id);
            if (visit == null) return null;

            visit.Result = _screeningResultRepository.GetScreeningResult(visit.ScreeningResultID);

            return visit;
        }

        protected void EnsureDemographicsExists(ScreeningResult screeningResult)
        {
            _logger.InfoFormat("Checking demographics record exists for screening result [{0}].", screeningResult.ID);
            if (!_bhsDemographicsService.Exists(screeningResult))
            {
                _logger.InfoFormat("Creating demographics record for screening result [{0}].", screeningResult.ID);

                _bhsDemographicsService.Create(screeningResult);

                _logger.InfoFormat("Demographics record has been created for screening result [{0}].", screeningResult.ID);
            }
        }

        public BhsVisit Create(long screeningResultId)
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

                var screeningResult = ScreeningResultHelper.GetScreeningResult(screeningResultId);
                var info = ServerScreening.GetByID(screeningResult.ScreeningID);

                var model = _bhsVisitFactory.Create(screeningResult, info);
                if (model != null)
                {
                    _bhsVisitRepository.Add(model);
                }


                EnsureDemographicsExists(screeningResult);


                _bhsVisitRepository.StopConnectionSharing();
                _bhsVisitRepository.CommitTransaction();


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

        public void FullfilPatientAddress(ScreeningResult patient)
        {
            if (patient == null)
            {
                throw new ArgumentNullException(nameof(patient));
            }

            if (patient.IsEmptyContactInfo())
            {
                _logger.InfoFormat("[Visit][Address] Patient does not have address details. Screening Result: [{0}]. EHR: [{1}]", patient.ID, patient.ExportedToHRN);

                return;
            }

            _logger.InfoFormat("[Visit][Address] Adding address into to patient details. Screening Result: [{0}]. EHR: [{1}]", patient.ID, patient.ExportedToHRN);

            _bhsVisitRepository.SetPatientAddressToAllPatientScreenings(patient);

            _logger.InfoFormat("[Visit][Address] Added address into to patient details. Screening Result: [{0}]. EHR: [{1}]", patient.ID, patient.ExportedToHRN);

        }

        public BhsVisit Create(ScreeningResult screeningResult, FrontDesk.Screening info)
        {
            const int PosititiveScreeningVisitCreationLimit = 3; //number of visits after positive screening

            _logger.InfoFormat("[Visit][Screening ID: {0}] Attempting to create new Visit automatically.", screeningResult.ID);

            var model = _visitCreator.Create(screeningResult, info);
            if (model != null)
            {
                _logger.InfoFormat("[Visit][Screening ID: {0}] Screening is positive, creating new Visit.", screeningResult.ID);
                //positive case, creating the visit
                CreateExecute(model, screeningResult, info);

                return model;
            }

            _logger.InfoFormat("[Visit][Screening ID: {0}] Screening is negative, checking other business rules.", screeningResult.ID);

            //check other rules
            //Rule 1. Check that previous N screening where negative
            var previousScreenings = _bhsHistoryRepository.GetLastNotEmptyScreenings(screeningResult, screeningResult.CreatedDate, PosititiveScreeningVisitCreationLimit - 1);
            var positiveScreenings = previousScreenings.Where(x => x.IsPositive).ToArray();

            _logger.InfoFormat("[Visit][Screening ID: {0}] Found {1} previous screenings. {2} are positive.", screeningResult.ID, previousScreenings.Count, positiveScreenings.Length);
            //if no positive screening at all in the received results, no need to create a visit.
            if (positiveScreenings.Length == 0)
            {
                _logger.InfoFormat("[Visit][Screening ID: {0}] Did not find any positive screening. No need to create Visit.", screeningResult.ID);

                return null; //
            }

            //there is one positive result, let's check positiveScreenings.
            BhsScreeningHistoryItem lastPositiveScreening = null;

            {
                foreach (var positiveScreeningItem in positiveScreenings)
                {
                    //read full object of screening and run it though visitcreator
                    //TODO: Implement
                    var item = _screeningResultRepository.GetScreeningResult(positiveScreeningItem.ScreeningResultID);
                    if (_visitCreator.Create(item, info) != null)
                    {
                        _logger.InfoFormat("[Visit][Screening ID: {0}] Found positive screening which meet visit trigger conditions. Positive Screening ID: [{1}].", screeningResult.ID, positiveScreeningItem.ScreeningResultID);

                        //it's positive;
                        lastPositiveScreening = positiveScreeningItem;
                    }
                }
            }

            if (lastPositiveScreening == null)
            {
                _logger.InfoFormat("[Visit][Screening ID: {0}] All positive screenings appeared to be negative and did not trigger the visit. No need to create Visit.", screeningResult.ID);
                return null;
            }


            //get the latest positive and check that there are no visits or follow-up which discharged visits
            var visitsAndFollowUps = _bhsHistoryRepository.GetVisitsAndFollowUps(screeningResult, lastPositiveScreening.CreatedDate, screeningResult.CreatedDate);

            if (visitsAndFollowUps.Any(x => TestNeedToStopCreatingVisitsOnPositive(x)))
            {
                _logger.InfoFormat("[Visit][Screening ID: {0}] Found Visit or Follow-Up with Discharged or Not Acceptance treatment. Visit not created.", screeningResult.ID, previousScreenings.Count, positiveScreenings.Length);

                return null;
            }

            _logger.InfoFormat("[Visit][Screening ID: {0}] Creating Visit.", screeningResult.ID, previousScreenings.Count, positiveScreenings.Length);

            //need to create visit on positive screening
            model = _bhsVisitFactory.Create(screeningResult, info);
            CreateExecute(model, screeningResult, info);
            return model;
        }

        protected bool TestNeedToStopCreatingVisitsOnPositive(BhsHistoryItem item)
        {
            if (item.NewVisitReferralRecommendationAcceptedID == 2) //Yes
            {
                return true;
            }

            if (item.DischargeID.HasValue && DischargedDescriptor.IsDischarged(item.DischargeID.Value))
            {
                return true;
            }

            return false;
        }


        protected void CreateExecute(BhsVisit visit, ScreeningResult screeningResult, FrontDesk.Screening info)
        {
            EnsureDemographicsExists(screeningResult);
            _bhsVisitRepository.Add(visit);

            _logger.InfoFormat("[Visit] Visit created. Screening ID: [{0}]", screeningResult.ID);

        }


        //update only manually entered values
        public void Update(BhsVisit model, IUserPrincipal user)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            model.BhsStaffNameCompleted = user.FullName;
            model.CompleteDate = DateTimeOffset.Now;

            if (!model.NewVisitDate.HasValue)
            {
                model.FollowUpDate = null;
            }

            _bhsVisitRepository.UpdateManualEntries(model);

            if (model.ThirtyDatyFollowUpFlag)
            {
                //ensure follow up exists
                _bhsFollowUpService.Create(model);
            }
        }

        [Obsolete("migrated")]
        public void UpdateDrugOfChoiceInResult(BhsVisit model, IUserPrincipal user)
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


                SecurityLog.Add(new SecurityLog(SecurityEvents.UpdateDrugOfChoice,
                    string.Join("~", model.ScreeningResultID, model.ID,
                    dbResult.Primary, dbResult.Secondary, dbResult.Tertiary,
                    updateModel.Primary, updateModel.Secondary, updateModel.Tertiary)
                    ));
            }

        }

        public long? FindByScreeningResultId(long screeningResultId)
        {
            return _bhsVisitRepository.FindByScreeningResultId(screeningResultId);
        }



        //overriden version to accept int as report type filter from Object Data Source
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public DataSet GetUniqueVisitsForDataSource(
            string firstNameFilter,
            string lastNameFilter,
            int? locationFilter,
            DateTime? startDateFilter,
            DateTime? endDateFilter,
            int? reportTypeFilter,
            long? screeningResultIdFilter,
            string orderBy,
            int maximumRows,
            int startRowIndex
            )
        {
            var typeFilter = reportTypeFilter.ConvertIntToBhsReportType();

            return GetUniqueVisits(new BhsSearchFilterModel(
                firstNameFilter,
                lastNameFilter,
                locationFilter,
                startDateFilter,
                endDateFilter,
                typeFilter,
                screeningResultIdFilter),
                orderBy,
                maximumRows,
                startRowIndex);
        }
        public DataSet GetUniqueVisits(
            BhsSearchFilterModel filter,
            string orderBy,
            int maximumRows,
            int startRowIndex)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            return _bhsVisitRepository.GetUniqueVisits(
                filter,
                startRowIndex,
                maximumRows,
                orderBy);
        }

        //overriden version to accept int as report type filter from Object Data Source
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public int GetUniqueVisitsCountForDataSource(
            string firstNameFilter,
            string lastNameFilter,
            int? locationFilter,
            DateTime? startDateFilter,
            DateTime? endDateFilter,
            int? reportTypeFilter,
            long? screeningResultIdFilter
            )
        {
            var typeFilter = reportTypeFilter.ConvertIntToBhsReportType();

            return GetUniqueVisitsCount(new BhsSearchFilterModel(
                firstNameFilter,
                lastNameFilter,
                locationFilter,
                startDateFilter,
                endDateFilter,
                typeFilter,
                screeningResultIdFilter)
                );
        }


        public int GetUniqueVisitsCount(BhsSearchFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            return _bhsVisitRepository.GetUniqueVisitsCount(filter);
        }

        public List<BhsVisitViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            var result = _bhsVisitRepository.GetRelatedReports(filter);

            return result;
        }


        public List<BhsVisitListItemPrintoutModel> GetAllForPrintout(BhsSearchFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            List<BhsVisitListItemDtoModel> items = _bhsVisitRepository.GetAllItems(filter);

            var result = items.ToViewModel().ToList();

            return result;
        }

    }
}

