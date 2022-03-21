using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

namespace FrontDesk.Deployment
{
    public class OSInfo
    {
        public string Caption { get; set; }
        public PlatformArchitecture Platform { get; set; }
        public ProductVersion Version { get; set; }
        public ProductVersion ServicePack { get; set; }

        public OSInfo()
        {
            Version = new ProductVersion();
            ServicePack = new ProductVersion();
        }


        #region OS Info

        /// <summary>
        /// Get Operating System information
        /// </summary>
        /// <returns></returns>
        public static OSInfo GetOsInfo()
        {
            OSInfo osInfo = new OSInfo();
            ManagementObjectSearcher objMOS = new ManagementObjectSearcher("SELECT * FROM  Win32_OperatingSystem");

            foreach (ManagementObject objManagement in objMOS.Get())
            {
                // Get OS version from WMI - This also gives us the edition
                osInfo.Caption = Convert.ToString(objManagement.GetPropertyValue("Caption"));
                if (!string.IsNullOrEmpty(osInfo.Caption))
                {
                    osInfo.Caption = osInfo.Caption.Trim();

                    UInt16 spMajorVersion = 0;
                    UInt16 spMinorVersion = 0;
                    //get sp version
                    object spVer = objManagement.GetPropertyValue("ServicePackMajorVersion");
                    if (spVer != null)
                    { 
                        //missing WMI info
                        spMajorVersion = Convert.ToUInt16(spVer);

                        spVer = objManagement.GetPropertyValue("ServicePackMinorVersion");
                        if (spVer != null) spMinorVersion = Convert.ToUInt16(spVer);

                        osInfo.ServicePack.VersionAsString = string.Format("{0}.{1}", spMajorVersion, spMinorVersion);

                    }
                    else
                    {
                        osInfo.ServicePack = GetOSServicePackLegacy();
                    }

                    //from WMI there is not information how distinguish AMD64 and IA64,
                    // we better us System parameter
                    osInfo.Platform = GetOSArchitectureLegacy();

                    //object osA = null;
                    //try
                    //{
                    //    int osSKU = 0;
                    //    object objSKU = objManagement.GetPropertyValue("OperatingSystemSKU");
                    //    if (objSKU != null)
                    //        osSKU = Convert.ToInt32(objSKU);
                    //    //detect itanium
                    //    if (osSKU == 15) //might be itanium
                    //        osInfo.Platform = GetOSArchitectureLegacy();

                    //    else
                    //    {
                    //        // Get OS architecture from WMI
                    //        osA = objManagement.GetPropertyValue("OSArchitecture");
                    //        if (osA != null)
                    //        {


                    //            string osAString = osA.ToString();
                    //            // If "64" is anywhere in there, it's a 64-bit architectore.
                    //            osInfo.Platform = (osAString.Contains("64") ? PlatformArchitecture.x64 : PlatformArchitecture.x86);


                    //        }
                    //        else
                    //        {
                    //            osInfo.Platform = GetOSArchitectureLegacy();
                    //        }
                    //    }
                    //}
                    //catch (Exception)
                    //{
                    //}
                }
                else
                {
                    osInfo = GetOSLegacy();
                }

            }

            if (osInfo.Version.IsEmpty)
            { //try again
                osInfo = GetOSLegacy();
            }
            return osInfo;
        }

        /// <summary>
        /// Gets Operating System info using .Net's Environment class.
        /// </summary>
        private static OSInfo GetOSLegacy()
        {
            var osInfo = new OSInfo();
            var os = Environment.OSVersion;
            osInfo.Version.VersionAsString = os.Version.ToString();
            osInfo.Caption = GetOSCaptionLegacy();

            osInfo.Platform = GetOSArchitectureLegacy();
            osInfo.ServicePack = GetOSServicePackLegacy();
            return osInfo;
        }

        private static string GetOSCaptionLegacy()
        {
            //Get Operating system information.
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "Windows ";

            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                        {
                            operatingSystem = "2000";
                        }
                        else
                        {
                            operatingSystem = "XP";
                        }
                        break;
                    case 6:
                        if (vs.Minor == 0)
                        {
                            operatingSystem = "Vista";
                        }
                        else
                        {
                            operatingSystem = "7";
                        }
                        break;
                    default:
                        break;
                }
            }
            //Make sure we actually got something in our OS check
            //We don't want to just return " Service Pack 2"
            //That information is useless without the OS version.
            if (operatingSystem != "")
            {
                //Got something.  Let's see if there's a service pack installed.
                operatingSystem += GetOSServicePackLegacy();
            }
            //Return the information we've gathered.
            return operatingSystem;
        }

        /// <summary>
        /// Gets Operating System Architecture.  This does not tell you if the program in running in
        /// 32- or 64-bit mode or if the CPU is 64-bit capable.  It tells you whether the actual Operating
        /// System is 32- or 64-bit.
        /// </summary>
        private static PlatformArchitecture GetOSArchitectureLegacy()
        {
            PlatformArchitecture platform = PlatformArchitecture.x86;
            string pa = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            if (!String.IsNullOrEmpty(pa))
            {
                pa = pa.ToUpperInvariant();

                if (pa.StartsWith("X86")) platform = PlatformArchitecture.x86;
                else if (pa.StartsWith("IA64")) platform = PlatformArchitecture.IA64;
                else platform = PlatformArchitecture.x64; //AMD64

            }

            return platform;
        }


        /// <summary>
        /// Gets the installed Operating System Service Pack using .Net's Environment class.
        /// </summary>
        /// <returns>String containing the operating system's installed service pack (if any)</returns>
        private static ProductVersion GetOSServicePackLegacy()
        {
            var spVer = new ProductVersion();

            // Get service pack from Environment Class
            string sp = Environment.OSVersion.ServicePack;
            if (!string.IsNullOrEmpty(sp))
            {
                sp = sp.Replace("Service Pack", "");
                sp.Trim();

                try
                {
                    spVer.VersionAsString = sp;
                }
                catch (Exception) { }
            }
            // No service pack.  Return an empty string
            return spVer;
        }

        #endregion
    }

    public enum PlatformArchitecture
    {
        x86,
        x64,
        IA64
    }
}
