using System;
using System.Runtime.Serialization;
using System.Text;

namespace RPMS.Common.Security
{
    [DataContract]
    public class BasicAuthCredentials
    {
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Password { get; set; }


        public string ToBase64()
        {
            return Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(Username + ":" + Password));
        }
    }
}
