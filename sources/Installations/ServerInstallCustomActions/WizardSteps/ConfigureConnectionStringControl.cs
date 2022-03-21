using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontDesk.Deployment;
using System.Text.RegularExpressions;
using FrontDesk.Deploy.Server.Actions.Properties;

namespace FrontDesk.Deploy.Server.Actions.WizardSteps
{
    public partial class ConfigureConnectionStringControl : UserControl, IWizardStep
    {
        const string PasswordMask = @"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$";
        const string PasswordMessage = @"Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters";


        public ConfigureConnectionStringControl()
        {
            InitializeComponent();

            Visible = false;
            txtUsername.Enabled = false;
            this.Load += new EventHandler(ConfigureConnection_Load);
            Title = Resources.SelectDatabaseServer;
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


        #region IWizardStep Members

        public string Title
        {
            get;
            set;
        }

        public void ShowStep()
        {
            Visible = true;
        }

        public void HideStep()
        {
            Visible = false;
        }

        public bool Validate(out string errorMessage)
        {
            bool isValid = true;
            errorMessage = "";

            if (String.IsNullOrEmpty(txtPassword.Text))
            {
                errorMessage = "Password is required.";
                isValid = false;
            }
            else
            {
                Regex regex = new Regex(PasswordMask);
                if (!regex.IsMatch(txtPassword.Text))
                {
                    errorMessage = PasswordMessage;
                }
            }

            return isValid;
        }

        #endregion

    }
}
