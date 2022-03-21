using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk;
using FrontDesk.Common.Configuration;
using FrontDesk.Common.Messages;
using FrontDesk.Common.Screening;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;
using ScreenDox.Server.Common.Configuration;

public partial class SystemTools_AgeGroupSettings : BasePage
{
    public List<SystemSettings> systemSettings = new List<SystemSettings>();
    public readonly IScreeningAgesSettingsProvider _ageProvider = new ScreeningAgesDbProvider();

    protected string DefaultAgeGroupValues;
    protected string ClientRegexExpression;

    protected void Page_Init(object sender, EventArgs e)
    {
        DefaultAgeGroupValues = AppSettingsProxy.GetStringValue("IndicatorReport_AgeGroups", string.Empty);

        rgvAgeSettings.Text = "Age groups should follow the sample format: {0}"
            .FormatWith(DefaultAgeGroupValues);

        ClientRegexExpression = @"/{0}/i".FormatWith(ScreeningAgesSettingsProvider.RegexValidationExpression.Replace(@"\", @"\\"));

        //rgvAgeSettings.ValidationExpression = ScreeningAgesSettingsProvider.RegexExpression;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Age Groups for Indicator Report";
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
            if (setting.Key == "IndicatorReport_AgeGroups")
            {
                txtAgeSettings.Text = setting.Value;
            }
        }


        lblPreview.Text = string.Join("<br />", _ageProvider.AgeGroupsLabels);
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (systemSettings.Count > 0)
            {

                foreach (SystemSettings setting in systemSettings)
                {
                    if (setting.Key == "IndicatorReport_AgeGroups")
                    {
                        SystemSettings.UpdateSystemSettingsValue(setting.Key, txtAgeSettings.Text.Trim());
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


    protected void rgvAgeSettings_ServerValidate(object source, ServerValidateEventArgs args)
    {
        var re = new Regex(ScreeningAgesSettingsProvider.RegexValidationExpression, RegexOptions.IgnoreCase);
        args.IsValid = re.IsMatch(args.Value);
    }
}
