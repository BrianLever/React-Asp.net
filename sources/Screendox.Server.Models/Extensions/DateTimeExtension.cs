using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace FrontDesk.Server.Extensions
{
    public static class DateTimeExtension
    {
        public static int GetDiffInMonths(this DateTime endDate, DateTime startDate)
        {
            int sign = 1;
            DateTime tmp;

            if (startDate > endDate)
            {
                sign = -1; //minus
                tmp = startDate;
                startDate = endDate;
                endDate = tmp;
            }

            tmp = startDate;
            int monthDiff = 0;
            while ((tmp = tmp.AddMonths(1)) < endDate)
            {
                ++monthDiff;
            }
            return monthDiff * sign;

        }
    }
}
