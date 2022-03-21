using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHSReportTobaccoAnswersControl : BHSReportGenericSectionAnswers
    {

        private string _screeningSectionID = ScreeningSectionDescriptor.Tobacco;

        public override string ScreeningSectionID
        {
            get { return _screeningSectionID; }
        }

        public override ScreeningResult ScreeningResult
        {
            get
            {
                return base.ScreeningResult;
            }

            set
            {
                base.ScreeningResult = value;

                
                //if tobacco is empty
                if (_screeningSectionResult == null)
                {
                    //tobacco is empty, switch to SIH
                    _screeningSectionID = ScreeningSectionDescriptor.SmokerInHome;
                    
                    _screeningSectionResult = _screeningResult.FindSectionByID(_screeningSectionID);
                    
                }
            }
        }

        protected override IList<ScreeningSectionQuestion> GetMainSectionQuestionsDataSource()
        {
            List<ScreeningSectionQuestion> result = new List<ScreeningSectionQuestion>();
            if (_screeningSectionID != ScreeningSectionDescriptor.SmokerInHome &&
                _screeningResult.FindSectionByID(ScreeningSectionDescriptor.SmokerInHome) != null) //add SIH only when user has been asked question from this section
            {
                result = this.ScreeningInfo.FindSectionByID(ScreeningSectionDescriptor.SmokerInHome).MainSectionQuestions;
                
            }
            result.AddRange(base.GetMainSectionQuestionsDataSource());
            return result;
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
