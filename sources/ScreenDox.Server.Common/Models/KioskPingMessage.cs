using System;
using System.Runtime.Serialization;

namespace ScreenDox.Server.Common.Models
{
    [DataContract]
    public class KioskPingMessage
    {
        [DataMember]
        public short KioskID { get; set; }
        [DataMember]
        public string SecretKey { get; set; }
        [DataMember]
        public string KioskAppVersion { get; set; }
        [DataMember]
        public string IpAddress { get; set; }
    }
}


namespace FrontDesk.Server.Services
{
    [Obsolete("Backward compatitiblity for Ping_v2 method")]
    public class KioskPingMessage
    {
        public short KioskID { get; set; }
        public string KioskAppVersion { get; set; }
        public string IpAddress { get; set; }
    }
}