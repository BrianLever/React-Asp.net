using Newtonsoft.Json;
using RPMS.Data.CareBridge.Dto;

namespace RPMS.Data.CareBridge.Dto
{
    public class FindPatientVisitRequest : GetPatientRequest
    {
        [JsonProperty(Order = 2)]
        public PatientName PatientName { get; set; }

        public FindPatientVisitRequest()
        {
            PatientName = new PatientName();
        }
    }
}