using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.Windows.Forms;
using System.IO;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.Web.Configuration;
using FrontDesk.Deployment;
using System.Security.Principal;
using FrontDesk.Deploy.Server.Actions.Properties;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using FrontDesk.Deploy.Server.Actions.WizardSteps;

namespace FrontDesk.Deploy.Server.Actions
{
    [RunInstaller(true)]
    public partial class FrontDeskServerSetupCustonAction : Installer
    {
        protected string TargetDir;

        private string installerPath;
        private bool isDemoMode;
        private string sqlScriptName;
        private bool isLicenseSetup;

        SetupWizardForm setupForm;

        string privateKeyAuthorityFileName = "FrontDesk Root Authority.pvk";
        string certificateAuthorityFileName = "FrontDesk Root Authority.cer";
        string certificateFileName = "FrontDesk Root Authority.pfx";
        string certificateSubject = "FrontDesk";

        private readonly ConfigurationHelper configHelper = new ConfigurationHelper();

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);


            string action = Context.Parameters["action"];
            this.TargetDir = Context.Parameters["targetdir"];
            isLicenseSetup = Convert.ToBoolean(Context.Parameters["islicense"]);


            if (action == "InstallSrv")
            {
                InstallServer(TargetDir);
            }
            else if (action == "InstallSrvEncrypt")
            {
                EncryptServerConfig(TargetDir);
            }
            else if (action == "CreateLocalGroup")
            {
                CreateLocalGroup();
            }

        }

        #region Action - InstallSrv - Install Server

        /// <summary>
        /// Custom step for installing FrontDesk Server
        /// </summary>
        /// <param name="targetDir"></param>
        public void InstallServer(string targetDir)
        {
            // Retrieve configuration settings
            string datasourceMode = Context.Parameters["datasourcemode"]; // 1 - new express, 2 - existing
            // Retrieve configuration settings

            string webSite = Context.Parameters["website"];

            string rpmsServiceString = Context.Parameters["rpmspath"];

            try
            {
                Uri rpmsServiceUri = new Uri(new Uri(rpmsServiceString, UriKind.Absolute), "/EhrInterface.svc");
                //override the EHR Frontdesk Interface WCF service reference to given in parameter
                Configuration configFile = configHelper.OpenWebConfigurationByPhysicalPath(this.TargetDir);
                configHelper.SetWcfServiceServerAddress(configFile, rpmsServiceUri);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("FrontDesk EHR Interface service URI has invalid format.\n\n" +
                    "{0} was not recognized as absolute URI. Error message: {1}", ex.Message));
            }

            // Enable Windows Authentication for created website
            try
            {
                string siteId = configHelper.ParseWebsiteIdFromMSI(webSite);
                configHelper.ModifyIISAuthenticationMode(siteId, 0x4);  // see http://msdn.microsoft.com/en-us/library/ms524513.aspx
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Please enable Windows authentication for the site manually.\n\n" +
                    "Failed to set up automatically: {0}", ex.Message));
            }

            string vdirectory = Context.Parameters["vdirectory"];

            //get config file params
            //string webServerAddress = Context.Parameters["webserver"];
            string sqlServerName = Context.Parameters["sqlserver"];
            string sqlLoginPass = Context.Parameters["sqluserpass"];

            installerPath = Path.GetDirectoryName(Context.Parameters["InstallerPath"]);

            // bool encryptConnectionString = Context.Parameters["encryptconnection"] == "1";

            sqlScriptName = Context.Parameters["sqlscriptname"];

            isDemoMode = false;

            ShowConfigurationWizard();

            configHelper.RegisterWCFScripts(); //register WCF
            //start ASP.NET State Server service and set it start automatically
            var aspNetStateServiceName = "aspnet_state";
            configHelper.StartWinService(aspNetStateServiceName);

            configHelper.ChangeWinServiceStartMode(aspNetStateServiceName, ConfigurationHelper.Win32ServiceStartMode.Automatic);

        }

        /// <summary>
        /// Show install db form
        /// </summary>
        public void ShowConfigurationWizard()
        {
            setupForm = new SetupWizardForm();

            setupForm.Next += new EventHandler<NextStepEventArgs>(setupForm_Next);
            setupForm.Previous += new EventHandler(setupForm_Previous);
            setupForm.Completed += new EventHandler(setupForm_Completed);
            setupForm.BeforeStepShown += new EventHandler<StepShownEventArgs>(setupForm_BeforeStepShown);

            //setupForm.AddStep(new WizardSteps.AppLogon());

            ConfigureConnectionStringControl configureConnStrControl = new WizardSteps.ConfigureConnectionStringControl();
            configureConnStrControl.UserName = isLicenseSetup ? "fdlicense_appuser" : "frontdesk_appuser";
            setupForm.AddStep(configureConnStrControl);

            setupForm.AddStep(new ConfigureSSL());


            if (!isLicenseSetup)
            {
                //only for server installation
                setupForm.AddStep(new WizardSteps.EnableKiosksAuthentificationControl());
            }


            if (setupForm.ShowDialog() == DialogResult.Cancel)
            {
                setupForm.Close();
                throw new InstallException(Resources.Installation_Aborted);
            }
        }

        void setupForm_BeforeStepShown(object sender, StepShownEventArgs e)
        {
        }

        void setupForm_Completed(object sender, EventArgs e)
        {
            setupForm.DialogResult = DialogResult.OK;
            setupForm.Hide();
            bool isMessage = false;

            if (!isLicenseSetup)
            {
                //only for server installation
                EnableKiosksAuthentificationControl kiosksAuthentificationCtrl =
                    (setupForm.Steps[2] as EnableKiosksAuthentificationControl);

                try
                {
                    if (kiosksAuthentificationCtrl.IsAuthentificationEnabled)
                    {

                        if (!isDemoMode)
                        {
                            CreateCertificate(kiosksAuthentificationCtrl.CertificateDestinationPath);
                        }
                    }
                }
                catch (InstallException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw new InstallException(
                        String.Format(Resources.Installation_Failed, "Cannot install server certificate."));
                }


                isMessage = (setupForm.Steps[2] as WizardSteps.EnableKiosksAuthentificationControl).IsAuthentificationEnabled;
            }

            //make config changes
            //change config
            bool isHttps = (setupForm.Steps[1] as ConfigureSSL).UseHTTPS;

            if (isHttps || isMessage)
            {
                ServerProductCustomInstaller.UpdateWCFServiceConfigurationWithMessageCridentials(this.TargetDir,
                    certificateSubject, certificateSubject, isHttps, isMessage);
            }



            try
            {
                if (!isDemoMode)
                {
                    InstallDb();
                }
            }
            catch (InstallException)
            {
                throw;
            }
            catch (Exception)
            {
                throw new InstallException(
                    String.Format(Resources.Installation_Failed, "Cannot install FrontDesk database."));
            }
        }

        void setupForm_Previous(object sender, EventArgs e)
        {

        }

        void setupForm_Next(object sender, NextStepEventArgs e)
        {

        }

        public void EncryptServerConfig(string targetDir)
        {
            bool encryptConnectionString = Context.Parameters["encryptconnection"] == "1";

            if (encryptConnectionString)
            {
                configHelper.EncryptConnectionString(targetDir);
                configHelper.EncryptMachineKeyString(targetDir);

                //grant access
                configHelper.GrantAccessToRsaKeyContainer(); //to network service


            }
        }

        /// <summary>
        /// Create local windows group for LMT website authorization purposes.
        /// </summary>
        public void CreateLocalGroup()
        {
            // See group name in LMT web_setup.config:
            // <authorization>
            //  <allow roles="FrontDesk Licensing Application Users"/>
            string groupName = "FrontDesk Licensing Application Users";
            configHelper.CreateLocalGroup(groupName,
                "Members of this group are allowed to use FrontDesk License Management Tool website.");

            string currentAccountMessage = "Your account was just added to this group. Please log off from Windows and then log on again after installation " +
                "for this change to take effect.";

            // Add current user to LMT users group
            try
            {
                //string currentUser = WindowsIdentity.GetCurrent().Name; // DOMAIN\NAME
                //string currentUser = Environment.UserName; // DOMAIN\NAME
                string currentUser = Context.Parameters["username"];
                configHelper.AddWindowsUserToLocalGroup(currentUser, groupName);
            }
            catch (Exception ex)
            {
                currentAccountMessage = string.Format("We tried but failed to add your account to this group: {0}", ex.Message);
            }

            MessageBox.Show(string.Format("After installation please add Windows users or groups, that will be allowed " +
                "to use FrontDesk License Management Tool website, to the following local Windows group on this server:\n\n" +
                "'{0}'\n\n{1}",
                groupName, currentAccountMessage),
                "Notice on permissions", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public bool CreateCertificate(string destinationPath)
        {
            //show notification if destination folder already contains FrontDesk certificates
            if (File.Exists(Path.Combine(destinationPath, privateKeyAuthorityFileName))
                || File.Exists(Path.Combine(destinationPath, certificateAuthorityFileName))
                || File.Exists(Path.Combine(destinationPath, certificateFileName)))
            {
                //Destination folder already contains certificate file from previous installations.
                if (MessageBox.Show(String.Format(Resources.CertificateExistsMessage, destinationPath),
                    Resources.InstallationMessageCaption,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    File.Delete(Path.Combine(destinationPath, privateKeyAuthorityFileName));
                    File.Delete(Path.Combine(destinationPath, certificateAuthorityFileName));
                    File.Delete(Path.Combine(destinationPath, certificateFileName));
                }
                else
                {
                    return false;
                }
            }


            bool createNewCertificates = true;


            X509Store storeAuthority = new X509Store(StoreName.Root, StoreLocation.LocalMachine);
            X509Store storePersonal = new X509Store(StoreName.My, StoreLocation.LocalMachine);


            //Check for installed FrontDesk server certificate
            storePersonal.Open(OpenFlags.ReadWrite);
            X509Certificate2Collection certificates = storePersonal.Certificates.Find(X509FindType.FindBySubjectName,
                    certificateSubject, true);

            if (certificates != null && certificates.Count > 0)
            {
                //Show notification if the system has installed FrontDesk certificates
                if (MessageBox.Show(Resources.CertificateInstalledMessage,
                    Resources.InstallationMessageCaption,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    storePersonal.RemoveRange(certificates);

                    //uninstall certificate authority
                    storeAuthority.Open(OpenFlags.ReadWrite);
                    X509Certificate2Collection certificatesAuthority = storeAuthority.Certificates.Find(
                        X509FindType.FindBySubjectName, certificateSubject, true);
                    if (certificatesAuthority != null && certificatesAuthority.Count > 0)
                    {
                        storeAuthority.RemoveRange(certificatesAuthority);
                    }
                    storeAuthority.Close();
                }
                else
                {
                    createNewCertificates = false;
                }
            }

            if (createNewCertificates)
            {
                //create and install certificate from cmd
                string fullPath = System.Reflection.Assembly.GetAssembly(typeof(FrontDeskServerSetupCustonAction)).Location;
                string directory = Path.GetDirectoryName(fullPath);

                //command
                string createCmd = Path.Combine(directory, "create_cert.bat");



                string appPool = Context.Parameters.ContainsKey("targetapppool") ?
                    Context.Parameters["targetapppool"] : String.Empty;

                //command arguments
                string arguments = String.Format(" \"{0}\" \"{1}\" \"{2}\" \"{3}\"",
                    Path.Combine(destinationPath, privateKeyAuthorityFileName),
                    Path.Combine(destinationPath, certificateAuthorityFileName),
                    Path.Combine(destinationPath, certificateFileName),
                    appPool);

                using (Process p = new Process())
                {
                    p.StartInfo = new ProcessStartInfo(createCmd, arguments)
                    {
                        WorkingDirectory = directory,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    p.Start();
                    string output = p.StandardOutput.ReadToEnd();

                    //using (StreamWriter writer = File.CreateText(@"c:\output.txt"))
                    //{
                    //    writer.Write(output);
                    //}

                    p.WaitForExit();
                }
                Thread.Sleep(2000);
                //restart app pool when all server certificates already installed
                //at the end of certificates installation process
                RestartAppPoolOrIIS(appPool);
            }

            return true;
        }

        /// <summary>
        /// If app pool is supported - restarts AppPool else restart iis (for v 5.0)
        /// </summary>
        private void RestartAppPoolOrIIS(string appPoolName)
        {

            try
            {
                IIsHelper.RestartIISOrAppPool(appPoolName);
            }
            catch
            {
                Version iisVersion = IIsHelper.GetIISVersion();
                string errMes = iisVersion.Major == 5 ?
                    "Unable to restart Internet Information Service. Please restart IIS manually after the installation is complete." :
                    String.Format("Unable to restart \"{0}\" Application Pool. Please restart it manually after the installation is complete.",
                    appPoolName);

                MessageBox.Show(errMes);
            }

        }

        private void InstallDb()
        {
            ConfigureConnectionStringControl configureConnStrControl =
                setupForm.Steps[0] as ConfigureConnectionStringControl;


            try
            {
                string fullPath = System.Reflection.Assembly.GetAssembly(typeof(FrontDeskServerSetupCustonAction)).Location;

                //get the folder that's in 
                string directory = Path.GetDirectoryName(fullPath);
                string sqlFileName = Path.Combine(directory, sqlScriptName);

                if (SQLExpressInstaller.IsDatabaseExists(configureConnStrControl.ServerName,
                    configureConnStrControl.UserName, configureConnStrControl.Password, "FrontDesk"))
                {
                    if (MessageBox.Show(String.Format("FrontDesk database already exists. Would you like to replace it?"),
                        Resources.InstallationMessageCaption, MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        throw new InstallException(Resources.Installation_Aborted);
                    }
                }

                SQLExpressInstaller.CreateDatabase(configureConnStrControl.ServerName,
                    configureConnStrControl.UserName, configureConnStrControl.Password, sqlFileName);

                if (isLicenseSetup)
                {
                    ServerProductCustomInstaller.UpdateLMTWebConfig(this.TargetDir,
                        configureConnStrControl.ServerName, configureConnStrControl.Password);
                }
                else
                {
                    ServerProductCustomInstaller.UpdateServerWebConfig(this.TargetDir,
                        configureConnStrControl.ServerName, configureConnStrControl.Password);
                }


            }
            catch (InstallException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InstallException("Failed to create FrontDesk database.", ex);
            }

        }

        #endregion
    }
}
