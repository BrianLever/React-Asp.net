
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScreenDox.Server.Security.Exceptions
{
    public class InvalidPasswordException : Exception
    {
        public InvalidPasswordException(string errorMessage)
            : base(errorMessage)
        { }
    }
}
