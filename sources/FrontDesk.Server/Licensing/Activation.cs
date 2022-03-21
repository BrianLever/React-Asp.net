using System;
using System.Globalization;
using System.Text;
using FrontDesk.Common;
using FrontDesk.Licensing;

namespace FrontDesk.Server.Licensing
{
    public class Activation
    {
        /*
         * Activation Request binary form:
         * 
         * 20 bit license [S]erial number
         * 4 bit license [Y]ears
         * 12x8 = 96 bit last 12 letters of [W]indows product id, dashes stripped
         * 
         * 0xSSSSSYWWWWWWWWWWWWWWWWWWWWWWWW
         * 
         * Activation Key binary form:
         * 
         * 20 bit license [S]erial number
         * 32 bit expiration date, with decimal letters [0-9] for convenience
         * 
         * 0xSSSSSyyyymmdd         
         */

        public int LicenseSerialNumber;
        public int Years;
        public string WindowsProductIDPart; // only 12 last letters of actual Product ID, no dashes

        public string ActivationRequest;
        /// <summary>
        /// Activation date
        /// </summary>
        public DateTime? ExpirationDate { get; set; }        
        public string ActivationKey{ get; set;}

        public Activation()
        {
        }

        public Activation(License license, string fullWindowsProductId)
        {
            fullWindowsProductId = fullWindowsProductId.Trim().Replace("-", "");
            if (fullWindowsProductId.Length < 20)
            {
                throw new ArgumentException("Invalid Windows ID");
            }

            this.WindowsProductIDPart = fullWindowsProductId.Substring(8, 12);
            this.LicenseSerialNumber = license.SerialNumber;
            this.Years = license.Years;
        }



        public static string CreateActivationRequest(Activation act)
        {
            // HFWJP-WDN5N-DEQ1J-GZCNB-VRFQVR

            StringBuilder sb = new StringBuilder(string.Format("{0:x5}{1:x1}", act.LicenseSerialNumber, act.Years));
            byte[] bytes = Encoding.ASCII.GetBytes(act.WindowsProductIDPart);  // 12 bytes
            foreach (byte b in bytes) { sb.Append(string.Format("{0:x2}", b)); }
            byte[] encrypted = LicenseUtil.Encrypt(TextFormatHelper.HexStringToBytes(sb.ToString()), LicenseUtil.DESKey2.Value);
            string rawRequest = TextFormatHelper.PackString(encrypted);

            string request = TextFormatHelper.FormatWithGroups(rawRequest, 5, false, true);

            return request;
        }

        
       

        /// <summary>
        /// Parse Activation Key string and set License Serial Number and Expiration Date values.
        /// </summary>
        /// <param name="activationKey"></param>
        /// <param name="fullWindowsProductId"></param>
        /// <returns></returns>
        public static Activation ParseActivationKey(string activationKey, string fullWindowsProductId)
        {
            try
            {
                if (activationKey.Length > (15 + 2))
                {
                    throw new ArgumentException("Bad activation key");
                }

                Activation act = new Activation();
                StringBuilder sbDecryptionKey = new StringBuilder();
                string windowsProductIDPart = fullWindowsProductId.Trim().Replace("-", "").Substring(8, 12);
                byte[] bytes = Encoding.ASCII.GetBytes(windowsProductIDPart);  // 12 bytes
                foreach (byte b in bytes)
                {
                    sbDecryptionKey.Append(string.Format("{0:x2}", b));
                }
                sbDecryptionKey.Append("11223344"); // pad to 16 bytes - used in CalculateActivationKey and ParseActivationKey

                byte[] unpacked = TextFormatHelper.UnpackString(activationKey);
                if (unpacked[0] != unpacked[8]) // See ActivationEntry::CalculateActivationKey()
                {
                    throw new ArgumentException("Bad activation key");
                }

                byte[] encrypted = new byte[8];
                Array.Copy(unpacked, encrypted, 8);    // strip garbage
                byte[] decrypted = LicenseUtil.Decrypt(encrypted, sbDecryptionKey.ToString());
                string rawActivation = TextFormatHelper.BytesToHexString(decrypted);

                act.LicenseSerialNumber = int.Parse(rawActivation.Substring(0, 5), NumberStyles.HexNumber);
                act.ExpirationDate = new DateTime(
                    int.Parse(rawActivation.Substring(5, 4)),
                    int.Parse(rawActivation.Substring(9, 2)),
                    int.Parse(rawActivation.Substring(11, 2)));

                act.ActivationKey = activationKey;
                return act;
            }
            catch (Exception ex)   // any exception means "bad key", do not disclose encryption issues
            {
                throw new ArgumentException("Bad activation key", ex);
            }
        }

       
        /// <summary>
        /// Clear Expiration date and activation key
        /// </summary>
        internal void ClearActivationKey()
        {
            this.ExpirationDate = null;
            this.ActivationKey = String.Empty;
        }

        internal static bool ValidateRequestString(string activationRequestStr)
        {
            if (!string.IsNullOrEmpty(activationRequestStr) && activationRequestStr.Length == 30)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
