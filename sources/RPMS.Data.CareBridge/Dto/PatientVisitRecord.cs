using System;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class PatientVisitRecord
    {
        [JsonProperty("VisitID")]
        public int VisitId { get; set; }

        [JsonProperty("AdmitDate")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime AdmitionDate { get; set; }
        public string ServiceCategory { get; set; }
        public PatientName PatientName { get; set; }
        public string LocationName { get; set; }

        [JsonProperty("DOB")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Birthday { get; set; }
    }
}