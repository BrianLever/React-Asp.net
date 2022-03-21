using System;
using FrontDesk.Server.Membership;
using FrontDesk.Server.Web;
using Resources;
using RPMS.Common.Security;

public partial class SystemTools_NewRpmsCredentials : BasePage
{
    private readonly IRpmsCredentialsService _credentialsService;


    public SystemTools_NewRpmsCredentials()
    {
        _credentialsService = new RpmsCredentialsService(new RpmsCredentialsRepository(), new CryptographyService());
    }

    protected override void SetPagePermissions()
    {
        base.SetPagePermissions();

        if (!User.IsInRole(UserRoles.SuperAdministrator))
        {
            RedirectToErrorPage(TextMessages.AccessPermissionsErrorMessage);
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "New EHR Credentials";
        if (!IsPostBack)
        {
            dtExpireOnDate.SelectedDate = DateTime.Today.AddDays(90);
        }
    }

    public override void SetTabIndex()
    {
        short tabIndex = 1;
        txtAccessCode.TabIndex = tabIndex++;
        txtVerifyCode.TabIndex = tabIndex++;

        if (!IsPostBack)
        {
            txtAccessCode.Focus();
        }
        Form.DefaultButton = btnAdd.UniqueID;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid)
        {
            return;
        }

        _credentialsService.AddCredentials(new RpmsCredentials
        {
            AccessCode = txtAccessCode.Text.Trim(),
            VerifyCode = txtVerifyCode.Text.Trim(),
            ExpireAt = dtExpireOnDate.SelectedDate.Value
        });

        Response.Redirect("RpmsCredentials.aspx", false);
    }
}