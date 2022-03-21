using FrontDesk.Server.Services.Security;

using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace FrontDesk.Server.IntegrationTests.KioskEndpoint
{
    public abstract class KioskEndpointTestBase
    {
        private const string KIOSK_ID = "PRUW";

        private const string KIOSK_SECRET = "7da891c424f26415aa18f104f67df4fe719114b148c65f59d80042ee3fe17f58";


        internal TResult ExecuteSut<TResult>(Func<KioskEndpointClient, TResult> func)
        {
            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                {KioskEndpointHeaderDescriptor.KioskIDHeader, KIOSK_ID},
                {KioskEndpointHeaderDescriptor.KioskSecretHeader, KIOSK_SECRET}
            };

            return ExecuteSut(func, headers);
        }

        internal TResult ExecuteSut<TResult>(Func<KioskEndpointClient, TResult> func, Dictionary<string, string> headers)
        {
            TResult result;

            KioskEndpointClient client = new KioskEndpointClient();
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                                ((sender, certificate, chain, sslPolicyErrors) => true);

                using (var scope = new OperationContextScope(client.InnerChannel))
                {
                    foreach(var header in headers)
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
