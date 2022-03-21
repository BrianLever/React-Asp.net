using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using RPMS.Data.CareBridge.Converters;

namespace RPMS.Data.CareBridge.Dto
{
    /*
     * "Address1": "1305 Remington Road",
                "Address2": "Suite P",
                "City": "Schaumburg",
                "State": "IL ",
                "Zip": "60173"
                */
    public class StreetAddress
    {
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        public string Address1 { get; set; }
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        public string Address2 { get; set; }
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        public string City { get; set; }
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        public string State { get; set; }
        [JsonConverter(typeof(StringSpaceRemovalConverter))]
        public string Zip { get; set; }
    }
}
