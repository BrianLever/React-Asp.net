using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace FrontDesk.Common.Bhservice
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    public class BhsDemographics : ScreeningPatientIdentityWithAddress, IBhsEntry
    {
        public long ID { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset ScreeningDate { get; set; }
        public string BhsStaffNameCompleted { get; set; }
        public DateTimeOffset? CompleteDate { get; set; }
        public long? ScreeningResultID { get; set; }
        
        //improve the filter performance by location
        public int LocationID { get; set; }
        public LookupValue Race { get; set; }
        public LookupValue Gender { get; set; }
        public LookupValue SexualOrientation { get; set; }
        public string TribalAffiliation { get; set; }
        public LookupValue MaritalStatus { get; set; }
        public LookupValue EducationLevel { get; set; }
        public LookupValue LivingOnReservation { get; set; }
        public string CountyOfResidence { get; set; }
        public List<LookupValue> MilitaryExperience { get; set; } = new List<LookupValue>();
        public string LocationLabel { get; set; }


        public ScreeningResult ToScreeningResultModel()
        {
          return new ScreeningResult
          {
              ID = this.ID,
              LocationID = this.LocationID,
              LocationLabel = this.LocationLabel,
              Birthday = this.Birthday,
              LastName = this.LastName,
              FirstName = this.FirstName,
              MiddleName = this.MiddleName,
              StreetAddress = this.StreetAddress,
              City = this.City,
              ZipCode = this.ZipCode,
              StateID = this.StateID,
              StateName = this.StateName,
              Phone = this.Phone,
              CreatedDate = this.CreatedDate,
              ExportedToHRN = this.ExportedToHRN
              
          };
        }
    }
}
