using FrontDesk.Common.Screening;

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHSReportAnxietyAnswersControl : BHSReportGenericSectionAnswers
    {

        public override string ScreeningSectionID
        {
            get { return ScreeningSectionDescriptor.Anxiety; }
        }


        protected AnxietyScreeningSectionResult _anxietyScreeningResult;

        public override ScreeningResult ScreeningResult
        {
            get
            {
                return base.ScreeningResult;
            }

            set
            {
                base.ScreeningResult = value;
                _anxietyScreeningResult = _screeningSectionResult.AsAnxietySection();
            }
        }


        protected override IList<ScreeningSectionQuestion> GetQuestionsDataSource()
        {
            if (_anxietyScreeningResult.IsGad2Mode)
            {
                return ScreeningSectionInfo.MainSectionQuestions;
            }

            return ScreeningSectionInfo.Questions;
        }

        protected override Repeater GetSectionAnswerRepeater()
        {
            return rptSectionAnswers;
        }

        protected override void SetFormData()
        {
            if (ScreeningSectionResult == null || ScreeningSectionInfo == null ||
                ScreeningSectionResult.Answers.Count <= 0)
            { return; }

            if (_anxietyScreeningResult.IsGad2Mode) return;


            var list = new List<ScreeningSectionQuestion>(1);
            rptDifficulty.ItemDataBound += new RepeaterItemEventHandler(rptDifficulty_ItemDataBound);

            LastQuestionInfo = ScreeningSectionInfo.FindQuestionByID(8);
            if (LastQuestionInfo != null) // if last question has been asked
            {
               
                LastQuestionAnswer = ScreeningSectionResult.FindQuestionByID(LastQuestionInfo.QuestionID);
                list.Add(LastQuestionInfo);

            }
            else
            {
                list.Add(new ScreeningSectionQuestion()); // leave empy when not asked
            }

            rptDifficulty.DataSource = list;
            rptDifficulty.DataBind();

        }

        void rptDifficulty_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                PerformReaderItemBinding(e.Item, LastQuestionAnswer, LastQuestionInfo);
            }
        }

        protected ScreeningSectionQuestionResult LastQuestionAnswer = null;
        protected ScreeningSectionQuestion LastQuestionInfo = null;


        protected override bool IgnoreQuestion(ScreeningSectionQuestionResult answer, ScreeningSectionQuestion questionInfo)
        {
            //return base.IgnoreQuestion(answer, questionInfo);
            return questionInfo.QuestionID == 8; //ignore last 10th question
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
