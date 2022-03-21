using System;
using System.Collections.Generic;
using System.Globalization;
using FrontDesk.Deployment;
using System.Configuration;

namespace FrontDesk.RPMS.InstallerCustomActions
{
    public class RpmsInterfaceServiceInstallerActions
    {
        public string Server { get; set; }
        public string Port { get; set; }
        public string Namespace { get; set; }

        public string AccessCode { get; set; }
        public string VerifyCode { get; set; }
        public string ExpirationDateString { get; set; }



        public readonly ConfigurationHelper configHelper;


        public RpmsInterfaceServiceInstallerActions(
            System.Collections.Specialized.StringDictionary installerParameters)
        {
            if (!installerParameters.ContainsKey("server"))
            {
                throw new ArgumentException("Server Address parameter was not found. Cannot complete the installation.");
            }
            this.Server = installerParameters["server"];

            if (!installerParameters.ContainsKey("port"))
            {
                throw new ArgumentException("Server Port parameter was not found. Cannot complete the installation.");
            }
            this.Port = installerParameters["port"];

            if (!installerParameters.ContainsKey("namespace"))
            {
                throw new ArgumentException("EHR Database Namespace parameter was not found. Cannot complete the installation.");
            }
            this.Namespace = installerParameters["namespace"];

            if (!installerParameters.ContainsKey("accessCode"))
            {
                throw new ArgumentException("Access Code parameter was not found. Cannot complete the installation.");
            }
            this.AccessCode = installerParameters["accessCode"];

            if (!installerParameters.ContainsKey("verifyCode"))
            {
                throw new ArgumentException("Verify Code parameter was not found. Cannot complete the installation.");
            }
            this.VerifyCode = installerParameters["verifyCode"];

            if (!installerParameters.ContainsKey("verifyCodeExpirationDate"))
            {
                throw new ArgumentException("Verify Code Expiration Date parameter was not found. Cannot complete the installation.");
            }
            this.ExpirationDateString = installerParameters["verifyCodeExpirationDate"];


            this.configHelper = new ConfigurationHelper();
        }

        public IList<string> ValidateParameters()
        {
            List<string> validationErrors = new List<string>();

            if (string.IsNullOrEmpty(this.Port))
            {
                validationErrors.Add("EHR Server Port is required");
            }
            else
            {
                uint result;
                if (!UInt32.TryParse(this.Port, NumberStyles.Integer, CultureInfo.InvariantCulture, out result))
                {
                    validationErrors.Add("EHR Server Port port parameter is not a valid value. Cannot complete the installation.");
                }
            }


            if (string.IsNullOrEmpty(this.AccessCode))
            {
                validationErrors.Add("Access code is required");
            }

            if (string.IsNullOrEmpty(this.VerifyCode))
            {
                validationErrors.Add("Verify code is required");
            }

            if (string.IsNullOrEmpty(this.ExpirationDateString))
            {
                validationErrors.Add("Verify code expiration date is required");
            }
            else
            {
                DateTime result;

                if (!DateTime.TryParse(this.ExpirationDateString, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out result))
                {
                    validationErrors.Add("Verify code expiration date string was not recognized as valid date format MM/DD/YYYY");
                }
            }
            return validationErrors;
        }

        public void UpdateWebConfigFile(string targetDirectory)
        {
            var validationErrors = ValidateParameters();

            if (validationErrors.Count > 0)
            {
                throw new ApplicationException(string.Join("\r\n", validationErrors));
            }

            Configuration webConfig = configHelper.OpenWebConfigurationByPhysicalPath(targetDirectory);

            //this.SetupCredentials(webConfig);

            this.SetupBmxConnectionString(webConfig);

        }

        //public void SetupCredentials(Configuration webConfig)
        //{
        //    var credentialsSection = CreateCredentialsSection();

        //    var webConfigCredentialsSection = webConfig.GetSection("rpmsCredentials") as RpmsCredentialsSection;

        //    if (webConfigCredentialsSection == null)
        //    {
        //        //add section
        //        webConfig.Sections.Add("rpmsCredentials", credentialsSection);
        //    }
        //    else
        //    {
        //        webConfigCredentialsSection.AccessCode = credentialsSection.AccessCode;
        //        webConfigCredentialsSection.VerifyCode = credentialsSection.VerifyCode;
        //        webConfigCredentialsSection.ExpireAt = credentialsSection.ExpireAt;
        //    }

        //    //commit changes
        //    webConfig.Save();
        //}

        public void SetupBmxConnectionString(Configuration webConfig)
        {
          
            var connectionString = webConfig.ConnectionStrings.ConnectionStrings["IHS"];

            if (connectionString == null)
            {
                //add section
                webConfig.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("IHS", ""));
                connectionString = webConfig.ConnectionStrings.ConnectionStrings["IHS"];
            }


            connectionString.ConnectionString = string.Format("Server={0}; Port={1}; Namespace={2};",
                this.Server,
                this.Port,
                this.Namespace
                );

            //commit changes
            webConfig.Save();
        }
    }
}
