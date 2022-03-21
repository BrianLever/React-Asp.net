using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FrontDesk.Deploy.Server.Actions.Properties;
using System.Threading;
using System.Text.RegularExpressions;
using FrontDesk.Deployment;

namespace FrontDesk.Deploy.Server.Actions.WizardSteps
{
    public partial class EnableKiosksAuthentificationControl : UserControl, IWizardStep
    {
        public bool IsAuthentificationEnabled 
        {
            get
            { 
                return rbEnableAuthentification.Checked;
            }
        }

        public string CertificateDestinationPath
        {
            get
            {
                return IsAuthentificationEnabled ? txbCertificatePath.Text : String.Empty;
            }
        }

        public bool Validate(out string errorMessage)
        {
            bool isValid = true;
            errorMessage = "";

            if (rbEnableAuthentification.Checked)
            {
                isValid = !String.IsNullOrEmpty(txbCertificatePath.Text) &&
                    Regex.IsMatch(txbCertificatePath.Text, Resources.FolderPathRegexp);
                errorMessage = Resources.InvalidCertificateFolder;

            }

            return isValid;

        }

        private static object _locker = new object();

        public EnableKiosksAuthentificationControl()
        {
            InitializeComponent();
            this.Title = Resources.ConfigureKiosksAuthenticationTitle;
            this.Hide();
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

        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogResult.Cancel;

            Thread thread = new Thread(delegate()
            {
                lock (_locker)
                {
                    dialogResult = folderDlg.ShowDialog();
                }
            });
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();


            if (dialogResult == DialogResult.OK)
            {
                txbCertificatePath.Text = folderDlg.SelectedPath;
            }
        }

        private void rbEnableAuthentification_CheckedChanged(object sender, EventArgs e)
        {
            pnlCertificatePath.Enabled = rbEnableAuthentification.Checked;
        }
    }
}
