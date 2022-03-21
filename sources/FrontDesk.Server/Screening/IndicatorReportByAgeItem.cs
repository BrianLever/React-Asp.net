using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening
{
    public class IndicatorReportByAgeItem
    {
        /// <summary>
        /// The number of decimals in Percent fields
        /// </summary>
        public const int DecimalsInPercentValues = 2;

        public string ScreeningSectionID { get; set; }
        public string ScreeningSectionQuestion { get; set; }
        public string ScreeningSectionIndicates { get; set; }
        public int Age { get; set; }
        public long PositiveCount { get; set; }
        public int QuestionID { get; set; }
    }

    public class IndicatorReportByAgeItemFactory
    {
        public IndicatorReportByAgeItem CreateForDepression(IDataReader reader, string sectionId)
        {
            var item = new IndicatorReportByAgeItem
            {
                ScreeningSectionID = sectionId,
                ScreeningSectionQuestion = reader.Get<string>("Name"),
                QuestionID = reader.Get<int>("ScoreLevel"),
                Age = reader.Get<int>("Age"),
                ScreeningSectionIndicates = reader.Get<string>("Indicates", String.Empty),
                PositiveCount = reader.Get<Int64>("TotalPositive"),
                
            };

            return item;
        }

        public IndicatorReportByAgeItem CreateForScreeningSection(IDataReader reader)
        {
            return new IndicatorReportByAgeItem
            {
                ScreeningSectionID = reader.Get<string>("ScreeningSectionID"),
                ScreeningSectionQuestion = reader.Get<string>("Name"),
                ScreeningSectionIndicates = reader.Get<string>("Indicates", String.Empty),
                Age = reader.Get<int>("Age"),
                PositiveCount = reader.Get<Int64>("TotalPositive"),
            };

        }
    }
}
