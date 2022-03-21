using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace FrontDesk
{
    [DataContract(Name = "ScreeningParameterBase", Namespace = "http://www.frontdeskhealth.com")]
    public abstract class ScreeningParameterBase
    {
        protected DateTime _lastModifiedDateUTC = DateTime.UtcNow;
        /// <summary>
        /// Get the UTC date and time when record has been modified lat time
        /// </summary>
        [DataMember]
        public DateTime LastModifiedDateUTC
        {
            get
            {
                return _lastModifiedDateUTC;
            }
            set
            {
                _lastModifiedDateUTC = value;
            }
        }
    }
}
