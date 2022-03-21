using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using System.Web.UI.HtmlControls;
using System.Data;
using FrontDesk.Server;
using System.Web.Security;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web.Extensions;
using FrontDesk.Common.Extensions;

public partial class management_BranchLocation : BasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsBranchLocations.TypeName = typeof(FrontDesk.Server.Screening.Services.BranchLocationService).FullName;

        RegisterGridViewForCustomPaging(gvBranchLocations);
        gvBranchLocations.RowDataBound += new GridViewRowEventHandler(gvBranchLocations_RowDataBound);
        ddlScreeningProfile.AddDefaultNotSelectedItem();
    }

    void gvBranchLocations_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dataItem = e.Row.DataItem as BranchLocation;
            if (dataItem != null)
            {
                if (dataItem.Disabled) e.Row.CssClass = (e.Row.RowIndex % 2 > 0 ? "alt" : "") + " disabled";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Branch Locations";

        GetSearchParametersFromForm();
    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        EnsureDataBound();
    }

    protected void btnNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/management/BranchLocationDetails.aspx");
    }

    protected void lnbNew_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/management/BranchLocationDetails.aspx");
    }

    
    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator))
        {
            ReadPermission = true;
            WritePermission = true;
            PrintPermission = true;
            DeletePermission = true;
        }
        else
        {
            ReadPermission = false;
            WritePermission = false;
            PrintPermission = false;
            DeletePermission = false;

            Response.Redirect("~/Default.aspx");
        }

    }




    #region filter property

    protected string LocationNameFilter
    {
        get
        {
            bool isFound;
            string value = GetPageIDValue<string>("LocationNameFilter", out isFound);

            if (!isFound)
                return String.Empty;
            else
            {
                return value;
            }
        }
        set
        {
            ViewState["LocationNameFilter"] = value;
        }
    }

    protected bool ShowDisabledFilter
    {
        get
        {
            bool isFound;
            int value = GetPageIDValue<int>("HideDisabledFilter", out isFound);

            if (!isFound) return false;
            else
            {
                return value > 0;
            }
        }
        set
        {
            ViewState["HideDisabledFilter"] = value;
        }
    }

    protected int? ScreeningProfileFilter
    {
        get
        {
            return GetPageIDValue<int>("ScreeningProfileFilter");
        }
        set
        {
            ViewState["ScreeningProfileFilter"] = value;
        }
    }

    protected void GetSearchParametersFromForm()
    {
        if (!IsPostBack)
        {

            txtLocationNameFilter.Text = LocationNameFilter;
            ddlHideDisabled.SetValueOrDefault(ShowDisabledFilter);
            ddlScreeningProfile.SetValueOrDefault(ScreeningProfileFilter);

        }
        else
        {
            LocationNameFilter = !_forceClearSearchParams ? Page.Request.Form[txtLocationNameFilter.UniqueID] : string.Empty;
            ShowDisabledFilter = !_forceClearSearchParams ? Page.Request.Form[ddlHideDisabled.UniqueID] == "1" : false;
            ScreeningProfileFilter = !_forceClearSearchParams ? Page.Request.Form[ddlScreeningProfile.UniqueID].AsNullable<int>(): null;

        }
        //set page index
        gvBranchLocations.PageIndex = 0;
    }

    protected void ApplySearchParametersFromForm()
    {
        odsBranchLocations.SelectParameters["filterByName"].DefaultValue = this.LocationNameFilter;
        odsBranchLocations.SelectParameters["showDisabled"].DefaultValue = this.ShowDisabledFilter ? "true" : "false";
        odsBranchLocations.SelectParameters["screeningProfileID"].DefaultValue = this.ScreeningProfileFilter.As<string>();

    }
    bool _forceClearSearchParams = false; //true when clear button has been pressed

    protected void ClearParameters()
    {
        txtLocationNameFilter.Text = string.Empty;
        ddlHideDisabled.SetValueOrDefault(0);
        ddlScreeningProfile.SetValueOrDefault(0);
        AddAjaxScriptStatement(GetControlClearStateScript(ddlHideDisabled.ClientID, HTMLControlType.Combobox, "0"));
        AddAjaxScriptStatement(GetControlClearStateScript(txtLocationNameFilter.ClientID, HTMLControlType.Textbox, txtLocationNameFilter.Text));
        AddAjaxScriptStatement(GetControlClearStateScript(ddlScreeningProfile.ClientID, HTMLControlType.Combobox, ""));

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

            BindList();
        }
    }
    /// <summary>
    /// Bind Filter By list or show Filter by label
    /// </summary>
    protected void BindList()
    {
        gvBranchLocations.DataBind();
    }


    #endregion
}
