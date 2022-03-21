using ScreenDox.Licensing;

using System;
using System.Configuration;

namespace ScreenDox.Server.Security.Configuration
{
    /// <summary>
    /// Custom config section <screendox.Security />
    /// </summary>
    public class SecuritySecretsConfig
    {
        //This class reads the defined config section (if available) and stores it locally in the static _Config variable.
        //This config data is available by calling MedGroups.GetMedGroups().

        //private static SecretConfigSection _config = (ConfigurationManager.GetSection("screendox.Secrets") as SecretConfigSection);

        private static Lazy<SecurityConfigSection> _lazyInstance
            = new Lazy<SecurityConfigSection>(() => ConfigurationManager.GetSection("screendox.Security") as SecurityConfigSection, false);

        public static SecurityConfigSection GetConfiguration()
        {
            return _lazyInstance.Value;
        }
    }
}
