using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ScreenDoxKioskInstallApi.Models
{
    public class KioskInstallationInfo
    {
        public string Key { get; set; }
        public DateTime InstallOn { get; set; }
    }


 
    [DataContract]
    public class InstallationPackageInfo
    {
        [DataMember]
        public string Version { get; set; }

        [DataMember]
        public DateTime InstallOn { get; set; }
        [IgnoreDataMember]
        public List<KioskInstallationInfo> Kiosks = new List<KioskInstallationInfo>();

    }
}