using System;
using System.Web.UI.WebControls;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDeskServer.Controls;
using Resources;
using FrontDesk.Server.Messages;
using FrontDesk.Server.Web.Formatters;

namespace FrontDesk.Server.Web.Controls
{
    

    public partial class BHIPatientDemographics : BHIReportByAgeBase
    {

        #region Filter Params

        public int? BranchLocationID = null;
        public DateTime? StartDate = null;
        public DateTime? EndDate = null;
        public bool RenderUniquePatientsReportType = true;


        #endregion

        public BhsDemographicsReportByAgeViewModel model = null;
        private readonly BhsDemographicsIndicatorReportService _indicatorReportService = new BhsDemographicsIndicatorReportService();

        #region Report UI binding
        protected void Page_Init(object sender, EventArgs e)
        {
           
        }
       
        protected void rptSectionAnswers_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                RenderPositiveScreeningsByAge(e.Item.DataItem as IndicatorReportByAgeItemViewModel, e.Item);
            }
           
        }

        protected void RenderPositiveScreeningsByAge(IndicatorReportByAgeItemViewModel dataItem, RepeaterItem container)
        {
            if (dataItem == null) return;

            var ltrQuestion = container.FindControl("ltCategory") as Label;
            
            var placeHolder = container.FindControl("plhAgeValues") as PlaceHolder;

            if (ltrQuestion != null)
            {
                ltrQuestion.Text = dataItem.ScreeningSectionQuestion.FormatAsQuestionWithPreamble();

                if (ApplyPaddingForItemsStartingFromLine.HasValue &&
                _incrementalLineNumber >= ApplyPaddingForItemsStartingFromLine.Value)
                {
                    ltrQuestion.CssClass += " lpad";
                }

            }
            
            if (placeHolder != null && dataItem.PositiveScreensByAge != null)
            {
                foreach (var age in dataItem.PositiveScreensByAge)
                {
                    var ltrHtml = new Literal
                    {
                        Mode = LiteralMode.PassThrough,
                        Text = "<td>{0}</td>".FormatWith(age.Value),
                    };

                    placeHolder.Controls.Add(ltrHtml);
                }

                //render total
                placeHolder.Controls.Add(new Literal
                {
                    Mode = LiteralMode.PassThrough,
                    Text = "<td>{0}</td>".FormatWith(dataItem.Total),
                });

            }

            _incrementalLineNumber++;
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindReportData();

            ctrlRaceSection.Model = model.RaceSection;
            ctrlGenderSection.Model = model.GenderSection;
            ctrlSexualOrientationSection.Model = model.SexualOrientationSection;
            ctrlMaritalStatus.Model = model.MaritalStatusSection;
            ctrlEducationLevel.Model = model.EducationLevelSection;
            ctrlLeavingOnOrOffReservation.Model = model.LeavingOnOrOffReservationSection;
            ctrlMilitaryExperience.Model = model.MilitaryExperienceSection;


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

                model = _indicatorReportService.GetDemographicsReportByAge(
                   filter,
                    _ageGroupsProvider.AgeGroups,
                    RenderUniquePatientsReportType);

            }

            ltrCreatedDate.Text = DateTimeOffset.Now.ToString("MM/dd/yyyy HH:mm zzz");

            if (BranchLocationID.HasValue)
            {
                ltrBranchLocation.Text = branchLocationService.Get(BranchLocationID.Value).Name;
            }
            else
            {
                ltrBranchLocation.Text = TextMessages.DropDown_AllText;
            }

            ltrReportType.Text = RenderUniquePatientsReportType ? TextStrings.REPORT_INDICATOR_TYPE_UNIQUE_PATIENTS : TextStrings.REPORT_INDICATOR_TYPE_TOTAL_REPORTS;

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
        