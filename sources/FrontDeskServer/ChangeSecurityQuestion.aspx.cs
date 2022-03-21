using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web;
using FrontDesk.Common.Messages;
using System.Web.Security;
using FrontDesk.Server.Logging;

public partial class ChangeSecurityQuestion : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.PageHeaderText = "Change Security Question and Answer";
        ucChangeQuestion.UserName = Page.User.Identity.Name;
        ucChangeQuestion.Password = txtPassword.Text;
    }

    protected void ucChangeQuestion_ChangeFailed(object sender, QuestionChangeFailedEventArgs e)
    {
        lblError.Text = e.ErrorMessage;
    }

    protected void ucChangeQuestion_PasswordChanged(object sender, EventArgs e)
    {
        //add log record
        SecurityLog.Add(new SecurityLog(SecurityEvents.SecurityQuestionAndAnswerChanged, null));


        this.SetRedirectSuccessAlert(CustomMessage.GetUpdateMessage("Security question and answer"));
        try
        {
            Response.Redirect("UserProfile.aspx", true);
        }
        catch (System.Threading.ThreadAbortException) { }
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        ucChangeQuestion.ChangeSequrityQuestionAndAnswer();
    }

    public override void SetTabIndex()
    {
        if (!IsPostBack)
        {
            txtPassword.Focus();
                
        }
    }

}
