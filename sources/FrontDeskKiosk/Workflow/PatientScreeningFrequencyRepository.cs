using Common.Logging;

using System;
using System.Collections.Generic;

namespace FrontDesk.Kiosk.Workflow
{
    internal class PatientScreeningFrequencyRepository : IPatientScreeningFrequencyRepository
    {
        private readonly ILog _logger = LogManager.GetLogger<PatientScreeningFrequencyRepository>();

        /// <summary>
        /// Call Ping method on server
        /// </summary>
        public Dictionary<string, int> GetPatientScreeningFrequencyStatistics(ScreeningPatientIdentity patient)
        {
            Dictionary<string, int> screeningsInCurrentGpraInterval = null;

            var client = new KioskEndpointService.KioskEndpointClient();
            try
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback =
                    ((sender, certificate, chain, sslPolicyErrors) => true);

                screeningsInCurrentGpraInterval = client.GetPatientScreeningFrequencyStatistics_v2(patient, Settings.AppSettings.KioskID);

                if (screeningsInCurrentGpraInterval == null)
                {
                    _logger.Warn("Error while reading patient screening frequency statistics from server. Returned null dictionary.");
                }

                client.Close();

            }
            catch (Exception ex)
            {
                _logger.Error("Failed to ping WCF service on server side.", ex);
                client.Abort();
            }
            return screeningsInCurrentGpraInterval;
        }

    }
}
