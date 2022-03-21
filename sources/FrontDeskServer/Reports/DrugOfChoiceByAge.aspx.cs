using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Reports;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

public partial class DrugOfChoiceReportByAge : IndicatorReportBasePage
{
    public bool IsUniquePatientMode
    {
        get
        {
            return ucReportType.ReportType == UI.IndicatorReportTypeControl.IndicatorReportRenderType.UniquePatients;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsLocations.TypeName = typeof(BranchLocationService).FullName;
        odsrYears.TypeName = typeof(ScreeningResultHelper).FullName;


        upd.Triggers.Add(new AsyncPostBackTrigger
        {
            ControlID = btnApply.UniqueID,
            EventName = "Click",
        });
        upd.Triggers.Add(new AsyncPostBackTrigger
        {
            ControlID = btnClear.UniqueID,
            EventName = "Click",
        });
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Drug Use Results by Age";
        if (!HasWritePermission)
        {
            currentLocation = branchLocationService.GetAccesibleCheckInLocationsForCurrentUser()[0];
            lblMyLocation.Text = currentLocation.Name;
        }

        if (IsPostBack)
        {
            if (SelectedBranchLocationID != ddlLocations.SelectedValue)
            {
                upd.Update();
            }
            if (SelectStartDate != dtrDateFilter.StartDate.ToString())
            {
                upd.Update();
            }
            if (SelectEndDate != dtrDateFilter.EndDate.ToString())
            {
                upd.Update();
            }
            if (SelectedReportTypeIsUniquePatientsFilter != (ucReportType.ReportType == UI.IndicatorReportTypeControl.IndicatorReportRenderType.UniquePatients))
            {
                upd.Update();
            }
        }
        else
        {
            minStartDate = GetMinStartDate();
                  
            endDate = DateTime.Now;

            dtrDateFilter.CustomStartDate = minStartDate;
            dtrDateFilter.CustomEndDate = endDate;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (HasWritePermission)
        {
            SelectedBranchLocationID = ddlLocations.SelectedValue;
        }
        else
        {
            SelectedBranchLocationID = currentLocation != null ? currentLocation.BranchLocationID.ToString() : String.Empty;
        }
        SelectStartDate = dtrDateFilter.StartDate.ToString();
        SelectEndDate = dtrDateFilter.EndDate.ToString();
        SelectedReportTypeIsUniquePatientsFilter = IsUniquePatientMode;

        BindReportParameters();
    }

    protected void BindReportParameters()
    {
        if (string.IsNullOrEmpty(ddlLocations.SelectedValue) && HasWritePermission)
        {
            ucReport.BranchLocationID = null;
        }
        else if (HasWritePermission)
        {
            ucReport.BranchLocationID = Convert.ToInt32(ddlLocations.SelectedValue);
        }
        else
        {
            if (currentLocation != null)
            {
                ucReport.BranchLocationID = currentLocation.BranchLocationID;
            }
        }

        ucReport.RenderUniquePatientsReportType = IsUniquePatientMode;


        if (dtrDateFilter.StartDate.HasValue)
        {
            ucReport.StartDate = dtrDateFilter.StartDate;
        }
        else
        {
            ucReport.StartDate = null;
        }
        if (dtrDateFilter.EndDate.HasValue)
        {
            ucReport.EndDate = dtrDateFilter.EndDate;
        }
        else
        {
            ucReport.EndDate = DateTime.Now;
        }
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        dtrDateFilter.Reset(GetMinStartDate(), null);
        ucReportType.Reset();

    }


    protected override void PrintReport()
    {
        try
        {
            //prepare date for repor
            int? branchLocationID;
            if (HasWritePermission)
            {
                branchLocationID = string.IsNullOrEmpty(ddlLocations.SelectedValue) ? (int?)null : Convert.ToInt32(ddlLocations.SelectedValue);
            }
            else
            {
                branchLocationID = currentLocation.BranchLocationID;
            }
            var start = !dtrDateFilter.StartDate.HasValue ? (DateTime?)null : dtrDateFilter.StartDate;
            var end = dtrDateFilter.EndDate;

            //Print report into context
            var filter = new SimpleFilterModel
            {
                Location = branchLocationID,
                StartDate = start,
                EndDate = end
            };

            var report = new DrugsOfChoiceByAgePdfReport(filter, IsUniquePatientMode);
            report.CreatePDF(HttpContext.Current, "IR_DrugsOfChoiceByAge.pdf");

        }
        catch (Exception ex)
        {
            SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            ErrorLog.AddServerException(CustomError.GetMessageForCustomOperation("print", "Drugs Of Choice Report By Age"), ex);
        }
    }

    protected override Button PrintButton
    {
        get { return btnPrint; }
    }

    protected override DropDownList LocationsDropDownList
    {
        get { return ddlLocations; }
    }

    protected override Label MyLocationLabel
    {
        get { return lblMyLocation; }
    }

}
