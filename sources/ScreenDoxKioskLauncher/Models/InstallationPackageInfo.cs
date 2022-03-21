using System;
using System.Runtime.Serialization;

namespace ScreenDoxKioskLauncher.Models
{
    [DataContract]
    public class InstallationPackageInfo
    {
        [DataMember]
        public Version Version { get; set; }
        [DataMember]
        public DateTime InstallOn { get; set; }

        /// <summary>
        /// Creates a copy of the current object
        /// </summary>
        /// <returns></returns>
        public InstallationPackageInfo Clone()
        {
            return new InstallationPackageInfo
            {
                Version = this.Version,
                InstallOn = this.InstallOn
            };
        }
    }
}