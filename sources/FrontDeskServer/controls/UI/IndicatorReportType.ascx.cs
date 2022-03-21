using System;
using FrontDesk.Server.Web.Controls;
using FrontDesk.Server.Web;

namespace UI
{
    public partial class IndicatorReportTypeControl : BaseUserControl
    {
        public IndicatorReportRenderType ReportType
        {
            get
            {
                bool isFound;
                IndicatorReportRenderType value = (IndicatorReportRenderType)this.GetIDValue<int>("ReportType", out isFound);
                if (!isFound)
                    return DefaultValue;
                else
                {
                    return value;
                }
            }
            set
            {
                ViewState["ReportType"] = (int)value;
                Session["ReportType"] = (int)value;
            }
        }


        public virtual IndicatorReportRenderType DefaultValue { get; set; } 

        protected void Page_Init(object sender, EventArgs e)
        {
            DefaultValue = IndicatorReportRenderType.UniquePatients;
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
            ReportType = rbtUniquePatients.Checked ? IndicatorReportRenderType.UniquePatients : IndicatorReportRenderType.TotalReports;
        }
        public void Reset()
        {
            //set default values
            this.ReportType = IndicatorReportRenderType.UniquePatients;

            BindUserControls();

            //reset UI
            Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(rbtUniquePatients.ClientID, HTMLControlType.Checkbox, "true"));
        }


        #endregion

        private void BindUserControls()
        {
            rbtUniquePatients.Checked = this.ReportType == IndicatorReportRenderType.UniquePatients;
            rbtTotalPatients.Checked = !rbtUniquePatients.Checked;

        }



        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
            this.rbtUniquePatients.TabIndex = startTabIndex++;
            this.rbtTotalPatients.TabIndex = startTabIndex++;
        }

        public enum IndicatorReportRenderType
        {
            UniquePatients = 0,
            TotalReports = 1
        }
    }
}