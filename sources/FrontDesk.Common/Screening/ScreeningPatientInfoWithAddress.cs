using FrontDesk.Common.Screening;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Xml.Serialization;

namespace FrontDesk
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    [Serializable()]
    [DataContract(Name = "ScreeningPatientInfoWithAddress", Namespace = "http://www.frontdeskhealth.com")]
    [KnownType(typeof(ScreeningResult))]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningPatientIdentityWithAddress : ScreeningPatientIdentity, IScreeningPatientIdentityWithAddress
    {
        /// <summary>
        /// Patient's street address
        /// </summary>
        [DataMember]
        public string StreetAddress { get; set; }
        /// <summary>
        /// Patient's city
        /// </summary>
        [DataMember]
        public string City { get; set; }
        /// <summary>
        /// Patient's state (2 letters)
        /// </summary>
        [DataMember]
        public string StateID { get; set; }
        /// <summary>
        /// Patient's state full name
        /// </summary>
        [DataMember]
        public string StateName { get; set; }
        /// <summary>
        /// Patient's Zip code
        /// </summary>
        [DataMember]
        public string ZipCode { get; set; }
        /// <summary>
        /// Patient's primary phone number
        /// </summary>
        [DataMember]
        public string Phone { get; set; }


        public string FullName
        {
            get
            {
                return this.GetFullName();
            }
        }

        /// <summary>
        /// Patient's HRN in the EHR database to which exported report has been linked
        /// </summary>
        [IgnoreDataMember]
        [XmlIgnore]
        public string ExportedToHRN { get; set; }

    }
}
