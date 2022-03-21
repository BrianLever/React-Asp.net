using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening.Models;

using ScreenDox.Server.Models;

using System.Collections.Generic;

namespace FrontDesk.Server.Screening.Services
{
    /// <summary>
    /// IFollowUpService interface used in Screendox Api
    /// </summary>
    public interface IFollowUpService
    {
        SearchResponse<UniqueFollowUpViewModel> GetUniqueItems(PagedVisitFilterModel filter);
        List<BhsFollowUpViewModel> GetRelatedItems(long id, VisitFilterModel filter);

        List<BhsFollowUpListItemPrintoutModel> GetAllForPrintout(BhsSearchFilterModel filter);

        BhsFollowUp Get(long id);
        void Update(BhsFollowUp updatedModel, IUserPrincipal currentPrincipal);

        List<BhsFollowUpViewModel> GetFollowUpsForVisit(long visitId);
    }

}