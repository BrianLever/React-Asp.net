using FrontDesk.Common;
using FrontDesk.Common.Bhservice;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// View model for updating visit detals
    /// </summary>
    public class VisitRequest
    {

        //Indicators
        /// <summary>
        /// Drag of choice manual entries if DASt-10 was positive
        /// </summary>
        public ScreeningSectionResultRequest DrugOfChoice = new ScreeningSectionResultRequest();



        public List<ManualScreeningResultValue> OtherScreeningTools = new List<ManualScreeningResultValue>(4);

        public List<TreatmentAction> TreatmentActions = new List<TreatmentAction>(5);

        public LookupValueWithDescription NewVisitReferralRecommendation { get; set; }

        public LookupValue NewVisitReferralRecommendationAccepted { get; set; }

        public LookupValue ReasonNewVisitReferralRecommendationNotAccepted { get; set; }

        public DateTimeOffset? NewVisitDate { get; set; }

        public LookupValue Discharged { get; set; }

        public bool ThirtyDatyFollowUpFlag { get { return FollowUpDate.HasValue; } }

        public DateTimeOffset? FollowUpDate { get; set; }

        public string Notes { get; set; }

    }
}
