using Common.Logging;

using FrontDesk;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Reports.ExcelReports;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using System;
using System.Web;
using System.Web.UI.WebControls;

public partial class ExportToExcelPage : IndicatorReportBasePage
{

    private BhsExportService _service = new BhsExportService();
    private ILog _logger = LogManager.GetLogger<ExportToExcelPage>();

    public bool IsUniquePatientMode
    {
        get
        {
            return ucReportType.ReportType == UI.IndicatorReportTypeControl.IndicatorReportRenderType.UniquePatients;
        }
    }

    protected void Page_Init(object sender, EventArgs e)
    {
        ucReportType.DefaultValue = UI.IndicatorReportTypeControl.IndicatorReportRenderType.TotalReports;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Export to Excel";
        if (!HasWritePermission)
        {
            currentLocation = branchLocationService.GetAccesibleCheckInLocationsForCurrentUser()[0];
            lblMyLocation.Text = currentLocation.Name;
        }

        if (!IsPostBack)
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
    }

    protected void btnClear_Click(object sender, EventArgs e)
    {
        dtrDateFilter.Reset(GetMinStartDate(), null);

    }


    protected override void PrintReport()
    {
        var filter = new BhsExportFilterModel();


        try
        {
            //prepare date for repor

            if (HasWritePermission)
            {
                filter.LocationId = string.IsNullOrEmpty(ddlLocations.SelectedValue) ? (int?)null : Convert.ToInt32(ddlLocations.SelectedValue);
            }
            else
            {
                filter.LocationId = currentLocation.BranchLocationID;
            }
            filter.StartDate = !dtrDateFilter.StartDate.HasValue ? (DateTime?)null : dtrDateFilter.StartDate;
            filter.EndDate = dtrDateFilter.EndDate;

            filter.IncludeVisits = chkVisit.Checked;
            filter.IncludeFollowUps = chkFollowUp.Checked;
            filter.IncludeDemographics = chkDemographics.Checked;
            filter.IncludeScreenings = chkScreening.Checked;
            filter.IncludeDrugsOfChoice = chkDrugsOfChoice.Checked;
            filter.IncludeCombined = chkCombined.Checked;
            filter.UniquePatientMode = IsUniquePatientMode;

            var data = _service.GetReports(filter);



            //Print report into context
            new BhsReportExcelReport(data).Create(HttpContext.Current, "Export_Reports_{0:yyyyMMdd}_{1:yyyyMMdd}{2}.xlsx"
                .FormatWith(
                filter.StartDate,
                filter.EndDate,
                filter.UniquePatientMode ? "_(unique_patients)" : "_(total_reports)"
                ));

            SecurityLog.Add(new SecurityLog(SecurityEvents.ExportBhsReports,
                "{0}~{1}~{2}~{3}".FormatWith(
                    filter.StartDate, 
                    filter.EndDate, 
                    filter.IncludedReports, 
                    filter.UniquePatientMode? "Unique Patients" : "Total Reports"
                ), filter.LocationId));

        }
        catch (Exception ex)
        {
            _logger.ErrorFormat("[ExportToExcelPage] Failed to export to Excel.", ex);
            ErrorLog.AddServerException(CustomError.GetMessageForCustomOperation("export", "Reports"), ex);

            RedirectToErrorPage();
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
