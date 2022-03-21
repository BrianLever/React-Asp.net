using System;

namespace FrontDesk.Server.Screening.Models
{
    public static class VisitSettingsDescriptor
    {
        public const string TobaccoUseCeremony = "TCC1";
        public const string TobaccoUseSmoking = "TCC2";
        public const string TobaccoUseSmokeless = "TCC3";

        public const string SmokerInHome = "SIH";
        /// <summary>
        /// CAGE Screening Tool
        /// </summary>
        public const string Alcohol = "CAGE";
        /// <summary>
        /// DAST-10 Screening Tool
        /// </summary>
        public const string SubstanceAbuse = "DAST";
        /// <summary>
        /// DAST-10 Drug's of Choice
        /// </summary>
        public const string DrugOfChoice = "DOCH";
        /// <summary>
        /// PHQ-9 Screening Tool
        /// </summary>
        public const string Depression = "PHQ1";
        public const string DepressionThinkOfDeath = "PHQ2";


        /// <summary>
        /// GAD-7 (Anxiety) Screening Tool
        /// </summary>
        public const string Anxiety = "GAD-7";


        /// <summary>
        /// HITS Screening Tool
        /// </summary>
        public const string PartnerViolence = "HITS";


        /// <summary>
        /// BBGS (Problem Gambling) Screening Tool
        /// </summary>
        public const string ProblemGambling = "BBGS";


        public static readonly string[] KnownMeasurements = new string[]{
                    TobaccoUseCeremony,
                    TobaccoUseSmokeless,
                    TobaccoUseSmoking,
                    SmokerInHome,
                    Alcohol,
                    Depression,
                    DepressionThinkOfDeath,
                    PartnerViolence,
                    SubstanceAbuse,
                    Anxiety,
                    ProblemGambling
        };

        public static int MapTobaccoSettingToTobaccoQuestion(string tobaccoSettingId)
        {
            switch(tobaccoSettingId)
            {
                case TobaccoUseCeremony:
                    return TobaccoQuestionsDescriptor.UseForCeremonyOnlyQuestionID;
                case TobaccoUseSmoking:
                    return TobaccoQuestionsDescriptor.DoYouSmokeQuestionID;
                case TobaccoUseSmokeless:
                    return TobaccoQuestionsDescriptor.DoYouSmokeSmokelessQuestionID;

            }
            throw new ArgumentOutOfRangeException("Unknown tobacco setting ID: " + tobaccoSettingId);
        }
    }
}
