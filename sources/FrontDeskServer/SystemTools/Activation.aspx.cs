using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Configuration;

public partial class SystemTools_Activation : BaseManagementWebForm<LicenseCertificate, string>
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
        Master.PageHeaderText = "Product Activation";

        TryGetPageIDValue("key");
        try
        {
            if (string.IsNullOrEmpty(formObjectID))
            {
                var cert = LicenseService.Current.GetLicenseCertificate();
                formObjectID = cert.License.LicenseString;

            }
            EnsureFormObjectCreated();
            if (!IsNewInstance)
            {
                if (!string.IsNullOrEmpty(CurFormObject.ActivationKeyString))
                {
                    SetRedirectInfoAlert(Resources.TextMessages.Activation_LisenceIsAlreadyActivatedMessage);
                    Response.Redirect("LicenseManagement.aspx", false);
                    return;
                }


                EditModeDataPrepare(formObjectID);
            }
            else
            {
                Response.Redirect("LicenseManagement.aspx", false);
            }

            btnCopyRequestCode.Attributes.Add("onclick", string.Format("copyToClipboard($get('{0}')); return false;", ltrActivationRequest.ClientID));
            btnCopyLicense.Attributes.Add("onclick", string.Format("copyToClipboard($get('{0}')); return false;", ltrLicenseKey.ClientID));


            if (!IsPostBack)
            {
                txtActivationKey.Attributes.Add("onkeydown", string.Format("$get('{0}').innerText = '';", lblValidationResult.ClientID));
            }
        }
        catch (Exception ex)
        {
            ErrorLog.AddServerException("Failed to open Product Activation page", ex);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }

    }



    protected override LicenseCertificate GetFormObjectByID(string objectID)
    {
            return LicenseService.Current.GetLicenseCertificate(objectID);
        
    }

    protected override void EditModeDataPrepare(string objectID)
    {
        ltrLicenseKey.Text = CurFormObject.License.LicenseString;
        ltrActivationRequest.Text = CurFormObject.ActivationRequestCode;
    }

    protected override LicenseCertificate GetFormData()
    {
        throw new NotImplementedException();
    }

    protected override void SetControlsEnabledState()
    {
        base.SetControlsEnabledState();
        try
        {
            if (!string.IsNullOrEmpty(ServerSettings.ActivationSupportEmail) && !string.IsNullOrEmpty(ltrActivationRequest.Text))
            {
                string sendEmailLink = string.Format("mailto:{0}?subject={1}&body={2}",
                    ServerSettings.ActivationSupportEmail,
                    ServerSettings.ActivationSupportEmailSubject,
                    string.Format(ServerSettings.ActivationRequestEmailTemplate, CurFormObject.ActivationRequestCode));
                lnkSendRequestCode.NavigateUrl = sendEmailLink;
                lnkSendRequestCode.Visible = true;
            }
            else
            {
                lnkSendRequestCode.Visible = false;
            }

        }
        catch (Exception ex)
        {
            ErrorLog.AddServerException(string.Empty, ex);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
        btnCopyRequestCode.Visible = !string.IsNullOrEmpty(ltrActivationRequest.Text);
        btnCopyLicense.Visible = !string.IsNullOrEmpty(ltrLicenseKey.Text);
            
    }

    #region Activation

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            string key = txtActivationKey.Text.Trim();
            try
            {
                ActivateLicenseKey(key);
            }
            catch (System.Threading.ThreadAbortException) { }
            catch (Exception ex)
            {
                ErrorLog.AddServerException("License key activation failed", ex);
                SetErrorAlert(CustomError.GetMessageForCustomOperation("activate", "product license"), this.GetType());
            }
        }
    }

    private void ActivateLicenseKey(string activationKey)
    {
        LicenseService.ActivateProductLicenseResult result = LicenseService.Current.ActivateLicense(CurFormObject.License.LicenseString, activationKey);

        if (result == LicenseService.ActivateProductLicenseResult.Activated)
        {
            SetRedirectSuccessAlert(Resources.TextMessages.Activation_SucceedMessage);

            Response.Redirect("LicenseManagement.aspx", false);
           
        }
        else if (result == LicenseService.ActivateProductLicenseResult.InvalidActivationKey)
        {
            lblValidationResult.Text = Resources.TextMessages.Activation_InvalidKeyMessage;
        }

    }

    #endregion

    #region Activation Request Code

    protected void btnGetRequestCode_Click(object sender, EventArgs e)
    {
        try
        {
            GenerateNewActivationRequestCode();

        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.AddServerException("Activation request code generation failed", ex);
            SetErrorAlert(CustomError.GetMessageForCustomOperation("generate", "activation request key"), this.GetType());
        }
    }

    private void GenerateNewActivationRequestCode()
    {
        if (!IsNewInstance)
        {
            var activationRequestKey = LicenseService.Current.UpdateActivationRequestKey(this.CurFormObject);
            if (string.IsNullOrEmpty(activationRequestKey))
            {
                SetErrorAlert(Resources.TextMessages.LicenseKey_CreateActivationREquestKeyFailedMessage, this.GetType());
            }
            ltrActivationRequest.Text = activationRequestKey;
            btnCopyRequestCode.Visible = true;
        }
    }

    #endregion
}
