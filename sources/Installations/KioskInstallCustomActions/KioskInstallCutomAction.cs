namespace KioskInstallCustomActions
{
    using FrontDesk.Deployment;
    using ScreenDox.Kiosk.Configuration;
    using System;
    using System.Collections.Specialized;
    using System.ComponentModel;
    using System.Configuration;
    using System.Configuration.Install;
    using System.IO;
    using System.ServiceModel;
    using System.ServiceModel.Configuration;

    [RunInstaller(true)]
    public partial class KioskInstallCutomAction : System.Configuration.Install.Installer
    {
       
        private const string KioskExeName = "ScreenDoxKiosk.exe";

        private readonly ConfigurationHelper configHelper = new ConfigurationHelper();

        public string KioskKey { get; set; }
        public string KioskSecret { get; set; }
        public string ApplicationDirectory { get; set; }
        public string ServerAddress { get; set; }

        public string KioskInstallationApiAddress { get; set; }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);

            InitParameters(Context.Parameters);
            try
            {
                WriteLaucherConfigurationSettings();

                CreateApplicationStateFile();
            }
            catch (Exception ex)
            {
                throw new InstallException(ex.ToString());
            }

        }

        public void InitParameters(StringDictionary parameters)
        {
            //read params
            this.KioskKey = parameters["kioskkey"];
            this.KioskSecret = parameters["kiosksecret"];
            this.ApplicationDirectory = parameters["appfolder"];
            this.ServerAddress = parameters["server"];
            this.KioskInstallationApiAddress = parameters["installapi"];


            Context.LogMessage("Parameters:");
            Context.LogMessage($"Kiosk Key: {KioskKey}");
            Context.LogMessage($"Kiosk Secret: {KioskSecret}");
            Context.LogMessage($"Application Directory: {ApplicationDirectory}");
            Context.LogMessage($"ServerAddress: {ServerAddress}");
            Context.LogMessage($"Kiosk Installation Api Address: {KioskInstallationApiAddress}");
        }


        public void SetKioskConfigurationFileParamteres(bool useHttps)
        {
            if (useHttps && !ServerAddress.ToLower().Contains("https"))
            {
                ServerAddress = ServerAddress.Replace("http", "https");
            }


            //find configuration file
            var configPath = Path.Combine(ApplicationDirectory, "app", KioskExeName);
            var configPathWithExtention = configPath + ".config";

            Uri serviceUri = GetServicePath(ServerAddress);

            Configuration configFile = ConfigurationManager.OpenExeConfiguration(configPath);
            configHelper.SetWcfServiceServerAddress(configFile, serviceUri);


            if (useHttps)
            {
                ConfigureWcfServiceWithMessageSecuritySettings(configFile, useHttps);
            }

            //save kiosk key and secret

            configHelper.SetAppSettingValue(configFile, "KioskKey", KioskKey);
            configHelper.SetAppSettingValue(configFile, "KioskSecret", KioskSecret);

            configFile.Save(); //save changes
        }

        protected void WriteLaucherConfigurationSettings()
        {
            var yamlConfigFullPath = Path.Combine(ApplicationDirectory, @"Launcher\data\configuration.yaml");

            var repository = new YamlAppConfigurationRepository();

            var configuration = new AppConfiguration
            {
                Key = this.KioskKey,
                Secret = this.KioskSecret,
                KioskApplicationDirectory = Path.Combine(ApplicationDirectory, "app").Replace(@"\\", @"\"),
                KioskExeName = KioskExeName,
                ScreenDoxInstallationServiceBaseUrl = KioskInstallationApiAddress,
                ScreendoxServerBaseUrl = this.ServerAddress
            };

            try
            {
                repository.Write(configuration, yamlConfigFullPath);
            }
            catch (Exception ex)
            {
                throw new InstallException("Failed to write YAML config file with Launcher settings", ex);
            }
        }

        private void CreateApplicationStateFile()
        {

            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData, 
                Environment.SpecialFolderOption.Create),
                @"ScreenDox");

            if(!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            var file = Path.Combine(folder, "version.yaml");

            if(!File.Exists(file))
            {
                File.WriteAllText(file, "version: 0.0.0.0");
            }
        }

        /// <summary>
        /// Add message security settings to service configuration
        /// </summary>
        private void ConfigureWcfServiceWithMessageSecuritySettings(Configuration configFile, bool isHttps)
        {
            System.ServiceModel.SecurityMode securityMode = SecurityMode.None;

            if (isHttps)
            {
                securityMode = SecurityMode.Transport;
            }

            var serviceModelSection = (ServiceModelSectionGroup)configFile.GetSectionGroup("system.serviceModel");
            serviceModelSection.Bindings.WSHttpBinding.Bindings[0].Security.Mode = securityMode;
        }


        /// <summary>
        /// Get path to the Kiosk Endpoint service
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <returns></returns>
        public Uri GetServicePath(string serverAddress)
        {
            if (!serverAddress.EndsWith("/")) serverAddress += "/";
            return new Uri(new Uri(serverAddress), "endpoint/kioskendpoint.svc");

        }
    }
}
