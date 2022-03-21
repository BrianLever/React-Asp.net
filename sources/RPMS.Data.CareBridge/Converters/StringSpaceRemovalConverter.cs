using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace RPMS.Data.CareBridge.Converters
{
    public class StringSpaceRemovalConverter : JsonConverter<string>
    {
        public override string ReadJson(JsonReader reader, Type objectType, string existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            string value = Convert.ToString(reader.Value).TrimEnd();
            return value;
        }

        public override void WriteJson(JsonWriter writer, string value, JsonSerializer serializer)
        {
            writer.WriteValue((value?? string.Empty).Trim());
        }
    }
}
