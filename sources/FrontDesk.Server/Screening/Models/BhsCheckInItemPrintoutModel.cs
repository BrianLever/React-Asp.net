using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsCheckInItemPrintoutModel: IPatientId
    {
        public string PatientName { get; set; }
        public DateTime Birthday { get; set; }

        public List<PatientCheckInViewModel> ReportItems { get; set; }


        //readonly dynamic properties
        public DateTimeOffset LastCreatedDate
        {
            get
            {
                return ReportItems.Max(x => x.CreatedDate);
            }
        }


        public BhsCheckInItemPrintoutModel()
        {
            ReportItems = new List<PatientCheckInViewModel>();
        }

        public override int GetHashCode()
        {
            return this.GetUniquePatientKey().GetHashCode();
        }

    }
}