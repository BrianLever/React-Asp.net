namespace ScreenDox.Server.Models
{
    public interface IUserPrincipal
    {
        int? BranchLocationID { get; set; }
        string Email { get; set; }
        string FirstName { get; set; }
        string FullName { get;}
        bool IsBlock { get; set; }
        string LastName { get; set; }
        string MiddleName { get; set; }
        string RoleName { get; set; }
        int UserID { get; set; }
        string UserName { get; set; }
    }


    public static class UserPrincipalExtensions
    {
        /// <summary>
        /// Check user in specific role
        /// </summary>
        /// <param name="principal"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public static bool IsUserInRole(this IUserPrincipal principal, string roleName)
        {
            return string.Compare(principal.RoleName, roleName, System.StringComparison.OrdinalIgnoreCase) == 0;
        }
    }
}