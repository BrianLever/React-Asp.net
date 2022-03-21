using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsScreeningHistoryItem
    {
        public Int64 ScreeningResultID { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public bool IsPositive { get; set; }
        public bool HasAnyScreening { get; set; }
    }
}
