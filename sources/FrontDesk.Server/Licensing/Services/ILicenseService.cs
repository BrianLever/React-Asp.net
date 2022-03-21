using System.Collections.Generic;

namespace FrontDesk.Server.Licensing.Services
{
    public interface ILicenseService
    {
        LicenseService.ActivateProductLicenseResult ActivateLicense(string licenseKey, string activationKey);
        LicenseCertificate GetActivatedLicense();
        List<LicenseCertificate> GetAllProductLicenses();
        LicenseCertificate GetLicenseCertificate();
        LicenseCertificate GetLicenseCertificate(string licenseKey);
        bool HasLicenseKey();
        LicenseService.RegisterProductLicenseResult RegisterProductLicense(string licenseKey);
        void RemoveLicense(string licenseKey);
        string UpdateActivationRequestKey(LicenseCertificate certificate);
        string UpdateActivationRequestKey(string licenseKey);
    }
}