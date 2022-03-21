using FrontDesk.Common.Messages;
using FrontDesk.Control;
using FrontDesk.Server;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Web;

using System;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ForgotPassword : BasePage
{
    //entered user name
    private string UserName
    {
        get
        {
            return txtUserName.Text.Trim();
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Password Recovery";
        if (wizardForgotPassword.ActiveStepIndex == 2)
        {
            ucResetPassword.UserName = this.UserName;
        }
    }

    /// <summary>
    /// Render header for wizard (view all wizard steps as link)
    /// </summary>
    protected string RenderHeaderStepsHeaders()
    {
        StringBuilder stringTable = new StringBuilder();
        WizardStepBase step;

        for (int i = 0; i < wizardForgotPassword.WizardSteps.Count; i++)
        {
            step = wizardForgotPassword.WizardSteps[i];
            stringTable.AppendFormat("<li {0}>{1}</li>",
                step == wizardForgotPassword.ActiveStep ? "class=\"selected\"" : String.Empty,
                step.Title);
        }
        return stringTable.ToString();
    }

    public override void SetTabIndex()
    {
        //if (!IsPostBack)
        //{
            if (wizardForgotPassword.ActiveStepIndex == 0)
            {
                txtUserName.Focus();
            }
            if (wizardForgotPassword.ActiveStepIndex == 1)
            {
                txtAnswer.Focus();
                //ucSecurityQuestion.SetTabIndex();
            }
            if (wizardForgotPassword.ActiveStepIndex == 2)
            {
                ucResetPassword.SetTabIndex();
            }
        //}
    }


    protected void OnNextButtonClick(object sender, WizardNavigationEventArgs e)
    {
        if (wizardForgotPassword.ActiveStepIndex == 0) // getting user name
        {
            try
            {
                Page.Validate("EnterUserName");
                if (Page.IsValid)
                {
                    MembershipUser user = Membership.GetUser(UserName);
                    FDUser u = FDUser.FindUsersByName(UserName);
                    if (u == null || u.IsLockedOut || u.IsBlock)
                    {
                        lblNotValidUserName.Text = 
                            "You have entered invalid user name or your account has been blocked.";
                        e.Cancel = true;
                    }
                    else if (String.IsNullOrEmpty(u.PasswordQuestion))
                    {
                        lblNotValidUserName.Text = "Your account has not been activated.";
                        e.Cancel = true;
                    }
                    else
                    {
                        lblQuestion.Text = user.PasswordQuestion;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                SetErrorAlert(CustomError.GetInternalErrorMessage(), this.GetType());
            }
        }
        else if (wizardForgotPassword.ActiveStepIndex == 1) // sec. question and answer
        {
            Page.Validate("SecurityQuestion");
            if (Page.IsValid)
            {
                //ucSecurityQuestion.UserName = this.UserName;
                bool isValid = false;
                try
                {
                    isValid = FDUser.ValidateSecurityQuestionAndAnswer(this.UserName, txtAnswer.Text);
                }
                catch (Exception ex)
                {
                    ErrorLog.Add(ex.Message, ex.StackTrace, null);
                    this.SetErrorAlert(CustomError.GetInternalErrorMessage(), Page.GetType());
                    return;
                }

                if (!isValid)
                {
                    lblNotValidSecurityQuestion.Text = "You have entered invalid security answer.";
                    e.Cancel = true;
                }
            }
        }
        wizardForgotPassword.ActiveStepIndex = wizardForgotPassword.ActiveStepIndex + 1;
    }


    protected void OnComplete(object sender, EventArgs e)
    {
        Page.Validate("SetupNewPassword");
        if (Page.IsValid)
        {
            ucResetPassword.UserName = this.UserName;
            ucResetPassword.ModifyPassword();
        }
    }

    protected void ucResetPassword_ChangeFailed(object sender, PasswordChangeFailedEventArgs e)
    {
        lblInvalidPassword.Text = e.ErrorMessage;
    }

    protected void ucResetPassword_Changed(object sender, EventArgs e)
    {
        this.SetRedirectSuccessAlert("Your password has been successfully changed.\n Please login using new password.");
        try
        {
            Response.Redirect("Login.aspx", true);
        }
        catch (System.Threading.ThreadAbortException) { }

    }
}
