using System;
using System.Runtime.Serialization;

namespace RPMS.Common.Security
{
    [DataContract]
    public class RpmsCredentials
    {
        [DataMember]
        public Guid Id { get; set; }
        [DataMember]
        public string AccessCode { get; set; }
        [DataMember]
        public string VerifyCode { get; set; }
        [DataMember]
        public DateTime ExpireAt { get; set; }


    }
}
