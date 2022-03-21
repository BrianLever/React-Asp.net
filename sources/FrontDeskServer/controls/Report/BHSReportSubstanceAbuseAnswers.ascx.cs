using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHSReportSubstanceAbuseAnswersControl : BHSReportGenericSectionAnswers
    {

        public override string ScreeningSectionID
        {
            get { return ScreeningSectionDescriptor.SubstanceAbuse; }
        }

        protected override IList<ScreeningSectionQuestion> GetMainSectionQuestionsDataSource()
        {
            return new List<ScreeningSectionQuestion>();
        }

        protected override IList<ScreeningSectionQuestion> GetQuestionsDataSource()
        {
            return ScreeningSectionInfo.Questions;
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
