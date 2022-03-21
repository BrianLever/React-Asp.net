namespace ScreenDox.Server.Security.Exceptions
{
    using System;

    public class SetupUserPasswordException : Exception
    {
        public SetupUserPasswordException(string message) : base(message)
        {
        }
    }
}
