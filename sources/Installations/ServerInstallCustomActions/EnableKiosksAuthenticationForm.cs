using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using FrontDesk.Deploy.Server.Actions.Properties;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using System.Threading;
using System.Configuration.Install;

namespace FrontDesk.Deploy.Server.Actions
{
    public partial class EnableKiosksAuthenticationForm : Form
    {
        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }

        public event EventHandler<KiosksAuthenticationConfiguredEventArgs> AuthentificationConfigured;

        public EnableKiosksAuthenticationForm()
        {
            InitializeComponent();
            this.Load += new EventHandler(EnableKiosksAuthenticationForm_Load);
        }

        void EnableKiosksAuthenticationForm_Load(object sender, EventArgs e)
        {
            Title = Properties.Resources.ConfigureKiosksAuthenticationTitle;
        }

        private static object locker = new object();

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = DialogResult.Cancel;

            Thread thread = new Thread(delegate()
            {
                lock(locker)
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

        private void btnNext_Click(object sender, EventArgs e)
        {
            KiosksAuthenticationConfiguredEventArgs args = 
                new KiosksAuthenticationConfiguredEventArgs();

            if (rbEnableAuthentification.Checked)
            {
                if (!Validate())
                {
                    MessageBox.Show(Resources.InvalidCertificateFolder);
                    return;
                }
                else
                {
                    args.AuthentificationEnabled = true;
                    args.DestinationPath = txbCertificatePath.Text;
                }
            }


            if (AuthentificationConfigured != null)
            {
                this.Close();
                AuthentificationConfigured(this, args);
            }
        }

        private void rbEnableAuthentification_CheckedChanged(object sender, EventArgs e)
        {
            pnlCertificatePath.Enabled = rbEnableAuthentification.Checked;
        }

        private bool Validate()
        { 
            return !String.IsNullOrEmpty(txbCertificatePath.Text) &&
                Regex.IsMatch(txbCertificatePath.Text, Resources.FolderPathRegexp);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
 
    }




    public class KiosksAuthenticationConfiguredEventArgs: EventArgs
    {
        public bool AuthentificationEnabled { get; set; }
             
        public string DestinationPath { get; set; }

        public KiosksAuthenticationConfiguredEventArgs() { }

        public KiosksAuthenticationConfiguredEventArgs(string destinationPath)
        {
            this.DestinationPath = destinationPath;
        }
    }
}
