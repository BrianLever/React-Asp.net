using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    public class PatientName
    {
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        public string LastName { get; set; }
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        public string FirstName { get; set; }
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string MiddleName { get; set; }

        public PatientName()
        {
            LastName = string.Empty;
            FirstName = string.Empty;
            MiddleName = string.Empty;
        }
    }
}
