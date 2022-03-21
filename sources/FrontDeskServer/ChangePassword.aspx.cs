using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using FrontDesk.Server;
using FrontDesk.Control;
using FrontDesk.Common.Messages;
using System.Web.Security;
using FrontDesk.Server.Logging;

public partial class ChangePassword : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Change Password";

        //hide cancel button if user doesn't logged in.
        //It happens when password has been expired
        btnCancel.Visible = User.Identity.IsAuthenticated;
    }

    protected void btnChangePassword_Click(object sender, EventArgs e)
    {
        ucChangePassword.ModifyPassword();
    }

    protected void ucChangePassword_PasswordChanged(object sender, EventArgs e)
    {

        //add log record
        SecurityLog.Add(new SecurityLog(SecurityEvents.PasswordChanged, null));

        this.SetRedirectSuccessAlert(CustomMessage.GetUpdateMessage("Password"));
        try
        {
            Response.Redirect("UserProfile.aspx", true);
        }
        catch (System.Threading.ThreadAbortException) { }
    }

    protected void ucChangePassword_ChangeFailed(object sender, PasswordChangeFailedEventArgs e)
    {
        lblError.Text = e.ErrorMessage;
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            ucChangePassword.SetTabIndex();
        }
    }
}
