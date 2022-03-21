using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    public class IndicatorReportItem
    {
        /// <summary>
        /// The number of decimals in Percent fields
        /// </summary>
        public const int DecimalsInPercentValues = 1;

        public string ScreeningSectionID { get; internal set; }
        public string ScreeningSectionQuestion { get; internal set; }
        public string ScreeningSectionIndicates { get; internal set; }

        public long PositiveCount { get; internal set; }
        public long NegativeCount { get; internal set; }
        public long TotalCount { get { return PositiveCount + NegativeCount; } }


        /// <summary>
        /// Percent of Positive screenings from total. Value from 0.00 to 100.00
        /// If Total = 0, returns Double.NaN
        /// </summary>
        public double PositivePercent
        {
            get
            {
                return TotalCount > 0 ? Math.Round((double)PositiveCount / (double)TotalCount * 100, DecimalsInPercentValues) : Double.NaN;
            }
        }
        
        /// <summary>
        /// Percent of Negative screenings from total. Value from 0.00 to 100.00
        /// If Total = 0, returns Double.NaN
        /// </summary>   
        public double NegativePercent
        {
            get
            {
                return TotalCount > 0 ? Math.Round(100.0 - PositivePercent, DecimalsInPercentValues): Double.NaN;
            }
        }

        public int QuestionId { get; set; }


        public IndicatorReportItem() { }

        public IndicatorReportItem(
            string sectionID,
            int questionId,
            string question,
            string indicates,
            //long yesCount,
            //long noCount,
            long positiveCount,
            long negativeCount
        )
        {
            ScreeningSectionID = sectionID;
            ScreeningSectionQuestion = question;
            QuestionId = questionId;
            ScreeningSectionIndicates = indicates;
            //YesCount = yesCount;
            //NoCount = noCount;
            PositiveCount = positiveCount;
            NegativeCount = negativeCount;
        }
    }
}
