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
            if(String.Compare(txtPass.Text, txtConfirmPass.Text) != 0)
            {
                errorProvider1.SetError(txtConfirmPass, "Confirm does not match the password");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPass, string.Empty);
                 
            }
          
        }

        private void txtPass_Validating(object sender, CancelEventArgs e)
        {
            if(String.IsNullOrEmpty(txtPass.Text))
            {
                errorProvider1.SetError(txtPass, "Password is required.");
                e.Cancel = true;
            }
            else{
                Regex regex = new Regex(PasswordMask);
                if(!regex.IsMatch(txtPass.Text))
                {
                    errorProvider1.SetError(txtPass, PasswordMessage);
                   
                }
                else{
                    errorProvider1.SetError(txtPass, string.Empty);
                }
            }
            
            
            

        }

        const string PasswordMask = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$";
        const string PasswordMessage = @"Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters";

        public string Password { get { return txtPass.Text; } }

        public string ServerName { get { return txtSrvName.Text; } }

    }
}
