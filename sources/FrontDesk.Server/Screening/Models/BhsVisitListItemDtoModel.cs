
using ScreenDox.Server.Models;

using System;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsVisitListItemDtoModel : BhsVisitViewModel, IPatientId, IScreeningPatientIdentity
    {
        public string PatientName { get; set; }
        public DateTime Birthday { get; set; }

        public DateTimeOffset? DemographicsScreeningDate { get; set; }
        public DateTimeOffset? DemographicsCreatedDate { get; set; }
        public DateTimeOffset? DemographicsCompleteDate { get; set; }
        public Int64? DemographicsID { get; set; }
        public string FirstName { get;set; }
        public string LastName { get; set; }
        public string MiddleName { get;set; }

        public override int GetHashCode()
        {
            return this.GetUniquePatientKey().GetHashCode();
        }

    }
}