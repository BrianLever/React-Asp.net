using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace FrontDesk.Server.Extensions
{
    public static class DoubleExtensions
    {
        public static string FormatAsPercentage(this double value)
        {
            if(!Double.IsNaN(value) && !Double.IsNegativeInfinity(value) && !Double.IsPositiveInfinity(value))
            {
                return value.ToString("0.0'%'", CultureInfo.InvariantCulture);
            }
            else{
                return "-- %";
            }
        }
    }
}
