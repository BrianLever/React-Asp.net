using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using ScreenDox.EHR.Common.Properties;

namespace RPMS.Common.Models
{
    [DataContract(Name = "CrisisAlert", Namespace = "http://www.screendox.com")]
    public class CrisisAlert
    {
        public int ID { get; set; }

        [DataMember]
        public string Title { get; set; }
        /// <summary>
        /// Date of note for reference and sorting. Should be the date of screening
        /// </summary>
        [DataMember]
        public DateTime DateOfNote { get; set; }
        [DataMember]
        public string Author { get; set; }
        [DataMember]
        public DateTime EntryDate { get; set; }
        [DataMember]
        public string Details { get; set; }
        [DataMember]
        public int DocumentTypeID { get; set; }
        [DataMember]
        public string DocumentTypeAbbr { get; set; }

        public CrisisAlert()
        {
            EntryDate = DateTime.Now;
            Author = Resources.Provider;
        }


        public int PatientID { get; set; }

        public int VisitID { get; set; }
    }
}
