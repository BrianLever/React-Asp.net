using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace FrontDesk
{
    /// <summary>
    /// Patient's screening section question result
    /// </summary>
    [Serializable()]
    [DataContract(Name = "ScreeningSectionQuestionResult", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningSectionQuestionResult
    {
        #region Properties
        /// <summary>
        /// Refarence to the Result's Header record
        /// </summary>
        [IgnoreDataMember]
        public long ScreeningResultID { get; set; }

        /// <summary>
        /// Reference to the Screening Section
        /// </summary>
        [DataMember]
        public string ScreeningSectionID { get; set; }
        /// <summary>
        /// Reference to the Section's Question id
        /// </summary>
        [DataMember]
        public int QuestionID { get; set; }

        /// <summary>
        /// Answer
        /// </summary>
        [DataMember]
        public int AnswerValue { get; set; }

        #endregion

        #region Constructors
        /// <summary>
        /// Default parameterless contructor
        /// </summary>
        public ScreeningSectionQuestionResult() { }

        // <summary>
        /// Constructor with required screening data
        /// </summary>
        public ScreeningSectionQuestionResult(string sectionID, int questionID, int value)
        {
            ScreeningSectionID = sectionID;
            QuestionID = questionID;
            AnswerValue = value;
        }
        // <summary>
        /// Constructor with required screening data
        /// </summary>
        public ScreeningSectionQuestionResult(int questionID, int value)
        {
            QuestionID = questionID;
            AnswerValue = value;
        }


        #endregion
    }
}
