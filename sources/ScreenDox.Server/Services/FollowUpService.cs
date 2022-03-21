using FrontDesk.Common.Bhservice;
using FrontDesk.Server.Data.BhsVisits;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Services;

using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;

namespace ScreenDox.Server.Screening.Services
{
    public class FollowUpService : BhsFollowUpService, IFollowUpService
    {

        private readonly ISecurityLogService _securityLogService;

        public FollowUpService(
            IBhsFollowUpRepository followUpRepository, 
            IBhsFollowUpFactory followUpFactory, 
            IBhsVisitRepository visitRepository,
            ISecurityLogService securityLogService
        ) : base(
            followUpRepository,
            followUpFactory,
            visitRepository
        )
        {
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
        }

        /// <summary>
        /// Get unique visit follow up records that have created in given period of time
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public SearchResponse<UniqueFollowUpViewModel> GetUniqueItems(PagedVisitFilterModel filter)
        {
            var result = new SearchResponse<UniqueFollowUpViewModel>();

            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }
        
            result.TotalCount = _followUpRepository.GetLatestItemsForDisplayCount(filter);

            result.Items = _followUpRepository.GetLatestItemsForDisplay(filter);

            return result;
        }

        /// <summary>
        /// Get Follow-Up records for specific patient
        /// </summary>
        /// <param name="id">Screendox ID of the Screening Result which is used to get patient identity.</param>
        /// <param name="filter">Filter condition.</param>
        /// <returns>>List of follow up records.</returns>
        public List<BhsFollowUpViewModel> GetRelatedItems(long id, VisitFilterModel filter)
        {
            if (filter.EndDate.HasValue)
            {
                filter.EndDate = filter.EndDate.Value.Date.AddDays(1); // we use "Less than" operator, so we add one day
            }

            return _followUpRepository.GetRelatedReports(
                new BhsSearchRelatedItemsFilter
                {
                    MainRowID = id,
                    ScreeningResultID = filter.ScreeningResultID,
                    StartDate = filter.StartDate,
                    EndDate = filter.EndDate,
                    LocationId = filter.Location,
                    ReportType = filter.ReportType,
                }
            );

        }

        
    }
}

