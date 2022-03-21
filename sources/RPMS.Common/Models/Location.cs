using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;

namespace RPMS.Common.Models
{
    [DataContract(Name = "Location", Namespace = "http://www.screendox.com")]
    public class Location
    {
        [DataMember]
        public int ID { get; set; }

        [DataMember]
        public string Name { get; set; }

        public Location() { }

        
    }
}
