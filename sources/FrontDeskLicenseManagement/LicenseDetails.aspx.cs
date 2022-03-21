using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Licensing.Management;
//using FrontDesk.Server.Web;
using FrontDesk.Server.Logging;
using System.Threading;
using FrontDesk.Server.LicenseManagerWeb;

public partial class LicenseDetails : LMBaseManagementWebForm<LicenseEntry, Int32>
{
    private ActivationEntry activation;

    protected void Page_Load(object sender, EventArgs e)
    {
        bool isIdFound = false;

        this.formObjectID = GetPageIDValue("id", out isIdFound);
        EnsureFormObjectCreated();

        if (IsNewInstance)
        {
            Master.PageHeaderText = "Create New Licenses";
        }
        else
        {
            Master.PageHeaderText = "License Details";
        }

        btnCopyLicenseCode.Attributes.Add("onclick", string.Format("copyToClipboard($get('{0}')); return false;", lblLicenseString.ClientID));
        btnCopyActivationRequest.Attributes.Add("onclick", string.Format("copyToClipboard($get('{0}')); return false;", lblActivationRequest.ClientID));
        btnCopyActivationKey.Attributes.Add("onclick", string.Format("copyToClipboard($get('{0}')); return false;", lblActivationKey.ClientID));

        if (!IsPostBack)
        {
            EditModeDataPrepare(this.formObjectID);
        }
    }

    protected override LicenseEntry GetFormData()
    {
        LicenseEntry license = (CurFormObject == null) ? new LicenseEntry() : CurFormObject;

        license.Years = int.Parse(txtYears.Text);
        license.MaxBranchLocations = int.Parse(txtMaxLocations.Text);
        license.MaxKiosks = int.Parse(txtMaxKiosks.Text);
        bool isClientFound = false;
        int clientID = GetPageIDValue("client", out isClientFound);
        license.ClientID = clientID > 0 ? clientID : (int ?)null;

        return license;
    }

    protected override void EditModeDataPrepare(int objectID)
    {
        plcNew.Visible = IsNewInstance;
        plcExisting.Visible = !IsNewInstance;
        plcOwner.Visible = !IsNewInstance && CurFormObject.IsAssignedToClient;
        plcActivation.Visible = !IsNewInstance && CurFormObject.IsActivated;

        btnCreate.Visible = IsNewInstance;
        btnDelete.Visible = !IsNewInstance && !CurFormObject.IsAssignedToClient;

        txtYears.ReadOnly = !IsNewInstance;
        txtMaxLocations.ReadOnly = !IsNewInstance;
        txtMaxKiosks.ReadOnly = !IsNewInstance;


        if (!IsNewInstance)
        {
            lblSerialNumber.Text = CurFormObject.SerialNumber.ToString();
            txtYears.Text = CurFormObject.Years.ToString();
            txtMaxLocations.Text = CurFormObject.MaxBranchLocations.ToString();
            txtMaxKiosks.Text = CurFormObject.MaxKiosks.ToString();

            lblLicenseString.Text = CurFormObject.LicenseString;
            lblIssued.Text = CurFormObject.Issued.ToString("MM/dd/yyyy");
            if (CurFormObject.IsAssignedToClient)
            {
                //lblCompanyName.Text = CurFormObject.CompanyName;
                hlnClientDetails.Text = CurFormObject.CompanyName;
                hlnClientDetails.NavigateUrl = string.Format("ClientDetails.aspx?id={0}", CurFormObject.ClientID);
            }

            if (CurFormObject.IsActivated && this.activation != null)
            {
                lblActivationDate.Text = this.activation.Issued.ToString("MM/dd/yyyy");
                lblActivationRequest.Text = this.activation.ActivationRequest;
                lblActivationKey.Text = this.activation.ActivationKey;
                lblExpirationDate.Text = this.activation.ExpirationDate.Value.ToString("MM/dd/yyyy");
            }
        }
        else
        {
            lblSerialNumber.Text = "<< New License >>";
            txtYears.Text = "1";
            txtNumLicenses.Text = "1";
        }
    }

    protected override LicenseEntry GetFormObjectByID(int objectID)
    {
        LicenseEntry license = null;
        try
        {
            license = LicenseEntityHelper.Instance.GetByID(this.formObjectID);
            if (license != null && license.IsActivated)
            {
                this.activation = ActivationEntry.GetByLicenseID(license.LicenseID);
            }
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
        }
        return license;
    }

    private void CreateLicenses()
    {

        try
        {
            LicenseEntry license = GetFormData();
            int quantity = int.Parse(txtNumLicenses.Text);

            if (quantity == 1)
            {
                // single license
                int newId = LicenseEntityHelper.Instance.Create(license);
                //Assign license to client
                if (license.ClientID != null)
                {
                    SetRedirectAlert("License has been created and assigned to client successfully.");
                    Response.Redirect(String.Format("~/ClientDetails.aspx?id={0}", license.ClientID, false));
                }
                else
                {
                    SetRedirectAlert(FrontDesk.Common.Messages.CustomMessage.GetAddMessage("License"));
                    Response.Redirect(String.Format("~/LicenseDetails.aspx?id={0}", newId, false));
                }
            }
            else
            {
                // pack
                int count = LicenseEntityHelper.Instance.CreatePack(license, quantity);
                //Assign license pack to client
                if (license.ClientID != null)
                {
                    SetRedirectAlert(string.Format("{0} licenses have been created  successfully.", count));
                    Response.Redirect(String.Format("~/ClientDetails.aspx?id={0}", license.ClientID, false));
                }
                else
                {
                    SetRedirectAlert(string.Format("{0} licenses have been created successfully.", count));
                    Response.Redirect("~/Licenses.aspx");
                }
            }
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
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetAddMessage("License"), this.GetType());
        }
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            txtYears.Focus();
        }
    }

    protected void btnCreate_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            CreateLicenses();
        }
    }

    protected void btnDelete_Click(object sender, EventArgs e)
    {
        try
        {
            LicenseEntityHelper.Instance.Delete(this.formObjectID);
            Response.Redirect("Licenses.aspx");
        }
        catch (ThreadAbortException)
        {
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert(FrontDesk.Common.Messages.CustomError.GetDeleteMessage("License"), this.GetType());
        }
    }
}
