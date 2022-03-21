using Newtonsoft.Json;

namespace RPMS.Data.CareBridge.Dto
{
    public class GetPatientRequest
    {
        [JsonProperty("RowID", Order = 1)]
        public int PatientId { get; set; }
    }
}
