using FrontDesk.Server.Data.Logging;
using FrontDesk.Server.Logging;

using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Security;

using System;
using System.Collections.Generic;

namespace ScreenDox.Server.Logging
{
    public interface ISecurityLogService
    {
        void Add(SecurityLog logItem);

        List<SecurityEventCategory> GetCategories(bool isSA);
        List<SecurityEventView> GetEvents(int categoryID);

        List<SecurityLogEventSettingResponse> GetEventsSettings(int? categoryID);
        int GetReportItemsCount(int categoryID, int eventID, bool isSA, DateTime? startDate, DateTime? endDate);
        void SetEventEnabledStatus(int eventID, bool enabled);
    }

    public class SecurityLogService : ISecurityLogService
    {

        private readonly ISecurityLogRepository _repository;
        private readonly IUserPrincipalService _userService;

        public SecurityLogService(ISecurityLogRepository repository, IUserPrincipalService userService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        /// <summary>
        /// Add new security log record
        /// </summary>
        public void Add(SecurityLog logItem)
        {
            if (_repository.IsEventEnabled(logItem.LoggedEvent.Event))
            {
                logItem.UserID = _userService.GetCurrent().UserID;
                logItem.LogDate = DateTimeOffset.Now;

                _repository.Add(logItem);
            }
        }


        /// <summary>
        /// Get items count for paging
        /// </summary>
        public int GetReportItemsCount(int categoryID, int eventID,
             bool isSA, DateTime? startDate, DateTime? endDate)
        {
            return _repository.GetReportItemsCount(
                categoryID == 0 ? (int?)null : categoryID,
                eventID == 0 ? (int?)null : eventID,
                startDate, endDate, isSA,
                _userService.GetCurrent().UserID);
        }


        /// <summary>
        /// Get list of available security event categories
        /// </summary>
        public List<SecurityEventCategory> GetCategories(bool isSA)
        {
            return _repository.GetCategories(isSA);
        }

        /// <summary>
        /// Get the list of events by category
        /// </summary>
        public List<SecurityEventView> GetEvents(int categoryID)
        {
            return _repository.GetEvents(categoryID);
        }



        /// <summary>
        /// Set the status of security log event
        /// </summary>
        public void SetEventEnabledStatus(int eventID, bool enabled)
        {
            _repository.SetEventEnabledStatus(eventID, enabled);
        }

        public List<SecurityLogEventSettingResponse> GetEventsSettings(int? categoryID)
        {
            return _repository.GetEventsSettings(categoryID);
        }
    }

}
