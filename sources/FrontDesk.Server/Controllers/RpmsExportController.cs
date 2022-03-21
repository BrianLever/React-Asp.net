using System;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Resources;
using RPMS.Common.GlobalConfiguration;
using RPMS.Common.Security;

namespace FrontDesk.Server.Controllers
{
    public class RpmsExportController
    {
        private readonly IDateService _dateService;
        private readonly Lazy<RpmsCredentials> _rpmsCredential;
        private readonly IRpmsCredentialsService _rpmsCredentialsService;
        private readonly IGlobalSettingsService _globalSettingsService;

        private bool isInErrorState; //when true, there is no access to the EHR Inteface


        public RpmsExportController(
            IDateService dateService,
            IRpmsCredentialsService rpmsCredentialsService,
            IGlobalSettingsService globalSettingsService
            )
        {
            if (dateService == null)
                throw new ArgumentNullException("dateService");

            if (rpmsCredentialsService == null)
                throw new ArgumentNullException("rpmsCredentialsService");

            _dateService = dateService;
            _rpmsCredentialsService = rpmsCredentialsService;
            _globalSettingsService = globalSettingsService ?? throw new ArgumentNullException(nameof(globalSettingsService));
            _rpmsCredential = new Lazy<RpmsCredentials>(() => _rpmsCredentialsService.GetCredentials());
        }

        public bool HasConnectionToExternalService
        {
            get { return !isInErrorState; }
        }

        public DateTime? GetCredentialsExpirationDate()
        {
            DateTime? result = null;

            if (isInErrorState)
                return null; //if service is not accessible = don't wait eve

            try
            {
                RpmsCredentials credentials = _rpmsCredential.Value;
                if (credentials != null)
                {
                    result = credentials.ExpireAt;
                }
            }
            catch (Exception ex)
            {
                ErrorLog.AddServerException("ScreenDox RPMS Interface service is not available.", ex);
                isInErrorState = true;
                result = null;
            }
            return result;
        }

        public int? GetDaysToCredentialsExpiration()
        {
            if (_globalSettingsService.IsRpmsMode)
            {
                DateTime? expirationDate = GetCredentialsExpirationDate();
                if (expirationDate.HasValue)
                {
                    return (int)((expirationDate.Value - _dateService.GetCurrentDate()).TotalDays);
                }
            }
            return null;
        }

        public bool ShouldDisplayAlertMessage()
        {
            if (_globalSettingsService.IsRpmsMode)
            {

                int days2Expiration = GetDaysToCredentialsExpiration().GetValueOrDefault();

                return days2Expiration <= AppSettings.RPMSCredentialsExpirationNotificationInDays;
            }
            return false;

        }

        public bool? RpmsCredentialsAreExpired()
        {
            if (_globalSettingsService.IsRpmsMode)
            {
                if (isInErrorState)
                    return null; //if no access - return unknown

                int? days2Expiration = GetDaysToCredentialsExpiration();
                if (days2Expiration.HasValue)
                {
                    return days2Expiration.Value < 0;
                }

                return true;
            }

            return false;
        }

        public string GetAlertMessageText()
        {
            string result = string.Empty;
            int? daysToExp = GetDaysToCredentialsExpiration();
            if (ShouldDisplayAlertMessage())
            {
                if (daysToExp >= 0)
                {
                    result = TextMessages.RPMSCredentialsExpirationAlertMessage.FormatWith(daysToExp);
                }
                else
                {
                    result = TextMessages.RPMSCredentialsExpiredMessage;
                }
            }
            return result;
        }
    }
}