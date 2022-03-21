using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace RPMS.Common
{
    [DataContract(Name = "HealthRecord", Namespace = "http://www.frontdeskhealth.com")]
    public class HealthRecord
    {

        /// <summary>
        /// Health Record Number Value
        /// </summary>
        [DataMember]
        public string HRCN { get; set; }

        /// <summary>
        /// Location Name
        /// </summary>
        [DataMember]
        public Location Location { get; set; }

        public HealthRecord() { }

        public HealthRecord(IDataReader reader) 
        {
            this.HRCN = Convert.ToString(reader["HEALTH_RECORD_NO"]);
            this.Location = new Location(reader);
        }
    }
}
