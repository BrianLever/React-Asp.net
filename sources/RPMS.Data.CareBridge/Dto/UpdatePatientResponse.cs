using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class UpdatePatientResponse
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public UpdatePatientResponseStatus Status { get; set; }
    }

    public enum UpdatePatientResponseStatus
    {
        
        SuccessfullyChanged = 0,
        Failed = 1
    }
}