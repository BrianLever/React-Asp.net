using FrontDesk.Common;

using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// View model for getting follow up detals
    /// </summary>
    public class FollowUpResponse : BhsResponseBase
    {
        public long ScreeningResultID { get; set; }
        public long BhsVisitID { get; set; }

        /// <summary>
        /// The ID of the other Follow-Up report from which this report has been created using "30-Day Follow-Up Report" toggle flag
        /// </summary>
        public long? ParentFollowUpID { get; set; }

        /// <summary>
        /// read only object, initialized from ScreeningResultID
        /// </summary>
        public ScreeningResultResponse Result { get; set; }
        /// <summary>
        /// read only object, initialized from BhsVisitID
        /// </summary>
        public VisitResponse Visit { get; set; }

        /// <summary>
        /// Scheduled date of the follow up. BY default it's 30 days after the scheduled Visit
        /// </summary>
        public DateTimeOffset ScheduledFollowUpDate { get; set; }

        //Filled from BhsVisit "New Visit Date"
        public DateTimeOffset ScheduledVisitDate { get; set; }


        public string VisitRefferalRecommendation { get; set; }
       


        public LookupValue PatientAttendedVisit { get; set; }
        public DateTimeOffset? FollowUpContactDate { get; set; }
        public LookupValue FollowUpContactOutcome { get; set; }

        public bool IsCompleted
        {
            get
            {
                return CompleteDate.HasValue;
            }
        }

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
