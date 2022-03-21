using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Permissions;
using FrontDesk.Server.Membership;
using FrontDesk.Common.Debugging;

namespace FrontDesk.Server.Licensing.Services
{
    /// <summary>
    /// Provides methods to work with Lisence key on server web application
    /// </summary>
    public class LicenseService : ILicenseService
    {
        #region Constructor

        private static object _syncObj = new object();
        private static LicenseService _instance = null;

        public string CurrentWindowsProductID { get; private set; } = null;
        /// <summary>
        /// Get current service instance
        /// </summary>
        public static LicenseService Current
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObj)
                    {
                        if (_instance == null) _instance = new LicenseService();
                    }

                }
                return _instance;
            }
        }

        public LicenseService() { }
        #endregion


        /// <summary>
        /// Get activated license
        /// </summary>
        /// <returns></returns>
        public LicenseCertificate GetActivatedLicense()
        {
           
            var licenses = DbObject.GetActivatedLicenses();

            //find first valid activated license
            foreach (var cert in licenses)
            {
                this.CurrentWindowsProductID = cert.WindowsProductID ?? this.CurrentWindowsProductID;

                if (cert.ActivationKey != null && !string.IsNullOrEmpty(cert.ActivationKeyString))
                {
                    if (cert.ExpirationDate > DateTime.Now)
                    {
                       return cert;
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// Get database object
        /// </summary>
        private static LicenseServiceDb DbObject { get { return new LicenseServiceDb(); } }

        #region License Keys

        /// <summary>
        /// Get if product has any valid license
        /// </summary>
        /// <returns></returns>
        public bool HasLicenseKey()
        {
            bool hasLicense = false;
            //get all valid licenses
            List<LicenseCertificate> licenses = DbObject.GetAllLicenses();

            //find first valid activated license
            foreach (var license in licenses)
            {
                if (license.License != null) hasLicense = true;
            }

            return hasLicense;
        }




        /// <summary>
        /// Get last entered valid license key with related data
        /// </summary>
        /// <returns></returns>
        internal LicenseCertificate GetMostRecentLicenseKey()
        {
            LicenseCertificate validLicense = null;
            //get all valid licenses
            List<LicenseCertificate> licenses = DbObject.GetAllLicenses();

            //find first valid activated license
            foreach (var license in licenses)
            {
                if (license.License != null) validLicense = license;
            }

            return validLicense;
        }

        /// <summary>
        /// Register new prouduct License key
        /// </summary>
        /// <param name="licenseKey"></param>
        /// <returns></returns>
        public RegisterProductLicenseResult RegisterProductLicense(string licenseKey)
        {
            RegisterProductLicenseResult result = RegisterProductLicenseResult.None;

            try
            {
                License license = new License(licenseKey);
                //create license key
                LicenseCertificate licenseK = new LicenseCertificate(license);
                licenseK.CreatedDate = DateTimeOffset.Now;

                bool succeed = DbObject.RegisterProductLicense(licenseK);
                if (succeed) result = RegisterProductLicenseResult.RegisteredNewLicenseKey;
                else result = RegisterProductLicenseResult.DuplicateLicenseKey;

            }
            catch (ArgumentException) //invalid license
            {
                //do nothing - just return false
                result = RegisterProductLicenseResult.InvalidLicenseKey;
            }

            return result;
        }

        #region Registration and Activation operation results
        /// <summary>
        /// Operation result of registering new license key
        /// </summary>
        public enum RegisterProductLicenseResult
        {
            None = 0,
            RegisteredNewLicenseKey,
            InvalidLicenseKey,
            DuplicateLicenseKey
        }

        /// <summary>
        /// Operation result of registering new license key
        /// </summary>
        public enum ActivateProductLicenseResult
        {
            None = 0,
            Activated,
            InvalidActivationKey
        }

        #endregion

        /// <summary>
        /// Get all entered product licenses
        /// </summary>
        /// <returns></returns>
        public List<LicenseCertificate> GetAllProductLicenses()
        {
            return DbObject.GetAllLicenses();
        }
        /// <summary>
        ///  Get all entered product licenses
        /// </summary>
        /// <returns></returns>
        [System.ComponentModel.DataObjectMethod(System.ComponentModel.DataObjectMethodType.Select)]
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        public static List<LicenseCertificate> GetAllProductLicensesForDisplay()
        {
            return Current.GetAllProductLicenses();
        }

        #endregion

        /// <summary>
        /// Get license certificate
        /// </summary>
        public LicenseCertificate GetLicenseCertificate(string licenseKey)
        {
            return DbObject.GetLicenseCertificate(licenseKey);
        }
        /// <summary>
        /// Get most recent valid license certificate
        /// </summary>
        public LicenseCertificate GetLicenseCertificate()
        {
            return DbObject.GetMostRecentLicenseCertificate();
        }
        /// <summary>
        /// Get new activation request key and write it to the database
        /// </summary>
        /// <param name="p"></param>
        public string UpdateActivationRequestKey(string licenseKey)
        {
            //step 1 - get license certificate
            var cert = GetLicenseCertificate(licenseKey);
            if (cert == null) return null;
            cert.UpdateActivationRequestKey();

            DbObject.UpdateActivationKeys(cert);

            return cert.ActivationRequestCode;

        }


        /// <summary>
        /// Get new activation request key and write it to the database
        /// </summary>
        public string UpdateActivationRequestKey(LicenseCertificate certificate)
        {
            if (certificate == null) throw new ArgumentNullException("certificate");
            certificate.UpdateActivationRequestKey();

            DbObject.UpdateActivationKeys(certificate);

            return certificate.ActivationRequestCode;

        }



        public ActivateProductLicenseResult ActivateLicense(string licenseKey, string activationKey)
        {
            ActivateProductLicenseResult result = ActivateProductLicenseResult.None;
           

            var appLicenseCert = DbObject.GetLicenseCertificate(licenseKey);
            if (appLicenseCert == null)
            {
                result = ActivateProductLicenseResult.InvalidActivationKey; //license not found
            }
            else
            {
                try
                {
                    appLicenseCert.Activate(activationKey);
                }
                catch (Exception ex)
                {
#if DEBUG
                    DebugLogger.DebugException(ex); //only debug!
#endif
                    return  ActivateProductLicenseResult.InvalidActivationKey; //key is not valid
                }
                if (appLicenseCert.ExpirationDate == null)
                {
                    result = ActivateProductLicenseResult.InvalidActivationKey; //activation failed
                }
                else
                {
                    //save activation
                    DbObject.UpdateActivationKeys(appLicenseCert);
                    result = ActivateProductLicenseResult.Activated;
                }
            }
            return result;


        }
        //TODO: uncomment [PrincipalPermission(SecurityAction.Demand, Role= UserRoles.SuperAdministrator)]
        public void RemoveLicense(string licenseKey)
        {
            DbObject.RemoveLicense(licenseKey);
        }
    }
}
