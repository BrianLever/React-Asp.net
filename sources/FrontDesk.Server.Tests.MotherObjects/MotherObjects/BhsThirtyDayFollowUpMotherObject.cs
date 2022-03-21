using FrontDesk.Common.Bhservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.MotherObjects
{
    public static class BhsThirtyDayFollowUpMotherObject
    {
        public static BhsFollowUp GetDefault()
        {
            return new BhsFollowUp
            {
                ID = 1,
                BhsVisitID = 1,
                Visit = BhsVisitMotherObject.GetDefault(),
                ScreeningResultID = 1001,
                CreatedDate = DateTimeOffset.UtcNow.AddHours(-1)
            };
        }
    }
}
