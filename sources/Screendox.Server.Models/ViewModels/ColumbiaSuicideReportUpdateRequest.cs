using ScreenDox.Server.Models.ColumbiaReports;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class ColumbiaSuicideReportUpdateRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }

        public DateTime Birthday { get; set; }

        public string FullName { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }
        public string StateID { get; set; }
        public string StateName { get; set; }
        public string StreetAddress { get; set; }
        public string ZipCode { get; set; }

        public int BranchLocationID { get; set; }

        public ColumbiaRiskAssessmentReport RiskAssessmentReport { get; set; }


        /* SUICIDAL IDEATION */
        public ColumbiaSuicidalIdeation WishToDead { get; set; }

        public ColumbiaSuicidalIdeation NonSpecificActiveSuicidalThoughts { get; set; } 

        public ColumbiaSuicidalIdeation ActiveSuicidalIdeationWithAnyMethods { get; set; } 


        public ColumbiaSuicidalIdeation ActiveSuicidalIdeationWithSomeIntentToAct { get; set; }

        public ColumbiaSuicidalIdeation ActiveSuicidalIdeationWithSpecificPlanAndIntent { get; set; }



        /* INTENSITY OF IDEATION */

        public ColumbiaIntensityIdeation Frequency { get; set; }

        public ColumbiaIntensityIdeation Duration { get; set; }

        public ColumbiaIntensityIdeation Controllability { get; set; } 

        public ColumbiaIntensityIdeation Deterrents { get; set; }

        public ColumbiaIntensityIdeation ReasonsForIdeation { get; set; }

        public int? LifetimeMostSevereIdeationLevel { get; set; }
        public string LifetimeMostSevereIdeationDescription { get; set; }

        public int? RecentMostSevereIdeationLevel { get; set; }
        public string RecentMostSevereIdeationDescription { get; set; }



        /* SUICIDE BEHAVIOR */
        public ColumbiaSuicialBehaviorAct ActualAttempt { get; set; }

        public ColumbiaSuicialBehaviorAct HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior { get; set; }

        public ColumbiaSuicialBehaviorAct InterruptedAttempt { get; set; } = new ColumbiaSuicialBehaviorAct
        {
            QuestionId = ColumbiaSuicideBehaviorActQuesions.InterruptedAttempt
        };

        public ColumbiaSuicialBehaviorAct AbortedAttempt { get; set; }

        public ColumbiaSuicialBehaviorAct PreparatoryActs { get; set; }


        public DateTime? SuicideMostRecentAttemptDate { get; set; }
        public DateTime? SuicideMostLethalRecentAttemptDate { get; set; }
        public DateTime? SuicideFirstAttemptDate { get; set; }

        public ColumbiaSuicialBehaviorLethality ActualLethality { get; set; }

        public ColumbiaSuicialBehaviorLethality PotentialLethality { get; set; }
    }
}
