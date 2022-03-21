using FrontDesk;

using System;
using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    [DataContract(Name = "PatientInfo", Namespace = "http://www.screendox.com")]
    public class PatientInfo : IScreeningPatientIdentityWithAddress
    {
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string StateID { get; set; }
        [DataMember]
        public string StateName { get; set; }

        [DataMember]
        public string StreetAddress { get; set; }
        [DataMember]
        public string ZipCode { get; set; }
        [DataMember]
        public DateTime Birthday { get; set; }

        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string MiddleName { get; set; }
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public int Age { get; set; }
    }
}
