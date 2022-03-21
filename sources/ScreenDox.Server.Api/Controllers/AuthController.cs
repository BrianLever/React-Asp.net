using Common.Logging;

using ScreenDox.Server.Api.Infrastructure;
using ScreenDox.Server.Api.Models.Security;
using ScreenDox.Server.Api.Security.Models;
using ScreenDox.Server.Api.Security.Models.Validation;
using ScreenDox.Server.Models.Security;
using ScreenDox.Server.Models.ViewModels;
using ScreenDox.Server.Models.ViewModels.Validators;
using ScreenDox.Server.Security.Exceptions;
using ScreenDox.Server.Security.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Web.Api.Infrastructure.Auth;

namespace ScreenDox.Server.Api.Controllers
{
    /// <summary>
    /// Api for managing authentication JWT tockens
    /// </summary>
    public class AuthController : ApiController
    {
        private readonly IAuthService _authService;
        private readonly ILog _logger = LogManager.GetLogger<AuthController>();


        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));

        }

        [AllowAnonymous]
        [HttpPost()]
        [Route("api/auth/authorize")]
        public LoginResponse Login([FromBody] LoginRequest request)
        {
            LoginResponse response = null;

            ValidateRequest(request);

            if (_authService.ValidateUser(request.Username, request.Password))
            {
                response = _authService.CreateLoginToken(request.Username, GetCallerApiAddress());
            }
            else
            {
                ResponseDataFactory.Unauthorized("Invalid username or password.");
            }

            return response;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/auth/refreshtoken")]
        public ExchangeRefreshTokenResponse RefreshToken([FromBody] ExchangeRefreshTokenRequest request)
        {
            ExchangeRefreshTokenResponse response = null;

            ValidateRequest(request);
            try
            {
                response = _authService.RefreshToken(request.RefreshToken, GetCallerApiAddress());
            }
            catch(TokenNotFoundException)
            {
                ResponseDataFactory.ThrowBadRequestMessage(new string[] { "Refresh token is invalid or expired." });
            }
            catch (AccessDeniedException)
            {
                ResponseDataFactory.Unauthorized("Access denied. Please contact system administrator." );
            }

            response.ShouldNotBeNull("Invalid access or refresh token.");

            return response;
        }


        [HttpPost]
        [Route("api/auth/logout")]
        public IHttpActionResult Logout([FromBody] ExchangeRefreshTokenRequest request)
        {
            // expire refresh token
            ValidateRequest(request);

            try
            {
                _authService.RevokeRefreshToken(request.RefreshToken, GetCallerApiAddress());
            }
            catch (TokenNotFoundException)
            {
                _logger.Warn("[Auth][Logout] Refresh token is invalid or expired.");
                return NotFound();
            }

            return Ok();
        }


        /// <summary>
        /// Reset password - step 1. Get security question by username
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("api/auth/resetpassword/{username}")]
        public IHttpActionResult GetSecurityQuestion([FromUri]string username)
        {
            username.ShouldNotBeNull();

            var user = GetValidUserAccount(username);

            var result = new ResetPasswordSecurityQuestion
            {
                SecurityQuestion = user.PasswordQuestion
            };

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/auth/resetpassword/{username}")]
        public IHttpActionResult ResetPassword([FromUri] string username, [FromBody] UserResetPasswordRequest request)
        {
            request.ShouldNotBeNull();

            Validate(request);

            var user = GetValidUserAccount(username);
            try
            {
                _authService.ResetPassword(user, request.SecurityQuestionAnswer, request.NewPassword);
            }
            catch(InvalidPasswordException ex)
            {
                ResponseDataFactory.ThrowBadRequestMessage(ex);
            }
            catch (DuplicatePasswordException ex)
            {
                _logger.Info(ex);

                ResponseDataFactory.ThrowBadRequestMessage(new[] { Resources.TextMessages.Membership_DuplicatePassword });
            }

            return Ok();
        }

        private UserPrincipalAccount GetValidUserAccount(string username)
        {
            var user = _authService.GetActiveUser(username);

            user.ShouldNotBeNull();

            if (_authService.IsMustSetupPasswordQuestion(user))
            {
                // security question was not set.
                ResponseDataFactory.ThrowBadRequestMessage(new string[] { Resources.TextMessages.Membership_ResetPassword_SecurityQuestionNotSet });
            }

            return user;
        }

        private void ValidateRequest(LoginRequest loginRequest)
        {
            loginRequest.ShouldNotBeNull();

            var validator = new LoginRequestValidator();

            var result = validator.Validate(loginRequest);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }

        private void ValidateRequest(ExchangeRefreshTokenRequest request)
        {
            request.ShouldNotBeNull();

            var validator = new ExchangeRefreshTokenRequestValidator();

            var result = validator.Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }
        protected void Validate(UserResetPasswordRequest request)
        {

            var result = new UserResetPasswordRequestValidator().Validate(request);

            if (result.IsValid) return;

            ResponseDataFactory.ThrowInvalidModelMessage(result.Errors);
        }

        private string GetCallerApiAddress()
        {
            var hostname = HttpContext.Current.Request.UserHostAddress;
            IPAddress ipAddress;
            if (IPAddress.TryParse(hostname, out ipAddress))
            {
                return ipAddress.ToString();
            }

            return string.Empty;
        }
    }
}