using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk;

namespace FrontdDeskKiosk.Tests.MotherObjects
{
    public static class ScreeningResultMotherObject
    {
        public static ScreeningResult GetPatientNameAndBirthday()
        {
            return new ScreeningResult
            {
                FirstName = "ADAM",
                MiddleName = "JR.",
                LastName = "Doe",
                Birthday = new DateTime(1980, 04, 24)
            };
        }
    }
}
