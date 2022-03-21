using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FrontDesk.Server.Web.Controls;
using FrontDesk.Server;
using System.Web.Security;
using FrontDesk.Server.Membership;
using FrontDesk.Common.Messages;
using FrontDesk.Common.Debugging;
using FrontDesk.Server.Membership.Providers;
using FrontDesk.Server.Logging;

namespace FrontDesk.Control
{
    /// <summary>
    /// Change password control
    /// </summary>
    public partial class ModifyPasswordCtrl : BaseUserControl
    {
        public event EventHandler PasswordChanged;
        public event EventHandler<PasswordChangeFailedEventArgs> ChangeFailed;

        /// <summary>
        /// Get or Set a value indicating is user required to enter his current password
        /// </summary>
        public bool IsOldPasswordRequired { get; set; }
        
        public string ValidationGroup
        {
            get
            {
                return vldPassword.ValidationGroup;
            }
            set
            {
                vldPassword.ValidationGroup = value;
                vldNewPassword.ValidationGroup = value;
                revNewPassword.ValidationGroup = value;
                rfvConfirmNewPassword.ValidationGroup = value;
                vldCmpPassword.ValidationGroup = value;
                cvldNewPassword.ValidationGroup = value;
            }
        }

        public string UserName { get; set; }

        public string NewPassword 
        {
            get
            {
                return txtNewPassword.Text;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            phOldPasswordRequired.Visible = IsOldPasswordRequired;
        }

        /// <summary>
        /// Validate new password value
        /// Password can not contain user name
        /// </summary>
        public void ValidationNewPassword(object source, ServerValidateEventArgs args)
        {
            string userName = "";
            if (IsOldPasswordRequired)
            {
                FDUser user = FDUser.GetCurrentUser();
                userName = user.UserName;
            }
            else
            {
                userName = UserName;
            }
            args.IsValid = !txtNewPassword.Text.Contains(userName) &&
                           !txtNewPassword.Text.Contains(userName.ToUpper()) &&
                           !txtNewPassword.Text.Contains(userName.ToLower());
        }

        /// <summary>
        /// Perform password changing action
        /// </summary>
        public void ModifyPassword()
        {
            Page.Validate(ValidationGroup);

            if(Page.IsValid)
            {
                if(!IsOldPasswordRequired)
                {
                    if(String.IsNullOrEmpty(UserName))
                    {
                        throw new ApplicationException("User name is not provided.");
                    }

                    ChangeWithoutOldPassword();
                }
                else
                {
                    ChangeWithOldPassword();
                }
            }
        }

        private void ChangeWithOldPassword()
        {
            MembershipUser user = Membership.GetUser();
            if (Membership.ValidateUser(user.UserName, txtPassword.Text))
            {
                try
                {
                    user.ChangePassword(txtPassword.Text, txtNewPassword.Text);
                }
                catch (DuplicatePasswordException)
                {
                    RaiseFaildEvent(Resources.TextMessages.Membership_DuplicatePassword);
                    return;
                }
                catch (Exception ex)
                {
                    ErrorLog.Add(ex.Message, ex.StackTrace, null);
                    RaiseFaildEvent(CustomError.GetUpdateMessage("password"));
                    return;
                }

                if(PasswordChanged!= null)
                {
                    PasswordChanged(this, new EventArgs());
                }
            }
            else
            {
                RaiseFaildEvent(Resources.TextMessages.Membership_InvalidCurrentPwd);
            }
        }

        private void ChangeWithoutOldPassword()
        {
            string encodedPwd = 
                ((SingleApplicationMembershipProvider)Membership.Provider).EncodePassword(txtNewPassword.Text);
            try
            {
                FDUser.UpdatePassword(UserName, encodedPwd);
            }
            catch (DuplicatePasswordException)
            {
                RaiseFaildEvent(Resources.TextMessages.Membership_DuplicatePassword);
                return;
            }
            catch (Exception ex)
            {
                ErrorLog.Add(ex.Message, ex.StackTrace, null);
                RaiseFaildEvent(CustomError.GetUpdateMessage("password"));
                return;
            }
            if (PasswordChanged != null)
            {
                PasswordChanged(this, new EventArgs());
            }
        }

        public override void ApplyTabIndexToControl(ref short startTabIndex)
        {
        }

        private void RaiseFaildEvent(string errorMessage)
        {
            if (ChangeFailed != null)
            {
                ChangeFailed(this, new PasswordChangeFailedEventArgs(errorMessage));
            }
        }

        public void SetTabIndex()
        {
            //if (!IsPostBack)
            //{
                if (IsOldPasswordRequired)
                {
                    txtPassword.Focus();
                }
                else
                {
                    txtNewPassword.Focus();
                }
            //}
        }
    }

    public class PasswordChangeFailedEventArgs: EventArgs
    {
        public string ErrorMessage { get; set; }

        public PasswordChangeFailedEventArgs(){ }

        public PasswordChangeFailedEventArgs(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}
