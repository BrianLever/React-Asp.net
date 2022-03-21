﻿using System;
using System.Web.UI.WebControls;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDeskServer.Controls;
using Resources;
using FrontDesk.Server.Messages;
using FrontDesk.Server.Configuration;

namespace FrontDesk.Server.Web.Controls
{


    public partial class BHIDrugOfChoiceReportByAgeControl : BhiBehavioralReportByAge
    {

        #region Filter Params

        public int? BranchLocationID = null;
        public DateTime? StartDate = null;
        public DateTime? EndDate = null;
        public bool RenderUniquePatientsReportType = true;


        #endregion

        public IndicatorReportByAgeViewModel model = null;
        private readonly IndicatorReportService _indicatorReportService = new IndicatorReportService();

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

        #endregion


        FrontDesk.Screening _screeningInfo = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            _screeningInfo = ServerScreening.Get();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindReportData();

            ctrlDrugOfChoiceSectionPrimary.Model = model.DrugOfChoicePrimaryAnswers;
            ctrlDrugOfChoiceSectionSecondary.Model = model.DrugOfChoiceSecondaryAnswers;
            ctrlDrugOfChoiceSectionTertiary.Model = model.DrugOfChoiceTertiaryAnswers;


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

                model = _indicatorReportService.GetDrugsOfChoiceByAge(
                    _screeningInfo,
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
