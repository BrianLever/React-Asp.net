using FrontDesk.Common.Extensions;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Screening;
using FrontDesk.Server.Web;
using FrontDesk.Server.Web.Controls;

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class controls_CheckInList : ScreenListUserControl
{
    private bool ShowExportButton = true;

    public Button DefaultSearchButton { get { return btnSearch; } }

    #region Page Events



    protected void Page_Init(object sender, EventArgs e)
    {
        try
        {
            // Enable obfuscation for TypeName="...". This overloads class name from .aspx
            // Method name stays readable: unable to determine obfuscated name.
            // Method names and parameters may stay in .aspx.
            odsCheckInList.TypeName = typeof(FrontDesk.Server.Screening.ScreeningResultHelper).FullName;
            odsLocations.TypeName = typeof(FrontDesk.Server.Screening.Services.BranchLocationService).FullName;

            Page.RegisterGridViewForCustomPaging(grvList);
            //bind curernt user id
            //odsLocations.SelectParameters[0].DefaultValue = FDUser.CurrentUserID.ToString();

            //bind to grid events
            grvList.RowDataBound += new GridViewRowEventHandler(grvList_RowDataBound);
            grvList.NestedDataBinding += new EventHandler<FrontDesk.Server.Web.Controls.HierarDynamicGrid.HierarDynamicGridRelatedDataBindingEventArg>(grvList_NestedDataBinding);



            var roles = new List<string>(Roles.GetRolesForUser());
            // Check Role-based permissions
            if (roles.Contains(UserRoles.Staff))
            {
                ShowExportButton = false;

            }
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
            var cell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (cell.Controls.Count > 0)
            {
                cell.Controls.Clear();
            }
            //get cell with export date
            var cellExport = e.Row.Cells[e.Row.Cells.Count - 2];
            if (cellExport.Controls.Count > 0)
            {
                cellExport.Controls.Clear();
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

                bool hasExport = Convert.ToBoolean(dataItem["HasExport"]);
                if (hasExport)
                {
                    if (ShowExportButton)
                    {
                        //add export icon
                        Image imgExportDate = new Image();
                        imgExportDate.ImageUrl = "~/images/export-icon.png";
                        imgExportDate.AlternateText = "Export";
                        imgExportDate.CssClass = "datagrid export icon";
                        cellExport.Controls.Add(imgExportDate);
                    }
                }
                else
                {
                    //add maximum export date
                    var exportDate = (DateTimeOffset)dataItem["ExportDate"];
                    if (exportDate.ToLocalTime().Date == DateTimeOffset.Now.Date)
                    {
                        cellExport.Text = string.Format("{0:HH:mm}", exportDate);
                    }
                    else
                    {
                        cellExport.Text = string.Format("{0:MM/dd/yyyy, HH:mm}", exportDate);
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
                        ltrDate.Text = string.Format("{0:MM/dd/yyyy, HH:mm}", checkInDate);
                    }
                }
            }

            //get place holder control
            var phExportDate = e.Item.FindControl("phExportDate") as PlaceHolder;
            if (phExportDate != null)
            {
                if (dataItem.ExportDate.HasValue)
                {
                    //view export date
                    DateTimeOffset exportDate = dataItem.ExportDate.Value;
                    Literal lblExportDate = new Literal();
                    if (exportDate.ToLocalTime().Date == DateTimeOffset.Now.Date)
                    {
                        lblExportDate.Text = string.Format("{0:HH:mm}", exportDate);
                    }
                    else
                    {
                        lblExportDate.Text = string.Format("{0:MM/dd/yyyy, HH:mm}", exportDate);
                    }
                    phExportDate.Controls.Add(lblExportDate);
                }
                else if (dataItem.ShowBeginExportButton && ShowExportButton)
                {
                    //view clickable ‘export’ icon 
                    Image imgExportDate = new Image();
                    imgExportDate.ImageUrl = "~/images/export-icon.png";
                    imgExportDate.AlternateText = "Export";
                    imgExportDate.CssClass = "datagrid export icon";
                    //add html <a> control
                    //TODO: change href to export wizard page 
                    phExportDate.Controls.Add(new LiteralControl(string.Format("<a href='ExportWizard.aspx?id={0}'>", dataItem.ScreeningResultID)));
                    //add icon
                    phExportDate.Controls.Add(imgExportDate);
                    phExportDate.Controls.Add(new LiteralControl("</a>"));
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
            e.DataSource = ScreeningResultHelper.GetRelatedPatientScreenings(
                mainRowID,
                LocationFilter,
                dtrDateFilter.StartDate,
                dtrDateFilter.EndDate,
                ScreenDoxIDFilter);
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
            ScreenDoxIDFilter = (!_forceClearSearchParams ? Page.Request.Form[txtScreendoxIdFilter.UniqueID] : string.Empty).ParseTo<long>();

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
            AutoRefreshEnabled = hdnDisableAutoRefresh.Value.ParseToOrDefault<int>(1) == 0 ? false : true;
        }
        //set page index
        grvList.PageIndex = 0;
    }

    protected void ApplySearchParametersFromForm()
    {
        odsCheckInList.SelectParameters["firstNameFilter"].DefaultValue = this.FirstnameFilter;
        odsCheckInList.SelectParameters["lastNameFilter"].DefaultValue = this.LastnameFilter;
        odsCheckInList.SelectParameters["screeningResultIdFilter"].DefaultValue = ScreenDoxIDFilter.ToStringOrDefault(string.Empty);

        odsCheckInList.SelectParameters["locationFilter"].DefaultValue = LocationFilter.HasValue ? LocationFilter.Value.ToString() : string.Empty;

        odsCheckInList.SelectParameters["startDateFilter"].DefaultValue = dtrDateFilter.StartDate.HasValue ? dtrDateFilter.StartDate.Value.ToString() : string.Empty;
        odsCheckInList.SelectParameters["endDateFilter"].DefaultValue = dtrDateFilter.EndDate.HasValue ? dtrDateFilter.EndDate.Value.ToString() : string.Empty;


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
}
