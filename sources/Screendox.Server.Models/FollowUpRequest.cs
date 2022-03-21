using FrontDesk.Common;

using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// View model for getting follow up detals
    /// </summary>
    public class FollowUpRequest
    {
        public string VisitRefferalRecommendation { get; set; }

        public LookupValue PatientAttendedVisit { get; set; }
        public DateTimeOffset? FollowUpContactDate { get; set; }
        public LookupValue FollowUpContactOutcome { get; set; }

        public LookupValue Discharged { get; set; }
        public DateTimeOffset? NewVisitDate { get; set; }
        public LookupValueWithDescription NewVisitReferralRecommendation { get; set; }
        public LookupValue NewVisitReferralRecommendationAccepted { get; set; }
        public LookupValue ReasonNewVisitReferralRecommendationNotAccepted { get; set; }
        public bool ThirtyDatyFollowUpFlag { get { return FollowUpDate.HasValue; } }

        public DateTimeOffset? FollowUpDate { get; set; }

        public string Notes { get; set; }
    }
}
