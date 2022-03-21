using FrontDesk.Common.Extensions;

using System;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// View model for activating License Key
    /// </summary>
    public class ActivationLicenseCertificateResponse
    {
        public string LicenseString { get; set; }


        public string ActivationRequestString { get; set; }

    }
}
