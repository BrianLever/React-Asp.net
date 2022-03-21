using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Extensions
{
    public static class DateFormattingExtentions
    {
        public static string FormatAsDateWithTimeWithoutTimeZone(this DateTimeOffset value)
        {
            return string.Format("{0:MM/dd/yyyy, HH:mm}", value, System.Globalization.DateTimeFormatInfo.InvariantInfo);
        }

        public static string FormatAsDateWithTimeWithoutTimeZone(this DateTimeOffset? value)
        {
            if (value.HasValue)
            {
                return string.Format("{0:MM/dd/yyyy, HH:mm}", value, System.Globalization.DateTimeFormatInfo.InvariantInfo);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
