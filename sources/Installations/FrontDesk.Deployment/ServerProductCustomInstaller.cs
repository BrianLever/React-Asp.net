using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.ServiceModel.Configuration;
using System.ServiceModel;

namespace FrontDesk.Deployment
{
    public static class ServerProductCustomInstaller
    {
        private readonly static ConfigurationHelper configHelper = new ConfigurationHelper();


        public static void UpdateServerWebConfig(string targetPhysicalPath, string sqlServerName, string sqlLoginPassword)
        {
            //find configuration file
            Configuration configFile = configHelper.OpenWebConfigurationByPhysicalPath(targetPhysicalPath);
            configHelper.SetSQL2008ConnectionStringSqlAuthentication(configFile, "LocalConnection", sqlServerName, "FrontDesk",
                "frontdesk_appuser", sqlLoginPassword, "FrontDesk Server", false);
            configFile.Save(); //save changes
        }

        public static void UpdateLMTWebConfig(string targetPhysicalPath, string sqlServerName, string sqlLoginPassword)
        {
            //find configuration file
            Configuration configFile = configHelper.OpenWebConfigurationByPhysicalPath(targetPhysicalPath);
            configHelper.SetSQL2008ConnectionStringSqlAuthentication(configFile, "Licensing", sqlServerName, "FrontDeskLicense",
                "fdlicense_appuser", sqlLoginPassword, "FrontDesk License Manager", false);
            configFile.Save(); //save changes
        }

        public static void UpdateWCFServiceConfigurationWithMessageCridentials(string configFilePath,
            string certificateSubject, string dnsValue, bool isHttps, bool isMessage)
        {
            Configuration configFile = configHelper.OpenWebConfigurationByPhysicalPath(configFilePath);
            
            ServiceModelSectionGroup serviceModelSection = configFile.GetSectionGroup("system.serviceModel") as ServiceModelSectionGroup;

            //change security mode in binding configuratio
            WSHttpBindingElement binding =
                serviceModelSection.Bindings.WSHttpBinding.Bindings[0];

            System.ServiceModel.SecurityMode securityMode = SecurityMode.None;

            if (isHttps && !isMessage)
            {
                securityMode = SecurityMode.Transport;
            }
            else if (!isHttps && isMessage)
            {
                securityMode = SecurityMode.Message;
            }
            else if (isHttps && isMessage)
            {
                securityMode = SecurityMode.TransportWithMessageCredential;
            }


            binding.Security.Mode = securityMode;
            binding.Security.Message.ClientCredentialType = System.ServiceModel.MessageCredentialType.Certificate;

            //append service credentials section to behaviour configuration
            ServiceBehaviorElement behaviour = serviceModelSection.Behaviors.ServiceBehaviors[0];

            ServiceCredentialsElement serviceCredentialsElement = new ServiceCredentialsElement();
            serviceCredentialsElement.ServiceCertificate.FindValue = certificateSubject;
            serviceCredentialsElement.ServiceCertificate.X509FindType = System.Security.Cryptography.X509Certificates.X509FindType.FindBySubjectName;
            serviceCredentialsElement.ServiceCertificate.StoreLocation = System.Security.Cryptography.X509Certificates.StoreLocation.LocalMachine;
            serviceCredentialsElement.ServiceCertificate.StoreName = System.Security.Cryptography.X509Certificates.StoreName.My;

            serviceCredentialsElement.ClientCertificate.Authentication.CertificateValidationMode = 
                System.ServiceModel.Security.X509CertificateValidationMode.ChainTrust;
            serviceCredentialsElement.ClientCertificate.Authentication.RevocationMode = 
                System.Security.Cryptography.X509Certificates.X509RevocationMode.NoCheck;

            behaviour.Add(serviceCredentialsElement);


            //change DNS value to Front
            serviceModelSection.Services.Services[0].Endpoints[0].Identity.Dns.Value = dnsValue;

            configFile.Save(ConfigurationSaveMode.Modified);
        }
    }
}
