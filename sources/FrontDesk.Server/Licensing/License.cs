using FrontDesk.Common;
using FrontDesk.Licensing;

using System;
using System.Globalization;

namespace FrontDesk.Server.Licensing
{
    /// <summary>
    /// Common license information, that is used in all applications
    /// </summary>
    public class License
    {
        /*
         Binary format: 
         
         1. License Key:
            - 20 bit [S]erial Number, allows to have 1048575 unique licenses;
            - 4 bit [Y]ears, allows to assign 1 to 16 years for license;
            - 12 bit max [K]iosks, allows 1 to 4096 kiosks;
            - 12 bit max [B]ranches, allows 1 to 4096 branches.
          
         0xSSSSSYKKKBBB         
         */

        public int LicenseID;
    
        private int serialNumber;
        public int SerialNumber
        {
            get
            {
                return this.serialNumber;
            }
            set
            {
                if (value > 0xFFFFF)
                {
                    throw new ArgumentException("Max serial number is 0xFFFFF");
                }
                this.serialNumber = value;
            }
        }

        private int years;
        /// <summary>
        /// Years
        /// </summary>
        public int Years
        {
            get
            {
                return this.years;
            }
            set
            {
                if (value > 0xF)
                {
                    throw new ArgumentException("Max years is 0xF");
                }
                this.years = value;
            }
        }

        private int maxKiosks;
        /// <summary>
        /// Max Kiosks
        /// </summary>
        public int MaxKiosks
        {
            get
            {
                return this.maxKiosks;
            }
            set
            {
                if (value > 0xFFF)
                {
                    throw new ArgumentException("Max kiosks is 0xFFF");
                }
                this.maxKiosks = value;
            }
        }

        private int maxBranchLocations;
        /// <summary>
        /// Max Branch locations
        /// </summary>
        public int MaxBranchLocations
        {
            get
            {
                return this.maxBranchLocations;
            }
            set
            {
                if (value > 0xFFF)
                {
                    throw new ArgumentException("Max locations is 0xFFF");
                }
                this.maxBranchLocations = value;
            }
        }
    
        public string LicenseString{get; private set;}

        public License()
        {
        }
      
        /// <summary>
        /// Recreate only License features from license key. LicenseID, ClientID will be unknown.
        /// </summary>
        /// <param name="bytes"></param>
        public License(string licenseKey)
        {
            try
            {
                //format string and asign it
                this.LicenseString = TextFormatHelper.FormatWithGroups(licenseKey, 5, false, false);

                if (LicenseString.Length != (15 + 2))
                {
                    throw new ArgumentException("Invalid license key");
                }

                byte[] unpacked = TextFormatHelper.UnpackString(licenseKey);

                if (unpacked[0] != unpacked[8]) // see CreateLicenseKeyString()
                {                    
                    throw new ArgumentException("Invalid license key");
                }

                byte[] encrypted = new byte[8];
                Array.Copy(unpacked, encrypted, 8); // remove garbage
                byte[] decrypted = LicenseUtil.Decrypt(encrypted, LicenseUtil.DESKey1.Value);
                this.Init(decrypted);
               
            }
            catch (Exception ex)   // any error means invalid license string
            {
                throw new ArgumentException("Invalid license key", ex);
            }
        }

        /// <summary>
        /// Recreate values from binary representation 0xSSSSSYKKKBBB         
        /// </summary>
        /// <param name="bytes"></param>
        private void Init(byte[] bytes)
        {
            string hexstring = TextFormatHelper.BytesToHexString(bytes);
            this.SerialNumber = int.Parse(hexstring.Substring(0, 5), NumberStyles.HexNumber);
            this.Years = int.Parse(hexstring.Substring(5, 1), NumberStyles.HexNumber);
            this.MaxKiosks = int.Parse(hexstring.Substring(6, 3), NumberStyles.HexNumber);
            this.MaxBranchLocations = int.Parse(hexstring.Substring(9, 3), NumberStyles.HexNumber);
        }

        /// <summary>
        /// Produce binary representation 0xSSSSSYKKKBBB         
        /// </summary>
        /// <returns></returns>
        public byte[] ToByteArray()
        {
            string hexstring = string.Format("{0:x5}{1:x1}{2:x3}{3:x3}", this.SerialNumber, this.Years, this.MaxKiosks, this.MaxBranchLocations);
            return TextFormatHelper.HexStringToBytes(hexstring);
        }

        /// <summary>
        /// Generate License Key from license serial number and features.
        /// </summary>
        /// <param name="license"></param>
        /// <returns></returns>
        public static string CreateLicenseKeyString(License license)
        {
            byte[] bytes = license.ToByteArray();
            byte[] encrypted = LicenseUtil.Encrypt(bytes, LicenseUtil.DESKey1.Value);
                        
            // Eliminate padding with random letters in license key string. Add more bytes to byte array.
            // For 15 letters, 9 bytes required.
            byte[] padded = new byte[9];
            Array.Copy(encrypted, padded, 8);
            padded[8] = padded[0];  // allows additional check

            //string licenseString = TextFormatHelper.PackString(encrypted);            
            string licenseString = TextFormatHelper.PackString(padded);            
            licenseString = TextFormatHelper.FormatWithGroups(licenseString, 5, true, false);

            return licenseString;
        }

        /// <summary>
        /// Check that license key string is actually corresponds to the license.
        /// </summary>
        /// <param name="features"></param>
        /// <param name="licenseKey"></param>
        /// <returns></returns>
        public static bool ValidateLicenseKey(License features, string licenseKey)
        {
            License lic = new License(licenseKey);
            return (features.SerialNumber == lic.SerialNumber)
                && (features.Years == lic.Years)
                && (features.MaxKiosks == lic.MaxKiosks)
                && (features.MaxBranchLocations == lic.MaxBranchLocations);
        }

    }
}

