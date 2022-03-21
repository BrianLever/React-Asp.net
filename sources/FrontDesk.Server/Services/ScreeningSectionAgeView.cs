using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Permissions;
using System.Runtime.Serialization;
using System.Reflection;

namespace FrontDesk.Server
{
    [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
    [DataContract(Name = "ScreeningSectionAgeView", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningSectionAgeView
    {
        #region properties
        [DataMember]
        public string ScreeningSectionID { get; set; }

        /// <summary>
        /// Minimal age for the patient to be screened with questions from this section
        /// </summary>
        /// <remarks>Automatically tracks minimal value poperty changes and updates LastModifiedDateUTC</remarks>
        [DataMember]
        public byte MinimalAge { get; set; }

        /// <summary>
        /// If Section turned On or Off
        /// </summary>
        [DataMember]
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Get the UTC date and time when record has been modified lat time
        /// </summary>
        [DataMember]
        public DateTime LastModifiedDateUTC { get; set; }


        #endregion

        #region constructors

        public ScreeningSectionAgeView() { }

        /// <summary>
        /// init minimal age parameter from database
        /// </summary>
        /// <param name="screeningSectionID"></param>
        /// <param name="minimalAge"></param>
        /// <param name="lastModifiedDateUTC"></param>
        public ScreeningSectionAgeView(ScreeningSectionAge ageSetting)
        {

            this.ScreeningSectionID = ageSetting.ScreeningSectionID;
            this.MinimalAge = ageSetting.MinimalAge;
            this.LastModifiedDateUTC = ageSetting.LastModifiedDateUTC;
            this.IsEnabled = ageSetting.IsEnabled;
        }

        #endregion

    }
}
