using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Common.Models;
using ScreenDox.Server.Common.Resources;
using ScreenDox.Server.Common.Services;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Security;

using System;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Manages Kiosks
    /// </summary>
    public class KioskController : ApiController
    {
        private readonly IKioskService _service;
        private readonly IUserPrincipalService _userService;
        private readonly ISecurityLogService _securityLogService;
        private readonly ILicenseService _licenseService;
        private readonly IBranchLocationService _branchLocationService;

        private readonly ILog _logger = LogManager.GetLogger<KioskController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityLogService"></param>
        /// <param name="licenseService"></param>
        /// <param name="branchLocationService"></param>
        /// <param name="userService"></param>
        public KioskController(IKioskService service,
                               IUserPrincipalService userService,
                               ISecurityLogService securityLogService,
                               ILicenseService licenseService,
                               IBranchLocationService branchLocationService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _licenseService = licenseService ?? throw new ArgumentNullException(nameof(licenseService));
            _branchLocationService = branchLocationService ?? throw new ArgumentNullException(nameof(branchLocationService));
        }


        /// <summary>
        /// Get filtered list of kiosks for pages view.
        /// </summary>
        /// <param name="filter">Filter.
        /// OrderBy options:
        ///     KioskID (Default)
        ///     KioskName
        ///     BranchLocationName
        ///     ScreeningProfileName
        ///     LastActivityDate
        ///     IpAddress
        ///     KioskAppVersion
        /// </param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/kiosk/search")]
        public SearchResponse<Kiosk> GetAll([FromBody] KioskSearchFilter filter)
        {
            filter = filter ?? new KioskSearchFilter();

            Guard.ValidateOrderByClause(filter, new[] { "KioskID", "KioskName", "BranchLocationName", "ScreeningProfileName", "LastActivityDate", "IpAddress", "KioskAppVersion" });

            int? filterForUserId = null;
            //TODO: complete authorization

            //if (!Roles.IsUserInRole(UserRoles.SuperAdministrator))
            //{
            //    odsKiosk.SelectParameters["userID"].DefaultValue = FDUser.GetCurrentUser().UserID.ToString();
            //    gvKiosks.PageIndex = 0;
            //    EnsureDataBound();
            //}

            return _service.GetAll(filter, filterForUserId);
        }


        /// <summary>
        /// Get kiosk record
        /// </summary>
        /// <param name="id">Kiosk Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/kiosk/{id}")]
        public Kiosk Get([FromUri] short id)
        {
            id.ShouldNotBeDefault();

            var model = _service.GetByID(id);

            model.ShouldNotBeNull(id);

            return model;
        }


        /// <summary>
        /// Create new kiosk record
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/kiosk")]
        public Kiosk Add(KioskRequest requestModel)
        {
            requestModel.ShouldNotBeNull();

            // validation
            Validate(requestModel);

            var model = Mapper.Map<Kiosk>(requestModel);


            //check that kiosk is not inserted into disabled location
            CheckLocationIsNotDisabled(model.BranchLocationID);

            // adding new kiosk
            var license = _licenseService.GetActivatedLicense();

            try
            {
                var id = _service.Add(model, license.License.MaxKiosks);

                _securityLogService.Add(new SecurityLog(SecurityEvents.NewKioskRegistered, "{0}~{1}".FormatWith(id, model.Name)));

                model = _service.GetByID(id);

            }
            catch (ApplicationException ex)
            {
                _logger.WarnFormat("[Kiosk] Validation error while creation. Message: {0}.", ex, ex.Message);

                ResponseDataFactory.ThrowBadRequestMessage(ex.Message);
            }


            return model;
        }

        private void CheckLocationIsNotDisabled(int branchLocationID)
        {
            var branch = _branchLocationService.Get(branchLocationID);
            if (branch != null && branch.Disabled)
            {
                ResponseDataFactory.ThrowBadRequestMessage(new[] { TextMessages.Kiosk_CannotAddKioskToBlockedLocation });
            }

        }


        /// <summary>
        /// Update existing kiosk record
        /// </summary>
        /// <param name="id">Kiosk ID</param>
        /// <param name="requestModel">Updated model.
        /// If Disabled flag has been changed, the location is moved to active or disabled state.
        /// </param>
        /// <returns>Status 200 if update was successful.</returns>
        [HttpPut]
        [Route("api/kiosk/{id}")]
        public IHttpActionResult Update([FromUri] short id, KioskRequest requestModel)
        {
            requestModel.ShouldNotBeNull();
            id.ShouldNotBeDefault();

            requestModel.KioskID = id;

            // validation
            Validate(requestModel);

            // check resource exists
            var dbModel = _service.GetByID(id);
            dbModel.ShouldNotBeNull();

            var dbDisabledState = dbModel.Disabled;

            // validate on active branch location only when changed
            if (dbModel.BranchLocationID != requestModel.BranchLocationID)
            {
                //check that kiosk is not inserted into disabled location
                CheckLocationIsNotDisabled(requestModel.BranchLocationID);
            }

            // update model from request
            var updateModel = Mapper.Map(requestModel, dbModel);


            // apply changes
            _service.Update(updateModel);

            if (dbDisabledState != updateModel.Disabled)
            {
                if (!updateModel.Disabled) // when enable kiosk
                {
                    var branch = _branchLocationService.Get(updateModel.BranchLocationID);
                    if (branch.Disabled)
                    {
                        ResponseDataFactory.ThrowBadRequestMessage(new[] { TextMessages.Kiosk_UnableToEnableBecauseLocationIsDisabled });
                    }
                }

                _service.SetDisabledStatus(id, updateModel.Disabled, _licenseService.GetActivatedLicense().License.MaxKiosks);

                _logger.InfoFormat("[Kiosk] Kiosk status has been changed. ID: {0}. Is Disabled: {1}", id, updateModel.Disabled);
            }

            return Ok(CustomMessage.GetUpdateMessage("Kiosk"));
        }


        /// <summary>
        /// Delete existing kiosk record
        /// </summary>
        /// <param name="id">Kiosk ID</param>
        /// <returns>Status 200 if delete was successful.</returns>
        [HttpDelete]
        [Route("api/kiosk/{id}")]
        public IHttpActionResult Delete([FromUri] short id)
        {
            id.ShouldNotBeDefault();

            // check resource exists
            var dbModel = _service.GetByID(id);
            dbModel.ShouldNotBeNull();

            // apply changes
            _service.Delete(id);


            _securityLogService.Add(new SecurityLog(SecurityEvents.KioskRemoved, dbModel.Name));


            return Ok(CustomMessage.GetDeleteMessage("Kiosk"));
        }


        protected AbstractValidator<KioskRequest> GetValidator()
        {
            return new KioskRequestValidator();
        }

        private void Validate(KioskRequest inputModel)
        {
            var result = GetValidator().Validate(inputModel);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }


    }
}
