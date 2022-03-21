using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// User account information
    /// </summary>
    public class UserPrincipalResponse : UserPrincipal
    {
        public bool IsMustChangePassword { get; set; } = false;
        public bool IsMustSetupSecurityQuestion { get; set; } = false;

    }
}
