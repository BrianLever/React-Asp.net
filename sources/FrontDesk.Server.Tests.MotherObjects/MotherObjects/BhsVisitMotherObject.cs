using FrontDesk.Common.Bhservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Tests.MotherObjects
{
    public static class BhsVisitMotherObject
    {
        public static BhsVisit GetDefault()
        {
            var result = MotherObjects.ScreeningResultMotherObject.GetAllNoAnswers();
            result.ID = 1001;

            return new BhsVisit
            {
                ID = 1,
                CreatedDate = DateTimeOffset.UtcNow.AddHours(-1),
                LocationID = 1001,
                ScreeningResultID = result.ID,
                AlcoholUseFlag = new ScreeningResultValue
                {
                    ScoreLevel = 1,
                    ScoreLevelLabel = "POSITIVE"
                },
                Discharged = new Common.LookupValue
                {
                    Id = 1
                },
                Result = result,
                NewVisitDate = DateTimeOffset.Now.AddDays(3)

            };
        }
    }
}
