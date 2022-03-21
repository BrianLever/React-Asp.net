using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using FrontDesk.Server.Web;
using FrontDesk.Server.Licensing.Management;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using System.Threading;
using FrontDesk.Server.LicenseManagerWeb;

public partial class AssignLicense : LMBaseManagementWebForm<Client, Int32>
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            bool isIdFound = false;
            this.formObjectID = GetPageIDValue("id", out isIdFound);            

            Master.PageHeaderText = "Client Licenses";

            if (!IsPostBack)
            {
                EditModeDataPrepare(this.formObjectID);
            }

            BindPageListControls();
        }
        catch (Exception ex)
        {
            ErrorLog.AddServerException(CustomError.GetFormMessage("client"), ex);
            RedirectToErrorPage(CustomError.GetInternalErrorMessage());
        }
    }

    protected override Client GetFormData()
    {
        return new Client();
    }

    protected override void EditModeDataPrepare(int objectID)
    {
        hlClient.NavigateUrl += "?id=" + this.formObjectID;
        hlClient.Text = this.CurFormObject.CompanyName;
    }

    protected override Client GetFormObjectByID(int objectID)
    {
        return Client.GetByID(objectID);
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            txtLicenseKey.Focus();
        }
    }

    protected void btnAssign_Click(object sender, EventArgs e)
    {
        try
        {
            string licenseKey = this.txtLicenseKey.Text.Trim();
            LicenseEntry license = LicenseEntityHelper.Instance.GetByLicenseKey(licenseKey);
            if (license == null)
            {
                //SetRedirectAlert(string.Format("License key '{0}' is invalid.", licenseKey));
                //Response.Redirect(Request.RawUrl);
                SetErrorAlert(string.Format("License key '{0}' is invalid.", licenseKey), this.GetType());
                return;
            }

            LicenseEntityHelper.Instance.AssignToClient(license, this.formObjectID);
            SetRedirectAlert("License is assigned successfully.");
            Response.Redirect(string.Format("~/ClientDetails.aspx?id={0}", this.formObjectID, false));
        }
        catch (ThreadAbortException)
        {
        }
        catch (ArgumentException ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert("This License is already assigned to another Client.", this.GetType());
        }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            SetErrorAlert("Failed to assign license.", this.GetType());
        }
    }
}
