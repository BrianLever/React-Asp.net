using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Common.Messages;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Web.Controls;

public partial class ModifySequrityQuestionCtrl : BaseUserControl
{
    public string ValidationGroup
    {
        get
        {
            return vldSecurityQuestion.ValidationGroup;
        }
        set
        {
            vldSecurityQuestion.ValidationGroup = value;
            vldOwnQuestion.ValidationGroup = value;
            vldSecurityAnswer.ValidationGroup = value;
        }
    }

    public string UserName { get; set; }

    public string Password { get; set; }

    protected void Page_Init(object sender, EventArgs e)
    {
        // Enable obfuscation for TypeName="...". This overloads class name from .aspx
        // Protect method name from obfuscation: unable to determine obfuscated name of method.
        // Method names and parameters may stay in .aspx. If named parameters used, protect whole class from obfuscation.
        odsrSecurityQuestion.TypeName = typeof(FrontDesk.Server.SecurityQuestion).FullName;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            EditModeDataPrepare();
        }
    }

    public event EventHandler<QuestionChangeFailedEventArgs> ChangeFailed;
    public event EventHandler PasswordChanged;

    private string Question
    {
        get
        {
            return ddlSecurityQuestion.SelectedValue == Resources.TextMessages.WriteMyOwnSecurityQuestion ?
                txtOwnQuestion.Text : ddlSecurityQuestion.SelectedValue;
        }
    }

    private string Answer
    {
        get
        {
            return txtSecurityAnswer.Text;
        }
    }

    public void EditModeDataPrepare()
    {
        ddlSecurityQuestion.DataBind();
        ddlSecurityQuestion.Items.Insert(0, new ListItem(Resources.TextMessages.DropDown_NotSelectedText, ""));
        ddlSecurityQuestion.Items.Add(new ListItem(Resources.TextMessages.WriteMyOwnSecurityQuestion));
    }

    /// <summary>
    /// Perform changing of security question and answer
    /// </summary>
    public void ChangeSequrityQuestionAndAnswer()
    {
        if (String.IsNullOrEmpty(Password))
        {
            throw new ApplicationException("Password is not provided");
        }

        MembershipUser user = Membership.GetUser();
        try
        {
            if (!user.ChangePasswordQuestionAndAnswer(Password, Question, Answer))
            {
                Page.SetRedirectFailureAlert("Failed to change security question and answer. You have entered invalid password value.");
                Response.Redirect("ChangeSecurityQuestion.aspx", false);
            }
            else
            {
                if (PasswordChanged != null)
                {
                    PasswordChanged(this, new EventArgs());
                }
            }
        }
        catch (System.Threading.ThreadAbortException) { }
        catch (Exception ex)
        {
            ErrorLog.Add(ex.Message, ex.StackTrace, null);
            RaiseFaildEvent(CustomError.GetInternalErrorMessage());
            return;
        }
    }

    protected void ddlSecurityQuestion_Select(object sender, EventArgs e)
    {
        phOwnQuestion.Visible = ddlSecurityQuestion.SelectedValue == Resources.TextMessages.WriteMyOwnSecurityQuestion;
    }

    public override void ApplyTabIndexToControl(ref short startTabIndex)
    {
    }


    private void RaiseFaildEvent(string errorMessage)
    {
        if (ChangeFailed != null)
        {
            ChangeFailed(this, new QuestionChangeFailedEventArgs(errorMessage));
        }
    }

    public void SetTabIndex()
    {
        //if(!IsPostBack)
        //{
            ddlSecurityQuestion.Focus();
        //}
    }
}

public class QuestionChangeFailedEventArgs: EventArgs
{
    public string ErrorMessage { get; set; }

    public QuestionChangeFailedEventArgs(){ }

    public QuestionChangeFailedEventArgs(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }
}
