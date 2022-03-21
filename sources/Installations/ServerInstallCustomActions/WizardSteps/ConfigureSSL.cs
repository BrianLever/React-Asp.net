using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontDesk.Deployment;
using FrontDesk.Deploy.Server.Actions.Properties;

namespace FrontDesk.Deploy.Server.Actions.WizardSteps
{
    public partial class ConfigureSSL : UserControl, IWizardStep
    {

        public bool UseHTTPS
        {
            get
            {
                return rbUseSSL.Checked;
            }
        }


        public ConfigureSSL()
        {
            InitializeComponent();
            Title = Resources.SetupSSL;
        }

        #region IWizardStep Members

        public string Title
        {
            get;
            set;
        }

        public void ShowStep()
        {
            this.Visible = true;
        }

        public void HideStep()
        {
            this.Visible = false;
        }

        public bool Validate(out string errorMessage)
        {
            errorMessage = "";
            return true;
        }

        #endregion
    }
}
