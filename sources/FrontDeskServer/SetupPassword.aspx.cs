using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using FrontDesk.Control;
using FrontDesk.Common.Messages;
using FrontDesk.Server;
using System.Web.Security;
using FrontDesk.Server.Logging;

public partial class SetupPassword : BasePage
{
    bool _passwordChanged = false;
    bool _questionAndAnswerChanged = false;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Setup password";
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        ucChangePassword.ModifyPassword();
    }

    protected void ucChangePassword_PasswordChanged(object sender, EventArgs e)
    {
        _passwordChanged = true;

        ucChangeQuestion.Password = ucChangePassword.NewPassword;
        ucChangeQuestion.ChangeSequrityQuestionAndAnswer();
    }

    protected void ucChangePassword_ChangeFailed(object sender, PasswordChangeFailedEventArgs e)
    {
        _passwordChanged = false;
        lblError.Text = e.ErrorMessage;
    }

    protected void ucChangeQuestion_Changed(object sender, EventArgs e)
    {
        _questionAndAnswerChanged = true;
    }

    protected void ucChangeQuestion_ChangeFailed(object sender, QuestionChangeFailedEventArgs e)
    {
        _questionAndAnswerChanged = false;
        lblError.Text = e.ErrorMessage;
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (_questionAndAnswerChanged && _passwordChanged)
        {
            SecurityLog.Add(new SecurityLog(SecurityEvents.AccountActivated, null));
            
            SetRedirectSuccessAlert(CustomMessage.GetUpdateMessage("Security information"));
            Response.Redirect("~/UserProfile.aspx");
        }
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            ucChangePassword.SetTabIndex();
        }
    }
}
