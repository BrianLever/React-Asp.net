using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Models
{
    public class PatientCheckInDtoModel : PatientCheckInViewModel, IPatientId
    {
        public string PatientName { get; set; }
        public DateTime Birthday { get; set; }

        public override int GetHashCode()
        {
            return this.GetUniquePatientKey().GetHashCode();
        }

    }
}