using System;
using System.Configuration;

namespace ScreenDox.Licensing
{
    /// <summary>
    /// Custom config section <screendox.Secrets />
    /// </summary>
    public class LicenseSecretsConfig
    {
        //This class reads the defined config section (if available) and stores it locally in the static _Config variable.
        //This config data is available by calling MedGroups.GetMedGroups().

        //private static SecretConfigSection _config = (ConfigurationManager.GetSection("screendox.Secrets") as SecretConfigSection);

        private static Lazy<SecretConfigSection> _lazyInstance 
            = new Lazy<SecretConfigSection>(() => (ConfigurationManager.GetSection("screendox.Secrets") as SecretConfigSection), false);

        public static SecretConfigSection GetConfiguration()
        {
            return _lazyInstance.Value;
        }
    }
}
