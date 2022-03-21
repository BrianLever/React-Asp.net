using Frontdesk.Server.SmartExport.Services;

using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;

using System;

public partial class SystemTools_AutoexportDashboard : BasePage
{

    private SmartExportService _service = new SmartExportService();

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (!User.IsInRole(UserRoles.SuperAdministrator) &&
            !User.IsInRole(UserRoles.BranchAdministrator))
        {
            RedirectToErrorPage(Resources.TextMessages.AccessPermissionsErrorMessage);
        }
    }


    protected void Page_Init(object sender, EventArgs e)
    {
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
        txtNameFilter.Text = string.Empty;

        AddAjaxScriptStatement(GetControlClearStateScript(dtStartDate.ClientID, HTMLControlType.Textbox, dtStartDate.Text));
        AddAjaxScriptStatement(GetControlClearStateScript(dtEndDate.ClientID, HTMLControlType.Textbox, dtEndDate.Text));
        AddAjaxScriptStatement(GetControlClearStateScript(txtNameFilter.ClientID, HTMLControlType.Textbox, txtNameFilter.Text));

    }

    protected void Page_PreLoad(object sender, EventArgs e)
    {
        var exportSummary = _service.GetExportSummary(dtStartDate.SelectedDate, dtEndDate.SelectedDate);

        lblSucceedCount.Text = exportSummary.Succeed.ToString();
        lblFailedCount.Text = exportSummary.Failed.ToString();
        lblTotal.Text = exportSummary.Total.ToString();
    }



    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Auto-Export Dashboard";
    }
}
