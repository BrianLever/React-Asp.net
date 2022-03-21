namespace ScreenDox.Server.Security.Exceptions
{
    using System;
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException() : base("User account is disabled")
        {
        }
    }
}
