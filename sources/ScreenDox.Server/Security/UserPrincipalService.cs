
using Common.Logging;

using FrontDesk.Common.Extensions;

using ScreenDox.Server.Api.Security;
using ScreenDox.Server.Data;
using ScreenDox.Server.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ScreenDox.Server.Security
{
    public class UserPrincipalService : IUserPrincipalService
    {
        private readonly IUserPrincipalRepository _repository;
        private readonly ILog _logger = LogManager.GetLogger<UserPrincipalService>();

        public UserPrincipalService(IUserPrincipalRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        public UserPrincipal Get(int id)
        {
            return _repository.GetUserByID(id);
        }

        public UserPrincipal GetCurrent()
        {
            var principle = GetPrincpal();

            if (principle == null) return null;

            int currentUserID = principle.UserID; // superadmin

            return Get(currentUserID);
        }

        public int GetExportSystemUserID()
        {
            return _repository.GetUserIDByName("export_service");
        }

        public void Update(UserPrincipal user)
        {

            try
            {
                _repository.BeginTransaction();
                _repository.StartConnectionSharing();

                _repository.UpdateDetails(user);

                if (!String.IsNullOrEmpty(user.RoleName))
                {
                    try
                    {
                        if (_repository.IsUserInRole(user.UserName))
                        {
                            //Update user to role
                            _repository.UpdateUserRole(user.UserName, user.RoleName.ToString());
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
                if (user.BranchLocationID != null)
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
                    catch (Exception ex)
                    {
                        _logger.Error("Failed to update user branch location.", ex);
                        throw new ApplicationException("Failed to update user branch location. Error has occurred.", ex);
                    }
                }

                // complete transation
                _repository.StopConnectionSharing();
                _repository.CommitTransaction();
            }
            catch (Exception ex)
            {
                _repository.StopConnectionSharing();
                _repository.RollbackTransaction();

                _logger.Error("Failed to update user info.", ex);
                throw;
            }
            finally
            {
                _repository.Disconnect();
            }
        }

        public IUserPrincipal GetPrincpal()
        {
            ClaimsPrincipal principal = HttpContext.Current.User as ClaimsPrincipal;
            if (principal == null || !principal.Claims.Any())
            {
                return null;

            }

            var user = new UserPrincipal();

            user.UserID = principal.FindFirstValue(JwtTokenClaims.UserId).As<int>();
            user.FullName = principal.FindFirstValue(ClaimTypes.Name) ?? string.Empty;
            user.Email = principal.FindFirstValue(ClaimTypes.Email) ?? string.Empty;
            user.BranchLocationID = principal.FindFirstValue(JwtTokenClaims.BranchLocationId).AsNullable<int>();
            user.UserName = principal.FindFirstValue(JwtTokenClaims.UniqueName);
            user.RoleName = principal.FindFirstValue(ClaimTypes.Role);

            return user;
        }

        public List<UserPrincipal> GetAll(int? branchLocationId, string orderBy)
        {
            return _repository.GetAll(branchLocationId, orderBy);
        }

        /// <summary>
        /// Get all roles withouth Super Administrator
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRoles()
        {
            return _repository.GetAllRoles();
        }

        public UserPrincipal GetUserByName(string userName)
        {
            var id = _repository.GetUserIDByName(userName);

            if(id > 0)
            {
                return Get(id);
            }
            return null;
        }
    }
}
