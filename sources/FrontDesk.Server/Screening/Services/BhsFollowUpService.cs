using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Logging;
using FrontDesk.Services;
using System;
using System.Data;
using System.Collections.Generic;

using Common.Logging;
using System.ComponentModel;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Mappers;
using System.Linq;
using ScreenDox.Server.Models;

namespace FrontDesk.Server.Screening.Services
{
    public interface IBhsFollowUpService
    {
        BhsFollowUp Create(BhsVisit bhsVisit);
        BhsFollowUp Create(BhsFollowUp bhsVisit);
        BhsFollowUp Get(long id);
        void Update(BhsFollowUp model, IUserPrincipal user);

        #region data source binding
        int GetUniqueVisitsCountForDataSource(string firstNameFilter, string lastNameFilter, int? locationFilter, DateTime? startDateFilter, DateTime? endDateFilter, int? reportTypeFilter, long? screeningResultIdFilter);
        DataSet GetUniqueVisitsForDataSource(string firstNameFilter, string lastNameFilter, int? locationFilter, DateTime? startDateFilter, DateTime? endDateFilter, int? reportTypeFilter, long? screeningResultIdFilter, string orderBy, int maximumRows, int startRowIndex);

        #endregion

        List<BhsFollowUpViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter);
        DataSet GetUniqueVisits(BhsSearchFilterModel filter, string orderBy, int maximumRows, int startRowIndex);
        int GetUniqueVisitsCount(BhsSearchFilterModel filter);
       
