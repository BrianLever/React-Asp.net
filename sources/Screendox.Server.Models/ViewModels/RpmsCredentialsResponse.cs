using FrontDesk.Common.Extensions;

using RPMS.Common.Security;

using System.Runtime.Serialization;

namespace ScreenDox.Server.Models
{
    public class RpmsCredentialsResponse : RpmsCredentials
    {

        /// <summary>
        ///  Expiration date, formatted
        /// </summary>
        [DataMember]
        public string ExpireAtFormatted
        {
            get { return ExpireAt.FormatAsDate(); }
        }

        /// <summary>
        /// True is this is the active EHR(RPMS) credentials. There is only single active EHR credentials in the system.
        /// </summary>
        [DataMember]
        public bool IsActive { get; set; } = false;
    }
}
