using System;
using System.Data;
using System.Web.UI.WebControls;

using FrontDesk.Common.Extensions;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Extensions;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Printouts.Bhs;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Screening.Models;
using FrontDesk.Server.Screening.Services;
using FrontDesk.Server.Web;
using FrontDesk.Server.Web.Controls;

public partial class controls_FollowUpList : ScreenListUserControl
{
    private readonly BhsFollowUpService _bhsService = new BhsFollowUpService();

    #region Filter properties
    private int ReportTypeFilter
    {
        get
        {
            bool isFound;
            int value = this.GetIDValue<int>("ReportTypeFilter", out isFound);
            if (!isFound)
                return (int)BhsReportType.AllReports;
            else
            {
                return value;
            }
        }
        set
        {
            ViewState["ReportTypeFilter"] = (int)value;
            Session["ReportTypeFilter"] = (int)value;
        }
    }

    #endregion
    public Button DefaultSearchButton { get { return btnSearch; } }

    #region Page Events

    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            // Enable obfuscation for TypeName="...". This overloads class name from .aspx
            // Method name stays readable: unable to determine obfuscated name.
            // Method names and parameters may stay in .aspx.
            odsList.TypeName = typeof(FrontDesk.Server.Screening.Services.BhsFollowUpService).FullName;
            odsLocations.TypeName = typeof(FrontDesk.Server.Screening.Services.BranchLocationService).FullName;

            Page.RegisterGridViewForCustomPaging(grvList);

