using System;
using System.Data;
using Common.Logging;
using FrontDesk.Common.Debugging;
using FrontDesk.Licensing;

namespace FrontDesk.Server.Licensing.Services
{
    public class LicenseCertificate
    {
        private readonly ILog _logger = LogManager.GetLogger<LicenseCertificate>();

        #region Timestamps & Activation

        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? ActivationRequestDate { get; set; }
        public DateTimeOffset? ActivatedDate { get; set; }

        #endregion

        #region License Parts

        private License _licenseKey;
        private Activation _activationKey;
        private string _activationRequestCode;

        /// <summary>
        /// Get license key
        /// </summary>
        public License License { get { return _licenseKey; } }

        public Activation ActivationKey { get { return _activationKey; } }

        public string ActivationRequestCode { get { return _activationRequestCode; } }
        /// <summary>
        /// License expiration date.
        /// </summary>
        public DateTime? ExpirationDate
        {
            get
            {
                if (_activationKey != null)
                {
                    return _activationKey.ExpirationDate;
                }
                return null;
            }
        }

        public string ActivationKeyString
        {
            get
            {
                if (_activationKey == null) return null;
                else return _activationKey.ActivationKey;
            }
        }


        public string WindowsProductID { get; private set; }

        #endregion

        #region Constructors

        public LicenseCertificate() { }

        public LicenseCertificate(License license)
        {

            this._licenseKey = license;
        }


        public LicenseCertificate(IDataReader reader)
        {
            string licenseKeyStr = Convert.ToString(reader["LicenseKey"]);
            string activationRequestStr = Convert.ToString(reader["ActivationRequest"]);
            string activationKeyStr = Convert.ToString(reader["ActivationKey"]);

            try
            {
                //parse license key
                _licenseKey = new License(licenseKeyStr);
            }
            catch (Exception ex)
            {
                var message = "Failed to read license key. Invalid license key. Key: {0}".FormatWith(licenseKeyStr);

                DebugLogger.TraceException(ex, message); 


                //eat exception to not buble up internal decode exception for hiding internal algorithms
                throw new DataException(message);
            }

            if (!string.IsNullOrEmpty(activationRequestStr))
            {
                //validate str
                if (Activation.ValidateRequestString(activationRequestStr))
                {
                    _activationRequestCode = activationRequestStr;
                }
            }

            //set activation code
            if (!string.IsNullOrEmpty(activationKeyStr))
            {
                try
                {
                    SetActivationKey(activationKeyStr);
                }
                catch
                {
                    _activationKey = null; //non valid activation key
                }
            }

            //read timestamps
            CreatedDate = (DateTimeOffset)reader["CreatedDate"];
            ActivationRequestDate = Convert.IsDBNull(reader["ActivationRequestDate"]) ? (DateTimeOffset?)null : (DateTimeOffset)reader["ActivationRequestDate"];
            ActivatedDate = Convert.IsDBNull(reader["ActivatedDate"]) ? (DateTimeOffset?)null : (DateTimeOffset)reader["ActivatedDate"];

        }


        #endregion

        #region Activation Request
        /// <summary>
        /// Get new activation key for license and assign it to the ActivationRequestCode property
        /// </summary>
        /// <exception cref="System.InvalidOperationException">License property is null</exception>
        public void UpdateActivationRequestKey()
        {
            if (License == null) throw new InvalidOperationException("LicenseCertificate need to be initialized with not null License value");
            var windowsID = LicenseUtil.GetWindowsProductID_WMI();
            _activationKey = new Activation(this.License, windowsID);
            ActivationRequestDate = DateTimeOffset.Now;
            _activationKey.ClearActivationKey();

            //_activationRequestCode = _activationKey.ToActivationRequestString();
            _activationRequestCode = Activation.CreateActivationRequest(_activationKey);
        }

        #endregion

        #region Activation

        /// <summary>
        /// Activate license
        /// </summary>
        /// <param name="activationKey"></param>
        internal void Activate(string activationKey)
        {
            _activationKey = null;
            var activation = Activation.ParseActivationKey(activationKey, LicenseUtil.GetWindowsProductID_WMI());
            if (this.License.SerialNumber == activation.LicenseSerialNumber)
            {
                _activationKey = activation;
                this.ActivatedDate = DateTimeOffset.Now;
            }
        }
        /// <summary>
        /// Set activation key if it is valid
        /// </summary>
        /// <param name="activationKeyString"></param>
        protected void SetActivationKey(string activationKeyString)
        {
            _activationKey = null;

            _logger.Trace("Getting Windows Product ID key...");

            try
            {
                WindowsProductID = LicenseUtil.GetWindowsProductID_WMI();
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to get Windows Product ID", ex);
            }

            _logger.TraceFormat("Windows Product ID received. Value: {0}", WindowsProductID);

            _logger.Trace("Parsing activaion key...");
            try
            {
                var activation = Activation.ParseActivationKey(activationKeyString, WindowsProductID);
                if (this.License.SerialNumber == activation.LicenseSerialNumber)
                {
                    _logger.TraceFormat("Serial number passed validation. Value: {0}", License.SerialNumber);

                    _activationKey = activation;
                }
                else
                {
                    _logger.TraceFormat("Serial number failed validation. License: {0}. Activation: {1}.",
                        License.SerialNumber,
                        activation.LicenseSerialNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Failed to parse activation key.", ex);

                throw;
            }
        }

        #endregion
    }
}
