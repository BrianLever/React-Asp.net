using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening.Services;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Api.Security;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Security;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Manages Branch locations and returns the list of available branch locations to the logged in user
    /// </summary>
    public class BranchLocationController : ApiController
    {
        private readonly IBranchLocationService _service;
        private readonly ISecurityLogService _securityLogService;
        private readonly IUserPrincipalService _userService;

        private readonly ILog _logger = LogManager.GetLogger<BranchLocationController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityLogService"></param>
        /// <param name="userService"></param>
        public BranchLocationController(IBranchLocationService service,
                                        ISecurityLogService securityLogService,
                                        IUserPrincipalService userService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }


        /// <summary>
        /// Get branch location for UI dropdown list binding. 
        /// </summary>
        /// <returns>Branch locations ID and Names sorted by name.</returns>
        [HttpCache(DefaultExpirySeconds = 10)] // 10 seconds
        [Route("api/branchlocation/list")]
        [HttpGet]
        public IEnumerable<ListItem> GetList()
        {
            var principal = _userService.GetCurrent();

            var items = new List<BranchLocation>();

            if (principal.IsUserInRole(UserRoles.BranchAdministrator) || principal.IsUserInRole(UserRoles.SuperAdministrator))
            {
                items.AddRange(_service.GetAll());
            }
            else
            {
                items.AddRange(_service.GetForUserID(principal.UserID));
            }

            return items.Select(x => new ListItem
            {
                ID = x.BranchLocationID,
                Name = x.Name
            });
        }

        /// <summary>
        /// Get filtered list of branch locations for pages view.
        /// </summary>
        /// <param name="filter">Filter.
        /// OrderBy options:
        ///     Name (Default)
        ///     Description
        ///     ScreeningProfileName
        /// </param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/branchlocation/search")]
        public SearchResponse<BranchLocation> GetAll([FromBody] BranchLocationSearchFilter filter)
        {
            filter = filter ?? new BranchLocationSearchFilter();

            Guard.ValidateOrderByClause(filter, new[] { "ScreeningProfileName", "Name", "Description" });

            return _service.GetAll(filter);
        }


        /// <summary>
        /// Get branch location record
        /// </summary>
        /// <param name="id">Branch location Id.</param>
        /// <returns></returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpGet]
        [Route("api/branchlocation/{id}")]
        public BranchLocation Get([FromUri] int id)
        {
            id.ShouldNotBeDefault();

            var model = _service.Get(id);

            model.ShouldNotBeNull(id);

            return model;
        }


        /// <summary>
        /// Create new branch location record
        /// </summary>
        /// <returns></returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator)]
        [HttpPost]
        [Route("api/branchlocation")]
        public BranchLocation Add(BranchLocationRequest requestModel)
        {
            requestModel.ShouldNotBeNull();

            // validation
            Validate(requestModel);

            var model = Mapper.Map<BranchLocation>(requestModel);

            try
            {
                var id = _service.Add(model);


                _securityLogService.Add(new SecurityLog(
                    SecurityEvents.NewBranchLocationAdded,
                    "{0}~{1}".FormatWith(model.BranchLocationID, model.Name)
                    ));

                model = _service.Get(id);
            }
            catch (ApplicationException ex)
            {
                _logger.WarnFormat("[Branch Location Api] Validation error while creation. Message: {0}.", ex, ex.Message);

                ResponseDataFactory.ThrowBadRequestMessage(ex.Message);
            }

            return model;
        }


        /// <summary>
        /// Update existing branch location record
        /// </summary>
        /// <param name="id">Branch Location ID</param>
        /// <param name="requestModel">Updated model.
        /// If Disabled flag has been changed, the location is moved to active or disabled state.
        /// </param>
        /// <returns>Status 200 if update was successful.</returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator)]
        [HttpPut]
        [Route("api/branchlocation/{id}")]
        public IHttpActionResult Update([FromUri] int id, BranchLocationRequest requestModel)
        {
            requestModel.ShouldNotBeNull();
            id.ShouldNotBeDefault();

            requestModel.BranchLocationID = id;

            // validation
            Validate(requestModel);

            // check resource exists
            var dbModel = _service.Get(id);
            dbModel.ShouldNotBeNull();

            var dbDisabledState = dbModel.Disabled;


            // update model from request
            var updateModel = Mapper.Map(requestModel, dbModel);

            // apply changes
            _service.Update(updateModel);

            if (dbDisabledState != updateModel.Disabled)
            {
                if (!_service.HasActiveKiosk(id))
                {
                    _service.SetDisabledStatus(id, updateModel.Disabled);

                    _logger.InfoFormat("[Branch Location] Location status has been changed. ID: {0}. Is Disabled: {1}", id, updateModel.Disabled);
                }
                else
                {
                    ResponseDataFactory.ThrowBadRequestMessage(new string[]{Resources.TextMessages.Location_Disabled_Failed});
                }
            }

            return Ok(CustomMessage.GetUpdateMessage("Branch location"));
        }


        /// <summary>
        /// Delete existing branch location record
        /// </summary>
        /// <param name="id">Branch Location ID</param>
        /// <returns>Status 200 if delete was successful.</returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator)]
        [HttpDelete]
        [Route("api/branchlocation/{id}")]
        public IHttpActionResult Delete([FromUri] int id)
        {
            id.ShouldNotBeDefault();

            // check resource exists
            var dbModel = _service.Get(id);
            dbModel.ShouldNotBeNull();

            // apply changes
            var success = _service.Delete(id);

            if (!success)
            {
                ResponseDataFactory.ThrowBadRequestMessage("Invalid operation", new[] {
                    "Cannot delete Branch Location while user or kiosk are assigned to it." }
                );
            }

            _securityLogService.Add(new SecurityLog(SecurityEvents.BranchLocationDeleted, dbModel.Name));

            return Ok(CustomMessage.GetDeleteMessage("Branch location"));
        }


        protected AbstractValidator<BranchLocationRequest> GetValidator()
        {
            return new BranchLocationRequestValidator();
        }

        private void Validate(BranchLocationRequest inputModel)
        {
            var result = GetValidator().Validate(inputModel);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }

    }
}
