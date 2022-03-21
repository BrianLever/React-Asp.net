using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace FrontDesk
{
    /// <summary>
    /// Patient's screening section result
    /// </summary>
    [Serializable()]
    [DataContract(Name = "AnxietyScreeningSectionResult", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class AnxietyScreeningSectionResult : ScreeningSectionResult
    {

        public bool IsGad2Mode
        {
            get
            {
                return Answers.Count == 2;
            }
        }

        public string ScreeningSectionName
        {
            get
            {
                if (IsGad2Mode)
                {
                    return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_GAD2;
                }

                return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_GAD7;
            }
        }

        public string ScreeningSectionShortName
        {
            get
            {
                if (IsGad2Mode)
                {
                    return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_ID_GAD2;
                }

                return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_ID_GAD7;
            }
        }


        #region Constructors
        /// <summary>
        /// Default parameterless contructor
        /// </summary>
        public AnxietyScreeningSectionResult() : base() { }

        // <summary>
        /// Constructor with required screening data
        /// </summary>
        public AnxietyScreeningSectionResult(ScreeningSectionResult sectionResult)
        {
            sectionResult.Answers.ForEach(x => this.AppendQuestionAnswer(x));

            this.ScreeningResultID = sectionResult.ScreeningResultID;
            this.ScreeningSectionID = sectionResult.ScreeningSectionID;
            this.AnswerValue = sectionResult.AnswerValue;
            this.QuestionText = sectionResult.QuestionText;
            this.Score = sectionResult.Score;
            this.ScoreLevel = sectionResult.ScoreLevel;
            this.ScoreLevelLabel = sectionResult.ScoreLevelLabel;
            this.Indicates = sectionResult.Indicates;

        }
        #endregion
    }
}
