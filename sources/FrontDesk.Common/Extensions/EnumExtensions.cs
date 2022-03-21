using System;
using System.ComponentModel;
using System.Reflection;

namespace FrontDesk.Common.Extensions
{
    public static class EnumExtensions
    {
        public static string GetEnumDescription(this Enum value)
        {
            
            FieldInfo fi = value.GetType().GetField(value.ToString());
            if (fi == null)
            {
                return "N/A";
            }
            var attributes =
                (DescriptionAttribute[]) fi.GetCustomAttributes(typeof (DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}