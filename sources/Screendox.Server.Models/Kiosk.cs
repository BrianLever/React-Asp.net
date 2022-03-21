using FrontDesk.Common;
using System;

namespace ScreenDox.Server.Common.Models
{
    public class Kiosk 
    {
        public short KioskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset? LastActivityDate { get; set; }
        public int BranchLocationID { get; set; }
        public bool Disabled { get; set; }

        public string IpAddress { get; set; }
        public string KioskAppVersion { get; set; }

        /// <summary>
        /// 256 bit secret key
        /// </summary>
        public string SecretKey { get; set; }

        public string ScreeningProfileName { get; set; }


        // read only properties
        public string KioskKey
        {
            get
            {
                return GetKioskIDAsString(this.KioskID);
            }
        }

        /// <summary>
        /// Get translated Kiosk ID as XXXX alphanumeric string
        /// </summary>
        /// <param name="kioskID"></param>
        /// <returns></returns>
        public static string GetKioskIDAsString(short kioskID)
        {
            if (kioskID <= 0) return "N/A";
            string keyAlpha = TextFormatHelper.FormatWithGroupsInt16(kioskID);
            return keyAlpha;
        }

        public static short GetKioskIDFromString(string kioskKey)
        {
            if (string.IsNullOrWhiteSpace(kioskKey))
            {
                return 0;
            }

            return TextFormatHelper.UnpackStringInt16(kioskKey);
        }

        public static short? GetKioskIDFromStringNullable(string kioskKey)
        {
            if (string.IsNullOrWhiteSpace(kioskKey))
            {
                return null;
            }

            return TextFormatHelper.UnpackStringInt16(kioskKey);
        }

    }
}
