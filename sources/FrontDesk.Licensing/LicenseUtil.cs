using Common.Logging;

using FrontDesk.Common;

using Microsoft.Win32;

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

using ScreenDox.Licensing;

using System;
using System.Management;
using System.Text;

namespace FrontDesk.Licensing
{
    public class LicenseUtil
    {
        // DES keys for BounceCastle, 24 or 16 bytes
        // use string instead of byte array for better obfuscation
        public static Lazy<string> DESKey1 = new Lazy<string>(() =>
        {
            var key = LicenseSecretsConfig.GetConfiguration().LicenseEncryptionKey;
            if (_logger.IsDebugEnabled)
            {
                _logger.DebugFormat("License: License Key: {0}", DESKey1);
            }
            return key;
        });

        public static Lazy<string> DESKey2 = new Lazy<string>(() =>
        {
            var key = LicenseSecretsConfig.GetConfiguration().ActivationRequestEncryptionKey;
            if (_logger.IsDebugEnabled)
            {
                _logger.DebugFormat("License: Activation Request Key: {0}", DESKey2);

            }
            return key;
        });

        private static ILog _logger = LogManager.GetLogger<LicenseUtil>();


        private static byte[] Encrypt(byte[] source, byte[] cryptKey)
        {
            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new DesEdeEngine()));
            KeyParameter keyParam = new KeyParameter(cryptKey);
            cipher.Init(true, keyParam);

            byte[] encrypted = new byte[cipher.GetOutputSize(source.Length)];

            int oLen = cipher.ProcessBytes(source, encrypted, 0);
            cipher.DoFinal(encrypted, oLen);

            return encrypted;
        }

        private static byte[] Decrypt(byte[] source, byte[] cryptKey)
        {
            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new CbcBlockCipher(new DesEdeEngine()));
            KeyParameter keyParam = new KeyParameter(cryptKey);
            cipher.Init(false, keyParam);

            byte[] decrypted = new byte[cipher.GetOutputSize(source.Length)];

            int oLen = cipher.ProcessBytes(source, decrypted, 0);
            cipher.DoFinal(decrypted, oLen);

            return decrypted;
        }

        /// <summary>
        /// Encrypt string
        /// </summary>
        /// <param name="source"></param>
        /// <param name="desKey"></param>
        /// <returns></returns>
        public static byte[] Encrypt(string source, Encoding encoding, string desKey)
        {
            //byte[] encrypted = Encrypt(Encoding.ASCII.GetBytes(source), TextFormatHelper.HexStringToBytes(desKey));
            byte[] encrypted = Encrypt(encoding.GetBytes(source), TextFormatHelper.HexStringToBytes(desKey));
            return encrypted;
        }

        /// <summary>
        /// Decrypt ASCII string
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="desKey"></param>
        /// <returns>ASCII string</returns>
        public static string Decrypt(byte[] bytes, Encoding encoding, string desKey)
        {
            byte[] decrypted = Decrypt(bytes, TextFormatHelper.HexStringToBytes(desKey));
            //return Encoding.ASCII.GetString(decrypted);
            return encoding.GetString(decrypted);
        }

        public static byte[] Encrypt(byte[] bytes, string desKey)
        {
            byte[] encrypted = Encrypt(bytes, TextFormatHelper.HexStringToBytes(desKey));
            return encrypted;
        }

        public static byte[] Decrypt(byte[] bytes, string desKey)
        {
            byte[] decrypted = Decrypt(bytes, TextFormatHelper.HexStringToBytes(desKey));
            return decrypted;
        }

        public static string GetWindowsProductID_WMI()
        {
            // http://msdn.microsoft.com/en-us/library/aa394239%28VS.85%29.aspx
            ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            ManagementObjectCollection.ManagementObjectEnumerator en = moc.GetEnumerator();
            en.MoveNext();

            //string sn = mc.GetPropertyValue("SerialNumber").ToString();
            string sn = en.Current.GetPropertyValue("SerialNumber").ToString();

            //#if DEBUG
            //            //TODO: REMOVE THIS STRING ON DEPLOYMENT 
            //            sn = "55041-049-9253961-86962";
            //#endif
            return sn;
        }

        public static string GetWindowsProductID()
        {
            RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");
            string productID = rk.GetValue("ProductID").ToString();
            return productID;
        }
    }
}
