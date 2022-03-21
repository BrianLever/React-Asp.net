using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using FrontDesk.Common.Data;
using System.Linq;

namespace FrontDesk
{
    /// <summary>
    /// Patient's screening section result
    /// </summary>
    [Serializable()]
    [DataContract(Name = "DepressionScreeningSectionResult", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class DepressionScreeningSectionResult : ScreeningSectionResult
    {

        public bool IsPhq2Mode
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
                if (IsPhq2Mode)
                {
                    return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_PHQ2;
                }

                return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_PHQ9;
            }
        }

        public string ScreeningSectionShortName
        {
            get
            {
                if (IsPhq2Mode)
                {
                    return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_ID_PHQ2;
                }

                return FrontDesk.Common.Resources.ScreeningLabels.Screening_Report_Section_ID_PHQ9;
            }
        }


        #region Constructors
        /// <summary>
        /// Default parameterless contructor
        /// </summary>
        public DepressionScreeningSectionResult() : base() { }

        // <summary>
        /// Constructor with required screening data
        /// </summary>
        public DepressionScreeningSectionResult(ScreeningSectionResult sectionResult)
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
