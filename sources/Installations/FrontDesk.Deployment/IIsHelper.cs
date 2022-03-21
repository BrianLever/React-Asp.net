using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.DirectoryServices;
using Microsoft.Win32;
using QtracVR.Deployment.WebServer;
using System.Web.Administration;
using System.ServiceProcess;
using System.Security.Principal;

namespace FrontDesk.Deployment
{
    public static class IIsHelper
    {
        public static bool EnableWindowsAuthentication(string sitename, string applicationName, bool enable)
        {
            Version iisVersion = GetIISVersion();
            bool succeed = false;
            if (iisVersion.Major >= 7)
            {
                succeed = IIs7Helper.EnableWindowsAuthForApplication(sitename, applicationName, enable);
            }
            else if (iisVersion.Major < 7 || !succeed)
            {


                //set anonimous users permissions
                DirectoryEntry webSite = new DirectoryEntry("IIS://localhost/w3svc");

                //find virtual catalog
                foreach (DirectoryEntry e in webSite.Children)
                {
                    if (Convert.ToString(e.Properties["ServerComment"].Value) == sitename)
                    {
                        DirectoryEntry vdir = new DirectoryEntry(e.Path + "/Root/" + applicationName);

                        AuthFlags nAuthFlags = (AuthFlags)(vdir.Properties[MetabasePropertyNames.AuthFlags][0]);

                        if (enable)
                        {
                            nAuthFlags = nAuthFlags | AuthFlags.AuthNTLM;
                        }
                        else
                        {
                            nAuthFlags = nAuthFlags & ~AuthFlags.AuthNTLM;
                        }
                        vdir.Properties[MetabasePropertyNames.AuthFlags][0] = nAuthFlags;
                        vdir.CommitChanges();

                        succeed = true;
                    }
                }



            }
            return succeed;
        }

