using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Common.Logging;

namespace FrontDesk.Kiosk.Discovery
{
    public class SelfDiscoveryService : ISelfDiscoveryService
    {
        private readonly ILog _logger = LogManager.GetLogger("SelfDiscoveryService");

        public string GetIpAddress()
        {
            if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                return null;
            }
            try
            {
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

                return host
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork)?.ToString();

            }
            catch
            {
                return null;
            }
        }

        public string GetAppVersion()
        {

            string version = "Unknown";
            try
            {
                var assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                version = assemblyName.Version.ToString();
            }
            catch (Exception ex)
            {
                _logger.Warn("Failed to get an assembly version.", ex);
            }

            return version;

        }
    }
}
