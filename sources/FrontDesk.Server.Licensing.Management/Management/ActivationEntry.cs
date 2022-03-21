using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using FrontDesk.Licensing;
using FrontDesk.Common;
using System.Globalization;
using System.Data;

namespace FrontDesk.Server.Licensing.Management
{
    public class ActivationEntry: Activation
    {
        public int ActivationID;
        public DateTimeOffset Issued;

        private static ActivationDb DbObject
        {
            get
            {
                return new ActivationDb();
            }
        }

        public ActivationEntry(): base()
        {
        }

       
        public static ActivationEntry CreateFromDataReader(IDataReader reader)
        {
            ActivationEntry act = new ActivationEntry();

            act.ActivationID = Convert.ToInt32(reader["ActivationID"]);
            act.ActivationRequest = Convert.ToString(reader["ActivationRequest"]);
            act.LicenseSerialNumber = Convert.ToInt32(reader["SerialNumber"]);
            act.WindowsProductIDPart = Convert.ToString(reader["ProductId"]);
            act.ExpirationDate = Convert.ToDateTime(reader["ExpirationDate"]);
            act.ActivationKey = Convert.ToString(reader["ActivationKey"]);
            act.Issued = (DateTimeOffset)reader["Issued"];

            return act;
        }

        public static int ActivateLicense(ActivationEntry act, LicenseEntry license)
        {
            return DbObject.Add(act, license);
        }

        public static ActivationEntry GetByLicenseID(int licenseID)
        {
            return DbObject.GetByLicenseID(licenseID);
        }

        /// <summary>
        /// Create activation key.
        /// 
        /// License years are added to the start date, giving expiration date.
        /// License serial number and expiration date are then encrypted with license windows ID used as encryption key.
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        public void CalculateActivationKey(DateTime startDate)
        {
            //DateTime expirationDate = startDate.AddYears(this.Years);
            this.ExpirationDate = startDate.AddYears(this.Years);

            StringBuilder sb = new StringBuilder(string.Format("{0:x5}{1:yyyyMMdd}", this.LicenseSerialNumber, this.ExpirationDate));

            StringBuilder sbEncryptionKey = new StringBuilder();
            byte[] bytes = Encoding.ASCII.GetBytes(this.WindowsProductIDPart);  // 12 bytes
            foreach (byte b in bytes)
            {
                sbEncryptionKey.Append(string.Format("{0:x2}", b));
            }
            sbEncryptionKey.Append("11223344"); // pad to 16 bytes - used in CalculateActivationKey and ParseActivationKey

            byte[] encrypted = LicenseUtil.Encrypt(TextFormatHelper.HexStringToBytes(sb.ToString()), sbEncryptionKey.ToString());
            // Eliminate random letters in activation key.
            // 15 letters require 9 bytes.
            byte[] padded = new byte[9];
            Array.Copy(encrypted, padded, 8);
            padded[8] = padded[0];  // allows additional check
            
            string rawKey = TextFormatHelper.PackString(padded);

            string key = TextFormatHelper.FormatWithGroups(rawKey, 5, true, false);
            this.ActivationKey = key;
            //return key;
        }


        public ActivationEntry(string activationRequest)
        {
            try
            {
                // HFWJP-WDN5N-DEQ1J-GZCNB-VRFQVR
                if (activationRequest.Length > 30)
                {
                    throw new ArgumentException("Bad activation request");
                }

                byte[] unpacked = TextFormatHelper.UnpackString(activationRequest);
                byte[] encrypted = new byte[16];
                Array.Copy(unpacked, encrypted, 16);    // trim padding
                byte[] decrypted = LicenseUtil.Decrypt(encrypted, LicenseUtil.DESKey2.Value);

                string hexstring = TextFormatHelper.BytesToHexString(decrypted);
                this.LicenseSerialNumber = int.Parse(hexstring.Substring(0, 5), NumberStyles.HexNumber);
                this.Years = int.Parse(hexstring.Substring(5, 1), NumberStyles.HexNumber);
                this.WindowsProductIDPart = Encoding.ASCII.GetString(decrypted, 3, 12);

                this.ActivationRequest = activationRequest;
            }
            catch (Exception)   // any exception means "bad request", do not disclose encryption issues
            {
                throw new ArgumentException("Bad activation request");
            }
        }
    }
}
