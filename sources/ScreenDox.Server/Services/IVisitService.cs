using System;
using System.Collections.Generic;
using System.Data;

using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;

using ScreenDox.Server.Models;
using ScreenDox.Server.Screening;

namespace FrontDesk.Server.Screening.Services
{
    /// <summary>
    /// Fork from iBhsVisitService interface used in Screendox Api
    /// </summary>
    public interface IVisitService
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
        void UpdateDrugOfChoiceInResult(BhsVisit updatedModel);

        #region data source binding

        SearchResponse<UniqueVisitViewModel> GetUniqueVisits(PagedVisitFilterModel filter);
        List<VisitViewModel> GetRelatedVisits(long id, VisitFilterModel filter);

        #endregion
    }
}