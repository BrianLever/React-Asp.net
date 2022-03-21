
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScreenDox.Server.Security.Exceptions
{
    public class DuplicatePasswordException : Exception
    {
        public DuplicatePasswordException() { }

        public DuplicatePasswordException(string errorMessage)
            : base(errorMessage)
        { }
    }
}
