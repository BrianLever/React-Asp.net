using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Models
{
    public class IndicatorReportByAgeItemViewModel
    {
        /// <summary>
        /// The number of decimals in Percent fields
        /// </summary>
        public const int DecimalsInPercentValues = 2;

        public string ScreeningSectionID { get; set; }
        public string ScreeningSectionQuestion { get; set; }
        public string ScreeningSectionIndicates { get; set; }

        public Dictionary<int, long> PositiveScreensByAge { get; set; }

        public long Total
        {
            get
            {
                if (PositiveScreensByAge == null || PositiveScreensByAge.Values.Count == 0)
                {
                    return 0;
                }

                return PositiveScreensByAge.Values.Sum();
            }
        }

        public IndicatorReportByAgeItemViewModel()
        {
            PositiveScreensByAge = new Dictionary<int, long>();
        }
        
    }
}
