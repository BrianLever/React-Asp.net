
using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Extensions;
using FrontDesk.Configuration;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Manages Screening Profile Frequency setting
    /// </summary>
    public class ScreeningProfileFrequencyController : ApiControllerWithValidationBase<ScreeningProfileFrequencyRequest>
    {
        private readonly IScreeningProfileService _profileService;
        private readonly IScreeningProfileFrequencyService _service;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<ScreeningProfileController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="profileService"></param>
        /// <param name="serviceFrequency"></param>
        /// <param name="securityLogService"></param>
        public ScreeningProfileFrequencyController(
            IScreeningProfileService profileService,
            IScreeningProfileFrequencyService serviceFrequency,
            ISecurityLogService securityLogService
            )
        {
            _profileService = profileService ?? throw new ArgumentNullException(nameof(profileService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _service = serviceFrequency ?? throw new ArgumentNullException(nameof(serviceFrequency));
        }

        /// <summary>
        /// Get options for Screening Frequency values
        /// </summary>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 3600)] // 1 hour cache
        [Route("api/screeningprofile/frequency/list")]
        [HttpGet]
        public IEnumerable<ListItem> GetList()
        {
            return _service.GetSupportedValues().Select(x => new ListItem(x.Key, x.Value)) ;
        }

        /// <summary>
        /// Get screening profile frequency record
        /// </summary>
        /// <param name="id">Screening profile Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [Route("api/screeningprofile/{id}/frequency")]
        public IEnumerable<ScreeningFrequencyItemViewModel> Get([FromUri] int id)
        {
            // ensure id is valid
            id.ShouldNotBeDefault();
            var profile = _profileService.Get(id);
            profile.ShouldNotBeNull();


            //return results
            var result = _service.GetAll(id)
                 .Select(x => AutoMapper.Mapper.Map<ScreeningFrequencyItemViewModel>(x))
                 .ToList();

            return result;
        }

        /// <summary>
        /// Update or create screening profile frequency settings
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpPut]
        [Route("api/screeningprofile/{id}/frequency")]
        public IHttpActionResult Add(int id, ScreeningProfileFrequencyRequest requestModel)
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
        private IEnumerable<ScreeningFrequencyItemViewModel> Save(
            int screeningProfileId,
            ScreeningProfileFrequencyRequest updateItems)
        {

            _service.Save(screeningProfileId, updateItems.Items);


            _logger.InfoFormat("[Screening Profile] Frequency Settings have been updated. Data: {0}.", updateItems.ToJson());
            return Get(screeningProfileId);

        }



        protected override AbstractValidator<ScreeningProfileFrequencyRequest> GetValidator()
        {
            return new ScreeningProfileFrequencyRequestValidator();
        }

        protected override void Validate(ScreeningProfileFrequencyRequest inputModel)
        {
            base.Validate(inputModel);

            // validate frequency values
            var allSupportedValues = _service.GetSupportedValues().Keys.ToArray();

            var notSupportedValues = inputModel.
                Items
                .Where(x => !allSupportedValues.Contains(x.Frequency))
                .Select(x => x.Frequency);

            if(notSupportedValues.Any())
            {
                var errorMessage = "Unsupported frequency values: {0}. Valid values: {1}"
                    .FormatWith(
                    string.Join(",", notSupportedValues),
                    string.Join(",", allSupportedValues));
                ResponseDataFactory.ThrowBadRequestMessage(new string[] { errorMessage });
            }
            
        }
    }
}
