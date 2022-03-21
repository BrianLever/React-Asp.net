using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using FrontDesk.Deployment;

namespace FrontDesk.Deploy.Server.Actions
{
    public partial class ConfigureConnection : UserControl
    {
        public ConfigureConnection()
        {
            InitializeComponent();

            txtUsername.Enabled = false;
            this.Load += new EventHandler(ConfigureConnection_Load);
        }

        void ConfigureConnection_Load(object sender, EventArgs e)
        {
            //get instances
            cmbServer.DataSource = SQLExpressInstaller.BrowseSQlServers(new ProductVersion("10.0.0.0"));
        }

        #region Properties

        public string UserName
        {
            get { return txtUsername.Text; }
            set { txtUsername.Text = value; }
        }

        public string Password
        {
            get { return txtPassword.Text; }
            set { txtPassword.Text = value; }
        }

        public string ServerName
        {
            get { return cmbServer.Text; }
            set { cmbServer.Text = value; }
        }

        private bool _embeddedExpressMode = false;
        /// <summary>
        /// True when we need just receive password
        /// </summary>
        public bool EmbeddedExpressMode
        {
            get { return _embeddedExpressMode; }
            set
            {
                _embeddedExpressMode = value;
                cmbServer.Enabled = !_embeddedExpressMode;
               
            }
        }

        #endregion


        private void btnTest_Click(object sender, EventArgs e)
        {
            string connectionError;
            if (SQLExpressInstaller.TestConnection(cmbServer.Text, out connectionError))
            {
                MessageBox.Show("Test completed successfully");
            }
            else
            {
                MessageBox.Show("Connection failed with error: " + connectionError);
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                errorProvider1.SetError(txtPassword, "Password is required.");
                e.Cancel = true;
            }
            else
            {
                Regex regex = new Regex(PasswordMask);
                if (!regex.IsMatch(txtPassword.Text))
                {
                    errorProvider1.SetError(txtPassword, PasswordMessage);

                }
                else
                {
                    errorProvider1.SetError(txtPassword, string.Empty);
                }
            }
        }

        const string PasswordMask = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$";
        const string PasswordMessage = @"Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters";


    }
}
