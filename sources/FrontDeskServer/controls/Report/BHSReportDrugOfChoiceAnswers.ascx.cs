using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Common;
using FrontDesk.Server.Data.BhsVisits;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHSReportDrugOfChoiceAnswersControl : BHSReportGenericSectionAnswers
    {
        private Lazy<IList<LookupValue>> answerValues = new Lazy<IList<LookupValue>>(() => new LookupListsDataSource().GetDrugOfChoice());


        public override string ScreeningSectionID
        {
            get { return ScreeningSectionDescriptor.DrugOfChoice; }
        }

        protected override IList<ScreeningSectionQuestion> GetMainSectionQuestionsDataSource()
        {
            return new List<ScreeningSectionQuestion>();
        }

        protected override IList<ScreeningSectionQuestion> GetQuestionsDataSource()
        {
            return new List<ScreeningSectionQuestion>
            {
                new ScreeningSectionQuestion{ QuestionID = 1, QuestionText = "Primary"},
                new ScreeningSectionQuestion{ QuestionID = 2, QuestionText = "Secondary"},
                new ScreeningSectionQuestion{ QuestionID = 3, QuestionText = "Tertiary"}
            };
        }

        protected override Repeater GetSectionAnswerRepeater()
        {
            return rptSectionAnswers;
        }

        protected void Page_Init(object sender, EventArgs e)
        {

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected override void SetFormData()
        {

        }

        protected override void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {


            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var questionInfo = e.Item.DataItem as ScreeningSectionQuestion;

                if (questionInfo == null) return;

                //answer can be null
                var answer = _screeningSectionResult.FindQuestionByID(questionInfo.QuestionID);


                {
                    var item = e.Item;
                    Label ltrAnswerText = null;
                    Literal ltrQuestion = null;
                    Literal ltrNo = null;


                    ltrQuestion = item.FindControl("ltrQuestion") as Literal;
                    if (ltrQuestion != null)
                    {
                        ltrQuestion.Text = questionInfo.QuestionText;
                    }
                    ltrNo = item.FindControl("ltrNo") as Literal;
                    if (ltrNo != null)
                    {
                        ltrNo.Text = string.Format("{0}.", AnswerListIndex++);
                    }

                    ltrAnswerText = item.FindControl("ltr") as Label;

                    if (ltrAnswerText != null)
                    {
                        ltrAnswerText.Text = answer != null ? answerValues.Value.FirstOrDefault(x => x.Id == answer.AnswerValue).Name
                            : answerValues.Value.First().Name;

                    }
                }
            }

        }

    }
}
