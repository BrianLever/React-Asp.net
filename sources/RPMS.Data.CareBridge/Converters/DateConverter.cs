using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FrontDesk.Common.Extensions;
using Newtonsoft.Json;

namespace RPMS.Data.CareBridge.Converters
{
    public class DateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(string) || objectType == typeof(DateTime));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.ValueType == typeof(DateTime))
                return reader.Value;


            Regex re = new Regex(@"(\d{4})(\d{2})(\d{2})(\d{2})?(\d{2})?(\d{2})?", RegexOptions.IgnoreCase);

            var match = re.Match(reader.Value?.ToString() ?? string.Empty);

            if (match.Success && match.Groups.Count == 7)
            {

                return new DateTime(
                      Convert.ToInt32(match.Groups[1].Value),
                      Convert.ToInt32(match.Groups[2].Value),
                      Convert.ToInt32(match.Groups[3].Value),
                      match.Groups[4].Success ? match.Groups[4].Value.As<int>() : 0,
                      match.Groups[5].Success ? match.Groups[5].Value.As<int>() : 0,
                      match.Groups[6].Success ? match.Groups[6].Value.As<int>() : 0
                      );

            }
            return new DateTime();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (typeof(object) == typeof(string))
                writer.WriteValue(value);

            var date = (DateTime)value;
            writer.WriteValue(date.ToString("yyyyMMdd"));
        }
    }
}
