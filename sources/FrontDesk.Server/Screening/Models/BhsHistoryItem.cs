using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Screening.Models
{
    public class BhsHistoryItem
    {
        public Int64 ID { get; set; }
        public DateTimeOffset CompletedDate { get; set; }
        public int? DischargeID { get; set; }
        public int? NewVisitReferralRecommendationAcceptedID { get; set; }
    }
}
