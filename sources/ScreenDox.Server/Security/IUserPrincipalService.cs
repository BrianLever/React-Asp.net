
using ScreenDox.Server.Models;

using System.Collections.Generic;

namespace ScreenDox.Server.Security
{
    public interface IUserPrincipalService
    {
        IUserPrincipal GetPrincpal();
        UserPrincipal GetCurrent();

        UserPrincipal Get(int id);
        /// <summary>
        /// Get user id as identity for automatic exports
        /// </summary>
        /// <returns></returns>
        int GetExportSystemUserID();
        void Update(UserPrincipal dbModel);
        /// <summary>
        /// Get user list
        /// </summary>
        /// <param name="branchLocationId">Optional filter</param>
        /// <returns></returns>
        List<UserPrincipal> GetAll(int? branchLocationId, string orderBy);
        List<string> GetAllRoles();
        /// <summary>
        /// Get User by user name
        /// </summary>
        UserPrincipal GetUserByName(string userName);
    }
}