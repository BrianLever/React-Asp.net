using System;
using System.Reflection;

namespace FrontDesk.Common.Bhservice
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]
    public class BhsFollowUp : BhsEntry, IBhsFollowUpSchedule
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
        public ScreeningResult Result { get; set; }
        /// <summary>
        /// read only object, initialized from BhsVisitID
        /// </summary>
        public BhsVisit Visit { get; set; }

        /// <summary>
        /// Read only object, initialized from BhsVisitID 
        /// </summary>
        public BhsFollowUp ParentFollowUpVisit { get; set; }

        /// <summary>
        /// Scheduled date of the follow up. BY default it's 30 days after the scheduled Visit
        /// </summary>
        public DateTimeOffset ScheduledFollowUpDate { get; set; }

        //Filled from BhsVisit "New Visit Date"
        public DateTimeOffset ScheduledVisitDate { get; set; }


        public string VisitRefferalRecommendation
        {
            get
            {
                LookupValueWithDescription recommendation = Visit.NewVisitReferralRecommendation;

                if(this.ParentFollowUpVisit != null && ParentFollowUpVisit.NewVisitReferralRecommendation != null)
                {
                    recommendation = ParentFollowUpVisit.NewVisitReferralRecommendation;
                }

                return "{0}. {1}".FormatWith(recommendation.Name, recommendation.Description);
            }
        }



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


    public static class BhsThirtyDayFollowUpFluentExtentions
    {
        public static BhsFollowUp SetFollowUpDate(this BhsFollowUp model, DateTimeOffset followUpDate)
        {
            if (model == null) { return model; }

            model.ScheduledFollowUpDate = followUpDate;

            return model;
        }

    }


}
