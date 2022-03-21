using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Entity
{
    /// <summary>
    /// Table entity is not valis
    /// </summary>
    public class NonValidEntityException : Exception
    {
        public NonValidEntityException() : base() { }

        public NonValidEntityException(string errorMessage) : base(errorMessage) { }
    }

}
