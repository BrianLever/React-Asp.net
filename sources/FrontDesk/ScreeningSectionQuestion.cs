using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk
{
    /// <summary>
    /// Questions in the Screening Question Section
    /// </summary>
    public class ScreeningSectionQuestion
    {
        #region properties

        public int QuestionID { get; set; }

        public string ScreeningSectionID { get; set; }

        public string PreambleText { get; set; }

        public string QuestionText { get; set; }


        public int AnswerScaleID { get; set; }

        public bool IsMainQuestion { get; set; }

        public bool ShowOnlyWhenPossitiveScore { get; set; } = false;

        public int IndexOrder { get; set; }

        #endregion


        private volatile object _lockAnswersObject = new object();

        private List<AnswerScaleOption> _answerOptions = null;

        //private List<AnswerScaleOption> _answerOptions = null;
        /// <summary>
        /// Get answer options
        /// </summary>
        public List<AnswerScaleOption> AnswerOptions
        {
            get
            {
                if (_answerOptions == null)
                {
                    lock (_lockAnswersObject)
                    {
                        if (_answerOptions == null)
                        {
                            _answerOptions = AnswerScaleCacheManager.Instance.GetAnswerOptions(this.AnswerScaleID);
                        }
                    }
                }
                return _answerOptions;
            }
            set
            {
                _answerOptions = value;
            }
        }
    }
}
