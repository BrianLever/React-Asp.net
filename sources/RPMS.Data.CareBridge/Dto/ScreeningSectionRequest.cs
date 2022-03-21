using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RPMS.Common.Models;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class ScreeningSectionRequest
    {
        [JsonProperty("PatientID")]
        public int PatientID { get; set; }
        public int VisitID { get; set; }
        public long ScreendoxRecordNo { get; set; }
        [JsonConverter(typeof(DateConverter))]
        public DateTime ScreeningDate { get; set; }

        public List<ScreeningResultSectionRecord> Sections = new List<ScreeningResultSectionRecord>();

    }
}
