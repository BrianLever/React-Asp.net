using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Web.Administration;


namespace FrontDesk.Deployment
{
    public static class IIs7Helper
    {
        public static void SetIIs7AnonymousIdentity(string sitename, string poolname, string virtualFolderName, string adminLogin, string adminPass)
        {
            var serverManager = new Microsoft.Web.Administration.ServerManager();
            Microsoft.Web.Administration.ApplicationPool pool = serverManager.ApplicationPools[poolname];
            if (pool == null)
            {
                throw new ApplicationException("Invalid pool name");
            }
            else
            {
                //Configure pool identity
                pool.ProcessModel.IdentityType = Microsoft.Web.Administration.ProcessModelIdentityType.SpecificUser;
                pool.ProcessModel.UserName = adminLogin;
                pool.ProcessModel.Password = adminPass;


                //Configure virtual directory identity, set to App Pool Identity
                Microsoft.Web.Administration.Site site = serverManager.Sites[sitename];

                //VirtualDirectory vdir = site.Applications["/QtracVR"].VirtualDirectories[session["IIS_VIRTUALDIRECTORY"]];
                Microsoft.Web.Administration.Application app = site.Applications["/" + virtualFolderName];
                var vdir = app.VirtualDirectories[0];
                vdir.UserName = adminLogin;
                vdir.Password = adminPass;

                serverManager.CommitChanges();
            }
        }

        /// <summary>
        /// Enable of disable Windows Authentication for application for II7
        /// </summary>
        /// <param name="sitename"></param>
        /// <param name="virtualFolderName"></param>
        /// <param name="enableWinAuthentication"></param>
        /// <returns>Returns false if operation cannot be performed</returns>
        public static bool EnableWindowsAuthForApplication(string sitename, string applicationName, bool enableWinAuthentication)
        {
            bool processed = false;
            var serverManager = new Microsoft.Web.Administration.ServerManager();
            var appIdentifier = sitename + "/" + applicationName;

            var globalConfig = serverManager.GetApplicationHostConfiguration();
            var locations = globalConfig.GetLocationPaths();
            bool overridenLocationExists = false;
            foreach (string locationOverriden in locations)
            {
                if (locationOverriden == appIdentifier)
                {
                    overridenLocationExists = true;
                    break;
                }
            }

            if (overridenLocationExists)
            {
                var winAuth = globalConfig.GetSection("system.webServer/security/authentication/windowsAuthentication", appIdentifier);
                var attr = winAuth.GetAttribute("enabled");
                attr.Value = true;

                processed = true;
                serverManager.CommitChanges();
            }
            else
            {
                //create location section

            }

            return processed;





        }

        /// <summary>
        /// Get app list for iis7 using Microsoft.Web.Administration.dll
        /// </summary>
        /// <returns></returns>
        public static List<string> GetIIs7AppPools()
        {
            List<string> poolList = new List<string>();
            var serverManager = new Microsoft.Web.Administration.ServerManager();

            foreach (Microsoft.Web.Administration.ApplicationPool pool in serverManager.ApplicationPools)
            {
                poolList.Add(pool.Name);
            }
            return poolList;
        }

        public static void RestartAppPool(string appPoolName)
        {
            if(String.IsNullOrEmpty(appPoolName))
            {
                throw new ArgumentException("App Pool Name argument cannot be empty");
            }

            ServerManager serverManager = new ServerManager();
            ApplicationPool pool = serverManager.ApplicationPools.FirstOrDefault(p=>p.Name == appPoolName);
            if (pool == null)
            {
                throw new ApplicationException(String.Format("App Pool with name \"{0}\" does not exists", appPoolName));
            }

            pool.Recycle();

            serverManager.CommitChanges();
        }
    }
}
