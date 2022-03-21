

using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using FrontDesk.Common.Entity;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models;
using ScreenDox.Server.Api.Security;
using ScreenDox.Server.Logging;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.Security;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Models.ViewModels.Validators;
using ScreenDox.Server.Security;
using ScreenDox.Server.Security.Exceptions;
using ScreenDox.Server.Security.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// User management API
    /// </summary>
    public class UserController : ApiController
    {
        private readonly IUserPrincipalService _userService;
        private readonly IAuthService _authService;
        private readonly ISecurityLogService _securityLogService;
        private readonly ILog _logger = LogManager.GetLogger<ProfileController>();

        /// <summary>
        /// Create controller
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="securityLogService"></param>
        /// <param name="authService"></param>
        public UserController(IUserPrincipalService userService,
                              ISecurityLogService securityLogService,
                              IAuthService authService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _securityLogService = securityLogService ?? throw new ArgumentNullException(nameof(securityLogService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        /// <summary>
        /// Get all roles (w/o Super Administrator)
        /// </summary>
        [HttpCache(DefaultExpirySeconds = 3600)] // cache 1 hour
        [HttpGet]
        [Route("api/user/roles")]
        public List<string> GetRoles()
        {
            return _userService.GetAllRoles();
        }

        // GET: api/user
        /// <summary>
        /// Get user list. Available only for Administrators.
        /// Branch Location filter is available only for Super Administrators.
        /// </summary>
        /// <param name="filter"> Filter
        /// Order by options:
        ///     - FirstName ASC (default)
        ///     - LastName
        ///     - Username
        ///     - BranchLocationName
        /// </param>
        /// <returns>User accounts</returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpPost]
        [Route("api/user/search")]
        public SearchResponse<UserSearchItemResponse> GetAll([FromBody] UserSearchRequest filter)
        {
            var result = new SearchResponse<UserSearchItemResponse>();

            filter = filter ?? new UserSearchRequest();

            Guard.ValidateOrderByClause(filter, new[] { "FirstName", "LastName", "Username", "BranchLocationName", "RoleName" });


            var currentPrincipal = _userService.GetPrincpal();

            currentPrincipal.ShouldNotBeNull();

            if (currentPrincipal.RoleName != UserRoles.SuperAdministrator)
            {
                filter.BranchLocationId = currentPrincipal.BranchLocationID ?? 0;
            }

            var items = _userService.GetAll(filter.BranchLocationId, filter.OrderBy);

            result.TotalCount = items.Count;

            // apply paging on API side
            result.Items = filter.FilterItems(items).Select(x => Mapper.Map<UserSearchItemResponse>(x))
                .ToList();

            return result;
        }

        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpGet]
        [Route("api/user/{userID}")]
        public UserPrincipalResponse Get([FromUri] int userId)
        {

            var currentPrincipal = _userService.GetPrincpal();
            var dbModel = _authService.GetUser(userId);

            dbModel.ShouldNotBeNull();

            // check additional permissions on role and Branch Location
            if (currentPrincipal.BranchLocationID != dbModel.BranchLocationID &&
                !currentPrincipal.IsUserInRole(UserRoles.SuperAdministrator))
            {
                ResponseDataFactory.Unauthorized("You don't have permissions to manage users from another Branch Location.");
            }

            var response = Mapper.Map<UserPrincipalResponse>(dbModel);

            response.IsMustChangePassword = _authService.IsPasswordExpired(dbModel);
            response.IsMustSetupSecurityQuestion = _authService.IsMustSetupPasswordQuestion(dbModel);

            return response;
        }

        /// <summary>
        /// Create new user account in specific branch location
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpPost]
        [Route("api/user")]
        public UserPrincipalResponse Add([FromBody] NewUserPrincipalRequest request)
        {
            ValidateNewRequest(request);
            var currentPrincipal = _userService.GetPrincpal();


            var model = Mapper.Map<UserPrincipalAccount>(request);

            // check additional permissions on role and Branch Location
            if (currentPrincipal.BranchLocationID != model.BranchLocationID &&
                !currentPrincipal.IsUserInRole(UserRoles.SuperAdministrator))
            {
                ResponseDataFactory.Unauthorized("You don't have permissions to manage users from another Branch Location.");
            }


            try
            {
                _authService.AddUser(model);

                _securityLogService.Add(new SecurityLog(SecurityEvents.NewUserCreated,
                       String.Format("{0}~{1}", model.UserID, model.UserName)));
            }
            catch (NonValidEntityException ex)
            {
                _logger.WarnFormat("[User] Validation error while creation. Message: {0}.", ex, ex.Message);

                ResponseDataFactory.ThrowBadRequestMessage(new[] { ex.Message });
            }

            return Get(model.UserID);
        }

        /// <summary>
        /// Update existing user account in specific branch location
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpPut]
        [Route("api/user/{userId}")]
        public IHttpActionResult Update([FromUri] int userId, [FromBody] UserPrincipalRequest request)
        {
            ValidateUpdateRequest(request);
            var currentPrincipal = _userService.GetPrincpal();
            var dbModel = _authService.GetUser(userId);

            var model = Mapper.Map<UserPrincipalRequest, UserPrincipalAccount>(request, dbModel);

            // check additional permissions on role and Branch Location
            if (currentPrincipal.BranchLocationID != model.BranchLocationID &&
                !currentPrincipal.IsUserInRole(UserRoles.SuperAdministrator))
            {
                ResponseDataFactory.Unauthorized("You don't have permissions to manage users from another Branch Location.");
            }


            _authService.UpdateUser(model);

            return Ok(CustomMessage.GetUpdateMessage("User"));
        }

        /// <summary>
        /// Delete existing iser record
        /// </summary>
        /// <returns>Status 200 if delete was successful.</returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpDelete]
        [Route("api/user/{userId}")]
        public IHttpActionResult Delete([FromUri] int userId)
        {
            userId.ShouldNotBeDefault();
            var currentPrincipal = _userService.GetPrincpal();

            // check resource exists
            var dbModel = _authService.GetUser(userId);
            dbModel.ShouldNotBeNull();

            // check additional permissions on role and Branch Location
            if (currentPrincipal.BranchLocationID != dbModel.BranchLocationID &&
                !currentPrincipal.IsUserInRole(UserRoles.SuperAdministrator))
            {
                ResponseDataFactory.Unauthorized("You don't have permissions to manage users from another Branch Location.");
            }

            if (dbModel.UserName == "su")
            {
                ResponseDataFactory.ThrowInvalidOperationError("Cannot delete Built-In users");
            }

            // apply changes
            _authService.Delete(userId);

            return Ok(CustomMessage.GetDeleteMessage("User"));
        }


        /// <summary>
        /// Block user account
        /// </summary>
        /// <returns>Status 200 if operation was successful.</returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpPost]
        [Route("api/user/block/{userId}")]
        public IHttpActionResult Block([FromUri] int userId)
        {
            userId.ShouldNotBeDefault();

            ChangeLockStatus(true, userId);

            return Ok("User has been blocked successfully");
        }

        /// <summary>
        /// Unlock user account
        /// </summary>
        /// <returns>Status 200 if operation was successful.</returns>
        [RestrictAccessToRoles(UserRoles.SuperAdministrator, UserRoles.BranchAdministrator)]
        [HttpPost]
        [Route("api/user/unblock/{userId}")]
        public IHttpActionResult Unlock([FromUri] int userId)
        {
            userId.ShouldNotBeDefault();

            ChangeLockStatus(false, userId);

            return Ok("User has been unblocked successfully.");
        }

        private void ChangeLockStatus(bool lockAccount, int userId)
        {
            var currentPrincipal = _userService.GetPrincpal();

            // check resource exists
            var dbModel = _authService.GetUser(userId);
            dbModel.ShouldNotBeNull();

            // check additional permissions on role and Branch Location
            if (currentPrincipal.BranchLocationID != dbModel.BranchLocationID &&
                !currentPrincipal.IsUserInRole(UserRoles.SuperAdministrator))
            {
                ResponseDataFactory.Unauthorized("You don't have permissions to manage users from another Branch Location.");
            }

            // apply changes
            _authService.BlockUser(userId, lockAccount);

        }

        protected void ValidateNewRequest(NewUserPrincipalRequest request)
        {
            var result = new NewUserPrincipalRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }

        protected void ValidateUpdateRequest(UserPrincipalRequest request)
        {
            var result = new UserPrincipalRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }
    }
}
