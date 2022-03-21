using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace RPMS.Common.Models
{
    [DataContract(Name = "ExportTask", Namespace = "http://www.screendox.com")]
    public class ExportTask
    {
        [DataMember]
        public List<PatientRecordModification> PatientRecordModifications { get; set; }
        [DataMember]
        public List<HealthFactor> HealthFactors { get; set; }
        [DataMember]
        public List<Exam> Exams { get; set; }
        [DataMember]
        public List<CrisisAlert> CrisisAlerts { get; set; }

        [DataMember]
        public List<ExportScreeningSectionPreview> ScreeningSections { get; set; }

        [DataMember]
        public List<string> Errors { get; set; }


        public ExportTask()
        {
            PatientRecordModifications = new List<PatientRecordModification>();
            HealthFactors = new List<HealthFactor>();
            Exams = new List<Exam>();
            CrisisAlerts = new List<CrisisAlert>();
            ScreeningSections = new List<ExportScreeningSectionPreview>();

            Errors = new List<string>();
        }

    }
}
