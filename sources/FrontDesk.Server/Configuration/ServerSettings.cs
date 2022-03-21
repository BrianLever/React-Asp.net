using ScreenDox.Server.Common.Configuration;

namespace FrontDesk.Server.Configuration
{
    public class ServerSettings : SystemSettings
    {
        /// <summary>
        /// Activation request email "To" email address
        /// </summary>
        public static string ActivationSupportEmail
        {
            get
            {
                return GetStringValue("ActivationSupportEmail", string.Empty);
            }
        }

        /// <summary>
        /// Activation request email "To" email address
        /// </summary>
        public static string ActivationRequestEmailTemplate
        {
            get
            {
                return GetStringValue("ActivationRequestEmailTemplate", "Please activate my product copy.%0A%0AMy activation request code: {0}");
            }
        }

        /// <summary>
        /// Activation request email "To" email address
        /// </summary>
        public static string ActivationSupportEmailSubject
        {
            get
            {
                return GetStringValue("ActivationSupportEmailSubject", "FrontDesk License Activation");
            }
        }
        



    }
}
