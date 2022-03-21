using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;


namespace FrontDesk.Configuration
{
    /// <summary>
    /// Screening Frequency Configuration Item
    /// </summary>
    [DataContract(Name = "ScreeningFrequencyItem", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningFrequencyItem : ScreeningParameterBase
    {
        [DataMember]
        public string ID { get; set; }
        

        private int? _frequency = 99; //default frequency
        
        /// <summary>
        /// The event frequency for the patient to be screened with questions from this section
        /// </summary>
        /// <remarks>Automatically tracks frequency poperty changes and updates LastModifiedDateUTC</remarks>
        [DataMember]
        public int Frequency
        {
            get
            {

                return _frequency ?? 99;
            }
            set
            {
                if (!_frequency.HasValue) { _frequency = value; } //initial evaluation
                //update record if value has been changed and change the last modified date
                else { if (_frequency.Value != value) { _frequency = value; _lastModifiedDateUTC = DateTime.UtcNow; } }

            }
        }
       
    }
}
