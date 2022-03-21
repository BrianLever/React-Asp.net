using System;
using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Models
{
    public class ScreeningTimeLogReportItem
    {
        /// <summary>
        /// The number of decimals in Percent fields
        /// </summary>
        public const int DecimalsInPercentValues = 2;

        public string ScreeningSectionID { get; set; }
        public string ScreeningSectionName { get; set; }
        public int NumberOfReports { get; set; }
        public TimeSpan TotalTime { get; set; }
        public TimeSpan AverageTime { get; set; }

        public ScreeningTimeLogReportItem()
        {
            
        }
        
    }
}
