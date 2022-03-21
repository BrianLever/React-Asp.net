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
    [DataContract(Name = "ScreeningSectionResult", Namespace = "http://www.frontdeskhealth.com")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class ScreeningSectionResult
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
        /// Answer
        /// </summary>
        [DataMember]
        public int AnswerValue { get; set; }

        /// <summary>
        /// Score value
        /// </summary>
        /// <remarks>Score is calculated on server and application does not exchange this value with kiosks</remarks>
        [DataMember]
        public int? Score { get; set; }
        /// <summary>
        /// Reference to the Score level definition
        /// </summary>
        /// <remarks>Score is calculated on server and application does not exchange this value with kiosks</remarks>
        [DataMember]
        public int? ScoreLevel { get; set; }
        /// <summary>
        /// Score level name
        /// </summary>
        /// <remarks>Used for binding to UI</remarks>
        [DataMember]
        public string ScoreLevelLabel { get; set; }

        [DataMember]
        public string Indicates { get; internal set; }


        public string ScoreLevelDisplayHTML
        {
            get
            {
                return string.Format("<b>{0}{1}</b>{2}", this.ScoreLevelLabel, !string.IsNullOrEmpty(this.Indicates) ? ": " : "", this.Indicates);

            }
        }


        /// <summary>
        /// Text of the Main section question
        /// </summary>
        [IgnoreDataMember]
        public string QuestionText { get; set; }
        #endregion

        #region Questions
        /// <summary>
        /// Answer hash table, where hash key is Question ID
        /// </summary>
        [DataMember]
        Dictionary<int, ScreeningSectionQuestionResult> _answers = new Dictionary<int, ScreeningSectionQuestionResult>();
        /// <summary>
        /// Get section's question answers
        /// </summary>
        public List<ScreeningSectionQuestionResult> Answers
        {
            get { lock (_lockListObject) { return new List<ScreeningSectionQuestionResult>(_answers.Values); } }
        }
        
        public List<ScreeningSectionQuestionResult> GetAnswersOnSpecificQuestions(ICollection<int> questionIds)
        {
            if (questionIds == null) return new List<ScreeningSectionQuestionResult>();

            return _answers.Values.Where(x => questionIds.Contains(x.QuestionID)).ToList();

        }

        private object _lockListObject = new object();

        /// <summary>
        /// Add new question answer to the section screening result
        /// </summary>
        /// <param name="answer"></param>
        public ScreeningSectionResult AppendQuestionAnswer(ScreeningSectionQuestionResult answer)
        {
            lock (_lockListObject)
            {
                //check if answer is already in the collection

                if (_answers.ContainsKey(answer.QuestionID))
                {
                    Debug.Assert(true, "Duplicate answer ID in the section answers.");
                    _answers.Remove(answer.QuestionID);
                }
                else
                {
                    _answers.Add(answer.QuestionID, answer);
                    answer.ScreeningSectionID = this.ScreeningSectionID;
                    answer.ScreeningResultID = this.ScreeningResultID;
                }
            }
            return this; //fluent syntax support
        }

        /// <summary>
        /// Import answer results range
        /// </summary>
        /// <param name="answers"></param>
        public ScreeningSectionResult ImportQuestionAnswerRange(IEnumerable<ScreeningSectionQuestionResult> answers)
        {
            if (answers == null) return this;
            lock (_lockListObject)
            {
                //check if answer is already in the collection
                foreach (var answer in answers)
                {
                    if (_answers.ContainsKey(answer.QuestionID))
                    {
                        Debug.Assert(true, "Duplicate answer ID in the section answers.");
                        _answers.Remove(answer.QuestionID);
                    }
                    else
                    {
                        _answers.Add(answer.QuestionID, answer);
                        answer.ScreeningSectionID = this.ScreeningSectionID;
                        answer.ScreeningResultID = this.ScreeningResultID;
                    }
                }
            }

            return this; //fluent syntax support
        }


        /// <summary>
        /// Find answer in the Answers collection by id
        /// </summary>
        /// <param name="sectionID"></param>
        /// <returns>Section question answer object if found or null.</returns>
        public ScreeningSectionQuestionResult FindQuestionByID(int questionID)
        {
            ScreeningSectionQuestionResult answer = null;

            _answers.TryGetValue(questionID, out answer);

            return answer;
        }

        #endregion

        #region Constructors
        /// <summary>
        /// Default parameterless contructor
        /// </summary>
        public ScreeningSectionResult() { }

        // <summary>
        /// Constructor with required screening data
        /// </summary>
        public ScreeningSectionResult(string sectionID, int value)
        {
            ScreeningSectionID = sectionID;
            AnswerValue = value;
        }

        public ScreeningSectionResult(IDataReader reader)
        {
            ScreeningResultID = Convert.ToInt64(reader["ScreeningResultID"]);
            ScreeningSectionID = Convert.ToString(reader["ScreeningSectionID"]).TrimEnd();
            AnswerValue = Convert.ToInt32(reader["AnswerValue"]);
            Score = Convert.IsDBNull(reader["Score"]) ? (int?)null : Convert.ToInt32(reader["Score"]);
            ScoreLevel = Convert.IsDBNull(reader["ScoreLevel"]) ? (int?)null : Convert.ToInt32(reader["ScoreLevel"]);

            var fields = DBDatabase.GetReaderColumnNames(reader);

            if (fields.Contains("ScoreLevelLabel"))
            {
                ScoreLevelLabel = Convert.ToString(reader["ScoreLevelLabel"]);
            }

            if (fields.Contains("ScoreName"))
            {
                ScoreLevelLabel = Convert.IsDBNull(reader["ScoreName"]) ? String.Empty : Convert.ToString(reader["ScoreName"]);
            }

            if (fields.Contains("QuestionText"))
            {
                QuestionText = Convert.ToString(reader["QuestionText"]);
            }

            if (fields.Contains("Indicates"))
            {
                Indicates = Convert.IsDBNull(reader["Indicates"]) ? String.Empty : Convert.ToString(reader["Indicates"]);
            }

        }

        #endregion

        [OnDeserialized]
        void OnDeserialized(StreamingContext context)
        {
            if (this._lockListObject == null) this._lockListObject = new object();
        }
        /// <summary>
        /// True if section has any positive answer
        /// </summary>
        /// <returns></returns>
        public bool HasPositiveAnswers()
        {
            return _answers.Any(x => x.Value.AnswerValue > 0);
        }
    }
}
