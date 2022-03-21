using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.Management.Instrumentation;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.ServiceModel.Configuration;
using System.Configuration;
using System.Web.Configuration;
using System.Collections;
using System.DirectoryServices;
using System.Security.Principal;

namespace FrontDesk.Deployment
{
    public class ConfigurationHelper
    {
        #region Constants

        private const string ManagementScopePath = @"\\localhost\root\MicrosoftIISV2";
        private const string IISWevServicePath = @"IIsWebService='W3SVC'";
        private const string ServerBindingPath = "ServerBinding";

        private const string VrootPathProperty = "PathOfRootVirtualDir";
        private const string ReturnValueProperty = "ReturnValue";

        public const string AccessSSLProperty = "AccessSSL";
        private const string AppFriendlyNameProperty = "AppFriendlyName";
        public const string AuthAnonymousProperty = "AuthAnonymous";
        public const string DefaultDocProperty = "DefaultDoc";
        private const string HttpErrorsProperty = "HttpErrors";

        private const string WMICreateNewSiteMethod = "CreateNewSite";
        private const string SelectWebSitesQuery = "SELECT * FROM IIsWebServerSetting";
        private const string SelectWebSiteByNameQueryTemplate = "SELECT * FROM IIsWebServerSetting WHERE Name='{0}'";

        private const string QuotationMark = "'";

        private const string WMIWebSitePathTemplate = "IIsWebServer='{0}'";
        private const string WMIStartWebSiteMethodName = "Start";

        private const string WebSitePhysicalPathProperty = "Path";
        private const string WMIWebSiteRootDirPathTemplate = "IIsWebVirtualDirSetting='{0}/root'";

        const string RegIISCommandLineArgsTemplate = @"-s {0}";
        const string AspNetRegIIsExecutable = "aspnet_regiis.exe";
        const string ConnectionStringEncryptFormat = @"-pef ""connectionStrings"" ""{0}""";
        const string ConnectionStringDecryptFormat = @"-pdf ""connectionStrings"" ""{0}""";
        const string GrantAccessToRsaContainerFormat = @" -pa ""NetFrameworkConfigurationKey"" ""{0}""";

        const string WcfRegIIsExecutable = "ServiceModelReg.exe";
        const string RegisterAspNetScriptsArgs = @"-i -enable
";
        

        const string MachineKeyEncryptFormat = @"-pef ""system.web/machineKey"" ""{0}""";
        const string MachineKeyDecryptFormat = @"-pdf ""system.web/machineKey"" ""{0}""";

        const string PasswordEncryptFormat = @"-pef ""system.web/identity"" ""{0}""";
        const string GrantUserAccessFormat = @"-ga ""{0}""";

        #endregion

        private readonly ManagementScope scope;

        public ConfigurationHelper()
        {
            scope = new ManagementScope(ManagementScopePath);
        }

        public ConfigurationHelper(string managementScopePath)
        {
            scope = new ManagementScope(ManagementScopePath);
        }

        

        #region IIS

        public void SetWebSiteProperties(string webSitePath, Hashtable properties)
        {
            string query = string.Format(SelectWebSiteByNameQueryTemplate, webSitePath);
            ManagementObjectSearcher webSitesSearcher = new ManagementObjectSearcher(scope, new ObjectQuery(query));
            ManagementObjectCollection webSitesList = webSitesSearcher.Get();

            foreach (ManagementObject webSite in webSitesList)
            {
                foreach (DictionaryEntry entry in properties)
                    webSite.Properties[(string)entry.Key].Value = entry.Value;
                webSite.Put();
            }
        }

        /// <summary>
        /// Changes the home directory of a website
        /// </summary>
        /// <param name="id">The id of the website. </param>
        /// <param name="newroot">The path to the new folder. </param>
        public void ModifyIISHomeDirectory(string id, string newroot)
        {
            DirectoryEntry site = new DirectoryEntry("IIS://localhost/w3svc/" + id + "/ROOT");

            //change the home folder
            site.Properties["Path"][0] = newroot;

            //commit the changes
            site.CommitChanges();
        }

        /// <summary>
        /// Allow authentication modes for website.
        /// See http://msdn.microsoft.com/en-us/library/ms524513.aspx for flag values.
        /// </summary>
        /// <param name="websiteId"></param>
        /// <param name="allowAuthFlags"></param>
        public string ModifyIISAuthenticationMode(string websiteId, UInt32 allowAuthFlags)
        {
            string debug = string.Empty;
            try
            {
                DirectoryEntry site = new DirectoryEntry("IIS://localhost/w3svc/" + websiteId + "/ROOT");

                object o = site.Properties["AuthFlags"].Value;
                debug = string.Format("type: '{0}', value: '{1}'", o.GetType().ToString(), o);

                UInt32 newFlags = Convert.ToUInt32(o) | allowAuthFlags;
                //site.Properties["AuthFlags"].Value = (UInt32)site.Properties["AuthFlags"].Value | allowAuthFlags;  // DWORD
                site.Properties["AuthFlags"].Value = newFlags;
                site.CommitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("{0}. {1}", ex.Message, debug));
            }

