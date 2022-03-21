namespace ScreenDox.Server.Security.Configuration
{
    using FrontDesk.Common.Extensions;

    using System.Collections.Specialized;
    using System.Configuration;

    public class SecurityConfigSection : ConfigurationSection
    {
        //Decorate the property with the tag for your collection.
        [ConfigurationProperty("secrets")]
        [ConfigurationCollection(typeof(NameValueCollection))]
        public NameValueConfigurationCollection Secrets
        {
            get { return (NameValueConfigurationCollection)this["secrets"]; }
        }



        /// <summary>
        /// Secret key for the JWt token encryption
        /// </summary>
        public string SecretKey
        {
            get
            {
                return Secrets["SecretKey"].Value;
            }
        }

        /// <summary>
        /// Issuer for JWT token
        /// </summary>
        public string Issuer
        {
            get
            {
                return Secrets["Issuer"].Value;
            }
        }

        /// <summary>
        /// Application ID for JWT token. Used as Audience.
        /// </summary>
        public string ApplicationId
        {
            get
            {
                return Secrets["ApplicationId"].Value;
            }
        }


        /// <summary>
        /// Issuer for JWT token
        /// </summary>
        public int TokenExpirationInMin
        {
            get
            {
                return Secrets["TokenExpirationInMin"].Value.As<int>();
            }
        }

    }
}
