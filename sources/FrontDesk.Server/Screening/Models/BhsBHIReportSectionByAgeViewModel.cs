using System.Collections.Generic;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsBHIReportSectionByAgeViewModel
    {
        public string Header { get; set; }
        public List<BhsIndicatorReportByAgeItemViewModel> Items { get; set; }

    }
}
