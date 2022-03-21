using System;
using System.Web.UI.WebControls;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;

public partial class SystemTools_LicenseManagement : BasePage
{
    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (!User.IsInRole(UserRoles.SuperAdministrator))
        {
            RedirectToErrorPage(Resources.TextMessages.AccessPermissionsErrorMessage);
        }
    }


    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Protect method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole class from obfuscation.
        odsLicences.TypeName = typeof(FrontDesk.Server.Licensing.Services.LicenseService).FullName;

        grvItems.RowDataBound += new GridViewRowEventHandler(grvItems_RowDataBound);
        grvItems.DataBinding += new EventHandler(grvItems_DataBinding);
        grvItems.RowCommand += new GridViewCommandEventHandler(grvItems_RowCommand);
    }

    void grvItems_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "remove")
        {
            try
            {

                var link = e.CommandSource as LinkButton;
                if (link != null)
                {
                    var licenseKey = (string)link.CommandArgument;
                    LicenseService.Current.RemoveLicense(licenseKey);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException(CustomError.GetDeleteMessage("License Key"), ex);
                SetErrorAlert(CustomError.GetDeleteMessage("License Key"), this.GetType());
            }
            grvItems.DataBind();

           
        }
    }

    

    void grvItems_DataBinding(object sender, EventArgs e)
    {
        activatedLicenseDisplayed = false;
    }

    protected bool activatedLicenseDisplayed = false;

    void grvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            LicenseCertificate license = e.Row.DataItem as LicenseCertificate;

            var cells = e.Row.Cells;
            cells[0].Text = license.License.LicenseString;
            cells[1].Text = license.CreatedDate.ToString("MM/dd/yyyy");
            cells[2].Text = license.License.MaxBranchLocations.ToString();
            cells[3].Text = license.License.MaxKiosks.ToString();
            cells[4].Text = license.License.Years.ToString();

            var btnDelete = cells[cells.Count - 1].FindControl("btnDelete") as LinkButton;
            if (btnDelete != null)
            {
                btnDelete.CommandArgument = license.License.LicenseString;
            }

            if (license.ActivationKey != null )
            {
                cells[5].Text = license.ActivatedDate.Value.ToString("MM/dd/yyyy");
                cells[6].Text = license.ExpirationDate.Value.ToString("MM/dd/yyyy");

                if (!activatedLicenseDisplayed && license.ExpirationDate > DateTime.Now)
                {
                    activatedLicenseDisplayed = true;
                }
                else
                {
                    e.Row.CssClass = "disabled";
                }
            }
            else
            {
                if (cells.Count == 8)
                {
                    //cells.RemoveAt(6);
                    //cells[5].ColumnSpan = 2;
                    cells[5].CssClass = "action";
                    HyperLink link = new HyperLink();
                    link.ApplyStyleSheetSkin(this);
                    link.Text = "Activate";
                    link.NavigateUrl = string.Format("Activation.aspx?key={0}", license.License.LicenseString);
                    cells[5].Controls.Add(link);
                }
            }

           
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Product Registration";
    }
}
