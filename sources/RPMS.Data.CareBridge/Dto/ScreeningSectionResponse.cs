using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class ScreeningSectionResponse
    {
        [JsonProperty("status")]
        [JsonConverter(typeof(StringEnumConverter))]
        public ScreeningSectionResponseStatus Status { get; set; }
    }

    public enum ScreeningSectionResponseStatus
    {

        Unknown = 0,
        Successfull = 1
    }
}
