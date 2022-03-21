using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Bhservice.Export
{
    public class BhsVisitExtendedWithIdentity : BhsVisit, IScreeningPatientIdentity
    {
        public DateTime Birthday { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public string FullName { get; set; }

        public string ExportedToHRN { get; set; }

        public long DemographicsID { get; set; }

        public string LocationName { get; set; }
    }
}