            return debug;
        }

        /// <summary>
        /// Get website id, e.g. "1", from its name, e.g. "Default Web Site"
        /// </summary>
        /// <param name="websiteName"></param>
        /// <returns></returns>
        public string GetWebsiteIdByName(string websiteName)
        {            
            // TODO: this method should work, but it is untested.

            DirectoryEntry w3svc = new DirectoryEntry("IIS://localhost/W3SVC");

            //DirectoryEntries sites = w3svc.Children;            
            foreach (DirectoryEntry site in w3svc.Children)
            {
                if (site.Properties["ServerComment"].Value.ToString().Equals(websiteName))
                {
                    return site.Name;
                }
            }

            return null;
        }

        /// <summary>
        /// Get website id from string path returned by InstallContext (Parameters["website"]),
        /// which is like "/LM/W3SVC/1".
        /// </summary>
        /// <param name="msiWebsitePath"></param>
        /// <returns></returns>
        public string ParseWebsiteIdFromMSI(string msiWebsitePath)
        {
            string[] parts = msiWebsitePath.Split(new char[] { '/' });
            return parts[3];
        }

        /// <summary>
        /// Get web.config configuration using application physical path
        /// </summary>
        /// <param name="physicalPath"></param>
        /// <returns></returns>
        public Configuration OpenWebConfigurationByPhysicalPath(string physicalPath)
        {
            string dummyVirtualPath = "/MyApp";

            WebConfigurationFileMap map = new WebConfigurationFileMap();
            map.VirtualDirectories.Add(dummyVirtualPath, new VirtualDirectoryMapping(physicalPath, true));
            return WebConfigurationManager.OpenMappedWebConfiguration(map, dummyVirtualPath);

        }

