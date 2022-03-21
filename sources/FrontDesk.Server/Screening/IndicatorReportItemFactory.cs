using FrontDesk.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Screening
{
    public class IndicatorReportItemFactory
    {
        public IndicatorReportItem CreateForDepression(IDataReader reader, string sectionId)
        {
            var item = new IndicatorReportItem
            {
                ScreeningSectionID = sectionId,
                ScreeningSectionQuestion = reader.Get<string>("Name"),
                ScreeningSectionIndicates = reader.Get<string>("Indicates", String.Empty),
                PositiveCount = reader.Get<Int64>("TotalPositive"),
                QuestionId = reader.Get<int>("ScoreLevel", true) 
            };
            var total = reader.Get<long>("Total");

            item.NegativeCount = total - item.PositiveCount;

            return item;
        }

        public IndicatorReportItem CreateForScreeningSection(IDataReader reader)
        {
            var item = new IndicatorReportItem
            {
                ScreeningSectionID = reader.Get<string>(0, string.Empty).TrimEnd(),
                ScreeningSectionQuestion = reader.Get<string>("Name"),
                ScreeningSectionIndicates = reader.Get<string>("Indicates", String.Empty),
                PositiveCount = reader.Get<Int64>("TotalPositive"),

            };
            var total = reader.Get<long>("Total");

            item.NegativeCount = total - item.PositiveCount;

            return item;
        }
    }
}
