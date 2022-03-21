using System.Collections.Specialized;
using System.Configuration;

namespace ScreenDox.Licensing
{
    public class SecretConfigSection : ConfigurationSection
    {
        //Decorate the property with the tag for your collection.
        [ConfigurationProperty("secrets")]
        [ConfigurationCollection(typeof(NameValueCollection))]
        public NameValueConfigurationCollection Secrets
        {
            get { return (NameValueConfigurationCollection)this["secrets"]; }
        }



        /// <summary>
        /// Encyption key for the license code
        /// </summary>
        public string LicenseEncryptionKey
        {
            get
            {
                return Secrets["LicenseEncryptionKey"].Value;
            }
        }

        /// <summary>
        /// Encyption key for the activation request code
        /// </summary>
        public string ActivationRequestEncryptionKey
        {
            get
            {
                return Secrets["ActivationRequestEncryptionKey"].Value;
            }
        }

    }
}
