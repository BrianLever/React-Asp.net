using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk
{
    public class ScreeningSection
    {
        #region properties
        public string ScreeningSectionID { get; set; }
        public string ScreeningID { get; set; }
        public string ScreeningSectionName { get; set; }
        public string ScreeningSectionShortName { get; set; }
        //public string QuestionText { get; set; }
        
        #endregion


        public List<ScreeningSectionQuestion> Questions = new List<ScreeningSectionQuestion>();

        public List<ScreeningSectionQuestion> MainSectionQuestions
        {
            get { return Questions.Where(x => x.IsMainQuestion).ToList(); }
        }
        public List<ScreeningSectionQuestion> NotMainSectionQuestions
        {
            get { return Questions.Where(x => !x.IsMainQuestion).ToList(); }
        }

        /// <summary>
        /// Find questions in the Questions collection by id
        /// </summary>
        /// <param name="questionID"></param>
        /// <returns>Question object if found or null.</returns>
        public ScreeningSectionQuestion FindQuestionByID(int questionID)
        {
            ScreeningSectionQuestion question = null;
            if (this.Questions.Count > 0)
            {
                int index = 0;
                for (; index < this.Questions.Count; index++)
                {
                    if (this.Questions[index].QuestionID == questionID)
                    {
                        question = this.Questions[index];
                        break;
                    }
                }
            }
            return question;
        }
    }
}
