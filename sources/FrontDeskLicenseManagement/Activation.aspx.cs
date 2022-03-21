using System;
using System.Threading;
using System.Web.UI;
using FrontDesk.Server.LicenseManagerWeb;
using FrontDesk.Server.Licensing.Management;
using FrontDesk.Server.Logging;

public partial class Activation : LMBasePage
{
    private const string ActivationRequestKey = "activationRequest";

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "License Activation";

        //btnCancel.OnClientClick = String.Format("location.href='{0}';return false;", Request.Url.AbsoluteUri);
        btnCancel.OnClientClick = String.Format("location.href='{0}';return false;", Request.Path); // to self without parameters
        btnCopyLicenseKey.Attributes.Add("onclick", string.Format("copyToClipboard($get('{0}')); return false;", lblLicense.ClientID));
        
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request[ActivationRequestKey]))
            {
                // activation request passed as request parameter
                txtActivationRequest.Text = Request[ActivationRequestKey];
                PrepareActivationStep();
            }
        }
    }
    
    private void Activate()
    {
        string activationRequest = txtActivationRequest.Text.Trim();

        try
        {
            ActivationEntry act = CreateActivationEntry(activationRequest);
            if (act == null) // invalid activation key
            {
                SetErrorAlert(string.Format(Resources.TextMessages.LicenseActivation_IvalidRequestActivationKey,
                    act.LicenseSerialNumber), this.GetType());
                return;
            }


            LicenseEntry lic = LicenseEntityHelper.Instance.GetBySerialNumber(act.LicenseSerialNumber);
            if (lic == null) // invalid license number
            {
                ErrorLog.Add(string.Format("Activation request '{0}' entered for non-existing license serial number '{1}'.", 
                    activationRequest, act.LicenseSerialNumber), null, null);
                SetErrorAlert(string.Format(Resources.TextMessages.LicenseActivation_LicenseNotExists, 
                    act.LicenseSerialNumber), this.GetType());
                return;
            }

            act.CalculateActivationKey(SelectedEffectiveDate);
            int newActID = ActivationEntry.ActivateLicense(act, lic);
            if (newActID == 0)
            {
                ErrorLog.Add(string.Format("Activation request '{0}' entered for license that is already activated. License serial number is '{1}'.",
                    activationRequest, act.LicenseSerialNumber), null, null);

                SetRedirectAlert("");
            }
            else
            {
                SetRedirectAlert(Resources.TextMessages.LicenseActivation_SuccessfullActivation);
                
            }            
            
            Response.Redirect(String.Format("~/LicenseDetails.aspx?id={0}", lic.LicenseID), false);
        }
        catch (ThreadAbortException)
        {

        }
    }

    private void PrepareActivationStep()
    {
        string activationRequest = txtActivationRequest.Text.Trim();
        ActivationEntry act = CreateActivationEntry(activationRequest);
        if (act == null) //invalid activation request code
        {
            SetErrorAlert(Resources.TextMessages.LicenseActivation_IvalidRequestActivationKey, Page.GetType());
            return;
        }

        LicenseEntry lic = LicenseEntityHelper.Instance.GetBySerialNumber(act.LicenseSerialNumber);
        if (lic == null) // invalid activation request code
        {
            ErrorLog.Add(string.Format("Activation request '{0}' entered for non-existing license serial number '{1}'.",
                activationRequest, act.LicenseSerialNumber), null, null);
            SetErrorAlert(string.Format(Resources.TextMessages.LicenseActivation_LicenseNotExists,
                act.LicenseSerialNumber), this.GetType());
        }
        else
        {
            DisplayLicense(lic);

            ActivationEntry ae = ActivationEntry.GetByLicenseID(lic.LicenseID);
            if (ae != null)
            {
                ErrorLog.Add(string.Format("Activation request '{0}' entered for license that is already activated. License serial number is '{1}'.",
                    activationRequest, act.LicenseSerialNumber), null, null);
                phErr.Visible = true;
                lblErr.Text = Resources.TextMessages.LicenseActivation_AlreadyActivated;
                phExpDate.Visible = false;
                btnActivate.Enabled = false;
            }
            else
            {
                if (lic.IsAssignedToClient)
                {
                    phExpDate.Visible = true;
                    SetExpirationDate();
                }
                else
                {
                    phExpDate.Visible = false;
                }
            }
        }

        // do not use Visible property for btnActive, it is required for refresh action
        btnContinue.Attributes.CssStyle.Add(HtmlTextWriterStyle.Display, "none");
        btnActivate.Visible = true;
        txtActivationRequest.ReadOnly = true;
    }

    protected void btnContinue_Click(object sender, EventArgs e)
    {
        PrepareActivationStep();
    }

    protected void btnActivate_Click(object sender, EventArgs e)
    {
        try
        {
            this.Activate();
        }
        catch (ThreadAbortException)
        {
        }
        catch (ApplicationException ex)
        {
            SetErrorAlert(ex.Message, this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert("Failed to activate license.", this.GetType());
        }
    }

    protected void OnTommorowSelected(object sender, EventArgs e)
    {
        rdpOtherDate.Enabled = false;
        lbnCalculate.Enabled = false;
        rfvOtherDate.Enabled = false;

        SetExpirationDate();
    }

    protected void OnOtherDateSelected(object sender, EventArgs e)
    {
        rdpOtherDate.Enabled = true;
        lbnCalculate.Enabled = true;
        rfvOtherDate.Enabled = true;
    }

    protected void CalculateOther_Click(object sender, EventArgs e)
    {
        SetExpirationDate();
    }

    private void DisplayLicense(LicenseEntry le)
    {
        phLicenseDetails.Visible = true;
        lblSerialNumber.Text = le.SerialNumber.ToString();
        lblYearsOfValidity.Text = le.Years.ToString();
        lblBrNum.Text = le.MaxBranchLocations.ToString();
        lblKiosksNum.Text = le.MaxKiosks.ToString();
        lblLicense.Text = le.LicenseString;

        phExistClient.Visible = le.IsAssignedToClient;
        phNoClient.Visible = !le.IsAssignedToClient;
        btnActivate.Enabled = le.IsAssignedToClient;
        phExpDate.Visible = le.IsAssignedToClient;

        if (le.IsAssignedToClient)
        {
            hlClient.Text = le.CompanyName;
            hlClient.NavigateUrl += "?id=" + le.ClientID.Value.ToString();
        }
        else
        {
            lblNoClient.Text = String.Format(Resources.TextMessages.ActivationError_LicenseWithoutCientMsg,
                Page.ResolveClientUrl("~/Clients.aspx"),
                //Page.GetPostBackClientHyperlink(btnActivate, ""));
                string.Format("{0}?{1}={2}", Request.Path, ActivationRequestKey, txtActivationRequest.Text));   // to self with parameter
        }
    }

    private DateTime SelectedEffectiveDate
    {
        get
        {
            if (rbnTomorrow.Checked)
            {
                return DateTime.Now.AddDays(1).Date;
            }
            else
            {
                return rdpOtherDate.SelectedDate.Value.Date;
            }
        }
    }

    private void SetExpirationDate()
    {
        string activationRequest = txtActivationRequest.Text.Trim();
        ActivationEntry act = CreateActivationEntry(activationRequest);
        if (act == null)
        {
            SetErrorAlert(Resources.TextMessages.LicenseActivation_IvalidRequestActivationKey, 
                this.GetType());
            return;
        }

        LicenseEntry lic = LicenseEntityHelper.Instance.GetBySerialNumber(act.LicenseSerialNumber);

        lblExpirationDate.Text = SelectedEffectiveDate.AddYears(lic.Years).ToShortDateString();
    }

    public ActivationEntry CreateActivationEntry(string activationRequest)
    {
        ActivationEntry ae = null;

        try
        {
            ae = new ActivationEntry(activationRequest);
        }
        catch { }

        return ae;
    }

}
