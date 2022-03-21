using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Common.Logging;

using FrontDesk.Common.Extensions;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;

using Newtonsoft.Json;

public partial class ScreeningResultsByProblemAgePage : IndicatorReportBasePage
{
    private readonly ILog _logger = LogManager.GetLogger<ScreeningResultsByProblemAgePage>();
    private readonly IScreeningResultService _service = new ScreeningResultService();

    #region Filter Properties

    public ScreeningResultFilterModel SearchOptions
    {
        get
        {
            return (ScreeningResultFilterModel)ViewState["Filter"];
        }
        set { ViewState["Filter"] = value; }
    }


    public int? LocationFilter
    {
        get
        {
            if (!HasWritePermission)
            {
                return currentLocation != null ? currentLocation.BranchLocationID : (int?)null;
            }
            return ddlLocations.SelectedValue.ParseTo<Int32>();
        }
    }

    protected bool AutoRefreshEnabled
    {
        get
        {
            bool isFound;
            bool value = GetPageIDValue<bool>("CheckIn_AutoRefreshEnabled", out isFound);
            if (!isFound)
                return true;
            else
            {
                return value;
            }
        }
        set
        {
            ViewState["CheckIn_AutoRefreshEnabled"] = value;
            Session["CheckIn_AutoRefreshEnabled"] = value;
        }
    }

    #endregion

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

        updFilter.Triggers.Add(new AsyncPostBackTrigger
        {
            ControlID = btnApply.UniqueID,
            EventName = "Click",
        });
        updFilter.Triggers.Add(new AsyncPostBackTrigger
        {
            ControlID = btnClear.UniqueID,
            EventName = "Click",
        });

        RegisterGridViewForCustomPaging(grvList);

        grvList.RowDataBound += new GridViewRowEventHandler(grvList_RowDataBound);
        grvList.NestedDataBinding += new EventHandler<FrontDesk.Server.Web.Controls.HierarDynamicGrid.HierarDynamicGridRelatedDataBindingEventArg>(grvList_NestedDataBinding);
        odsCheckInList.Selecting += OdsCheckInList_Selecting;
        btnApply.Click += BtnApply_Click;
    }

    private void BtnApply_Click(object sender, EventArgs e)
    {
        grvList.DataBind();
        upd.Update();
        updFilter.DataBind();

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Screening Results by Sort";
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
                updFilter.DataBind();
            }
            if (SelectStartDate != dtrDateFilter.StartDate.ToString())
            {
                upd.Update();
                updFilter.DataBind();
            }
            if (SelectEndDate != dtrDateFilter.EndDate.ToString())
            {
                upd.Update();
                updFilter.DataBind();
            }

            AutoRefreshEnabled = hdnDisableAutoRefresh.Value.ParseToOrDefault<int>(1) == 0 ? false : true;
        }
        else
        {
            //minStartDate = GetMinStartDate();

            endDate = DateTime.Now;

            dtrDateFilter.CustomStartDate = endDate;
            dtrDateFilter.CustomEndDate = endDate;
            hdnDisableAutoRefresh.Value = AutoRefreshEnabled ? "1" : "0";
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

        ucScoreFilter.SetCountsByFilterOptions(_service.GetScreeningsCountByScoreLevel(new ScreeningResultFilterModel
        {
            StartDate = dtrDateFilter.StartDate,
            EndDate = dtrDateFilter.EndDate,
            Location = LocationFilter

        }));

        Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "autoRefreshUpdatePanel.js", GetVirtualPath("~/scripts/controls/autoRefreshUpdatePanel.js"));

    }

    #region Change Check-In time label for today's records

    void grvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if check-in date is current date - show only time

            //get last cell
            var cell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (cell.Controls.Count > 0)
            {
                cell.Controls.Clear();
            }
          
            var dataItem = e.Row.DataItem as DataRowView;
            if (dataItem != null)
            {
                var checkInDate = (DateTimeOffset)dataItem["LastCheckinDate"];
                if (checkInDate.Date == DateTimeOffset.Now.Date)
                {
                    cell.Text = string.Format("{0:HH:mm}", checkInDate);
                }
                else
                {
                    cell.Text = string.Format("{0:MM/dd/yyyy, HH:mm}", checkInDate);
                }
                
            }
        }
    }

    //change check-in time label for today's records
    protected void rpt_RowDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        {
            //get data
            PatientCheckInViewModel dataItem = e.Item.DataItem as PatientCheckInViewModel;

            //if check-in date is current date - show only time
            //get literal control
            var ltrDate = e.Item.FindControl("ltrDate") as Literal;

            if (ltrDate != null)
            {
                if (dataItem != null)
                {
                    var checkInDate = dataItem.CreatedDate;
                    var today = DateTimeOffset.Now.Date;
                    if (checkInDate.Date == today)
                    {
                        ltrDate.Text = string.Format("{0:HH:mm}", checkInDate);
                    }
                    else
                    {
                        ltrDate.Text = checkInDate.FormatAsDateWithTimeWithoutTimeZone();
                    }
                }
            }
        }
    }

    #endregion

    #region Bind Related records

    void grvList_NestedDataBinding(object sender, FrontDesk.Server.Web.Controls.HierarDynamicGrid.HierarDynamicGridRelatedDataBindingEventArg e)
    {
        long mainRowID = 0;
        if (Int64.TryParse(e.mainRowDataKey, out mainRowID))
        {
            e.DataSource = _service.GetRelatedPatientScreeningsByProblemSort(mainRowID, SearchOptions);
        }
    }

    private void OdsCheckInList_Selecting(object sender, ObjectDataSourceSelectingEventArgs e)
    {
        if (!e.ExecutingSelectCount)
        {
            BindReportParameters();
            e.InputParameters["filter"] = this.SearchOptions;

            if (_logger.IsInfoEnabled)
            {
                _logger.InfoFormat("[ReportingResultbySort][Selecting] Filter condition: {0}", JsonConvert.SerializeObject(this.SearchOptions), e.ExecutingSelectCount);
            }
        }
        
    }


    protected void BindReportParameters()
    {

        SearchOptions = new ScreeningResultFilterModel
        {
            FirstName = null,
            LastName = null,
            Location = LocationFilter,
            StartDate = dtrDateFilter.StartDate,
            EndDate = dtrDateFilter.EndDate ?? DateTime.Now,
            ProblemScoreFilter = ucScoreFilter.GetModel()
        };


        if (_logger.IsTraceEnabled)
        {
            _logger.TraceFormat("[ReportingResultbySort][BindReportParameters] Filter condition: {0}", JsonConvert.SerializeObject(this.SearchOptions));
        }
    }

    #endregion

    protected void btnClear_Click(object sender, EventArgs e)
    {
        dtrDateFilter.Reset(GetMinStartDate(), null);
        ucScoreFilter.Reset();

        Response.Redirect(Request.RawUrl, false);
        
    }


    protected override void PrintReport()
    {
        try
        {
           
            var report = new BhsCheckInBySortPdfPrintout(SearchOptions);
            report.CreatePDF(HttpContext.Current, "ScreeningResultsBySort.pdf");

        }
        catch (Exception ex)
        {
            SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            ErrorLog.AddServerException(CustomError.GetMessageForCustomOperation("print", "Screening Results by Sort"), ex);
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
