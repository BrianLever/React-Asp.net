using System;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class FindPatientRequest
    {
        public string LastName { get; set; }
        [JsonProperty("DOB")]
        [JsonConverter(typeof(DateConverter))]
        public DateTime Birthday { get; set; }
    }
}
