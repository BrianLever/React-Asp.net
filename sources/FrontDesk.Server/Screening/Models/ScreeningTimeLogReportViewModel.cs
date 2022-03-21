using System.Collections.Generic;
using System.Linq;

namespace FrontDesk.Server.Screening.Models
{
    public class ScreeningTimeLogReportViewModel
    {
        public List<ScreeningTimeLogReportItem> Items { get; set; }

        public ScreeningTimeLogReportViewModel()
        {
            Items = new List<ScreeningTimeLogReportItem>();
        }


        public List<ScreeningTimeLogReportItem> SectionMeasures {
            get
            {
                return Items?.Where(x => !string.IsNullOrEmpty(x.ScreeningSectionID)).ToList();
            }
        }


        public ScreeningTimeLogReportItem EntireScreeningsMeasures
        {
            get
            {
                return Items?.FirstOrDefault(x => string.IsNullOrEmpty(x.ScreeningSectionID))?? new ScreeningTimeLogReportItem();
            }
        }
    }

}
