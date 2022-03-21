using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.MotherObjects
{
    public static class UserMotherObject
    {
        public static FDUser Create()
        {
            return new FDUser
            {
                UserID = Int32.MinValue, /* prevent calling database */
                LastName = "Doe",
                FirstName = "John",
                MiddleName = "",
                Email = "jdow.@sample.com",
                BranchLocationID = 1001,
               
            };
        }
    }
}
