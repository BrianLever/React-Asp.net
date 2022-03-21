using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;

using ScreenDox.Server.Common.Configuration;

using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SystemTools_SystemSettings : BasePage
{
    public List<SystemSettings> systemSettings = new List<SystemSettings>();

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Need to protect (select) method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole datasource class from obfuscation.
        odsEvent.TypeName = typeof(FrontDesk.Server.Logging.SecurityLog).FullName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "System Settings";
        GetFormData();
        if (!IsPostBack)
        {
            if (systemSettings.Count > 0)
            {
                DisplaySystemSettings();
            }
        }
    }

    public void GetFormData()
    {
        try
        {
            systemSettings = SystemSettings.GetSystemSettings();
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
        }
    }

    /// <summary>
    /// Display system settings on the page form
    /// </summary>
    public void DisplaySystemSettings()
    {
        foreach (SystemSettings setting in systemSettings)
        {
            if (setting.Key == "PasswordRenewalPeriodDays")
            {
                txtPasswordRenewalPeriodDays.Text = setting.Value;
            }
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (systemSettings.Count > 0)
            {

                foreach (SystemSettings setting in systemSettings)
                {
                    if (setting.Key == "PasswordRenewalPeriodDays")
                    {
                        int period = 0;
                        try
                        {
                            period = Convert.ToInt32(txtPasswordRenewalPeriodDays.Text.Trim());
                        }
                        catch
                        {
                            SetErrorAlert("Invalid data format", GetType());
                            return;
                        }
                        SystemSettings.UpdateSystemSettingsValue(setting.Key, period.ToString());
                    }
                }
                SetRedirectSuccessAlert(FrontDesk.Common.Messages.CustomMessage.GetUpdateMessage("System settings"));
                Response.Redirect(Request.Path, false);
            }
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetUpdateMessage("System settings"), this.GetType());
        }
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (Roles.IsUserInRole(UserRoles.SuperAdministrator))
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

    #region Security Log Settings

    protected void OnCategoryBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
          e.Item.ItemType == ListItemType.AlternatingItem)
        {
            DataRowView view = e.Item.DataItem as DataRowView;
            Repeater rptEvent = e.Item.FindControl("rptEvent") as Repeater;
            rptEvent.DataSource = view.CreateChildView("SecEventToCategory");
            rptEvent.DataBind();
        }
    }

    protected void OnSaveSecuritySettigns(object sender, EventArgs e)
    {
        bool isEventUpdateWithErrors = false;

        foreach (RepeaterItem item in rptCategory.Items)
        {
            Repeater rptEvent = item.FindControl("rptEvent") as Repeater;
            foreach (RepeaterItem eventItem in rptEvent.Items)
            {
                CheckBox cbEnabled = eventItem.FindControl("cbEnabled") as CheckBox;
                HiddenField hfID = eventItem.FindControl("hfID") as HiddenField;

                int eventID = Convert.ToInt32(hfID.Value);

                try
                {
                    SecurityLog.SetEventEnabledStatus(eventID, cbEnabled.Checked);
                }
                catch(Exception ex)
                {
                    ErrorLog.Add(ex.Message, ex.StackTrace, null);
                    isEventUpdateWithErrors = true;
                }
            }
        }

        if (isEventUpdateWithErrors)
        {
            SetErrorAlert(Resources.TextMessages.SecurityStateSavedWithErrors, Page.GetType());
        }
        else
        {
            SetRedirectSuccessAlert(CustomMessage.GetUpdateMessage("Security event list"));
            Response.Redirect(Request.Path, false);
        }
    }

    #endregion
}
