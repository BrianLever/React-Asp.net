using FrontDesk.Common;
using FrontDesk.Common.Bhservice;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// View model for getting demographics detals
    /// </summary>
    public class DemographicsRequest
    {
        public LookupValue Race { get; set; }
        public LookupValue Gender { get; set; }
        public LookupValue SexualOrientation { get; set; }
        public string TribalAffiliation { get; set; }
        public LookupValue MaritalStatus { get; set; }
        public LookupValue EducationLevel { get; set; }
        public LookupValue LivingOnReservation { get; set; }
        public string CountyOfResidence { get; set; }
        public List<LookupValue> MilitaryExperience { get; set; } = new List<LookupValue>();

    }
}
