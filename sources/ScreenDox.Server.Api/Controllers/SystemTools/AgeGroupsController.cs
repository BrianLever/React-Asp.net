
using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk.Common.Configuration;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.Screening;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Common.Configuration;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.SystemTools
{
    /// <summary>
    /// Manages Visit creation system settings
    /// </summary>
    public class AgeGroupsController : ApiControllerWithValidationBase<AgeGroupsSettingRequest>
    {
        private readonly IScreeningAgesSettingsProvider _ageProvider;
        private readonly ISystemSettingService _systemSettingService;

        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<AgeGroupsController>();

        private string AgeGroupSettingKey = "IndicatorReport_AgeGroups";

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="systemSettingService"></param>
        /// <param name="ageProvider"></param>
        /// <param name="securityLogService"></param>
        public AgeGroupsController(
            ISystemSettingService systemSettingService,
            IScreeningAgesSettingsProvider ageProvider,
            ISecurityLogService securityLogService)
        {
            _ageProvider = ageProvider ?? throw new ArgumentNullException(nameof(ageProvider));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _systemSettingService = systemSettingService ?? throw new ArgumentNullException(nameof(systemSettingService));
        }


        /// <summary>
        /// Get current state of age group settings
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        public AgeGroupSettingResponse Get()
        {
            var result = new AgeGroupSettingResponse();
            //return results
            result.Value = _systemSettingService.GetByKey(AgeGroupSettingKey);
            result.DefaultValue = AppSettingsProxy.GetStringValue(AgeGroupSettingKey, string.Empty);
            result.Labels = _ageProvider.AgeGroupsLabels;

            return result;
        }

        
        /// <summary>
        /// Update or create age groups settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPut]
        public IHttpActionResult Add(AgeGroupsSettingRequest requestModel)
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
        private AgeGroupSettingResponse Save(AgeGroupsSettingRequest updateItems)
        {

            _systemSettingService.UpdateSystemSettingsValue(updateItems.Value.Key, updateItems.Value.Value);

            _logger.InfoFormat("Age Groups Settings have been updated. Data: {0}.", updateItems.ToJson());

            // reload data into provider
            _ageProvider.Refresh();

            return Get();
        }



        protected override AbstractValidator<AgeGroupsSettingRequest> GetValidator()
        {
            return new AgeGroupsSettingRequestValidator();
        }
    }
}
