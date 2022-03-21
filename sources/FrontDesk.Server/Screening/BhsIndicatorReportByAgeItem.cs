using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    public class BhsIndicatorReportByAgeItem
    {
        /// <summary>
        /// The number of decimals in Percent fields
        /// </summary>
        public const int DecimalsInPercentValues = 2;

        public string CategoryID { get; set; }
        public string IndicatorName { get; set; }
        public int IndicatorID { get; set; }
        public int Age { get; set; }
        public long Count { get; set; }
    }
}
