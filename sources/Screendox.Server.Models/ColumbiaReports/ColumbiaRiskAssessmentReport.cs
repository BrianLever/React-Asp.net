using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ColumbiaReports
{
    public class ColumbiaRiskAssessmentReport
    {
        public long ID { get; set; }

        /* SUICIDAL AND SELF-INJURY BEHAVIOR (PAST WEEK) */

        public bool ActualSuicideAttempt { get; set; }
        public bool LifetimeActualSuicideAttempt { get; set; }
        public bool InterruptedSuicideAttempt { get; set; }
        public bool LifetimeInterruptedSuicideAttempt { get; set; }
        public bool AbortedSuicideAttempt { get; set; }
        public bool LifetimeAbortedSuicideAttempt { get; set; }
        public bool OtherPreparatoryActsToKillSelf { get; set; }
        public bool LifetimeOtherPreparatoryActsToKillSelf { get; set; }
        public bool ActualSelfInjuryBehaviorWithoutSuicideIntent { get; set; }
        public bool LifetimeActualSelfInjuryBehaviorWithoutSuicideIntent { get; set; }


        /* SUICIDAL IDEATION (MOST SEVERE IN PAST WEEK) */
        public bool WishToBeDead { get; set; }
        public bool SuicidalThoughts { get; set; }
        public bool SuicidalThoughtsWithMethod { get; set; }
        public bool SuicidalIntent { get; set; }
        public bool SuicidalIntentWithSpecificPlan { get; set; }

        /* ACTIVATING EVENTS (RECENT) */
        public bool RecentLoss { get; set; }
        public string DescribeRecentLoss { get; set; }
        public bool PendingIncarceration { get; set; }
        public bool CurrentOrPendingIsolation { get; set; }

        /* CLINICAL STATUS (RECENT) */
        public bool Hopelessness { get; set; }
        public bool Helplessness { get; set; }
        public bool FeelingTrapped { get; set; }
        public bool MajorDepressiveEpisode { get; set; }
        public bool MixedAffectiveEpisode { get; set; }
        public bool CommandHallucinationsToHurtSelf { get; set; }
        public bool HighlyImpulsiveBehavior { get; set; }
        public bool SubstanceAbuseOrDependence { get; set; }

        public bool AgitationOrSevereAnxiety { get; set; }

        public bool PerceivedBurdenOnFamilyOrOthers { get; set; }

        public bool ChronicPhysicalPain { get; set; }

        public bool HomicidalIdeation { get; set; }

        public bool AggressiveBehaviorTowardsOthers { get; set; }

        public bool MethodForSuicideAvailable { get; set; }
        public bool RefusesOrFeelsUnableToAgreeToSafetyPlan { get; set; }
        public bool SexualAbuseLifetime { get; set; }
        public bool FamilyHistoryOfSuicideLifetime { get; set; }


        /* TREATMENT HISTORY */
        public bool PreviousPsychiatricDiagnosesAndTreatments { get; set; }
        public bool HopelessOrDissatisfiedWithTreatment { get; set; }
        public bool NonCompliantWithTreatment { get; set; }
        public bool NotReceivingTreatment { get; set; }


        /* PROTECTIVE FACTORS (RECENT) */
        public bool IdentifiesReasonsForLiving { get; set; }
        public bool ResponsibilityToFamilyOrOthers { get; set; }
        public bool SupportiveSocialNetworkOrFamily { get; set; }
        public bool FearOfDeathOrDyingDueToPainAndSuffering { get; set; }
        public bool BeliefThatSuicideIsImmoral { get; set; }
        public bool EngagedInWorkOrSchool { get; set; }
        public bool EngagedWithPhoneWorker { get; set; }


        public List<string> OtherProtectiveFactors { get; set; } = new List<string>();
        public List<string> OtherRisksFactors { get; set; } = new List<string>();
        public string DescribeAnySuicidalSelfInjuriousOrAggressiveBehavior { get; set; }

    }
}
