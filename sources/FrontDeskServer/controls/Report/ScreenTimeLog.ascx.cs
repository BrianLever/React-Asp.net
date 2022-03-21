using System;

using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;

using FrontDeskServer.Controls;

using Resources;

namespace FrontDesk.Server.Web.Controls
{


    public partial class ScreenTimeLogControl : BHIReportByAgeBase
    {

        #region Filter Params

        public int? BranchLocationID = null;
        public DateTime? StartDate = null;
        public DateTime? EndDate = null;
        public bool RenderUniquePatientsReportType = true;


        #endregion

        public ScreeningTimeLogReportViewModel model = null;
        private readonly IScreeningTimeLogService _timeLogService = new ScreeningTimeLogService();
        private readonly IBranchLocationService _branchLocationService = new BranchLocationService();

        #region Report UI binding
        protected void Page_Init(object sender, EventArgs e)
        {
           
        }
       
        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindReportData();

            ctrlTimeMeasures.Model = model;
        }

        protected void BindReportData()
        {
            if (EndDate != null)
            {
                var filter = new SimpleFilterModel
                {
                    Location = BranchLocationID,
                    StartDate = StartDate,
                    EndDate = EndDate.Value.AddDays(1)
                };

                model = _timeLogService.GetReport(filter);

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


            DateTimeOffset? dbMinDate = ScreeningResultHelper.GetMinDate();

            if (StartDate == null && !dbMinDate.HasValue)
            {
                // no start date and no records in database: cannot show start date, show from today
                ltrReportPeriod.Text = String.Format("{0:MM/dd/yyyy} - {1:MM/dd/yyyy}", DateTime.Now, EndDate);
            }
            else
            {
                ltrReportPeriod.Text = String.Format("{0:MM/dd/yyyy} - {1:MM/dd/yyyy}",
                    StartDate != null ? StartDate : dbMinDate.Value.DateTime,
                    EndDate);
            }
            
            this.DataBind(); //bind all
        }
    }
}
        