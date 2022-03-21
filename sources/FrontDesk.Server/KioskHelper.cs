using FrontDesk.Server.Licensing.Services;

using ScreenDox.Server.Common.Data;
using ScreenDox.Server.Common.Models;
using ScreenDox.Server.Common.Services;
using System;

namespace FrontDesk.Server
{
    public static class KioskHelper
    {
        #region private fields

        private static KioskService Service { get { return new KioskService(); } } 

        private static KioskDatabase DbObject { get { return new KioskDatabase(); } }

        #endregion

        #region ADD/Update/Delete

        /// <summary>
        /// Add FrontDesk Kiosk to the system
        /// KioskID is automatically generated
        /// </summary>
        public static short Add(Kiosk kiosk)
        {
            var db = DbObject;
            var cert = LicenseService.Current.GetActivatedLicense();

            if (cert == null) throw new ApplicationException(Resources.TextMessages.LicenseValidation_NeedToActivateMessage);

            return Service.Add(kiosk, cert.License.MaxKiosks);
        }
       
        /// <summary>
        /// Enabled/Disabled kiosk
        /// </summary>
        public static void SetDisabledStatus(Int16 kioskID, bool isDisabled)
        {
            var db = DbObject;
            var cert = LicenseService.Current.GetActivatedLicense();
            if (cert == null) throw new ApplicationException(Resources.TextMessages.LicenseValidation_NeedToActivateMessage);

            Service.SetDisabledStatus(kioskID, isDisabled, cert.License.MaxKiosks);
        }

        #endregion
    }
}
