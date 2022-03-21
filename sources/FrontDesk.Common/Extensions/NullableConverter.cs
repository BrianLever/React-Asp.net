using System;

namespace FrontDesk.Common.Extensions
{
    public static class NullableConverter
    {
        public static Nullable<DateTime> ToNullable(this DateTime value)
        {
            if (value == DateTime.MinValue)
                return null;
            else
                return (Nullable<DateTime>)value;
        }


        public static DateTime FromNullable(this Nullable<DateTime> value)
        {
            if (value.HasValue)
                return value.Value;
            else
                return DateTime.MinValue;
        }
    }
}
