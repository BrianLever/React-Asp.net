using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDoxKioskLauncher.Models
{

    /// <summary>
    /// Application state model
    /// </summary>
    [DataContract]
    public class ApplicationState
    {
        [DataMember]
        public string Version { get; set; }

        public Version GetVersion()
        {
            return string.IsNullOrEmpty(Version) ? new Version() : new Version(Version);
        }
    }
}
