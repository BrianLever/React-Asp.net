using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Membership
{
    public class DuplicatePasswordException: Exception
    {
        public DuplicatePasswordException() { }

        public DuplicatePasswordException(string errorMessage)
            : base(errorMessage)
        { }
    }
}
