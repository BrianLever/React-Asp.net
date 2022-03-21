using Newtonsoft.Json;

using ScreenDox.Server.Models.Resources;

namespace FrontDesk.Server.Extensions
{
    /// <summary>
    /// Extension methods for String methods, including formatting
    /// </summary>
    public static class StringExtensions
    {
        public static string FormatAsNullableString(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value :  TextMessages.NA;
        }

        public static string AsJson(this object value)
        {
            if (value == null) return string.Empty;

            return JsonConvert.SerializeObject(value, Formatting.None);
        }

        public static T FromJson<T>(this string value)
            where T: class
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}
