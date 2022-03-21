using System;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class UpdatePatientRequest
    {
        [JsonProperty("RowID")]
        public int RowId { get; set; }

        [JsonConverter(typeof(DateConverter))]
        public DateTime AddressChangeDate { get; set; }

        public string AddressChangeReason { get; set; } = "addressreason";

        public PatientName PatientName { get; set; }

        public StreetAddress StreetAddress { get; set; }

        public string HomePhone { get; set; }
        public string OfficePhone { get; set; }
    }
}