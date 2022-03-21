using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHSReportAlcoholAnswersControl : BHSReportGenericSectionAnswers
    {

        public override string ScreeningSectionID
        {
            get { return ScreeningSectionDescriptor.Alcohol; }
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
