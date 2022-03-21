namespace ScreenDox.Server.Security.Exceptions
{
    using System;

    public class TokenNotFoundException : Exception
    {
        public TokenNotFoundException() : base("Refresh token not found or expired.")
        {
        }
    }
}
