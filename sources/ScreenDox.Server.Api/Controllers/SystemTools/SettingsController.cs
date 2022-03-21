
using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.InfrastructureServices;
using ScreenDox.Server.Resources;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Common.Configuration;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models.Configuration;
using ScreenDox.Server.Models.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers.SystemTools
{
    /// <summary>
    /// Manages System Settings
    /// </summary>
    public class SettingsController : ApiControllerWithValidationBase<SystemSettingRequest>
    {
        private readonly ISystemSettingService _systemSettingService;
        private readonly ITimeService _timeService;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<SystemSettingsController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="systemSettingService"></param>
        /// <param name="securityLogService"></param>
        /// <param name="timeService"></param>
        public SettingsController(
            ISystemSettingService systemSettingService,
            ISecurityLogService securityLogService,
            ITimeService timeService)
        {
            _systemSettingService = systemSettingService ?? throw new ArgumentNullException(nameof(systemSettingService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

        }


        /// <summary>
        /// Get current state of exposed system setttings
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        public IEnumerable<ISystemSettingItem> Get()
        {
            string[] supportedKeys = new[]
            {
                "PasswordRenewalPeriodDays",
                "IndicatorReport_AgeGroups"
            };

            //return results
            var result = _systemSettingService.GetAll()
                .Where(x => x.IsExposed && supportedKeys.Contains(x.Key))
                .ToArray();

            return result;
        }




        /// <summary>
        /// Update system setting values
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPut]
        public IHttpActionResult Add(SystemSettingRequest requestModel)
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
        private IEnumerable<ISystemSettingItem> Save(SystemSettingRequest updateItems)
        {
            foreach (var item in updateItems.Items)
            {
                _systemSettingService.UpdateSystemSettingsValue(item.Key, item.Value);

                _logger.InfoFormat("System Settings have been updated. Data: {0}.", item.ToJson());

            }
            // reload data into provider

            return Get();
        }

        protected override AbstractValidator<SystemSettingRequest> GetValidator()
        {
            return new SystemSettingRequestValidator();
        }

        protected override void Validate(SystemSettingRequest inputModel)
        {
            base.Validate(inputModel);

            // validate regular expressions for provided values
            var dbValues = Get();

            List<string> errors = new List<string>();

            foreach (var item in inputModel.Items)
            {
                var dbItem = dbValues.FirstOrDefault(x => String.Compare(x.Key, item.Key, StringComparison.OrdinalIgnoreCase) == 0);

                // check key is known
                if (dbItem == null)
                {
                    errors.Add("Configuration value with the key '{0}' was not found in the database.".FormatWith(item.Key));
                    continue;
                }
                // validate on regular expression
                if (!string.IsNullOrEmpty(dbItem.RegExp) && !string.IsNullOrEmpty(item.Value))
                {
                    Regex re = new Regex(item.RegExp);

                    if (!re.IsMatch(item.Value))
                    {
                        errors.Add("Configuration value '{0}' has invalid value. Validation rule failed. Value: '{1}'."
                            .FormatWith(item.Key, item.Value));
                        continue;
                    }
                }

                if (item.Key.ToUpper() == "PASSWORDRENEWALPERIODDAYS")
                {
                    var value = item.Value.AsNullable<int>() ?? 0;

                    if (value <= 0)
                    {
                        errors.Add(TextMessages.RenewalPeriodCompareError);
                        continue;
                    }
                }
            }

            if (errors.Any())
            {
                ResponseDataFactory.ThrowBadRequestMessage(errors);
            }
        }
    }
}
