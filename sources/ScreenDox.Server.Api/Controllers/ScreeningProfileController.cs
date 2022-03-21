using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
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
    /// Manages Screening Profiles
    /// </summary>
    public class ScreeningProfileController : EntityApiController<ScreeningProfile, ScreeningProfileRequest, int>
    {
        private readonly IScreeningProfileService _service;
        private readonly ISecurityLogService _securityLogService;

        private readonly ILog _logger = LogManager.GetLogger<ScreeningProfileController>();


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"></param>
        /// <param name="securityLogService"></param>
        public ScreeningProfileController(IScreeningProfileService service, ISecurityLogService securityLogService)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
        }

        /// <summary>
        /// Resource name
        /// </summary>
        protected override string ResourceName => "Screening Profile";

        /// <summary>
        /// Get screen profiles for UI dropdown list binding. 
        /// </summary>
        /// <returns>Screen profile ID and Name sorted by name.</returns>
        [HttpCache(DefaultExpirySeconds = 10)] // 10 seconds
        [Route("api/screeningprofile/list")]
        [HttpGet]
        public IEnumerable<ListItem> GetList()
        {
            /* if (Roles.IsUserInRole(UserRoles.BranchAdministrator) || Roles.IsUserInRole(UserRoles.SuperAdministrator))
            {
                return _repository.GetAll();
            }
            else
            {
                return _repository.GetForUserID(FDUser.CurrentUserID);
            }
            */

            var items = _service.GetAll();

            return items.Select(x => new ListItem
            {
                ID = x.ID,
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
        /// </param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [HttpPost]
        [Route("api/screeningprofile/search")]
        public SearchResponse<ScreeningProfile> GetAll([FromBody] SearchByNamePagedSearchFilter filter)
        {
            filter = filter ?? new SearchByNamePagedSearchFilter();

            Guard.ValidateOrderByClause(filter, new[] { "Name", "Description" });

            return _service.GetAll(filter);
        }


        /// <summary>
        /// Get screening profile record
        /// </summary>
        /// <param name="id">Screening profile Id.</param>
        /// <returns></returns>
        [HttpCache(DefaultExpirySeconds = 0)] // No cache
        [Route("api/screeningprofile/{id}")]
        public override ScreeningProfile Get([FromUri] int id)
        {
            id.ShouldNotBeDefault();

            var model = _service.Get(id);

            model.ShouldNotBeNull(id);

            return model;
        }


        /// <summary>
        /// Create new screening profile record
        /// </summary>
        /// <returns></returns>
        protected override ScreeningProfile AddResource(ScreeningProfileRequest requestModel)
        {
            requestModel.ShouldNotBeNull();

            // validation
            Validate(requestModel);

            var model = Mapper.Map<ScreeningProfile>(requestModel);

            _service.Add(model);

            _securityLogService.Add(new SecurityLog(
                SecurityEvents.NewScreeningProfileCreated,
                "{0}~{1}".FormatWith(model.ID, model.Name)
                ));


            return model;
        }
        
        /// <summary>
        /// Update existing screening profile record
        /// </summary>
        /// <param name="id">Screening profile ID</param>
        /// <param name="requestModel">Updated model.</param>
        [Route("api/screeningprofile/{id}")]
        public override IHttpActionResult Update([FromUri] int id, [FromBody]ScreeningProfileRequest requestModel)
        {
            requestModel.ShouldNotBeNull();
            id.ShouldNotBeDefault();

            requestModel.ID = id;

            // validation
            Validate(requestModel);

            // check resource exists
            var dbModel = _service.Get(id);
            dbModel.ShouldNotBeNull();


            // update model from request
            var updateModel = Mapper.Map(requestModel, dbModel);

            // apply changes
            _service.Update(updateModel);

            return GetSuccessfulUpdateResult();
        }


        /// <summary>
        /// Delete existing screening profile record
        /// </summary>
        /// <param name="id">Branch Location ID</param>
        /// <returns>Status 200 if delete was successful.</returns>
        [Route("api/screeningprofile/{id}")]
        public override IHttpActionResult Delete([FromUri] int id)
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

            _securityLogService.Add(new SecurityLog(
               SecurityEvents.ScreeningProfileDeleted,
               dbModel.Name
               ));

            return Ok(CustomMessage.GetDeleteMessage("Screening profile"));
        }

        /// <summary>
        /// Create validator
        /// </summary>
        /// <returns></returns>
        protected override AbstractValidator<ScreeningProfileRequest> GetValidator()
        {
            return new ScreeningProfileRequestValidator();
        }
    }
}
