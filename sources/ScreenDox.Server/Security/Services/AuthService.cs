using Common.Logging;

using FrontDesk.Common.Configuration;
using FrontDesk.Common.Entity;
using FrontDesk.Common.Extensions;
using FrontDesk.Common.InfrastructureServices;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Data;

using ScreenDox.Server.Api.Security.Models;
using ScreenDox.Server.Data;
using ScreenDox.Server.Models;
using ScreenDox.Server.Models.Security;
using ScreenDox.Server.Security.Exceptions;

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Configuration;

using Web.Api.Infrastructure.Auth;

namespace ScreenDox.Server.Security.Services
{
    public interface IAuthService
    {
        bool ValidateUser(string username, string password);
        LoginResponse CreateLoginToken(string userName, string ipAddress);
        ExchangeRefreshTokenResponse RefreshToken(string refreshToken, string ipAddress);
        void RevokeRefreshToken(string refreshToken, string ipAddress);
        void ChangePassword(int userId, string newPassword);

        UserPrincipalAccount GetUser(int userId);
        UserPrincipalAccount GetActiveUser(string username);
        bool IsPasswordExpired(UserPrincipalAccount user);
        bool IsMustSetupPasswordQuestion(UserPrincipalAccount user);
        bool ChangeSecurityQuestionAndAnswer(int userId, string password, string newQuestion, string newAnswer);
        bool ChangeSecurityQuestionAndAnswer(int userId, string password, string newPassword, string newQuestion, string newAnswer);
        List<string> GetSecurityQuestions();
        void ResetPassword(IUserPrincipal principal, string securityQuestionAnswer, string newPassword);
        int AddUser(UserPrincipalAccount model);
        void UpdateUser(UserPrincipalAccount model);
        void Delete(int userId);
        void BlockUser(int userId, bool lockAccount);
    }

    public class AuthService : IAuthService
    {
        private readonly IUserPrincipalRepository _repository;
        private readonly IUserRefreshTokenRepository _refreshTokenRepository;
        private readonly ISecurityQuestionRepository _securityQuestionRepository;


        private readonly IJwtFactory _jwtFactory;
        private readonly ITokenFactory _tokenFactory;

        private readonly ILog _logger = LogManager.GetLogger<AuthService>();
        private readonly ITimeService _timeService;


        private int MaxInvalidPasswordAttempts;

        public int PasswordAttemptWindow { get; set; }

        public string PasswordStrengthRegularExpression { get; set; }
        public string PasswordStrengthErrorMessage { get; set; }

        private MachineKeySection MachineKey;

