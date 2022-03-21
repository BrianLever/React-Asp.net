using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models
{
    public class RpmsCredentialsRequest
    {
        /// <summary>
        /// Access Code
        /// </summary>
        public string AccessCode { get; set; }
        /// <summary>
        /// Verify Code
        /// </summary>
        public string VerifyCode { get; set; }
        /// <summary>
        ///  Expiration date
        /// </summary>
        public DateTime? ExpireAt { get; set; }
    }
}
