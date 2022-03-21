using System.Collections.Generic;
using System.Linq;

namespace FrontDesk
{
    /// <summary>
    /// Keeps IDs for all supported screening sections
    /// </summary>
    public static class ScreeningSectionDescriptor
    {
        public const string Tobacco = "TCC";
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
        /// PHQ-2/PHQ-9 Screening Tool
        /// </summary>
        public const string Depression = "PHQ-9";


        /// <summary>
        /// PHQ-9 Allways Screening Tool
        /// </summary>
        public const string DepressionAllQuestions = "PHQ9A";



        /// <summary>
        /// GAD-2/GAD-7  Screening Tool
        /// </summary>
        public const string Anxiety = "GAD-7";


        /// <summary>
        /// GAD-7 Allways all questions Screening Tool
        /// </summary>
        public const string AnxietyAllQuestions = "GAD7A";


        /// <summary>
        /// PHQ-2 section header
        /// </summary>
        public const string DepressionPhq2Name = "Depression (PHQ-2)";

        public const string DepressionPhq2ID = "PHQ-2";


        /// <summary>
        /// GAD-2 section header
        /// </summary>
        public const string AnxietyGad2Name = "Anxiety (GAD-2)";

        public const string AnxietyGad2ID = "GAD-2";

        /// <summary>
        /// HITS Screening Tool
        /// </summary>
        public const string PartnerViolence = "HITS";

        /// <summary>
        /// DOCH Screening Tool
        /// </summary>
        public const string DrugOfChoice = "DOCH";

        /// <summary>
        /// Contact Info
        /// </summary>
        public const string ContactInfo = "CIF";

        /// <summary>
        /// Patient Demographcs
        /// </summary>
        public const string Demographics = "DMGR";


        /// <summary>
        /// Problem Gambling
        /// </summary>
        public const string ProblemGambling = "BBGS";


        public const string DepressionDifficulty = "PHQ-9 Difficulty";
        public const string DepressionThinkOfDeath = "PHQ-9 Death";
        public const int DepressionThinkOfDeathQuestionID = 9;
        public const string DrugOfChoiceSectionSettingsName = "Drug Use List"; //name shown as section name on the settings

        public static readonly string[] KnownScreeningSections = new string[]{
                    Tobacco,
                    SmokerInHome,
                    Alcohol,
                    Depression,
                    PartnerViolence,
                    SubstanceAbuse,
                    DrugOfChoice,
                    Anxiety,
                    ProblemGambling
        };

        /// <summary>
        /// Get all screening sections for model validations
        /// </summary>
        public static ICollection<string> AllScreeningSections
        {
            get
            {
                var list = new List<string>(KnownScreeningSections);
                list.AddRange(AlternativeOptionalMandatorySections.Select(x => x.AllQuestions));
                list.Add(ContactInfo);
                list.Add(Demographics);

                return list;
            }
        }

        public static readonly (string Primary, string AllQuestions)[] AlternativeOptionalMandatorySections = new[]
        {
            (Depression, DepressionAllQuestions),
            (Anxiety, AnxietyAllQuestions )
        };

    }

    public static class TobaccoQuestionsDescriptor
    {
        public const int UseForCeremonyOnlyQuestionID = 1;
        public const int DoYouSmokeQuestionID = 2;
        public const int DoYouSmokeSmokelessQuestionID = 3;

        public const string CeremonialUseOnly = "Ceremonial Use Only";
        public const string CurrentSmoker = "Current Smoker";
        public const string SmokerInTheHome = "Smoker in Home";
        public const string SmokeFreeHome = "Smoke Free Home";
        public const string CurrentNonSmoker = "Current Non-smoker";

    }


    /// <summary>
    /// HITS-9 section meta
    /// </summary>
    public static class PartnerViolenceQuestionsDescriptor
    {
        public const int PhysicallyHurtYouQuestion = 1;
        public const int NeverAnswer = 1;
        public const int RarelyAnswer = 2;
        public const int SometimesAnswer = 3;
        public const int FairlyOftenAnswer = 4;
        public const int FrequentlyAnswer = 5;

        public static Dictionary<int, string> AnswerTexts = new Dictionary<int, string>
        {
            {NeverAnswer, "Never"},
            {RarelyAnswer, "Rarely"},
            {SometimesAnswer, "Sometimes"},
            {FairlyOftenAnswer, "Fairly Often"},
            {FrequentlyAnswer, "Frequently"}
        };
    }
    /// <summary>
    /// PHQ-9 section meta
    /// </summary>
    public static class DepressionQuestionsDescriptor
    {
        public const int HurtYouselfQuestion = 9;
        public const int NotAtAllAnswer = 0;
        public const int SeveralDaysAnswer = 1;
        public const int MoreThanHalfTheDaysAnswer = 2;
        public const int NearlyEveryDayAnswer = 3;

        public static Dictionary<int, string> AnswerTexts = new Dictionary<int, string>
        {
            {NotAtAllAnswer, "Not at all"},
            {SeveralDaysAnswer, "Several days"},
            {MoreThanHalfTheDaysAnswer, "More than half the days"},
            {NearlyEveryDayAnswer, "Nearly every day"}
        };
    }

    public static class DrugOfChoiceDescriptor
    {
        public const int PrimaryQuestionId = 1;
        public const int SecondaryQuestionId = 2;
        public const int TertiaryQuestionId = 3;
    }
}
