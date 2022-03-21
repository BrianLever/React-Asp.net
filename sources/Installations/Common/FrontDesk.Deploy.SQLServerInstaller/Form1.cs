using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using FrontDesk.Deploy;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;

namespace FrontDesk.Deployment.SQLServerInstaller
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Properties
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

        #endregion

        #region Wizard Steps management

        SqlInstallWizardSteps _currentStep = SqlInstallWizardSteps.ExpressConfiguration;


        /// <summary>
        /// Update wizard steps visibility
        /// </summary>
        private void UpdateStepsUI()
        {
            ctrlExpressConfiguration.Visible = _currentStep == SqlInstallWizardSteps.ExpressConfiguration;
            ctrlExpressProgress.Visible = _currentStep == SqlInstallWizardSteps.InstallationProgress;
            ctrlInstallationCompleted.Visible = _currentStep == SqlInstallWizardSteps.InstallationCompleted;
            btnNext.Enabled = _currentStep != SqlInstallWizardSteps.InstallationProgress && _currentStep != SqlInstallWizardSteps.InstallationCompleted;

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.BeginInvoke((ThreadStart)delegate
           {
               if (ValidateForm())
               {
                   if (_currentStep == SqlInstallWizardSteps.ExpressConfiguration)
                   {
                       _currentStep = SqlInstallWizardSteps.InstallationProgress;
                       UpdateStepsUI();
                       RunSQlInstallation(ctrlExpressConfiguration.Password, ctrlExpressConfiguration.ServerName);
                       Title = Properties.Resources.InstallationDialog_Title2;

                   }
                   else if (_currentStep == SqlInstallWizardSteps.InstallationProgress)
                   {
                       _currentStep = SqlInstallWizardSteps.InstallationCompleted;
                       Title = Properties.Resources.InstallationDialog_Title6;
                       UpdateStepsUI();
                       TestSQLServerIsInstalled(@".\" + ctrlExpressConfiguration.ServerName);

                   }
               }
           });


        }

        /// <summary>
        /// Run SQL Express installation
        /// </summary>
        /// <param name="saPass"></param>
        /// <param name="instanceName"></param>
        private void RunSQlInstallation(string saPass, string instanceName)
        {

            Thread thread = new Thread((ThreadStart)delegate
            {
                try
                {
                    var filePath = SQLExpressInstaller.GetPathToSqlInstallationx86(InstallerPath);

                    if (!IsDemoMode) SQLExpressInstaller.RunSqlInstallationx86(saPass, instanceName, filePath);
                    else Thread.Sleep(20000);
                    //on the end
                    btnNext_Click(btnNext, new EventArgs()); //next
                }
                catch (InstallException ex)
                {
                    ShowError(ex.Message);

                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    ShowError();
                }
            });
            thread.Start();




        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InstallerPath = System.Environment.CurrentDirectory;
            Title = Properties.Resources.InstallationDialog_Title2;
            UpdateStepsUI();
            ctrlExpressConfiguration.Focus();

            if (!InstallMsi45IfRequired())
            {
                InstallPowershellIfRequired();
            }
        }
        #endregion

        protected void ShowError(string message)
        {
            _currentStep = SqlInstallWizardSteps.InstallationCompleted;
            this.BeginInvoke((ThreadStart)delegate
            {

                ctrlInstallationCompleted.ErrorText = string.Format(Properties.Resources.Installation_SQLInstallationFailedWithError, message);
                UpdateStepsUI();
            });

        }

        protected void ShowError()
        {
            _currentStep = SqlInstallWizardSteps.InstallationCompleted;
            this.BeginInvoke((ThreadStart)delegate
            {

                ctrlInstallationCompleted.ErrorText = Properties.Resources.Installation_SQLInstallationFailed;
                UpdateStepsUI();
            });

        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void TestSQLServerIsInstalled(string serverName)
        {
            string connectionError;
            ctrlInstallationCompleted.SuccessText = Properties.Resources.Installation_TestingServerConnection;
            if (SQLExpressInstaller.TestConnection(serverName, out connectionError))
            {
                ctrlInstallationCompleted.SuccessText = Properties.Resources.Installation_Successfull;
            }
            else
            {
                ctrlInstallationCompleted.SuccessText = string.Format(Properties.Resources.Installation_Failed, connectionError);
            }
        }
        protected bool ValidateForm()
        {
            bool allValid = true;
            if (_currentStep == SqlInstallWizardSteps.ExpressConfiguration)
            {
                allValid = ctrlExpressConfiguration.DoValidation();
            }


            return allValid;
        }


        #region Install Msi4.5

        protected bool InstallMsi45IfRequired()
        {
            var msiVersion = SQLExpressInstaller.GetInstalledMsiVersion();
            if (msiVersion < "4.5")
            {
                LockForm();

                //message that we need to install msi 4.5
                MessageBox.Show(Properties.Resources.Installation_NeedToInstallMsi45, "Install Windows Installer 4.5", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Thread thread = new Thread((ThreadStart)delegate
                {
                    if (!StartInstallMsi45())
                    {
                        this.BeginInvoke((ThreadStart)delegate
                        {

                            Application.Exit();//close window
                        });
                    }
                    else
                    {
                       this.BeginInvoke((ThreadStart)delegate
                       {
                           //continue 
                           UnlockForm();
                       });
                    }
                });
                thread.Start();

                return true;
            }
            else
                return false;
        }
        #endregion

        #region Install Powershell

        protected bool InstallPowershellIfRequired()
        {
            var powershellInstalled = SQLExpressInstaller.GetIfPowershellIsInstalled();
            if (!powershellInstalled)
            {
                LockForm();

                //message that we need to install powershell
                MessageBox.Show(Properties.Resources.Installation_NeedToInstallPowershell, "Install Windows Installer 4.5", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Thread thread = new Thread((ThreadStart)delegate
                {
                    if (!StartInstallPowershell())
                    {
                        this.BeginInvoke((ThreadStart)delegate
                        {

                            Application.Exit();//close window
                        });
                    }
                    else
                    {
                        this.BeginInvoke((ThreadStart)delegate
                        {
                            //continue 
                            UnlockForm();
                        });
                    }
                });
                thread.Start();

                return true;
            }
            else
                return false;
        }
        #endregion

        #region Execute Installation packages

        private bool StartInstallMsi45()
        {
            try
            {
                var directory = Path.Combine(Environment.CurrentDirectory, "WindowsInstaller4_5");
                var resultCode = SQLExpressInstaller.RunMsi45Installation(directory);

                if (resultCode != 0)
                {
                    if (MessageBox.Show(Properties.Resources.Installation_MsiMightNotInstallProperly, "Continue installation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        return false;
                    }
                }

            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Missing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }

        private bool StartInstallPowershell()
        {
            try
            {
                var directory = Path.Combine(Environment.CurrentDirectory, "Powershell");
                var resultCode = SQLExpressInstaller.RunPowershellInstallation(directory);

                if (resultCode != 0)
                {
                    if (MessageBox.Show(Properties.Resources.Installation_PowershellMightNotInstallProperly, "Continue installation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        return false;
                    }
                }

            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, "Missing file", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return true;
        }

        #endregion

        private void LockForm()
        {
            btnNext.Enabled = false;
        }

        private void UnlockForm()
        {
            btnNext.Enabled = true;
        }

 
    }
}
