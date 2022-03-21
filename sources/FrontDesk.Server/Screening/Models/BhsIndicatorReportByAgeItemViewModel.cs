using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsIndicatorReportByAgeItemViewModel
    {
        /// <summary>
        /// The number of decimals in Percent fields
        /// </summary>
        public const int DecimalsInPercentValues = 2;

        public string CategoryID { get; set; }
        public string Indicator { get; set; }
        public int IndicatorId { get; set; }


        public Dictionary<int, long> TotalByAge { get; set; }

        public long Total
        {
            get
            {
                if (TotalByAge == null || TotalByAge.Values.Count == 0)
                {
                    return 0;
                }

                return TotalByAge.Values.Sum();
            }
        }

        public BhsIndicatorReportByAgeItemViewModel()
        {
            TotalByAge = new Dictionary<int, long>();
        }
        
    }
}
