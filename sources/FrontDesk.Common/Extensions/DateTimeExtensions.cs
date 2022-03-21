using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Extensions
{
    public static class DateTimeExtensions
    {
        private static string GetDateFormatString(DateTime date)
        {
            string text = String.Empty;
            text = String.Format("{0:MM/dd/yyyy}", date);
            return text;
        }

        private static string GetDateTimeFormatString(DateTime date)
        {
            string text = String.Empty;
            text = String.Format("{0:MM/dd/yyyy' 'hh:mm' 'tt}", date);
            return text;
        }

        private static string GetTimeFormatString(DateTime date)
        {
            string text = String.Empty;
            text = String.Format("{0:HH':'mm}", date);
            return text;
        }

        private static string GetDateAndTimeFormatString(DateTimeOffset value)
        {
            return String.Format("{0:MM/dd/yyyy HH:mm zzz}", value);
        }


        public static string FormatAsDate(this DateTime dateTime, string emptyValue = "")
        {
            if (dateTime != DateTime.MinValue)
            {
                return GetDateFormatString(dateTime);
            }
            return emptyValue;
        }

        public static string FormatAsDate(this DateTime? dateTime, string emptyValue = "")
        {
            if (dateTime.HasValue && dateTime != DateTime.MinValue)
            {
                return GetDateFormatString(dateTime.Value);
            }
            return emptyValue;
        }

        public static string FormatAsDate(this DateTimeOffset? dateTime, string emptyValue = "")
        {
            if (dateTime.HasValue)
            {
                return GetDateFormatString(dateTime.Value.Date);
            }
            return emptyValue;
        }

        public static string FormatAsDate(this DateTimeOffset dateTime, string emptyValue = "")
        {
            return GetDateFormatString(dateTime.Date);
        }

        public static string FormatAsTime(this DateTime? dateTime, string emptyValue = "")
        {
            if (dateTime.HasValue && dateTime != DateTime.MinValue)
            {
                return GetTimeFormatString(dateTime.Value);
            }
            return emptyValue;
        }

        public static string FormatAsDateWithTime(this DateTime dateTime, string emptyValue = "")
        {
            if (dateTime != DateTime.MinValue)
            {
                return GetDateTimeFormatString(dateTime);
            }
            return emptyValue;
        }

        public static string FormatAsDateWithTime(this DateTime? dateTime, string emptyValue = "")
        {
            if (dateTime.HasValue && dateTime != DateTime.MinValue)
            {
                return GetDateTimeFormatString(dateTime.Value);
            }
            return emptyValue;
        }



        public static string FormatAsDateWithTime(this DateTimeOffset value, string emptyValue = "")
        {
            return GetDateAndTimeFormatString(value);
        }

        public static string FormatAsDateWithTime(this DateTimeOffset? value, string emptyValue = "")
        {
            if (value.HasValue )
            {
                return GetDateAndTimeFormatString(value.Value);
            }
            return emptyValue;
        }

        public static bool IsEmpty(this DateTime value)
        {
            return value == DateTime.MinValue;
        }

        public static bool IsEmpty(this DateTime? value)
        {
            return !value.HasValue || value.Value.IsEmpty();
        }
    }
}
