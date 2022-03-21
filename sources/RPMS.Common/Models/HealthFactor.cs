using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "HealthFactor", Namespace = "http://www.screendox.com")]
    public class HealthFactor
    {
        [DataMember]
        public string Factor { get; set; }
        [DataMember]
        public string Comment { get; set; }
        [DataMember]
        public int FactorID { get; set; }
        [DataMember]
        public string Code { get; set; }
    }
}
