using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class GetPatientResponse
    {
        [JsonProperty("patients")]
        public List<PatientRecord> Patients = new List<PatientRecord>();

        [JsonIgnore]
        public PatientRecord Value
        {
            get
            {
                return Patients != null && Patients.Any() ? Patients.First() : null;
            }
        }

    }

   
}
