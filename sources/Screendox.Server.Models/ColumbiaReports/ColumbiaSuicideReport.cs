using FrontDesk;
using FrontDesk.Common.Bhservice;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ColumbiaReports
{
    public class ColumbiaSuicideReport : BhsEntry, IScreeningPatientIdentityWithAddress
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

        public int? EhrPatientID { get; set; }
        public string EhrPatientHRN { get; set; }

        public int BranchLocationID { get; set; }
        public string BranchLocationName { get; set; }

        public long? ScreeningResultID { get; set; }
        public long? BhsVisitID { get; set; }

        /// <summary>
        /// Read only reference to the Demographics ID
        /// </summary>
        public long? DemographicsID { get; internal set; }


        public int? ScoreLevel { get; set; }
        public string ScoreLevelLabel { get; set; }


        public ColumbiaRiskAssessmentReport RiskAssessmentReport { get; set; } = new ColumbiaRiskAssessmentReport();


        /* SUICIDAL IDEATION */
        public ColumbiaSuicidalIdeation WishToDead { get; set; } = new ColumbiaSuicidalIdeation
        {
            QuestionId = ColumbiaSuicidalIdeationQuesions.WithTobeDead,
        };

        public ColumbiaSuicidalIdeation NonSpecificActiveSuicidalThoughts { get; set; } = new ColumbiaSuicidalIdeation
        {
            QuestionId = ColumbiaSuicidalIdeationQuesions.NonSpecificActiveSuicidalThoughts,
        };

        public ColumbiaSuicidalIdeation ActiveSuicidalIdeationWithAnyMethods { get; set; } = new ColumbiaSuicidalIdeation
        {
            QuestionId = ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithAnyMethods,

        };


        public ColumbiaSuicidalIdeation ActiveSuicidalIdeationWithSomeIntentToAct { get; set; } = new ColumbiaSuicidalIdeation
        {
            QuestionId = ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithSomeIntentToAct
        };

        public ColumbiaSuicidalIdeation ActiveSuicidalIdeationWithSpecificPlanAndIntent { get; set; } = new ColumbiaSuicidalIdeation
        {
            QuestionId = ColumbiaSuicidalIdeationQuesions.ActiveSuicidalIdeationWithSpecificPlanAndIntent,
        };



        /* INTENSITY OF IDEATION */

        public ColumbiaIntensityIdeation Frequency { get; set; } = new ColumbiaIntensityIdeation
        {
            QuestionId = ColumbiaIntensityOfIdeationQuesions.Frequency,
        };

        public ColumbiaIntensityIdeation Duration { get; set; } = new ColumbiaIntensityIdeation
        {
            QuestionId = ColumbiaIntensityOfIdeationQuesions.Duration,
        };

        public ColumbiaIntensityIdeation Controllability { get; set; } = new ColumbiaIntensityIdeation
        {
            QuestionId = ColumbiaIntensityOfIdeationQuesions.Controllability,
        };

        public ColumbiaIntensityIdeation Deterrents { get; set; } = new ColumbiaIntensityIdeation
        {
            QuestionId = ColumbiaIntensityOfIdeationQuesions.Deterrents,
        };

        public ColumbiaIntensityIdeation ReasonsForIdeation { get; set; } = new ColumbiaIntensityIdeation
        {
            QuestionId = ColumbiaIntensityOfIdeationQuesions.ReasonsForIdeation,
        };

        public int? LifetimeMostSevereIdeationLevel { get; set; }
        public string LifetimeMostSevereIdeationDescription { get; set; }

        public int? RecentMostSevereIdeationLevel { get; set; }
        public string RecentMostSevereIdeationDescription { get; set; }



        /* SUICIDE BEHAVIOR */
        public ColumbiaSuicialBehaviorAct ActualAttempt { get; set; } = new ColumbiaSuicialBehaviorAct
        {
            QuestionId = ColumbiaSuicideBehaviorActQuesions.ActualAttempt
        };

        public ColumbiaSuicialBehaviorAct HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior { get; set; } = new ColumbiaSuicialBehaviorAct
        {
            QuestionId = ColumbiaSuicideBehaviorActQuesions.HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior
        };

        public ColumbiaSuicialBehaviorAct InterruptedAttempt { get; set; } = new ColumbiaSuicialBehaviorAct
        {
            QuestionId = ColumbiaSuicideBehaviorActQuesions.InterruptedAttempt
        };

        public ColumbiaSuicialBehaviorAct AbortedAttempt { get; set; } = new ColumbiaSuicialBehaviorAct
        {
            QuestionId = ColumbiaSuicideBehaviorActQuesions.AbortedAttempt
        };

        public ColumbiaSuicialBehaviorAct PreparatoryActs { get; set; } = new ColumbiaSuicialBehaviorAct
        {
            QuestionId = ColumbiaSuicideBehaviorActQuesions.PreparatoryActs
        };


        public DateTime? SuicideMostRecentAttemptDate { get; set; }
        public DateTime? SuicideMostLethalRecentAttemptDate { get; set; }
        public DateTime? SuicideFirstAttemptDate { get; set; }

        public ColumbiaSuicialBehaviorLethality ActualLethality { get; set; } = new ColumbiaSuicialBehaviorLethality
        {
            QuestionId = ColumbiaSuicideBehaviorLethalityQuesions.ActualLethality
        };

        public ColumbiaSuicialBehaviorLethality PotentialLethality { get; set; } = new ColumbiaSuicialBehaviorLethality
        {
            QuestionId = ColumbiaSuicideBehaviorLethalityQuesions.PotentialLethality
        };

    }

    public static class ColumbiaSuicidalIdeationQuesions
    {
        public static int WithTobeDead = 1;
        public static int NonSpecificActiveSuicidalThoughts = 2;
        public static int ActiveSuicidalIdeationWithAnyMethods = 3;
        public static int ActiveSuicidalIdeationWithSomeIntentToAct = 4;
        public static int ActiveSuicidalIdeationWithSpecificPlanAndIntent = 5;
    }

    public static class ColumbiaIntensityOfIdeationQuesions
    {
        public static int Frequency = 1;
        public static int Duration = 2;
        public static int Controllability = 3;
        public static int Deterrents = 4;
        public static int ReasonsForIdeation = 5;
    }


    public static class ColumbiaSuicideBehaviorActQuesions
    {
        public static int ActualAttempt = 1;
        public static int HasSubjectEngagedInNonSuicidalSelfInjuriousBehavior = 2;
        public static int InterruptedAttempt = 3;
        public static int AbortedAttempt = 4;
        public static int PreparatoryActs = 5;
    }

    public static class ColumbiaSuicideBehaviorLethalityQuesions
    {
        public static int ActualLethality = 1;
        public static int PotentialLethality = 2;
    }

    public class ColumbiaSuicidalIdeation
    {
        public int QuestionId { get; set; }
        public bool? LifetimeMostSucidal { get; set; }
        public bool? PastLastMonth { get; set; }
        public string Description { get; set; }
    }

    public class ColumbiaIntensityIdeation
    {
        public int QuestionId { get; set; }
        /// <summary>
        /// 1 to 5 options
        /// </summary>
        public int? LifetimeMostSevere { get; set; }
        /// <summary>
        /// 1 - 5 options
        /// </summary>
        public int? RecentMostSevere { get; set; }
    }


    public class ColumbiaSuicialBehaviorAct
    {
        public int QuestionId { get; set; }
        public bool? LifetimeLevel { get; set; }
        public int? LifetimeCount { get; set; }
        public bool? PastThreeMonths { get; set; }
        public int? PastThreeMonthsCount { get; set; }
        public string Description { get; set; }
    }


    public class ColumbiaSuicialBehaviorLethality
    {
        public int QuestionId { get; set; }
        /// <summary>
        /// 0 - 3
        /// </summary>
        public int? MostRecentAttemptCode { get; set; }
        /// <summary>
        /// 0 - 3
        /// </summary>
        public int? MostLethalAttemptCode { get; set; }
        /// <summary>
        /// 0 - 3
        /// </summary>
        public int? InitialAttemptCode { get; set; }
    }
}


