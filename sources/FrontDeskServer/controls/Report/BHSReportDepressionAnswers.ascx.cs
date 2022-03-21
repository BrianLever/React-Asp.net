using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Common.Screening;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHSReporDepressionAnswersControl : BHSReportGenericSectionAnswers
    {

        public override string ScreeningSectionID
        {
            get { return ScreeningSectionDescriptor.Depression; }
        }


        protected DepressionScreeningSectionResult _depressionScreeningResult;

        public override ScreeningResult ScreeningResult
        {
            get
            {
                return base.ScreeningResult;
            }

            set
            {
                base.ScreeningResult = value;
                _depressionScreeningResult = _screeningSectionResult.AsDepressionSection();
            }
        }


        protected override IList<ScreeningSectionQuestion> GetQuestionsDataSource()
	    {

            if(_depressionScreeningResult.IsPhq2Mode)
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

            if (_depressionScreeningResult.IsPhq2Mode) return;


            LastQuestionInfo = ScreeningSectionInfo.FindQuestionByID(10);
            LastQuestionAnswer = ScreeningSectionResult.FindQuestionByID(LastQuestionInfo.QuestionID);

            rptDifficulty.ItemDataBound += new RepeaterItemEventHandler(rptDifficulty_ItemDataBound);

            var list = new List<ScreeningSectionQuestion>(1);
            list.Add(LastQuestionInfo);
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
            return questionInfo.QuestionID == 10; //ignore last 10th question
        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
