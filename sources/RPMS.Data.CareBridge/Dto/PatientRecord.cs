using System;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class PatientRecord
    {
        public int RowID { get; set; }
        public string HealthRecordNumber { get; set; }
        public PatientName PatientName { get; set; }
        //"DOB": "19720715",
        [JsonProperty("DOB")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Birthday { get; set; }
        public StreetAddress StreetAddress { get; set; }
        public string HomePhone { get; set; }
        public string OfficePhone { get; set; }

        public PatientRecord()
        {
            PatientName = new PatientName();
            StreetAddress = new StreetAddress();
        }
    }
}