        /// <summary>
        /// Register WCF scripts and mapping for IIS
        /// </summary>
        public void RegisterWCFScripts()
        {
            string RegisterWcfScriptsCommandPath = Path.Combine(Environment.GetEnvironmentVariable("SystemRoot"), @"Microsoft.Net\Framework\v3.0\Windows Communication Foundation\");


            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RegisterWcfScriptsCommandPath, WcfRegIIsExecutable));
            startinfo.Arguments = "-ry";
            startinfo.CreateNoWindow = true;
            startinfo.UseShellExecute = true;
            //startinfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();
        }


        #endregion

        #region ASP.NET

        private string FixAspNetAppInstallationPhysicalPath(string installationPath)
        {

            if (!string.IsNullOrEmpty(installationPath) && installationPath.EndsWith("\\")) installationPath = installationPath.Substring(0, installationPath.Length - 1);
            return installationPath;
        }

        public bool IsXP()
        {
            var os = System.Environment.OSVersion;
            if (os.Version.Major == 5 && os.Version.Minor < 2) return true;
            else return false;
        }

        /// <summary>
        /// Grant access to IIS and ASP.NET resources to the windows user account
        /// </summary>
        /// <param name="username"></param>
        public void RegisterAspNetUser(string username)
        {
            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), AspNetRegIIsExecutable));
            startinfo.CreateNoWindow = true;
            startinfo.Arguments = string.Format(ConnectionStringEncryptFormat, username);
            startinfo.UseShellExecute = false;
            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();
        }

       


        #region Grant Access to RSA key container

        public void GrantAccessToRsaKeyContainer(string account)
        {

            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), AspNetRegIIsExecutable));
            startinfo.CreateNoWindow = true;
            startinfo.Arguments = string.Format(GrantAccessToRsaContainerFormat, account);
            startinfo.UseShellExecute = true;
            startinfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();


        }
        /// <summary>
        /// Grant access to default ASP.NET service identity - "NT Authority\Network Service" and to "ASPNET" for XP
        /// </summary>
        public void GrantAccessToRsaKeyContainer()
        {
            if (IsXP()) GrantAccessToRsaKeyContainer("ASPNET");
            GrantAccessToRsaKeyContainer(@"NT Authority\Network Service");
        }

        #endregion

        #region Encrypt Config
        /// <summary>
        /// Encrypt connection strings
        /// </summary>
        /// <param name="installationPath"></param>
        public void EncryptConnectionString(string installationPath)
        {
            installationPath = FixAspNetAppInstallationPhysicalPath(installationPath);

            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), AspNetRegIIsExecutable));
            startinfo.CreateNoWindow = true;
            startinfo.Arguments = string.Format(ConnectionStringEncryptFormat, installationPath);
            startinfo.UseShellExecute = true;

            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();
        }

        /// <summary>
        /// Encrypt connection strings
        /// </summary>
        /// <param name="installationPath"></param>
        public void DecryptConnectionString(string installationPath)
        {
            installationPath = FixAspNetAppInstallationPhysicalPath(installationPath);



            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), AspNetRegIIsExecutable));
            startinfo.CreateNoWindow = true;
            startinfo.Arguments = string.Format(ConnectionStringDecryptFormat, installationPath);
            startinfo.UseShellExecute = false;

            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();
        }


        /// <summary>
        /// Encrypt machine key 
        /// </summary>
        /// <param name="installationPath"></param>
        public void EncryptMachineKeyString(string installationPath)
        {
            installationPath = FixAspNetAppInstallationPhysicalPath(installationPath);

            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), AspNetRegIIsExecutable));
            startinfo.CreateNoWindow = true;
            startinfo.Arguments = string.Format(MachineKeyEncryptFormat, installationPath);
            startinfo.UseShellExecute = true;

            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();
        }

        /// <summary>
        /// Encrypt machine key 
        /// </summary>
        /// <param name="installationPath"></param>
        public void DecryptMachineKeyString(string installationPath)
        {
            installationPath = FixAspNetAppInstallationPhysicalPath(installationPath);

            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), AspNetRegIIsExecutable));
            startinfo.CreateNoWindow = true;
            startinfo.Arguments = string.Format(MachineKeyDecryptFormat, installationPath);
            startinfo.UseShellExecute = true;

            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();
        }



        /// <summary>
        /// Encrypt system.web.indetity element
        /// </summary>
        /// <param name="installationPath"></param>
        public void EncryptPasswordString(string installationPath)
        {
            installationPath = FixAspNetAppInstallationPhysicalPath(installationPath);

            ProcessStartInfo startinfo = new ProcessStartInfo(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), AspNetRegIIsExecutable));
            startinfo.CreateNoWindow = true;
            startinfo.WindowStyle = ProcessWindowStyle.Minimized;
            startinfo.Arguments = string.Format(PasswordEncryptFormat, installationPath);
            startinfo.UseShellExecute = false;
            Process svnExecute = Process.Start(startinfo);
            svnExecute.WaitForExit();
        }

        #endregion

        public ServiceModelSectionGroup SetWcfServiceServerAddress(System.Configuration.Configuration configFile, Uri serverAddressUri)
        {

            var serviceModelSection = (ServiceModelSectionGroup)configFile.GetSectionGroup("system.serviceModel");
            var endpoint = serviceModelSection.Client.Endpoints[0];
            endpoint.Address = serverAddressUri;
            configFile.Save(ConfigurationSaveMode.Modified);

            return serviceModelSection;
        }
        /// <summary>
        /// Add or update application setting value
        /// </summary>
        /// <param name="configFile"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetAppSettingValue(Configuration configFile, string key, string value)
        {
            var appSettings = configFile.AppSettings.Settings;
            if (appSettings.AllKeys.Contains(key))
                appSettings[key].Value = value;
            else
            {
                appSettings.Add(key, value);
            }
        }


        public void SetSQL2008ConnectionStringSqlAuthentication(Configuration configFile, string name, string servername, string databasename, string username, string password, string applicationName, bool clearAll)
        {
            var csSection = configFile.ConnectionStrings.ConnectionStrings;
            ConnectionStringSettings connectionStringSetting = null;

            System.Data.SqlClient.SqlConnectionStringBuilder connStr = new System.Data.SqlClient.SqlConnectionStringBuilder();
            connStr.DataSource = servername;
            connStr.InitialCatalog = databasename;
            connStr.UserID = username;
            connStr.Password = password;
            connStr.IntegratedSecurity = false;
            connStr.ApplicationName = applicationName;

            if (clearAll)
                csSection.Clear();

            if (csSection[name] != null)
            {
                connectionStringSetting = csSection[name];
            }
            else
            {
                connectionStringSetting = new ConnectionStringSettings();
                connectionStringSetting.Name = name;
                csSection.Add(connectionStringSetting);
            }


            connectionStringSetting.ConnectionString = connStr.ToString();
            connectionStringSetting.ProviderName = "SQLNCLI10";
        }

        #endregion

        #region Windows Service
        /// <summary>
        /// Start windows service
        /// </summary>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public Win32ServiceReturnValue StartWinService(string serviceName)
        {
            string objPath = string.Format("Win32_Service.Name='{0}'", serviceName);
            using (ManagementObject service = new ManagementObject(new ManagementPath(objPath)))
            {
                try
                {
                    ManagementBaseObject outParams = service.InvokeMethod("StartService",
                        null, null);
                    return (Win32ServiceReturnValue)Enum.Parse(typeof(Win32ServiceReturnValue),
                    outParams["ReturnValue"].ToString());
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower().Trim() == "not found" || ex.GetHashCode() == 41149443)
                        return Win32ServiceReturnValue.ServiceNotFound;
                    else
                        throw ex;
                }
            }
        }

        public enum Win32ServiceReturnValue
        {
            Success = 0,
            NotSupported = 1,
            AccessDenied = 2,
            DependentServicesRunning = 3,
            InvalidServiceControl = 4,
            ServiceCannotAcceptControl = 5,
            ServiceNotActive = 6,
            ServiceRequestTimeout = 7,
            UnknownFailure = 8,
            PathNotFound = 9,
            ServiceAlreadyRunning = 10,
            ServiceDatabaseLocked = 11,
            ServiceDependencyDeleted = 12,
            ServiceDependencyFailure = 13,
            ServiceDisabled = 14,
            ServiceLogonFailure = 15,
            ServiceMarkedForDeletion = 16,
            ServiceNoThread = 17,
            StatusCircularDependency = 18,
            StatusDuplicateName = 19,
            StatusInvalidName = 20,
            StatusInvalidParameter = 21,
            StatusInvalidServiceAccount = 22,
            StatusServiceExists = 23,
            ServiceAlreadyPaused = 24,
            ServiceNotFound = 25

        }

        public enum Win32ServiceStartMode
        {
            /// <summary>
            /// Device driver started by the operating system loader. This value is valid only for driver services.
            /// </summary>
            Boot,
            /// <summary>
            /// Device driver started by the operating system initialization process. This value is valid only for driver services.
            /// </summary>
            System,
            /// <summary>
            /// Service to be started automatically by the Service Control Manager during system startup.
            /// </summary>
            Automatic,
            /// <summary>
            /// Service to be started by the Service Control Manager when a process calls the StartService method.
            /// </summary>
            Manual,
            /// <summary>
            /// Service that can no longer be started
            /// </summary>
            Disabled

        }

        public Win32ServiceReturnValue ChangeWinServiceStartMode(string serviceName, Win32ServiceStartMode startMode)
        {
            string objPath = string.Format("Win32_Service.Name='{0}'", serviceName);
            using (ManagementObject service = new ManagementObject(new ManagementPath(objPath)))
            {
                try
                {
                    ManagementBaseObject inParams = service.GetMethodParameters("ChangeStartMode");
                    inParams["StartMode"] = startMode.ToString();


                    ManagementBaseObject outParams = service.InvokeMethod("ChangeStartMode",
                        inParams, null);
                    return (Win32ServiceReturnValue)Enum.Parse(typeof(Win32ServiceReturnValue),
                    outParams["ReturnValue"].ToString());
                }
                catch (Exception ex)
                {
                    if (ex.Message.ToLower().Trim() == "not found" || ex.GetHashCode() == 41149443)
                        return Win32ServiceReturnValue.ServiceNotFound;
                    else
                        throw ex;
                }
            }
        }

        #endregion

        /// <summary>
        /// Create local windows group, if not exist.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public bool CreateLocalGroup(string groupName, string description)
        {
            DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");

            // check if group already exists
            try
            {
                DirectoryEntry existingGroup = localMachine.Children.Find(groupName, "group");
                // no exception == group exists
                return true;
            }
            catch (COMException ex)
            {
                //if ((ex.GetType() == typeof(DirectoryServicesCOMException)) && (ex.ErrorCode != 0x2030))  // according to MSDN
                if (ex.ErrorCode != -2147022676)   // 0x800708ac, "The group name could not be found" - actual exception on Vista
                {
                    throw;
                }                
            }

            DirectoryEntry newGroup = localMachine.Children.Add(groupName, "group");            
            newGroup.Properties["Description"].Value = description;
            newGroup.CommitChanges();

            return true;
        }

        /// <summary>
        /// Add Windows user to the local group.
        /// </summary>
        /// <param name="userName">Windows user name in form "DOMAIN\LOGIN"</param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        public bool AddWindowsUserToLocalGroup(string userName, string groupName)
        {        
            string userPath = userName.Replace("\\", "/");  // DOMAIN\LOGIN -> DOMAIN/LOGIN
            DirectoryEntry localMachine = new DirectoryEntry("WinNT://" + Environment.MachineName + ",Computer");
            DirectoryEntry group = localMachine.Children.Find(groupName, "group");
            group.Invoke("Add", new object[] { string.Format("WinNT://{0}", userPath) });
            
            return true;
        }

    }
}