        List<BhsFollowUpViewModel> GetFollowUpsForVisit(long visitId);
    }

    public class BhsFollowUpService : IBhsFollowUpService
    {
        protected readonly IBhsFollowUpRepository _followUpRepository;
        protected readonly IBhsFollowUpFactory _BhsFollowUpFactory;
        protected readonly IBhsVisitRepository _visitRepository;

        protected readonly ILog _logger = LogManager.GetLogger<BhsFollowUpService>();

        public BhsFollowUpService() : this(new BhsFollowUpDb(), new BhsFollowUpFactory(), new BhsVisitDb())
        {
        }

        public BhsFollowUpService(IBhsFollowUpRepository BhsFollowUpRepository, IBhsFollowUpFactory BhsFollowUpFactory, IBhsVisitRepository visitRepository)
        {
            _followUpRepository = BhsFollowUpRepository ?? throw new ArgumentNullException(nameof(BhsFollowUpRepository));
            _BhsFollowUpFactory = BhsFollowUpFactory ?? throw new ArgumentNullException(nameof(BhsFollowUpFactory));
            _visitRepository = visitRepository ?? throw new ArgumentNullException(nameof(visitRepository));
        }

        public BhsFollowUp Get(long id)
        {
            var model = _followUpRepository.Get(id);
            if (model == null) return null;

            model.Result = ScreeningResultHelper.GetScreeningResult(model.ScreeningResultID);

            //if follow up was created from visit - we take visit date and recommendation from visit
            //otherwise take this information from original parent follow-up report
            model.Visit = _visitRepository.Get(model.BhsVisitID);
            if (model.ParentFollowUpID.HasValue)
            {
                model.ParentFollowUpVisit = _followUpRepository.Get(model.ParentFollowUpID.Value);
            }

            return model;
        }



        public BhsFollowUp Create(BhsVisit bhsVisit)
        {
            if (bhsVisit == null)
            {
                throw new ArgumentNullException(nameof(bhsVisit));
            }

            try
            {
                _logger.InfoFormat("[Follow Up] Checking Follow Up record exists for Bhs Visit [{0}].", bhsVisit.ID);

                var followUpReportId = _followUpRepository.FindByVisitId(bhsVisit.ID);

                if (followUpReportId.HasValue)
                {
                    _logger.InfoFormat("[Follow Up]  Follow Up record for Bhs Visit [{0}] exists. Follow-Up Report ID: [{1}].", bhsVisit.ID, followUpReportId.Value);

                    var existingFollowUp = _followUpRepository.Get(followUpReportId.Value);

                    if (!existingFollowUp.IsCompleted)
                    {
                        _followUpRepository.Delete(followUpReportId.Value);
                    }
                    else if (existingFollowUp.ScheduledFollowUpDate.Date == bhsVisit.FollowUpDate.Value.Date)
                    {
                        _logger.InfoFormat("[Follow Up] Follow-Up date has not been changed. Do not create new follow-up for Visit ID [{0}]", bhsVisit.ID);

                        //if exist and the follow up date is the same - leave it as is without changes
                        return new BhsFollowUp { ID = followUpReportId.Value };
                    }
                }

                _logger.InfoFormat("[Follow Up] Creating Follow Up record for Bhs Visit [{0}].", bhsVisit.ID);

                var model = _BhsFollowUpFactory.Create(bhsVisit);

                _followUpRepository.Add(model);

                _logger.InfoFormat("[Follow Up] Follow Up record has been created for Bhs Visit [{0}]. Follow-Up Report ID [{1}]", bhsVisit.ID, model.ID);

                return model;

            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException("Failed to create Follow Up Report.", ex);

                throw;
            }

        }

        public BhsFollowUp Create(BhsFollowUp model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            try
            {
                _logger.InfoFormat("[Follow Up] Checking Follow Up record exists for Follow-Up Report [{0}].", model.ID);

                var followUpReportId = model.ID == 0 ? null : _followUpRepository.FindByParentFollowUpId(model.ID);

                if (followUpReportId.HasValue)
                {

                    var existingFollowUp = _followUpRepository.Get(followUpReportId.Value);

                    if (!existingFollowUp.IsCompleted)
                    {
                        _followUpRepository.Delete(followUpReportId.Value);
                    }
                    else if (existingFollowUp.ScheduledFollowUpDate.Date == model.FollowUpDate.Value.Date)
                    {
                        _logger.InfoFormat("[Follow Up] Follow-Up date has not been changed. Do not create new follow-up for ID [{0}]", model.ID);


                        //if exist and the follow up date is the same - leave it as is without changes
                        return new BhsFollowUp { ID = followUpReportId.Value };
                    }


                    return new BhsFollowUp { ID = followUpReportId.Value };
                }

                _logger.InfoFormat("[Follow Up] Creating Follow Up record for Follow-Up Report [{0}].", model.ID);

                _followUpRepository.Add(model);

                _logger.InfoFormat("[Follow Up] Follow Up record has been created for Follow-Up Report [{0}]. Follow-Up Report: [{1}]", model.ID, model.ID);

                return model;
            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException("Failed to create Follow Up Report.", ex);

                throw;
            }

        }

        //update only manually entered values
        public void Update(BhsFollowUp model, IUserPrincipal user)
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

            _followUpRepository.UpdateManualEntries(model);

            if (model.ThirtyDatyFollowUpFlag)
            {
                var followUpReportId = _followUpRepository.FindByParentFollowUpId(model.ID);

                if (!followUpReportId.HasValue)
                {

                    var newFollowUp = _BhsFollowUpFactory.Create(model)
                        .SetFollowUpDate(model.FollowUpDate.Value);

                    Create(newFollowUp);
                }
            }

        }

        [Obsolete("migrated")]

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
            int startRowIndex)
        {
            var typeFilter = reportTypeFilter.ConvertIntToBhsReportType();

            return GetUniqueVisits(
                new BhsSearchFilterModel(
                    firstNameFilter,
                    lastNameFilter,
                    locationFilter,
                    startDateFilter,
                    endDateFilter,
                    typeFilter,
                    screeningResultIdFilter
                ),
                orderBy,
                maximumRows,
                startRowIndex);
        }
        [Obsolete("migrated")]
        public DataSet GetUniqueVisits(BhsSearchFilterModel filter, string orderBy, int maximumRows, int startRowIndex)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            return _followUpRepository.GetUnique(filter, startRowIndex, maximumRows, orderBy);
        }
        [Obsolete("migrated")]
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

            return GetUniqueVisitsCount(
                new BhsSearchFilterModel(firstNameFilter, lastNameFilter, locationFilter, startDateFilter, endDateFilter, typeFilter, screeningResultIdFilter)
                );
        }


        public int GetUniqueVisitsCount(BhsSearchFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            return _followUpRepository.GetUniqueCount(filter);
        }

        public List<BhsFollowUpViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter)
        {
            if (filter == null)
            {
                throw new ArgumentNullException(nameof(filter));
            }

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            var result = _followUpRepository.GetRelatedReports(filter);

            return result;
        }

        public List<BhsFollowUpViewModel> GetFollowUpsForVisit(long visitId)
        {
            return _followUpRepository.GetFollowUpsForVisit(visitId);

        }

        public List<BhsFollowUpListItemPrintoutModel> GetAllForPrintout(BhsSearchFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            List<BhsFollowUpListItemDtoModel> items = _followUpRepository.GetAllItems(filter);


            var result = items.ToViewModel().ToList();

            return result;
        }
    }
}

