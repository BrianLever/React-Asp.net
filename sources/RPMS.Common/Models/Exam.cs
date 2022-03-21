using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "Exam", Namespace = "http://www.screendox.com")]
    public class Exam
    {
        [DataMember]
        public string ExamName { get; set; }
        [DataMember]
        public string Result { get; set; }
        [DataMember]
        public string ResultLabel { get; set; }
        [DataMember]
        public string Comment { get; set; }
        //[DataMember]
        //public string Provider { get; set; }
        [DataMember]
        public int ExamID { get; set; }
        [DataMember]
        public string Code { get; set; }

    }
}
