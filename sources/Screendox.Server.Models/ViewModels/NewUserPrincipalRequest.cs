using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Create new user account request
    /// </summary>
    public class NewUserPrincipalRequest : UserPrincipalRequest
    {
        public string UserName { get; set; }
        /// <summary>
        /// New password, that need to be changed during the first login
        /// </summary>
        public string Password { get; set; }
    }
}
