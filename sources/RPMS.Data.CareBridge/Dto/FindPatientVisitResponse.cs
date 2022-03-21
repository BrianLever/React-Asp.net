using System.Collections.Generic;
using Newtonsoft.Json;

namespace RPMS.Data.CareBridge.Dto
{
    public class FindPatientVisitResponse
    {
        [JsonProperty("visits")]
        public List<PatientVisitRecord> Items = new List<PatientVisitRecord>();

    }
}
