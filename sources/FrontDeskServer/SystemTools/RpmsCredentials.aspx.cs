using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls;
using FrontDesk;
using FrontDesk.Common;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Licensing.Services;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;
using iTextSharp.text.pdf.crypto;
using RPMS.Common.Security;

public partial class SystemTools_RpmsCredentials : BasePage
{
    private readonly IRpmsCredentialsService _rpmsCredentialsService;

    public SystemTools_RpmsCredentials()
    {
        _rpmsCredentialsService = new RpmsCredentialsService(new RPMS.Common.Security.RpmsCredentialsRepository(), new CryptographyService());
    }

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
                    var id = Guid.Parse(link.CommandArgument);
                    _rpmsCredentialsService.DeleteCredentials(id);
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException(CustomError.GetDeleteMessage("EHR Export Credentials"), ex);
                SetErrorAlert(CustomError.GetDeleteMessage("EHR Credentials"), this.GetType());
            }
            grvItems.DataBind();


        }
    }

    public void BindDataGrid()
    {
        grvItems.DataSource = _rpmsCredentialsService.GetAllCredentials().Select(x =>
        {
            x.VerifyCode = x.VerifyCode.AsMaskedPassword();

            return x;
        });
        grvItems.DataBind();
    }

   

    void Page_PreRender(object sender, EventArgs e)
    {
        BindDataGrid();
    }

    void grvItems_DataBinding(object sender, EventArgs e)
    {
        ActiveCredentialsDisplayed = false;
    }

    protected bool ActiveCredentialsDisplayed = false;

    void grvItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            var credentials = (RpmsCredentials)e.Row.DataItem;

            var cells = e.Row.Cells;
            cells[0].Text = credentials.AccessCode;
            cells[1].Text = credentials.VerifyCode;
            cells[2].Text = credentials.ExpireAt.ToString("MM/dd/yyyy");

            var btnDelete = cells[cells.Count - 1].FindControl("btnDelete") as LinkButton;
            if (btnDelete != null)
            {
                btnDelete.CommandArgument = credentials.Id.ToString();
            }



            if (!ActiveCredentialsDisplayed && credentials.ExpireAt > DateTime.Now)
            {
                ActiveCredentialsDisplayed = true;
            }
            else
            {
                e.Row.CssClass = "disabled";
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "EHR Credentials For Export";
    }
}
