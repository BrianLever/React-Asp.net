using FrontDesk.Kiosk.KioskEndpointService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Kiosk.Controllers
{
    internal static class KioskEndpointServiceClientFactory
    {
        internal static TResult Execute<TResult>(Func<KioskEndpointClient, TResult> func)
        {
            TResult result;

            var headers = new Dictionary<string, string>
            {
                {KioskEndpointHeaderDescriptor.KioskIDHeader, Settings.AppSettings.KioskKey},
                {KioskEndpointHeaderDescriptor.KioskSecretHeader, Settings.AppSettings.KioskSecret}
            };

            KioskEndpointClient client = new KioskEndpointClient();
            try
            {
                using (var scope = new OperationContextScope(client.InnerChannel))
                {
                    foreach (var header in headers)
                    {
                        var customHeader = new MessageHeader<string>(header.Value);
                        var wcfHeader = customHeader.GetUntypedHeader(header.Key, "");

                        OperationContext.Current.OutgoingMessageHeaders.Add(wcfHeader);
                    }

                    result = func(client);
                }

                client.Close();
            }
            catch
            {
                client.Abort();
                throw;
            }

            return result;
        }
    }
}
