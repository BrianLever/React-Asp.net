using System;

namespace FrontDesk.Common.Bhservice
{
    public interface IBhsFollowUpSchedule
    {
        LookupValue Discharged { get; set; }
        DateTimeOffset? NewVisitDate { get; set; }
        LookupValueWithDescription NewVisitReferralRecommendation { get; set; }
        LookupValue NewVisitReferralRecommendationAccepted { get; set; }
        LookupValue ReasonNewVisitReferralRecommendationNotAccepted { get; set; }
        bool ThirtyDatyFollowUpFlag { get;}

        DateTimeOffset? FollowUpDate { get; set; }
        string Notes { get; set; }
    }
}