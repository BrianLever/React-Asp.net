using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI.WebControls;
using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Extensions;
using FrontDesk.Server;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;
using FrontDesk.Server.Web.Extensions;
using ScreenDox.Server.Common.Models;

public partial class KioskManagement_KioskList : BasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        ddlBranchLocation.AddDefaultNotSelectedItem();
        ddlScreeningProfile.AddDefaultNotSelectedItem();
        odsKiosk.TypeName = typeof(ScreenDox.Server.Common.Services.KioskService).ToString();

        RegisterGridViewForCustomPaging(gvKiosks);
        gvKiosks.RowDataBound += new GridViewRowEventHandler(gvKiosks_RowDataBound);
    }

    #region GridView Data Binding
    void gvKiosks_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var dataItem = e.Row.DataItem as DataRowView;
            if (dataItem != null)
            {
                
                
                short keyInt = Convert.ToInt16(dataItem["KioskID"]);

                string keyAlpha = Kiosk.GetKioskIDAsString(keyInt);

                HyperLink ctrl = new HyperLink();
                ctrl.NavigateUrl = "KioskDetails.aspx?id={0}".FormatWith(keyInt);
                ctrl.Text = keyAlpha;

                e.Row.Cells[0].Text = string.Empty;
                e.Row.Cells[0].Controls.Add(ctrl);

                var disabled = Convert.IsDBNull(dataItem["Disabled"]) ? false : Convert.ToBoolean(dataItem["Disabled"]);
                if (disabled) e.Row.CssClass = (e.Row.RowIndex % 2 > 0 ? "alt" : "") + " disabled";
            }
        }
    }


    #endregion



    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "ScreenDox Kiosks";

        if (!Roles.IsUserInRole(UserRoles.SuperAdministrator))
        {
            odsKiosk.SelectParameters["userID"].DefaultValue = FDUser.GetCurrentUser().UserID.ToString();
            gvKiosks.PageIndex = 0;
            EnsureDataBound();
        }

    }

    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);
        EnsureDataBound();
    }

    protected void gvKiosks_Sorting(object sender, GridViewSortEventArgs e)
    {
        this.OnGridViewSorting(sender, e);
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator) || Roles.IsUserInRole(UserRoles.BranchAdministrator))
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

    #region Filter properties
    protected string NameFilter
    {
        get
        {
            bool isFound;
            string value = GetPageIDValue<string>("NameFilter", out isFound);

            if (!isFound)
                return String.Empty;
            else
            {
                return value;
            }
        }
        set
        {
            ViewState["NameFilter"] = value;
        }
    }

    protected int? BranchLocationNameFilter
    {
        get
        {
            return GetPageIDValue<int>("BranchLocationNameFilter");
        }
        set
        {
            ViewState["BranchLocationNameFilter"] = value;
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

    #endregion

    protected void GetSearchParametersFromForm()
    {
        if (!IsPostBack)
        {

            txtNameFilter.Text = NameFilter;
            ddlBranchLocation.SetValueOrDefault(BranchLocationNameFilter);
            ddlHideDisabled.SetValueOrDefault(ShowDisabledFilter);
            ddlScreeningProfile.SetValueOrDefault(ScreeningProfileFilter);

        }
        else
        {
            NameFilter = !_forceClearSearchParams ? Page.Request.Form[txtNameFilter.UniqueID] : string.Empty;
            ShowDisabledFilter = !_forceClearSearchParams ? Page.Request.Form[ddlHideDisabled.UniqueID] == "1" : false;
            ScreeningProfileFilter = !_forceClearSearchParams ? Page.Request.Form[ddlScreeningProfile.UniqueID].AsNullable<int>() : null;
            BranchLocationNameFilter = !_forceClearSearchParams ? Page.Request.Form[ddlBranchLocation.UniqueID].AsNullable<int>() : null;

        }
        //set page index
        gvKiosks.PageIndex = 0;
    }

    protected void ApplySearchParametersFromForm()
    {
        var filterbyName = this.NameFilter;

        Regex re = new Regex(@"^[A-Z0-9]{4}$", RegexOptions.ExplicitCapture);

        short kioskIdFilter = re.IsMatch(filterbyName) ? TextFormatHelper.UnpackStringInt16(filterbyName) : (short)0;
        odsKiosk.SelectParameters["kioskID"].DefaultValue = kioskIdFilter.ToString();
        odsKiosk.SelectParameters["filterByName"].DefaultValue = kioskIdFilter == 0 ? filterbyName : string.Empty;
        
        odsKiosk.SelectParameters["branchLocationID"].DefaultValue = this.BranchLocationNameFilter.As<string>();
        odsKiosk.SelectParameters["showDisabled"].DefaultValue = this.ShowDisabledFilter ? "true" : "false";
        odsKiosk.SelectParameters["screeningProfileID"].DefaultValue = this.ScreeningProfileFilter.As<string>();


    }
    bool _forceClearSearchParams = false; //true when clear button has been pressed

    protected void ClearParameters()
    {
        txtNameFilter.Text = string.Empty;
        ddlHideDisabled.SetValueOrDefault(0);
        ddlScreeningProfile.SetValueOrDefault(0);
        ddlBranchLocation.SetValueOrDefault(0);

        AddAjaxScriptStatement(GetControlClearStateScript(ddlHideDisabled.ClientID, HTMLControlType.Combobox, "0"));
        AddAjaxScriptStatement(GetControlClearStateScript(txtNameFilter.ClientID, HTMLControlType.Textbox, txtNameFilter.Text));
        AddAjaxScriptStatement(GetControlClearStateScript(ddlScreeningProfile.ClientID, HTMLControlType.Combobox, ""));
        AddAjaxScriptStatement(GetControlClearStateScript(ddlBranchLocation.ClientID, HTMLControlType.Combobox, ""));


        _forceClearSearchParams = true;
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
        gvKiosks.DataBind();
    }


}
