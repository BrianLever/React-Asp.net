using System;

namespace ScreenDox.Server.Models.ViewModels
{
    public class ColumbiaSuicideCreateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public DateTime Birthday { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }
        public string StateID { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }

        public int? EhrPatientID { get; set; }
        public string EhrPatientHRN { get; set; }

        public int BranchLocationID { get; set; }
  
        public long? ScreeningResultID { get; set; }
        public long? BhsVisitID { get; set; }

        /// <summary>
        /// Read only reference to the Demographics ID
        /// </summary>
        public long? DemographicsID { get; set; }

    }
}
