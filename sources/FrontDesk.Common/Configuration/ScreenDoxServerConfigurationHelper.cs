using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Common.Configuration
{
    /// <summary>
    /// Contains helper methods to generate ScreenDox url full path during installation and auto-update
    /// </summary>
    public static class ScreenDoxServerConfigurationHelper
    {
        public static string GetFqdnKioskEndpointAddress(string baseUrl)
        {
            if (baseUrl == null)
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            if(!baseUrl.EndsWith("/"))
            {
                baseUrl = baseUrl + "/";
            }

            string relativeEndpointUri = "endpoint/KioskEndpoint.svc";

            Uri address = new Uri(new Uri(baseUrl, UriKind.Absolute), relativeEndpointUri);

            return address.ToString();
        }
    }
}
