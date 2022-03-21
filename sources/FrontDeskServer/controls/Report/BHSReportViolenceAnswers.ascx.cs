using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHSReportViolenceAnswersControl : BHSReportGenericSectionAnswers
    {

        public override string ScreeningSectionID
        {
            get { return ScreeningSectionDescriptor.PartnerViolence; }
        }

        protected override Repeater GetMainSectionAnswerRepeater()
        {
            return mainQuestionsRepeater;
        }

        protected override IList<ScreeningSectionQuestion> GetMainSectionQuestionsDataSource()
        {
            var items = base.GetMainSectionQuestionsDataSource();

            foreach (var item in items)
            {
                item.QuestionText = item.PreambleText + " " + item.QuestionText;
            }
            return items;
        }

        protected override Repeater GetSectionAnswerRepeater()
        {
            return rptSectionAnswers;
        }

        protected override void SetFormData()
        {

        }


        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}