            //bind to grid events
            grvList.RowDataBound += new GridViewRowEventHandler(grvList_RowDataBound);
            grvList.NestedDataBinding += new EventHandler<HierarDynamicGrid.HierarDynamicGridRelatedDataBindingEventArg>(grvList_NestedDataBinding);
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            Page.RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        BindListControls();
        GetSearchParametersFromForm();
    }

    #endregion

    #region Change Check-In time label for today's records

    void grvList_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //if check-in date is current date - show only time

            //get last cell
            var fileCompletionDateCell = e.Row.Cells[e.Row.Cells.Count - 1];
            var createdDateCell = e.Row.Cells[e.Row.Cells.Count - 2];
            var followUpDateCell = e.Row.Cells[e.Row.Cells.Count -3];



            if (fileCompletionDateCell.Controls.Count > 0)
            {
                fileCompletionDateCell.Controls.Clear();
            }
            if (createdDateCell.Controls.Count > 0)
            {
                createdDateCell.Controls.Clear();
            }
            if (followUpDateCell.Controls.Count > 0)
            {
                followUpDateCell.Controls.Clear();
            }

            var dataItem = e.Row.DataItem as DataRowView;
            if (dataItem != null)
            {
                var visitDate = (DateTimeOffset)dataItem["LastVisitDate"];
                var followUpDate = (DateTimeOffset)dataItem["LastFollowUpDate"];
                if (visitDate.Date == DateTimeOffset.Now.Date)
                {
                    createdDateCell.Text = "Today";
                }
                else
                {
                    createdDateCell.Text = visitDate.FormatAsDate(); ;
                }

                if (followUpDate.Date == DateTimeOffset.Now.Date)
                {
                   followUpDateCell.Text = "Today";
                }
                else
                {
                    followUpDateCell.Text = followUpDate.FormatAsDate();
                }

                var completeDate = Convert.IsDBNull(dataItem["LastCompleteDate"]) ? (DateTimeOffset?)null : (DateTimeOffset?)dataItem["LastCompleteDate"];
                if (completeDate.HasValue)
                {
                    if (completeDate.Value.Date == DateTimeOffset.Now.Date)
                    {
                        fileCompletionDateCell.Text = string.Format("{0:HH:mm}", completeDate.Value);
                    }
                    else
                    {
                        fileCompletionDateCell.Text = completeDate.FormatAsDateWithTimeWithoutTimeZone();
                    }
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

    void grvList_NestedDataBinding(object sender, HierarDynamicGrid.HierarDynamicGridRelatedDataBindingEventArg e)
    {
        long mainRowID = 0;
        if (Int64.TryParse(e.mainRowDataKey, out mainRowID))
        {
            e.DataSource = _bhsService.GetRelatedReports(
                new BhsSearchRelatedItemsFilter(mainRowID, LocationFilter, dtrDateFilter.StartDate, dtrDateFilter.EndDate, (BhsReportType)ReportTypeFilter, ScreenDoxIDFilter)
                );
        }
    }

    #endregion

    #region Binding

    protected void BindListControls()
    {
        ddlLocations.Items.Clear();
        ddlLocations.DataBind();
        if (ddlLocations.Items.Count > 1)
        {
            ddlLocations.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedFilter, string.Empty));
        }

        if (!IsPostBack)
        {
            //if currently logger used is not Superuser, but Branch Admin - we selects his location by default
            if (Page.User.IsInRole(UserRoles.BranchAdministrator))
            {
                var user = FDUser.GetCurrentUser();
                var locationID = user.BranchLocationID.ToString();

                var item = ddlLocations.Items.FindByValue(locationID);
                if (item != null) item.Selected = true;
            }
        }
    }

    #endregion


    #region Search Parameters

    //private readonly DateTime DefaultStartDateTimeFilter = DateTime.Now.Date.AddDays(-1); // start from yesterday
    private readonly DateTime DefaultStartDateTimeFilter = DateTime.Now.Date;   // today

    protected void GetSearchParametersFromForm()
    {
        if (!IsPostBack)
        {

            ddlLocations.SelectedValue = LocationFilter == null ? null : LocationFilter.ToString();
            if (!string.IsNullOrEmpty(ddlLocations.SelectedValue))
            {
                //if user is assigned to the branch, we need to set the filter for the grid
                if (LocationFilter == null)
                {
                    try
                    {
                        LocationFilter = Int32.Parse(ddlLocations.SelectedValue);
                    }
                    catch (Exception) { }
                }
            }
            txFirstNameFilter.Text = FirstnameFilter;
            txtLastNameFilter.Text = LastnameFilter;
            txtScreendoxIdFilter.Text = ScreenDoxIDFilter.ToStringOrDefault();

            hdnDisableAutoRefresh.Value = AutoRefreshEnabled ? "1" : "0";
        }
        else
        {
            FirstnameFilter = !_forceClearSearchParams ? Page.Request.Form[txFirstNameFilter.UniqueID] : string.Empty;
            LastnameFilter = !_forceClearSearchParams ? Page.Request.Form[txtLastNameFilter.UniqueID] : string.Empty;
            ScreenDoxIDFilter = (!_forceClearSearchParams ? Page.Request.Form[txtScreendoxIdFilter.UniqueID] : string.Empty).ParseTo<long>(); ;

            int locID = 0;
            if (!_forceClearSearchParams)
            {
                if (Int32.TryParse(Page.Request.Form[ddlLocations.UniqueID], out locID))
                {
                    LocationFilter = locID;
                }
                else
                {
                    LocationFilter = null;
                }
            }
            else
            {
                if (Int32.TryParse(ddlLocations.SelectedValue, out locID))
                {
                    LocationFilter = locID;
                }
                else
                {
                    LocationFilter = null;
                }
            }

            ReportTypeFilter = (int)(rbtCompletedReports.Checked ? BhsReportType.CompletedReports :
                (rbtIncompleteReports.Checked ? BhsReportType.IncompleteReports : BhsReportType.AllReports));

            AutoRefreshEnabled = hdnDisableAutoRefresh.Value.ParseToOrDefault<int>(1) == 0 ? false : true;

        }
        //set page index
        grvList.PageIndex = 0;
    }

    protected void ApplySearchParametersFromForm()
    {
        odsList.SelectParameters["firstNameFilter"].DefaultValue = this.FirstnameFilter;
        odsList.SelectParameters["lastNameFilter"].DefaultValue = this.LastnameFilter;
        odsList.SelectParameters["screeningResultIdFilter"].DefaultValue = ScreenDoxIDFilter.ToStringOrDefault(string.Empty);

        odsList.SelectParameters["locationFilter"].DefaultValue = LocationFilter.HasValue ? LocationFilter.Value.ToString() : string.Empty;

        odsList.SelectParameters["startDateFilter"].DefaultValue = dtrDateFilter.StartDate.HasValue ? dtrDateFilter.StartDate.Value.ToString() : string.Empty;
        odsList.SelectParameters["endDateFilter"].DefaultValue = dtrDateFilter.EndDate.HasValue ? dtrDateFilter.EndDate.Value.ToString() : string.Empty;
        odsList.SelectParameters["reportTypeFilter"].DefaultValue = ((int)ReportTypeFilter).ToString();


    }

    protected void ClearParameters()
    {
        txFirstNameFilter.Text = string.Empty;
        txtLastNameFilter.Text = string.Empty;
        txtScreendoxIdFilter.Text = string.Empty;

        if (ddlLocations.Items.Count > 0)
        {
            ddlLocations.SelectedValue = ddlLocations.Items[0].Value;
            Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(ddlLocations.ClientID, HTMLControlType.Combobox, "0"));

        }

        dtrDateFilter.Reset();

        //clear UI
        Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(txFirstNameFilter.ClientID, HTMLControlType.Textbox, txFirstNameFilter.Text));
        Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(txtLastNameFilter.ClientID, HTMLControlType.Textbox, txtLastNameFilter.Text));
        Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(rbtAllReports.ClientID, HTMLControlType.Checkbox, "true"));
        Page.AddAjaxScriptStatement(Page.GetControlClearStateScript(txtScreendoxIdFilter.ClientID, HTMLControlType.Textbox, txtScreendoxIdFilter.Text));

        _forceClearSearchParams = true;
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        GetSearchParametersFromForm();
    }
    protected void btnClear_Click(object sender, EventArgs e)
    {
        ClearParameters();
        //update parameters
        GetSearchParametersFromForm();
    }

    private bool isBound = false;

    private void EnsureDataBound()
    {
        if (!isBound)
        {
            ApplySearchParametersFromForm();

            BindPatientList();
        }
    }
    /// <summary>
    /// Bind Filter By list or show Filter by label
    /// </summary>
    protected void BindPatientList()
    {
        grvList.DataBind();
    }


    #endregion

    public override void ApplyTabIndexToControl(ref short startTabIndex)
    {
        txFirstNameFilter.TabIndex = startTabIndex++;
        txtLastNameFilter.TabIndex = startTabIndex++;
        txtScreendoxIdFilter.TabIndex = startTabIndex++;
        ddlLocations.TabIndex = startTabIndex++;

        dtrDateFilter.ApplyTabIndexToControl(ref startTabIndex);
        rbtAllReports.TabIndex = startTabIndex++;
        rbtCompletedReports.TabIndex = startTabIndex++;
        rbtIncompleteReports.TabIndex = startTabIndex++;

        btnSearch.TabIndex = startTabIndex++;
        btnClear.TabIndex = startTabIndex++;
        grvList.TabIndex = startTabIndex++;
    }

    protected override void OnPreRender(EventArgs e)
    {
        EnsureDataBound();

        Page.ClientScript.RegisterClientScriptInclude(this.GetType(), "autoRefreshUpdatePanel.js", Page.GetVirtualPath("~/scripts/controls/autoRefreshUpdatePanel.js"));

        base.OnPreRender(e);
    }

    public override void Focus()
    {
        //base.Focus();
        txFirstNameFilter.Focus();
    }


    protected void btnPrint_Click(object sender, EventArgs e)
    {
        PrintPage();
    }


    protected void PrintPage()
    {
        try
        {
            //Print report into context
            var report = new BhsFollowUpListPdfPrintout(new BhsSearchFilterModel
            {
                LastName = LastnameFilter,
                FirstName = FirstnameFilter,
                LocationId = LocationFilter,
                ReportType = (BhsReportType)ReportTypeFilter,
                StartDate = dtrDateFilter.StartDate,
                EndDate = dtrDateFilter.EndDate,
                ScreeningResultID = ScreenDoxIDFilter

            });
            report.CreatePDF(System.Web.HttpContext.Current, "BhsFollowUpList.pdf");

        }
        catch (Exception ex)
        {
            Page.SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            ErrorLog.AddServerException(CustomError.GetMessageForCustomOperation("print", "Visit List page"), ex);
        }
    }
}
