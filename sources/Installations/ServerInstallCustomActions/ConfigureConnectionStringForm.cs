using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using FrontDesk.Deployment;
using System.Threading;
using System.Configuration.Install;
using System.IO;

namespace FrontDesk.Deploy.Server.Actions
{
    public partial class ConfigureConnectionStringForm : Form
    {
        public ConfigureConnectionStringForm()
        {
            InitializeComponent();

            this.Load += new EventHandler(ConfigureConnectionStringForm_Load);
        }

        void ConfigureConnectionStringForm_Load(object sender, EventArgs e)
        {
            PrepareConnectionStringStep();
            UpdateStepsUI();

            Title = Properties.Resources.InstallationDialog_Title3;
        }

        /// <summary>
        /// If true, installation actions are not executed
        /// </summary>
        public bool IsDemoMode { get; set; }

        /// <summary>
        /// Get or set caption label
        /// </summary>
        public string Title
        {
            get { return lblTitle.Text; }
            set { lblTitle.Text = value; }
        }
        /// <summary>
        /// Internal error information
        /// </summary>
        public Exception Error { get; set; }

        public string InstallerPath { get; set; }

        public string SqlScriptName
        {
            get;
            set;
        }

        private string sqlUserName;
        public string SqlUserName
        {
            get
            {
                return this.sqlUserName;
            }
            set
            {
                this.sqlUserName = value;
                this.ctrlConfigureConnection.UserName = this.sqlUserName;
            }
        }

        #region Wizard Steps management

        SqlInstallWizardSteps _currentStep = SqlInstallWizardSteps.SetConnectionString;


        /// <summary>
        /// Update wizard steps visibility
        /// </summary>
        private void UpdateStepsUI()
        {
            ctrlConfigureConnection.Visible = _currentStep == SqlInstallWizardSteps.SetConnectionString;
            ctrlDataCreationProgress.Visible = _currentStep == SqlInstallWizardSteps.CreatingDatabase;
            btnNext.Enabled = _currentStep != SqlInstallWizardSteps.InstallationProgress && _currentStep != SqlInstallWizardSteps.CreatingDatabase;

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.Invoke((ThreadStart)delegate
            {

                if (this.Validate())
                {
                   if (_currentStep == SqlInstallWizardSteps.SetConnectionString)
                    {
                        _currentStep = SqlInstallWizardSteps.CreatingDatabase;
                        Title = Properties.Resources.InstallationDialog_Title4;

                        RunFrontDeskDatabaseInstallation();
                    }

                    UpdateStepsUI();
                }
            });
        }



        /// <summary>
        /// Bind Connection string values
        /// </summary>
        private void PrepareConnectionStringStep()
        {
            ctrlConfigureConnection.EmbeddedExpressMode = false;
            //ctrlConfigureConnection.ServerName = @".\" + "SQLExpress";

            //ctrlConfigureConnection.UserName = "frontdesk_appuser";            
        }

        #endregion


        #region Actions

       
        /// <summary>
        /// Create new Database and all required objects
        /// </summary>
        private void RunFrontDeskDatabaseInstallation()
        {
            Thread thread = new Thread((ThreadStart)delegate
            {
                try
                {
                    if (DatabaseInstallationConfigured != null)
                    {
                        var e = new InstallSqlDatabaseEventArgs();
                        e.ServerName = ctrlConfigureConnection.ServerName;
                        e.Username = ctrlConfigureConnection.UserName;
                        e.Password = ctrlConfigureConnection.Password;
                        e.InstallerPath = this.InstallerPath;
                        e.SqlScriptName = this.SqlScriptName;

                        DatabaseInstallationConfigured(this, e);

                        if (e.Error != null) //succeed
                            this.Error = e.Error;


                    }

                }
                catch (Exception ex)
                {
                    this.Invoke((ThreadStart)delegate //run code in form's UI thread
                    {
                        this.Error = ex;
                        //ctrlDataCreationProgress.Stop();
                        //this.DialogResult = DialogResult.Abort;


                    });
                }
                finally
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            });

            thread.Start();
        }

        #endregion


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (Previous != null)
            {
                Previous(this, new EventArgs());
            }
        }


        public event EventHandler<InstallSqlDatabaseEventArgs> DatabaseInstallationConfigured;
        public event EventHandler Previous;
    }

    public class InstallSqlDatabaseEventArgs : EventArgs
    {
        public string ServerName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string InstallerPath { get; set; }
        public string SqlScriptName { get; set; }
        public Exception Error { get; set; }
    }

}
