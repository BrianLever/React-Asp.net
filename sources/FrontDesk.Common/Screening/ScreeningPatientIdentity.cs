using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace FrontDesk
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    [Serializable()]
    [DataContract(Name = "ScreeningPatientIdentity", Namespace = "http://www.frontdeskhealth.com")]
    [KnownType(typeof(ScreeningResult))]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningPatientIdentity : IScreeningPatientIdentity
    {
        /// <summary>
        /// Patient's First name
        /// </summary>
        [DataMember]
        public string FirstName { get; set; }
        /// <summary>
        /// Patient's Last name
        /// </summary>
        [DataMember]
        public string LastName { get; set; }
        /// <summary>
        /// Patient's middle name
        /// </summary>
        [DataMember]
        public string MiddleName { get; set; }
        /// <summary>
        /// Patient's Birthday
        /// </summary>
        [DataMember]
        public DateTime Birthday { get; set; }

    }
}
