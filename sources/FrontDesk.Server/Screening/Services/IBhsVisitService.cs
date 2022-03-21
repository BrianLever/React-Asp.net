using System;
using System.Collections.Generic;
using System.Data;
using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;

using ScreenDox.Server.Models;

namespace FrontDesk.Server.Screening.Services
{
    public interface IBhsVisitService
    {
        BhsVisit Create(long screeningResultId);
        BhsVisit Create(ScreeningResult screeningResult, FrontDesk.Screening info);
        long? FindByScreeningResultId(long screeningResultId);
        void FullfilPatientAddress(ScreeningResult patient);
        BhsVisit Get(long id);
        List<BhsVisitListItemPrintoutModel> GetAllForPrintout(BhsSearchFilterModel filter);
        List<BhsVisitViewModel> GetRelatedReports(BhsSearchRelatedItemsFilter filter);
        DataSet GetUniqueVisits(BhsSearchFilterModel filter, string orderBy, int maximumRows, int startRowIndex);
        int GetUniqueVisitsCount(BhsSearchFilterModel filter);
        void Update(BhsVisit model, IUserPrincipal user);

        #region data source binding

        int GetUniqueVisitsCountForDataSource(string firstNameFilter, string lastNameFilter, int? locationFilter, DateTime? startDateFilter, DateTime? endDateFilter, int? reportTypeFilter, long? screeningResultIdFilter);
        DataSet GetUniqueVisitsForDataSource(string firstNameFilter, string lastNameFilter, int? locationFilter, DateTime? startDateFilter, DateTime? endDateFilter, int? reportTypeFilter, long? screeningResultIdFilter, string orderBy, int maximumRows, int startRowIndex);


        #endregion
    }
}