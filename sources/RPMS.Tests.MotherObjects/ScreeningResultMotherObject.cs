using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FrontDesk;

namespace RPMS.Tests.MotherObjects
{
    public class ScreeningResultMotherObject
    {



        public static ScreeningResult GetMaryTest()
        {
            return new ScreeningResult
            {

                ID = 1664,
                LastName = "TEST",
                FirstName = "MARY",
                MiddleName = "",
                Birthday = new DateTime(1950, 01, 01),
                StreetAddress = "123 TEST ROAD",
                Phone = "(847) 490-6869",
                StateID = "NV",
                ZipCode = "89436",
                City = "SCHAUMBURG"

            };
        }
    }
}
