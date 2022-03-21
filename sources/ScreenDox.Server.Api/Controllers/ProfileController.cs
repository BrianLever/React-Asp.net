
using AutoMapper;

using CacheCow.Server.WebApi;

using Common.Logging;

using FluentValidation;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Models.ViewModels.Validators;
using ScreenDox.Server.Security;
using ScreenDox.Server.Security.Exceptions;
using ScreenDox.Server.Security.Services;

using System;
using System.Collections.Generic;
using System.Web.Http;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Get information about currently logged in user
    /// </summary>
    [Authorize]
    public class ProfileController : ApiControllerWithValidationBase<UserPrincipal>
    {
        private readonly IUserPrincipalService _userService;
        private readonly IAuthService _authService;
        private readonly ILog _logger = LogManager.GetLogger<ProfileController>();

        /// <summary>
        /// Create controller
        /// </summary>
        /// <param name="userService"></param>
        /// <param name="authService"></param>
        public ProfileController(IUserPrincipalService userService, IAuthService authService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        // GET: api/profile
        /// <summary>
        /// Get currently logged in user profile
        /// </summary>
        /// <returns>User account information</returns>
        [HttpGet]
        [Route("api/profile")]
        public UserPrincipalResponse Get()
        {
            var currentPrincipal = _userService.GetPrincpal();

            currentPrincipal.ShouldNotBeNull();

            // get full object to validate if password or question need to be changed
            var user = _authService.GetUser(currentPrincipal.UserID);


            user.ShouldNotBeNull();

            var response = Mapper.Map<UserPrincipalResponse>(user);


            response.IsMustChangePassword = _authService.IsPasswordExpired(user);
            response.IsMustSetupSecurityQuestion = _authService.IsMustSetupPasswordQuestion(user);

            return response;
        }


        [HttpPut]
        [Route("api/profile")]
        public IHttpActionResult Update([FromBody] UserPrincipal userPrincipal)
        {
            userPrincipal.ShouldNotBeNull();

            Validate(userPrincipal);

            // get current user
            var dbModel = _userService.GetCurrent();
            dbModel.ShouldNotBeNull();

            // allow update only contact info fields

            dbModel.FirstName = userPrincipal.FirstName;
            dbModel.LastName = userPrincipal.LastName;
            dbModel.MiddleName = userPrincipal.MiddleName;
            dbModel.Email = userPrincipal.Email;
            dbModel.City = userPrincipal.City;
            dbModel.AddressLine1 = userPrincipal.AddressLine1;
            dbModel.AddressLine2 = userPrincipal.AddressLine2;
            dbModel.ContactPhone = userPrincipal.ContactPhone;
            dbModel.Comments = userPrincipal.Comments;
            dbModel.StateCode = userPrincipal.StateCode;
            dbModel.PostalCode = userPrincipal.PostalCode;

            _userService.Update(dbModel);

            dbModel = Get();

            return AcceptedContentResult(dbModel);
        }

        [AllowAnonymous]
        [HttpPut]
        [Route("api/profile/password")]
        public IHttpActionResult ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            request.ShouldNotBeNull();

            Validate(request);

            // get current user
            var dbModel = _userService.GetCurrent();

            if (!_authService.ValidateUser(dbModel.UserName, request.CurrentPassword))
            {
                // invalid password

                ResponseDataFactory.ThrowBadRequestMessage(new[] { Resources.TextMessages.Membership_InvalidCurrentPwd });

            }
            try
            {

                // check password does not have username
                var passwordClearText = request.NewPassword.ToLowerInvariant();
                var usernameLowerCase = dbModel.UserName.ToLowerInvariant();

                if (passwordClearText.Contains(usernameLowerCase))
                {
                    ResponseDataFactory.ThrowBadRequestMessage(new[] { Resources.TextMessages.Membership_Username_Included_Error });
                }

                _authService.ChangePassword(dbModel.UserID, request.NewPassword);
            }
            catch (DuplicatePasswordException ex)
            {
                _logger.Info(ex);

                ResponseDataFactory.ThrowBadRequestMessage(new[] { Resources.TextMessages.Membership_DuplicatePassword });
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                throw;
            }

            return Ok();
        }
       
        [HttpCache(DefaultExpirySeconds = 3600)] // 1 hour
        [HttpGet]
        [Route("api/profile/list/securityquestion")]
        public List<string> GetSecurityQuestions()
        {
            return _authService.GetSecurityQuestions();
        }


        [HttpPut]
        [Route("api/profile/securityquestion")]
        public IHttpActionResult ChangSecurityQuestion([FromBody] UserChangeSecurityQuestionRequest request)
        {
            request.ShouldNotBeNull();

            // validate at least security
            ValidateChangeSecurityQuestion(request);

            // get current user
            var dbModel = _userService.GetCurrent();

            if (!_authService.ValidateUser(dbModel.UserName, request.Password))
            {
                // invalid password

                ResponseDataFactory.ThrowBadRequestMessage(new[] { Resources.TextMessages.Membership_InvalidCurrentPwd });
            }

            // check if security question exists.

            var userSecurityPrincipal = _authService.GetActiveUser(dbModel.UserName);


            if(!userSecurityPrincipal.IsSecuritySetupComplete)
            {
                ValidateFirstTimeLogin(request);
            }

            if (!_authService.ChangeSecurityQuestionAndAnswer(dbModel.UserID, request.Password, request.NewPassword, request.SecurityQuestion, request.SecurityQuestionAnswer))
            {
                ResponseDataFactory.ThrowBadRequestMessage(new string[] { "Failed to change the security question." });
                
            }
            return Ok();
        }

        protected override AbstractValidator<UserPrincipal> GetValidator()
        {
            return new UserPrincipalValidator();
        }

        protected void Validate(UserChangePasswordRequest request)
        {

            var result = new UserChangePasswordRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }

        protected void ValidateChangeSecurityQuestion(UserChangeSecurityQuestionRequest request)
        {

            var result = new UserChangeSecurityQuestionRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }


        protected void ValidateFirstTimeLogin(UserChangeSecurityQuestionRequest request)
        {

            var result = new UserChangeSecurityQuestionRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }
    }
}
