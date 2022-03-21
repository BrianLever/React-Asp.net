using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace FrontDesk.Deploy.Server.Actions
{
    public partial class ExpressConfiguration : UserControl
    {
        public ExpressConfiguration()
        {
            InitializeComponent();

            txtSrvName.Text = @"SQLExpress";
        }

        private void txtConfirmPass_Validating(object sender, CancelEventArgs e)
        {
            ValidatePasswordConfirmTextBox();
          
        }

        private void txtPass_Validating(object sender, CancelEventArgs e)
        {

            ValidatePasswordTextBox();
            
            

        }

        const string PasswordMask = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$";
        const string PasswordMessage = @"Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters";

        public string Password { get { return txtPass.Text; } }

        public string ServerName { get { return txtSrvName.Text; } }

        private void ExpressConfiguration_Validating(object sender, CancelEventArgs e)
        {
            txtPass_Validating(sender, e);
            txtConfirmPass_Validating(sender, e);
        }

        private void ExpressConfiguration_Enter(object sender, EventArgs e)
        {
            txtPass.Focus();
        }

        public bool DoValidation()
        {
            return (ValidatePasswordTextBox() && ValidatePasswordConfirmTextBox());
        }


        protected bool ValidatePasswordTextBox()
        {
            bool isValid = true;
            if (String.IsNullOrEmpty(txtPass.Text))
            {
                errorProvider1.SetError(txtPass, "Password is required.");
                isValid = false;
            }
            else
            {
                Regex regex = new Regex(PasswordMask);
                if (!regex.IsMatch(txtPass.Text))
                {
                    errorProvider1.SetError(txtPass, PasswordMessage);
                    isValid = false;
                }
                else
                {
                    errorProvider1.SetError(txtPass, string.Empty);
                }
            }
            return isValid;
        }

        protected bool ValidatePasswordConfirmTextBox()
        {
            bool isValid = true;
            if (String.Compare(txtPass.Text, txtConfirmPass.Text) != 0)
            {
                errorProvider1.SetError(txtConfirmPass, "Confirm does not match the password");
                isValid = false;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPass, string.Empty);

            }
            return isValid;
        }



        
        
    }
}
