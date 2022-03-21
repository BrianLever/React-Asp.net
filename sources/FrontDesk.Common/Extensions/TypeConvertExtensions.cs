using System;
using System.Collections.Generic;

namespace FrontDesk.Common.Extensions
{
    public static class TypeConvertExtensions
    {
        public static T? ConvertToNullableType<T>(this object value) where T : struct
        {
            T? converted = null;
            if (value != null && !Convert.IsDBNull(value))
            {
                if (value is string)
                {
                    return ((string)value).ParseToOrDefault<T>();
                }
                
                if (typeof(T) == typeof(Guid))
                {
                    converted = (T)Convert.ChangeType(new Guid(Convert.ToString(value)), typeof(T));
                }
                else
                {
                    converted = (T)Convert.ChangeType(value, typeof(T));
                }
            }
            return converted;
        }



        public static T ConvertToType<T>(this object value)
        {
            T converted = default(T);
            if (value == null) return converted;

            if (typeof(T) == typeof(Guid))
            {
                converted = (T)Convert.ChangeType(new Guid(Convert.ToString(value)), typeof(T));
            }
            else
            {
                converted = (T)Convert.ChangeType(value, typeof(T));
            }
            return converted;
        }


        public static T? ParseTo<T>(this string value) where T: struct
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.ConvertToType<T>();
        }

        public static T ParseToOrDefault<T>(this string value) where T : struct
        {
           
            return value.ParseToOrDefault<T>(default(T));
        }

        public static T ParseToOrDefault<T>(this string value, T defaultValue) where T : struct
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                return defaultValue;
            }

            return value.ConvertToType<T>();
        }

        public static bool IsDefaultValue<T>(this T value)  where T: IComparable<T>
        {
            return Comparer<T>.Default.Compare(value, default(T)) == 0;
        }

        public static TTarget As<TTarget>(this object value)
        {
            return value.ConvertToType<TTarget>();
        }

        public static TTarget? AsNullable<TTarget>(this object value) where TTarget : struct
        {
            return value.ConvertToNullableType<TTarget>();
        }

        public static TTarget As<TSource, TTarget>(this TSource value, TTarget defaultValue = default (TTarget))
        {  
            return Convert.IsDBNull(value) ? defaultValue : value.ConvertToType<TTarget>();
        }


        public static string ToStringOrDefault(this long? value, string defaultValue = "")
        {
            if (value.HasValue)
            {
                return value.Value.ToString();
            }
            return defaultValue;
        }

        public static string ToStringOrDefault(this int? value, string defaultValue = "")
        {
            if (value.HasValue)
            {
                return value.Value.ToString();
            }
            return defaultValue;
        }
    }
}
