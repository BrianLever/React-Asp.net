using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;

namespace RPMS.Common.Security
{
    public interface ICryptographyService
    {
        string Encrypt(string openTextString);
        string Decrypt(string encryptedString);

    }

    public class CryptographyService : ICryptographyService
    {
        private readonly byte[] _machine3DesKey;

        public CryptographyService()
        {
            var section = (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
            try
            {
                _machine3DesKey =
                    section.DecryptionKey.ToCharArray()
                        .Select(b => Byte.Parse(b.ToString(CultureInfo.InvariantCulture), NumberStyles.HexNumber))
                        .ToArray();
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException("Required system.web/machineKey not found or has wrong format.", ex);
            }
        }

        public string Encrypt(string openTextString)
        {
            return MachineKey.Encode(Encoding.UTF8.GetBytes(openTextString), MachineKeyProtection.Encryption);
            //return Encoding.UTF8.GetString(MachineKey.Protect(Encoding.UTF8.GetBytes(openTextString), "Encryption"));
        }

        public string Decrypt(string encryptedString)
        {
            if (string.IsNullOrEmpty(encryptedString))
            {
                return encryptedString;
            }

            return Encoding.UTF8.GetString(MachineKey.Decode(encryptedString, MachineKeyProtection.Encryption));
            //return Encoding.UTF8.GetString(MachineKey.Unprotect(Encoding.UTF8.GetBytes(encryptedString), "Encryption"));
        }
    }
}
