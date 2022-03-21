
using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk.Common.Extensions;
using FrontDesk.Server.Logging;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.SystemTools
{
    /// <summary>
    /// Manages System Settings
    /// </summary>
    public class SecurityLogSettingsController : ApiControllerWithValidationBase<SecurityLogSettingRequest>
    {
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<SecurityLogSettingsController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="securityLogService"></param>
        public SecurityLogSettingsController(
            ISecurityLogService securityLogService
            )
        {
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));

        }


        /// <summary>
        /// Get Security Log Categories
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/systemtools/SecurityLogSettings/category")]
        public IEnumerable<SecurityEventCategory> GetCategories()
        {
            var result = _securityLogService.GetCategories(true);

            return result;
        }

        /// <summary>
        /// Get All Security Log events
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/systemtools/SecurityLogSettings")]
        public IEnumerable<SecurityLogEventSettingResponse> GetAllEvents()
        {
            return GetEventsByCategory(null);
        }


        /// <summary>
        /// Get All Security Log events
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        public IEnumerable<SecurityLogEventSettingResponse> GetEventsByCategory(int? categoryId)
        {
            var result = _securityLogService.GetEventsSettings(categoryId);

            return result;
        }


        /// <summary>
        /// Update security log setting values
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPut]
        [Route("api/systemtools/securitylogsettings")]
        public IHttpActionResult Add(SecurityLogSettingRequest requestModel)
        {
            requestModel.ShouldNotBeNull();

            // validation
            Validate(requestModel);

            return AcceptedContentResult(Save(requestModel));
        }

        /// <summary>
        /// Apply changes
        /// </summary>
        /// <param name="updateItems"></param>
        /// <returns></returns>
        private IEnumerable<SecurityLogEventSettingResponse> Save(SecurityLogSettingRequest updateItems)
        {
            foreach (var item in updateItems.Items)
            {
                _securityLogService.SetEventEnabledStatus(item.ID, item.IsEnabled.Value);

                _logger.InfoFormat("Security Log Settings have been updated. Data: {0}.", item.ToJson());

            }
            return GetAllEvents();
        }

        protected override AbstractValidator<SecurityLogSettingRequest> GetValidator()
        {
            return new SecurityLogSettingRequestValidator();
        }
    }

}
