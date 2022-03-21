using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Common.Bhservice.Import
{
    [DataContract(Name = "PatientDemographicsKioskResult", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    public class PatientDemographicsKioskResult : ScreeningPatientIdentity
    {
        [DataMember]
        public int RaceId { get; set; }
        [DataMember]
        public int GenderId { get; set; }
        [DataMember]
        public int SexualOrientationId { get; set; }
        [DataMember]
        public string TribalAffiliation { get; set; }
        [DataMember]
        public int MaritalStatusId { get; set; }
        [DataMember]
        public int EducationLevelId { get; set; }
        [DataMember]
        public int LivingOnReservationId { get; set; }

        [DataMember(IsRequired = false)]
        public string CountyOfResidence
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CountyStateOfResidence))
                {
                    return CountyNameOfResidence?.Trim(' ', ',') ?? String.Empty;
                }

                return string.Join(", ", CountyNameOfResidence?.Trim(' ', ','), CountyStateOfResidence?.Trim(' ', ','));
            }
            set
            {
                var county = CountyTextFunctions.ParseCounty(value);

                CountyNameOfResidence = county.Name;
                CountyStateOfResidence = county.State;
            }
        }

        [DataMember(IsRequired = false)]
        public string CountyNameOfResidence { get; set; }

        [DataMember(IsRequired = false)]
        public string CountyStateOfResidence { get; set; }

        [DataMember]
        public List<int> MilitaryExperience { get; set; } = new List<int>();

        [IgnoreDataMember]
        public int Age
        {
            get
            {
                return ScreeningResult.GetAge(this.Birthday);
            }
        }

        //System properties
        [DataMember]
        public Int16 KioskID { get; set; }

        public void Init(ScreeningResult result)
        {
            FirstName = result.FirstName;
            MiddleName = result.MiddleName;
            LastName = result.LastName;
            Birthday = result.Birthday;
            KioskID = result.KioskID;
        }

    }
}
