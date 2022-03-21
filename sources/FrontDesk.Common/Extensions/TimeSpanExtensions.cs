using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Common.Extensions
{
    public static class TimeSpanExtensions
    {
        public static string AsFormattedString(this TimeSpan value)
        {
            return value.ToString(value.Days >= 1 ? "d' days(s) 'hh':'mm':'ss" : "hh':'mm':'ss");
        }

        public static string AsFormattedAverageString(this TimeSpan value)
        {
            return value.ToString(value.Hours >= 1 ? "hh':'mm':'ss" : "mm':'ss");
        }
    }
}
