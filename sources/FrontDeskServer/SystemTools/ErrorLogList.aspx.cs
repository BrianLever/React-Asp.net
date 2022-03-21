using System;
using System.Data;
using System.Web.Security;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;

public partial class ErrorLogListForm : BasePage
{
    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsItems.TypeName = typeof(FrontDesk.Server.Logging.ErrorLog).FullName;

        RegisterGridViewForCustomPaging(gvItems);

        btnExport.BeforeExport += BtnExport_BeforeExport;
    }

    private void BtnExport_BeforeExport(object sender, EventArgs e)
    {

        var dv = new DataView(ErrorLog.GetForExport(dtStartDate.SelectedDate, dtEndDate.SelectedDate, 0, Int32.MaxValue-1).Tables[0]);
        btnExport.BoundedView = dv;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Error Log";

        if (!IsPostBack)
        {
            dtEndDate.SelectedDate = DateTime.Today;
        }

       


        // render link to Elk logging
        var centralLoggingUrl = AppSettings.CentralLoggingUrl;

        if(string.IsNullOrWhiteSpace(centralLoggingUrl))
        {
            lnkLinkToCentralLogging.Visible = false;
        }
        else
        {
            lnkLinkToCentralLogging.NavigateUrl = centralLoggingUrl;
        }
    }

    protected void Search_Click(object sender, EventArgs e)
    {
        
        gvItems.PageIndex = 0;
        gvItems.DataBind();
    }
    protected void Clear_Click(object sender, EventArgs e)
    {
        dtStartDate.SelectedDate = null;
        dtEndDate.SelectedDate = DateTime.Today;

        AddAjaxScriptStatement(GetControlClearStateScript(dtStartDate.ClientID, HTMLControlType.Textbox, dtStartDate.Text));
        AddAjaxScriptStatement(GetControlClearStateScript(dtEndDate.ClientID, HTMLControlType.Textbox, dtEndDate.Text));

        gvItems.PageIndex = 0;
        gvItems.DataBind();
    }

    protected void btnDeleteAll_Click(object sender, EventArgs e)
    {
        try
        {
            ErrorLog.DeleteAll();
            gvItems.DataBind();
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(CustomError.GetMessageForCustomOperation("clear", "Error Log"), this.GetType());
        }
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(CustomError.GetMessageForCustomOperation("export", "Error Log"), this.GetType());
        }
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator) || Roles.IsUserInRole(UserRoles.BranchAdministrator))
        {
            ReadPermission = true;
            WritePermission = true;
            DeletePermission = true;
        }
        else
        {
            ReadPermission = false;
            WritePermission = false;
            DeletePermission = false;

            Response.Redirect("~/Default.aspx");
        }
    }
}
