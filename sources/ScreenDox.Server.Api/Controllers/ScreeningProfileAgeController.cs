
using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Server.Licensing;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Manages Screening Profile Minimum Age settings and turns on/off screening tools
    /// </summary>
    public class ScreeningProfileAgeController : ApiControllerWithValidationBase<ScreeningProfileAgeRequest>
    {
        private readonly IScreeningProfileService _profileService;
        private readonly IScreeningProfileMinimalAgeService _service;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<ScreeningProfileController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="serviceProfile"></param>
        /// <param name="serviceMinimalAge"></param>
        /// <param name="securityLogService"></param>
        public ScreeningProfileAgeController(
            IScreeningProfileService serviceProfile,
            IScreeningProfileMinimalAgeService serviceMinimalAge,
            ISecurityLogService securityLogService
            )
        {
            _profileService = serviceProfile ?? throw new ArgumentNullException(nameof(serviceProfile));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _service = serviceMinimalAge ?? throw new ArgumentNullException(nameof(serviceMinimalAge));
        }

        /// <summary>
        /// Get minumum age groups and depedencies for UI logic
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 3600)] // 1 hour cache
        [Route("api/screeningprofile/age/groups")]
        [HttpGet]
        public IEnumerable<ScreeningMinimalAgeGroup> GetGroups()
        {
            return ScreeningSectionAge.GetScreeningMinimalAgeGroups();
        }

        /// <summary>
        /// Get screening profile age record
        /// </summary>
        /// <param name="id">Screening profile Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [Route("api/screeningprofile/{id}/age")]
        public IEnumerable<ScreeningSectionAgeItemViewModel> Get([FromUri] int id)
        {
            // ensure id is valid
            id.ShouldNotBeDefault();
            var profile = _profileService.Get(id);
            profile.ShouldNotBeNull();


            //return results
            var result = _service.GetSectionMinimalAgeSettings(id)
                 .Select(x => {
                     var item = AutoMapper.Mapper.Map<ScreeningSectionAgeItemViewModel>(x);

                     if (item.ScreeningSectionID == ScreeningSectionDescriptor.SubstanceAbuse 
                        && !PasswordProtectedFeaturesProvider.IsDast10Enabled)
                     {
                         if (!item.IsEnabled) //allow to turn off if section is enabled
                         {
                             item.IsHidden = true;
                         }
                     }
                     return item;
                     })
                 .ToList();

            return result;
        }

        /// <summary>
        /// Update or create screening profile age settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPut]
        [Route("api/screeningprofile/{id}/age")]
        public IHttpActionResult Add(int id, ScreeningProfileAgeRequest requestModel)
        {
            requestModel.ShouldNotBeNull();
            id.ShouldNotBeDefault();

            // validation
            Validate(requestModel);

            return AcceptedContentResult(Save(id, requestModel));
        }

        /// <summary>
        /// Apply changes
        /// </summary>
        /// <param name="screeningProfileId"></param>
        /// <param name="updateItems"></param>
        /// <returns></returns>
        private IEnumerable<ScreeningSectionAgeItemViewModel> Save(
            int screeningProfileId,
            ScreeningProfileAgeRequest updateItems)
        {

            _service.UpdateMinimalAgeSettings(screeningProfileId, updateItems.Items);


            _logger.InfoFormat("[Screening Profile] Age Settings have been updated. Data: {0}.", updateItems.ToJson());
            return Get(screeningProfileId);

        }

        /// <summary>
        /// Get validator
        /// </summary>
        /// <returns></returns>
        protected override AbstractValidator<ScreeningProfileAgeRequest> GetValidator()
        {
            return new ScreeningProfileAgeRequestValidator();
        }
    }
}
