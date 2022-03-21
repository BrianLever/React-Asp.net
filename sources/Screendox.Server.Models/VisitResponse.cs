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
    /// View model for getting visit detals
    /// </summary>
    public class VisitResponse: BhsResponseBase
    {
        public DateTimeOffset ScreeningDate { get; set; }

        public long ScreeningResultID { get; set; }

        //read only object, initialized from ScreeningResultID
        public ScreeningResultResponse Result { get; set; }

        //improve the filter performance by location
        public int LocationID { get; set; }


        //Indicators
        public bool TobacoExposureSmokerInHomeFlag { get; set; }
        public bool TobacoExposureCeremonyUseFlag { get; set; }
        public bool TobacoExposureSmokingFlag { get; set; }
        public bool TobacoExposureSmoklessFlag { get; set; }
        public ScreeningResultValue AlcoholUseFlag { get; set; }
        public ScreeningResultValue SubstanceAbuseFlag { get; set; } //DAST-10
        public ScreeningResultValue AnxietyFlag { get; set; } // GAD-7
        public ScreeningResultValue DepressionFlag { get; set; }
        public string DepressionThinkOfDeathAnswer { get; set; }
        public ScreeningResultValue PartnerViolenceFlag { get; set; }

        public ScreeningResultValue ProblemGamblingFlag { get; set; } // BBGS

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

        public bool IsCompleted
        {
            get
            {
                return CompleteDate.HasValue;
            }
        }
    }
}
