using System.Collections.Generic;
using Newtonsoft.Json;

namespace RPMS.Data.CareBridge.Dto
{
    public class FindPatientResponse
    {
        [JsonProperty("patients")]
        public List<PatientRecord> Items = new List<PatientRecord>();

    }
}