        public AuthService(IUserPrincipalRepository repository,
                           IUserRefreshTokenRepository refreshTokenRepository,
                           ISecurityQuestionRepository securityQuestionRepository,
                           IJwtFactory jwtFactory,
                           ITokenFactory tokenFactory,
                           ITimeService timeService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));
            _jwtFactory = jwtFactory ?? throw new ArgumentNullException(nameof(jwtFactory));
            _tokenFactory = tokenFactory ?? throw new ArgumentNullException(nameof(tokenFactory));
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));

            Initialize();
            _securityQuestionRepository = securityQuestionRepository ?? throw new ArgumentNullException(nameof(securityQuestionRepository));
        }

        private string GetConfigValue(string parameterName, string defaultValue)
        {
            return AppSettingsProxy.GetStringValue("security:" + parameterName, defaultValue);
        }

        private void Initialize()
        {
            this.MaxInvalidPasswordAttempts = GetConfigValue("maxInvalidPasswordAttempts", "5").As<int>();
            this.PasswordAttemptWindow = GetConfigValue("passwordAttemptWindow", "10").As<int>();

            this.PasswordStrengthRegularExpression = GetConfigValue("passwordStrengthRegularExpression", @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$");
            this.PasswordStrengthErrorMessage = GetConfigValue("passwordStrengthErrorMessage", "Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters");


            // Get encryption and decryption key information from the configuration.

            // Get the Web application configuration object.
            this.MachineKey = (MachineKeySection)WebConfigurationManager.GetSection("system.web/machineKey");
        }

        /// <summary>
        /// Validate user's identity
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool ValidateUser(string username, string password)
        {
            bool isValid = false;
            try
            {
                var user = _repository.GetActiveUserByName(username);

                if (user == null)
                {
                    return false; // user not found
                }

                string dbPwd = user.Password;


                if (CheckPassword(password, dbPwd))
                {
                    isValid = true;

                    _repository.UpdateLastLoginDate(user.UserID, _timeService.GetLocalNow());
                }
                else
                {

                    if (!username.Equals("su"))
                    {
                        UpdateFailureCount(user, "password");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Auth Service] Failed to login user.", ex);

                throw;
            }

            return isValid;
        }

        //
        // CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        //

        private bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            pass1 = EncodePassword(password);

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }

        //
        // EncodePassword
        //   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //

        private string EncodePassword(string password)
        {
            string encodedPassword = password;
            if (encodedPassword == null)
            {
                return null;
            }

            var hash = CreateHmacHashProvider();
            hash.Key = HexToByte(MachineKey.ValidationKey);

            encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));

            return encodedPassword;
        }

        private HMAC CreateHmacHashProvider()
        {
            if (string.Compare(MachineKey.ValidationAlgorithm, "HMACSHA256", true) == 0)
            {
                return new HMACSHA256();
            }
            else if (string.Compare(MachineKey.ValidationAlgorithm, "HMACSHA384", true) == 0)
            {
                return new HMACSHA384();
            }
            else
            {
                return new HMACSHA1();
            }
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array. Used to convert encryption key values from the configuration.
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        protected void UpdateFailureCount(UserPrincipalAccount user, string failureType)
        {

            DateTime windowStart = _timeService.GetLocalNow(); // local time is used for backward compatibility with legacy system
            int failureCount = 0;

            try
            {


                if (failureType == "password")
                {
                    failureCount = user.FailedPasswordAttemptCount;
                    windowStart = user.FailedPasswordAttemptWindowStart;
                }

                if (failureType == "passwordAnswer")
                {
                    failureCount = user.FailedPasswordAnswerAttemptCount;
                    windowStart = user.FailedPasswordAnswerAttemptWindowStart;
                }


                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow. 
                    // Start a new password failure count from 1 and a new window starting now.

                    failureCount = 1;
                    var failureWindowStart = _timeService.GetLocalNow();

                    if (failureType == "password")
                    {
                        _repository.UpdatePasswordFailure(user.UserID, failureCount, failureWindowStart);

                    }
                    else if (failureType == "passwordAnswer")
                    {
                        _repository.UpdatePasswordAnswerFailure(user.UserID, failureCount, failureWindowStart);
                    }
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out
                        // the user.
                        _repository.Lock(user.UserID, _timeService.GetLocalNow());
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.

                        if (failureType == "password")
                        {
                            _repository.UpdatePasswordFailure(user.UserID,
                                                              failureCount,
                                                              user.FailedPasswordAttemptWindowStart);

                        }
                        else if (failureType == "passwordAnswer")
                        {
                            _repository.UpdatePasswordAnswerFailure(user.UserID,
                                                                    failureCount,
                                                                    user.FailedPasswordAnswerAttemptWindowStart);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Auth Service] Failed to update failure count.", ex);

                throw;
            }

        }
        /// <summary>
        /// Generate Login JWT token
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public LoginResponse CreateLoginToken(string userName, string ipAddress)
        {
            var user = _repository.GetActiveUserByName(userName);

            var refreshToken = CreateRefreshToken(user.UserID, ipAddress);

            var response = new LoginResponse(
                _jwtFactory.GenerateEncodedToken(user),
                refreshToken.Token
                );

            _logger.Warn($"[Auth] JWT Token created. Token: {response.AccessToken}. User: {userName}");

            return response;

        }
        private RefreshToken CreateRefreshToken(int userID, string ipAddress)
        {
            // generate token that is valid for 2 hours
            var refreshToken = new RefreshToken
            {
                UserID = userID,
                Token = _tokenFactory.GenerateToken(),
                Expires = _timeService.GetUtcNow().AddHours(_jwtFactory.ValidFor.TotalHours * 2),
                Created = _timeService.GetUtcNow(),
                CreatedByIp = ipAddress
            };

            _refreshTokenRepository.AddRefreshToken(refreshToken);

            _logger.Warn($"[Auth] Refresh token is created. Token: {refreshToken}");


            return refreshToken;
        }

        public ExchangeRefreshTokenResponse RefreshToken(string refreshToken, string ipAddress)
        {
            ExchangeRefreshTokenResponse response = null;

            var token = GetActiveRefreshToken(refreshToken);

            if (token == null)
            {
                throw new TokenNotFoundException();
            }

            var user = _repository.GetUserByID(token.UserID);

            if (user == null)
            {
                throw new TokenNotFoundException();
            }
            // check user is active
            if (user.IsBlock)
            {
                // user is blocked - revoke the token and return the messsage

                _logger.Warn($"[Auth] Blocked user tries to refresh the token. Username: {user.UserName}. Token: {refreshToken}");

                var revokeReason = "Revoked without replacement. Account is blocked.";
                _refreshTokenRepository.RevokeUserTokens(user.UserID, _timeService.GetUtcNow(), ipAddress, revokeReason);

                throw new AccessDeniedException();
            }

            // create new login token and revoke refresh token
            var exchangedRefreshToken = CreateRefreshToken(user.UserID, ipAddress);

            response = new ExchangeRefreshTokenResponse(
                _jwtFactory.GenerateEncodedToken(user),
                exchangedRefreshToken.Token
                );

            // revoke used refresh token token
            token.ReplacedByToken = exchangedRefreshToken.Token;
            token.Revoked = _timeService.GetUtcNow();
            token.RevokedByIp = ipAddress;
            token.ReasonRevoked = "Replaced by new token.";

            _refreshTokenRepository.Revoke(token);

            _logger.Warn($"[Auth] Refresh token is revoked. Username: {user.UserName}. Token: {refreshToken}");

            return response;
        }

        private RefreshToken GetActiveRefreshToken(string refreshToken)
        {
            RefreshToken token = _refreshTokenRepository.GetToken(refreshToken);

            if (token == null)
            {
                _logger.Warn($"[Auth] Refresh token not found. Token: {refreshToken}");

                return null;
            }

            if (token.IsRevoked)
            {
                _logger.Warn($"[Auth] Caller tries to use revoked refresh token. Token: {refreshToken}");
                return null;
            }

            if (!token.IsActive)
            {
                _logger.Warn($"[Auth] Token is expired. Token: {refreshToken}. Created date (UTC): {token.Created.FormatAsDateWithTime()}. Expires (UTC): {token.Expires.FormatAsDateWithTime()}. Current time (UTC): {_timeService.GetUtcNow()}");
                return null;
            }

            return token;
        }

        public void RevokeRefreshToken(string refreshToken, string ipAddress)
        {
            var token = GetActiveRefreshToken(refreshToken);

            if (token == null)
            {
                throw new TokenNotFoundException();
            }

            token.Revoked = _timeService.GetUtcNow();
            token.RevokedByIp = ipAddress;
            token.ReplacedByToken = null;
            token.ReasonRevoked = "Sign Out action called.";

            _refreshTokenRepository.Revoke(token);
        }

        public void ChangePassword(int userId, string newPassword)
        {
            var hashedPassword = EncodePassword(newPassword);

            try
            {
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();

                // validate password history
                if (!_repository.ValidateNewPasswordInHistory(userId, hashedPassword))
                {
                    throw new DuplicatePasswordException();
                }

                _repository.ChangePassword(userId, hashedPassword, _timeService.GetUtcNow());


                _repository.StopConnectionSharing();
                _repository.CommitTransaction();
            }
            catch (Exception ex)
            {
                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();

                _logger.Error("Change password failed.", ex);
                throw;
            }
            finally
            {
                _repository.Disconnect();
            }
        }



        public UserPrincipalAccount GetUser(int userId)
        {
            return _repository.GetByID(userId);
        }

        public UserPrincipalAccount GetActiveUser(string username)
        {
            if (string.IsNullOrEmpty(username)) return null;

            return _repository.GetActiveUserByName(username);
        }
        /// <summary>
        /// Returns true if user must change the password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsPasswordExpired(UserPrincipalAccount user)
        {

            // if password has never been changed use account creation date 
            // as start period of account's living
            // else last password changed date

            DateTime start = !user.LastPasswordChangedDate.HasValue || user.LastPasswordChangedDate.Value == DateTime.MinValue ?
                user.CreationDate : user.LastPasswordChangedDate.Value;

            return _timeService.GetLocalNow().Subtract(start).TotalDays > AppSettings.PasswordRenewalPeriodDays;
        }

        /// <summary>
        /// Returns true if user must change the password
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool IsMustSetupPasswordQuestion(UserPrincipalAccount user)
        {

            return string.IsNullOrEmpty(user.PasswordQuestion);
        }

        public List<string> GetSecurityQuestions()
        {
            return _securityQuestionRepository.GetQuestions();
        }

        public bool ChangeSecurityQuestionAndAnswer(int userId, string password, string newQuestion, string newAnswer)
        {
            return ChangeSecurityQuestionAndAnswer(userId, password, null, newQuestion, newAnswer);
        }


        public bool ChangeSecurityQuestionAndAnswer(int userId, string password, string newPassword, string newQuestion, string newAnswer)
        {
            var user = GetUser(userId);
            if (user == null) return false;

            if (ValidateUser(user.UserName, password))
            {
                if (!string.IsNullOrEmpty(newPassword))
                {
                    ChangePassword(userId, newPassword);
                }

                return _repository.UpdateSecurityQuestion(user.UserID, newQuestion, EncodePassword(newAnswer));
            }
            return false;
        }

        public bool ValidateSecurityQuestionAndAnswer(IUserPrincipal principal, string answer)
        {
            if (principal is null)
            {
                throw new ArgumentNullException(nameof(principal));
            }

            bool isValid = false;


            try
            {
                var user = _repository.GetActiveUserByName(principal.UserName);

                if (user == null)
                {
                    return false; // user not found
                }

                string dbPwd = user.PasswordAnswer;


                if (CheckPassword(answer, dbPwd))
                {
                    isValid = true;
                }
                else
                {
                    UpdateFailureCount(user, "passwordAnswer");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("[Auth Service] Failed to validate user's security question answer.", ex);

                throw;
            }

            return isValid;
        }

        public void ResetPassword(IUserPrincipal principal, string securityQuestionAnswer, string newPassword)
        {
            if (!ValidateSecurityQuestionAndAnswer(principal, securityQuestionAnswer))
            {
                throw new InvalidPasswordException("You have entered invalid security answer.");
            }

            ChangePassword(principal.UserID, newPassword);
        }

        public int AddUser(UserPrincipalAccount model)
        {

            string errrorMessage;

            CreateUserStatus createUserStatus = CreateUser(model);

            if (createUserStatus != CreateUserStatus.Success)
            {
                switch (createUserStatus)
                {
                    case CreateUserStatus.DuplicateUserName:
                        errrorMessage = "User name is already in use. Please enter a different user name";
                        break;
                    case CreateUserStatus.InvalidPassword:
                        errrorMessage = "Please enter a valid password";
                        break;
                    default:
                        errrorMessage = "Sorry, but user account was not created. Please try again";
                        break;
                }

                throw new NonValidEntityException(errrorMessage);
            }

            return model.UserID;
        }

        private CreateUserStatus CreateUser(UserPrincipalAccount user)
        {

            ValidatePassword(user.Password);

            CreateUserStatus status;


            if (_repository.GetUserIDByName(user.UserName) > 0)
            {
                return CreateUserStatus.DuplicateUserName;
            }


            user.CreationDate = _timeService.GetLocalNow();

            // encrypt credential
            user.Password = EncodePassword(user.Password);
            user.PasswordAnswer = EncodePassword(user.PasswordAnswer);

            try
            {
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();

                _repository.AddUser(user);
                _repository.AddDetails(user);
                _repository.AddUserToRole(user.UserName, user.RoleName);
                _repository.UpdateBranchLocation(user.UserID, user.BranchLocationID);

                _repository.StopConnectionSharing();
                _repository.CommitTransaction();

                status = CreateUserStatus.Success;
            }
            catch (Exception ex)
            {
                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();

                _logger.Error("Failed to create a new user.", ex);

                status = CreateUserStatus.Error;
            }

            return status;
        }

        /// <summary>
        /// Throw NonValidEntityException if not match regular expression
        /// </summary>
        /// <param name="password"></param>
        private void ValidatePassword(string password)
        {
            Regex re = new Regex(PasswordStrengthRegularExpression);

            if (!re.IsMatch(password))
            {
                throw new NonValidEntityException(PasswordStrengthErrorMessage);
            }
        }

        public void UpdateUser(UserPrincipalAccount user)
        {
            try
            {
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();

                _repository.UpdateUser(user);
                _repository.UpdateDetails(user);



                if (!String.IsNullOrEmpty(user.RoleName))
                {
                    try
                    {
                        if (_repository.IsUserInRole(user.UserName))
                        {
                            //Update user to role
                            _repository.UpdateUserRole(user.UserName, user.RoleName);
                        }
                        else
                        {
                            //Add user to role
                            _repository.AddUserToRole(user.UserName, user.RoleName);
                        }
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Failed to update user role. Error has occurred.");
                    }
                }
                if (user.BranchLocationID.HasValue)
                {
                    try
                    {
                        if (_repository.IsUserInBranchLocation(user.UserID))
                        {
                            //Update user to branch location
                            _repository.UpdateBranchLocation(user.UserID, user.BranchLocationID);
                        }
                        else
                        {
                            //Add user to branch location
                            _repository.SetBranchLocation(user.UserID, user.BranchLocationID);
                        }

                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Failed to update user branch location. Error has occurred.");
                    }
                }

                _repository.StopConnectionSharing();
                _repository.CommitTransaction();

            }
            catch (Exception ex)
            {
                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();

                _logger.Error("Failed to create a new user.", ex);

                throw;
            }
        }

        public void Delete(int userId)
        {
            _repository.Delete(userId);
        }

        public void BlockUser(int userId, bool blockAccount)
        {
            _repository.Block(userId, blockAccount);
        }
    }

    public enum CreateUserStatus
    {
        Success,
        DuplicateUserName,
        Error,
        DuplicateEmail,
        InvalidPassword
    }
}
