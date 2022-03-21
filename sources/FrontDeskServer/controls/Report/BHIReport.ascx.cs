using System;
using System.Web.UI;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using Resources;
using FrontDesk.Common.Debugging;
using FrontDesk.Server.Messages;
using FrontDesk.Server.Screening.Services;
using Common.Logging;

namespace FrontDesk.Server.Web.Controls
{
    public partial class BHIReportControl : UserControl
    {
        private readonly IBranchLocationService _branchLocationService = new BranchLocationService();
        private ILog _logger = LogManager.GetLogger<BHIReportControl>();

        #region Filter Params

        public int? BranchLocationID = null;
        public DateTime? StartDate = null;
        public DateTime? EndDate = null;
        public bool RenderUniquePatientsReportType = true;
        
        #endregion

        public IndicatorReportViewModel model = null;

        #region Report UI binding
        protected void Page_Init(object sender, EventArgs e)
        {
            //rptSectionAnswers.ItemDataBound += new RepeaterItemEventHandler(rptSectionAnswers_ItemDataBound);
        }

        #endregion

        
        FrontDesk.Screening _screeningInfo = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            _screeningInfo = ServerScreening.Get();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindReportData();

            ctrlTCCSection.Model = model.TobaccoSection;
            
            ctrlCAGESection.Model = model.AlcoholSection;

            ctrlPHQ9Section.Model = model.DepressionSectionPhq9;
            ctrlPHQ9Section.Model.MainQuestions.Clear(); //do not show main questions

            ctrlPHQ2Section.Model = model.DepressionSectionPhq2;
            ctrlPHQ2Section.Model.MainQuestions.Clear(); //do not show main questions


            ctrlGAD7Section.Model = model.AnxietySectionPhq7;
            ctrlGAD7Section.Model.MainQuestions.Clear(); //do not show main questions

            ctrlGAD2Section.Model = model.AnxietySectionGad2;
            ctrlGAD2Section.Model.MainQuestions.Clear(); //do not show main questions

            ctrlHITSSection.Model = model.PartnerViolenceSection;
            ctrlDASTSection.Model = model.SubstanceAbuseSection;

            ctrlGamblingSection.Model = model.ProblemGamblingSection;
        }

        protected void BindReportData()
        {
            if (EndDate != null)
            {
                _logger.DebugFormat("Redering BHI Problems by Severity for: {0}", RenderUniquePatientsReportType ? "Unique Patients" : "Total Reports");

                var filter = new SimpleFilterModel
                {
                    Location = BranchLocationID,
                    StartDate = StartDate,
                    EndDate = EndDate.Value.AddDays(1)
                };

                model = ScreeningResultHelper.GetBhsIndicatorReport(_screeningInfo, filter, RenderUniquePatientsReportType);
            }
            ltrCreatedDate.Text = DateTimeOffset.Now.ToString("MM/dd/yyyy HH:mm zzz");

            if (BranchLocationID.HasValue)
            {
                ltrBranchLocation.Text = _branchLocationService.Get(BranchLocationID.Value).Name;
            }
            else
            {
                ltrBranchLocation.Text = TextMessages.DropDown_AllText;
            }

            ltrReportType.Text = RenderUniquePatientsReportType ? TextStrings.REPORT_INDICATOR_TYPE_UNIQUE_PATIENTS : TextStrings.REPORT_INDICATOR_TYPE_TOTAL_REPORTS;

            DateTimeOffset ? dbMinDate = ScreeningResultHelper.GetMinDate();

            if (StartDate == null && !dbMinDate.HasValue)
            {
                // no start date and no records in database: cannot show start date, show from today
                ltrReportPeriod.Text = String.Format("{0:MM/dd/yyyy} - {1:MM/dd/yyyy}", DateTime.Now, EndDate);
            }
            else
            {
                ltrReportPeriod.Text = String.Format("{0:MM/dd/yyyy} - {1:MM/dd/yyyy}",
                    //StartDate != null ? StartDate : ScreeningResultHelper.GetMinDate().DateTime, 
                    StartDate != null ? StartDate : dbMinDate.Value.DateTime,
                    EndDate);
            }
            
            //rptSectionAnswers.DataSource = model.BriefQuestionsSectionItems;

            ltrTotal.Text = model.TotalPatientScreenings.ToString();
            this.DataBind(); //bind all
        }
    }
}
