using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web.Controls;
using FrontDesk.Server;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Web;

namespace UI
{
    public partial class GPRAPeriodTimeRangeControl : BaseUserControl
    {
        #region Properties


        public DateTime? CustomStartDate
        {
            get
            {
                bool isFound;
                DateTime value = this.GetIDValue<DateTime>("CustomDateFrom", out isFound);
                if (!isFound)
                    return DateTime.Today;
                else
                {
                    return (DateTime)value;
                }
            }
            set
            {
                ViewState["CustomDateFrom"] = value;
                Session["CustomDateFrom"] = value;
            }
        }

        public DateTime? CustomEndDate
        {
            get
            {
                bool isFound;
                DateTime value = this.GetIDValue<DateTime>("CustomDateEnd", out isFound);
                if (!isFound)
                    return DateTime.Today;
                else
                {
                    return (DateTime)value;
                }
            }
            set
            {
                ViewState["CustomDateEnd"] = value;
                Session["CustomDateEnd"] = value;
            }
        }

        public GPRATimePeriods PeriodType
        {
            get
            {
                bool isFound;
                GPRATimePeriods value = (GPRATimePeriods)this.GetIDValue<int>("PeriodType", out isFound);
                if (!isFound)
                    return GPRATimePeriods.GPRA;
                else
                {
                    return (GPRATimePeriods)value;
                }
            }
            set
            {
                ViewState["PeriodType"] = (int)value;
                Session["PeriodType"] = (int)value;
            }
        }

        public GPRAReportingTime GrpaYear
        {
            get
            {
                bool isFound;
                int year = this.GetIDValue<int>("GrpaYear", out isFound);
                if (!isFound)
                    return GPRAReportingTime.Current;
                else
                {
                    return new GPRAReportingTime(year);
                }
            }
            set
            {
                if (value != null)
                {
                    int year = value.Year;
                    ViewState["GrpaYear"] = year;
                    Session["GrpaYear"] = year;
                }
                else
                {
                    ViewState["GrpaYear"] = null;
                    Session["GrpaYear"] = null;
                }
            }
        }

        /// <summary>
        /// Get resulted start date
        /// </summary>
        public DateTime? StartDate
        {
            get
            {
                if (this.PeriodType == GPRATimePeriods.Custom)
                {
                    return this.CustomStartDate;
                }
                else
                {
                    return this.GrpaYear.StartDate;
                }
            }
        }

        /// <summary>
        /// Get resulted end date
        /// </summary>
        public DateTime? EndDate
        {
            get
            {
                if (this.PeriodType == GPRATimePeriods.Custom)
                {
                    return this.CustomEndDate;
                }
                else
                {
                    return this.GrpaYear.EndDate;
                }
            }
        }


        #endregion


        protected void Page_Init(object sender, EventArgs e)
        {
            cmbGpraPeriods.DataSource = GPRAReportingTime.GetPeriodsSince((ScreeningResultHelper.GetMinDate() ?? DateTimeOffset.Now).LocalDateTime);
            cmbGpraPeriods.DataTextField = "Label";
            cmbGpraPeriods.DataValueField = "Year";
            cmbGpraPeriods.DataBind();



        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                GetValuesFromForm();
            }
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            BindUserControls();
        }

        #region State management methods

        private void GetValuesFromForm()
        {

            var period = rbtGpra.Checked ? GPRATimePeriods.GPRA : GPRATimePeriods.Custom;
            this.PeriodType = period;

            if (period == GPRATimePeriods.Custom)
            {
                this.CustomStartDate = dpStartDate.SelectedDate;
                this.CustomEndDate = dpEndDate.SelectedDate;
                //this.GrpaYear = null;
            }
            else
            {

                //this.CustomStartDate = null;
                //this.CustomEndDate = null; ;
                this.GrpaYear = new GPRAReportingTime(Int32.Parse(cmbGpraPeriods.SelectedValue));
            }
        }

        public void Reset()
        {
            Reset(null, null);
        }


        public void Reset(DateTime? customStartDate, DateTime? customEndDate)
        {
            //set default values
            this.PeriodType = GPRATimePeriods.GPRA;
            this.CustomStartDate = customStartDate;
            this.CustomEndDate = customEndDate;
            this.GrpaYear = null;

            BindUserControls();

            //reset UI
            Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(dpStartDate.ClientID, HTMLControlType.Textbox, dpStartDate.Text));
            Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(dpEndDate.ClientID, HTMLControlType.Textbox, dpEndDate.Text));
            Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(cmbGpraPeriods.ClientID, HTMLControlType.Combobox, cmbGpraPeriods.SelectedIndex.ToString()));
            Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(rbtGpra.ClientID, HTMLControlType.Checkbox, "true"));

        }


        #endregion

        private void BindUserControls()
        {
            rbtCustom.Checked = this.PeriodType == GPRATimePeriods.Custom;
            rbtGpra.Checked = !rbtCustom.Checked;

            dpStartDate.SelectedDate = this.CustomStartDate;
            dpEndDate.SelectedDate = this.CustomEndDate;
            cmbGpraPeriods.SelectedValue = this.GrpaYear.Year.ToString();

        }



        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
            this.rbtCustom.TabIndex = startTabIndex++;
            this.dpStartDate.TabIndex = startTabIndex++;
            this.dpEndDate.TabIndex = startTabIndex++;

            this.rbtGpra.TabIndex = startTabIndex++;
            this.cmbGpraPeriods.TabIndex = startTabIndex++;

        }

        public enum GPRATimePeriods
        {
            Custom = 0,
            GPRA = 1
        }
    }
}