        public static Version GetIISVersion()
        {
            using (RegistryKey componentsKey =
            Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\InetStp", false))
            {
                if (componentsKey != null)
                {
                    int majorVersion = (int)componentsKey.GetValue("MajorVersion", -1);
                    int minorVersion = (int)componentsKey.GetValue("MinorVersion", -1);
                    if (majorVersion != -1 && minorVersion != -1)
                    {
                        return new Version(majorVersion, minorVersion);
                    }
                }
                return new Version(0, 0);
            }
        }

        [FlagsAttribute()]
        public enum AuthFlags
        {//from http://technet2.microsoft.com/WindowsServer/en/Library/271ae19b-853f-4672-b743-5ba126e902db1033.mspx?mfr=true
            AuthAnonymous = 1,
            AuthBasic = 2,
            AuthNTLM = 4,
            AuthMD5 = 16,
            AuthPassport = 64
        }

        public class MetabasePropertyNames
        {// from http://technet2.microsoft.com/WindowsServer/en/Library/271ae19b-853f-4672-b743-5ba126e902db1033.mspx?mfr=true
            //TODO add all Properties
            public const string AuthFlags = "AuthFlags";
            public const string keyType = "keyType";
        }

        #region Read IIS Structures

        /// <summary>
        /// Get List of Web Sites from IIS
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// List of Properties of the IIsWebServer: http://msdn.microsoft.com/en-us/library/ms524487(VS.90).aspx
        /// </remarks>
        public static List<WebSiteEntry> GetIIsWebSites()
        {
            List<WebSiteEntry> webSiteEntries = new List<WebSiteEntry>();
            WebSiteEntry wsEntry = null;


            Version iisVersion = IIsHelper.GetIISVersion();

            if (iisVersion.Major < 7)
            {
                //string machineName = System.Environment.MachineName;
                using (DirectoryEntry webSites = new DirectoryEntry("IIS://localhost/w3svc"))
                {
                    if (webSites != null)
                    {
                        foreach (DirectoryEntry lEntry in webSites.Children)
                        {
                            if (lEntry.SchemaClassName == "IIsWebServer")
                            {
                                // lEntry["Name"]                   // W3SVC/1, W3SVC/12378398, etc 
                                // lEntry["Name"].substr(6)         // 1, 12378398, etc 

                                wsEntry = new WebSiteEntry();
                                wsEntry.Id = Convert.ToInt32(lEntry.Name);
                                
                                //wsEntry.Id = Convert.ToInt32(((string)lEntry.Properties["Name "].Value).Substring(6));
                                wsEntry.Name = (string)lEntry.Properties["ServerComment"].Value;

                                webSiteEntries.Add(wsEntry);
                            }
                        }
                    }
                }
            }
            else
            {
                webSiteEntries = GetIIs7WebSites();
            }
            return webSiteEntries;
        }
        /// <summary>
        /// Get site list for iis7 using Microsoft.Web.Administration.dll
        /// </summary>
        private static List<WebSiteEntry> GetIIs7WebSites()
        {
            List<WebSiteEntry> webSiteEntries = new List<WebSiteEntry>();
            WebSiteEntry wsEntry = null;
            var serverManager = new Microsoft.Web.Administration.ServerManager();
            foreach (Microsoft.Web.Administration.Site site in serverManager.Sites)
            {
                wsEntry = new WebSiteEntry();


                wsEntry.Id = site.Id;
                wsEntry.Name = site.Name;

                webSiteEntries.Add(wsEntry);
            }
            return webSiteEntries;
        }
        /// <summary>
        /// get app pools for iis 5-7
        /// </summary>
        /// <returns></returns>
        public static List<string> GetIIsAppPools()
        {
            List<string> poolList = new List<string>();

            Version iisVersion = IIsHelper.GetIISVersion();


            if (iisVersion.Major < 7)
            {

                string metabasePath = "IIS://localhost/W3SVC/AppPools";
                using (DirectoryEntry pools = new DirectoryEntry(metabasePath))
                {


                    foreach (DirectoryEntry pool in pools.Children)
                    {
                        if (pool.SchemaClassName == "IIsApplicationPool")
                        {
                            poolList.Add(pool.Name);
                        }
                    }
                }
            }
            else
            {
                //make a call to another method because of not loading Microsoft.Web.Administration.dll
                poolList = GetIIs7AppPools();
            }

            return poolList;
        }
        /// <summary>
        /// Get app list for iis7 using Microsoft.Web.Administration.dll
        /// </summary>
        /// <returns></returns>
        private static List<string> GetIIs7AppPools()
        {
            List<string> poolList = new List<string>();
            var serverManager = new Microsoft.Web.Administration.ServerManager();

            foreach (Microsoft.Web.Administration.ApplicationPool pool in serverManager.ApplicationPools)
            {
                poolList.Add(pool.Name);
            }
            return poolList;
        }

        private static bool IsPoolExists(string poolName, DirectoryEntry root)
        {
            foreach (DirectoryEntry pool in root.Children)
            {
                if (pool.SchemaClassName == "IIsApplicationPool" && pool.Name == poolName)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Create new App Pool
        /// </summary>
        /// <param name="metabasePath"></param>
        /// <param name="appPoolName"></param>
        public static void CreateAppPool(string appPoolName)
        {
            //  metabasePath is of the form "IIS://<servername>/W3SVC/AppPools"
            //    for example "IIS://localhost/W3SVC/AppPools" 
            //  appPoolName is of the form "<name>", for example, "MyAppPool"
            Version iisVersion = IIsHelper.GetIISVersion();

            if (iisVersion.Major < 7)
            {
                try
                {
                    string metabasePath = "IIS://localhost/W3SVC/AppPools";

                    DirectoryEntry newpool;
                    DirectoryEntry apppools = new DirectoryEntry(metabasePath);
                    newpool = apppools.Children.Add(appPoolName, "IIsApplicationPool");
                    newpool.CommitChanges();
                    //Console.WriteLine("Done.");

                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Failed in CreateAppPool with the following exception: \n{0}", ex);
                }
            }
            else
            {
                CreateAppPoolIIs7(appPoolName);


            }
        }

        /// <summary>
        /// Create new App Pool for IIS 7 using System.Web.Administration.dll
        /// </summary>
        private static void CreateAppPoolIIs7(string appPoolName)
        {
            var serverManager = new Microsoft.Web.Administration.ServerManager();

            serverManager.ApplicationPools.Add(appPoolName);
            serverManager.CommitChanges();
        }

        #endregion

        #region Register MIME Types
        /// <summary>
        /// Register mime type in IIS 6.0
        /// </summary>
        /// <param name="extension">Extension, for ex. ".xap" </param>
        /// <param name="mimeType">Mime type</param>
        /// <returns>True if extension has ben registered. False if mime type is already registered and has been updated</returns>
        public static bool RegisterMIMETypeMapping(string extension, string mimeType)
        {
            bool found = false;
            using (DirectoryEntry mimeMap = new DirectoryEntry("IIS://localhost/MimeMap"))
            {
               
                foreach (IISOle.MimeMap m in mimeMap.Properties["MimeMap"])
                {
                    if (m.Extension == extension)
                    {
                        // found existing, update
                        m.MimeType = mimeType;
                        found = true;
                    }
                }

                // not found, create new
                if (!found)
                {
                    mimeMap.Properties["MimeMap"].Add(new IISOle.MimeMapClass { Extension = extension, MimeType = mimeType });
                }

                // commit changes
                mimeMap.CommitChanges();
            }
            return !found;
        }
        /// <summary>
        /// Register Silverlight MIME type for IIS 6.0
        /// </summary>
        /// <returns>True if extension has ben registered. False if mime type is already registered and has been updated</returns>
        public static bool RegisterSilverlightMIMEType()
        {

            return RegisterMIMETypeMapping(".xap", "application/x-silverlight-app");
        }

        #endregion

        /// <summary>
        /// If version of IIS is 5 - restart iis, else if version higher then restart App Pool
        /// </summary>
        public static void RestartIISOrAppPool(string appPoolName)
        {
            Version version = GetIISVersion();
            if (version.Major == 5)
            {
                //IIS 5
                using (ServiceController controller = new ServiceController("IIS Admin Service"))
                {
                    if (controller.Status != ServiceControllerStatus.Stopped ||
                        controller.Status != ServiceControllerStatus.StopPending)
                    {
                        controller.Stop();
                    }
                    else
                    {
                        controller.Start();
                    }
                }
            }
            else
            { 
                if(String.IsNullOrEmpty(appPoolName))
                {
                    throw new ArgumentException("Cannot restart Application Pool. Invalid Pool name.");
                }
                //IIS version 6 or higher
                string path = "IIS://localhost/W3SVC/AppPools/" + appPoolName;
                using (DirectoryEntry w3svc = new DirectoryEntry(path))
                {
                    if (w3svc == null)
                    {
                        throw new ApplicationException(
                            String.Format("Cannot restart Application Pool. Application Pool with the name \"{0}\" does not exists."));
                    }
                    w3svc.Invoke("Recycle", null);
                }

            }
            
        }
    }
}
