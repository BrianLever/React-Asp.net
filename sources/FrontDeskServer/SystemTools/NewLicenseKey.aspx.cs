using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;

public partial class SystemTools_NewLicenseKey : BasePage
{
    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (!User.IsInRole(UserRoles.SuperAdministrator))
        {
            RedirectToErrorPage(Resources.TextMessages.AccessPermissionsErrorMessage);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Register License Key";
        if (!IsPostBack)
        {
            txtLicenseKey.Attributes.Add("onkeydown", string.Format("$get('{0}').innerText = '';", lblValidationResult.ClientID));
        }
    }

    public override void SetTabIndex()
    {
        short tabIndex = 1;
        txtLicenseKey.TabIndex = tabIndex++;
        btnRegister.TabIndex = tabIndex++;

        if (!IsPostBack)
        {
            txtLicenseKey.Focus();
        }
        this.Form.DefaultButton = btnRegister.UniqueID;
    }


    protected void btnRegister_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string key = txtLicenseKey.Text.Trim();
            try
            {
                RegisterLicenseKey(key);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ErrorLog.AddServerException("License key registration failed", ex);
                SetErrorAlert(CustomError.GetMessageForCustomOperation("register", "license key"), this.GetType());
            }
        }
    }

    private void RegisterLicenseKey(string key)
    {
        var result = LicenseService.Current.RegisterProductLicense(key);
        
        if (result == LicenseService.RegisterProductLicenseResult.RegisteredNewLicenseKey)
        {
            SetRedirectSuccessAlert(Resources.TextMessages.LicenseKey_SucceedMessage);

            Response.Redirect("Activation.aspx?key=" + key, false);
            //go to activation

        }
        else if (result == LicenseService.RegisterProductLicenseResult.DuplicateLicenseKey)
        {
          
            lblValidationResult.Text = Resources.TextMessages.LicenseKey_DuplicateKeyMessage;
        }
        if (result == LicenseService.RegisterProductLicenseResult.InvalidLicenseKey)
        {
            lblValidationResult.Text = Resources.TextMessages.LicenseKey_InvalidKeyMessage;
        }
        
    }
}
