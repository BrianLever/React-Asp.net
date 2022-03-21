using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FrontDesk.Common.Extensions
{
    public static class XmlExtensions
    {
        public static string ToXmlString<T>(this T value)
        {
            try
            {
                using (var sw = new System.IO.StringWriter())
                {
                    var serializer = new XmlSerializer(typeof(T));
                    serializer.Serialize(sw, value);
                    return sw.ToString();
                }
            }
            catch
            {
                throw;
            }
        }

        public static T FromXmlString<T>(this string xmlString)
        {
            T result = default(T);
            try
            {
                using (var sr = new StringReader(xmlString))
                {
                    var serializer = new XmlSerializer(typeof(T));
                    result = (T)serializer.Deserialize(sr);
                    return result;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
