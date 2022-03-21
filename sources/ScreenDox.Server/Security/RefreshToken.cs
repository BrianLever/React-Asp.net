using FrontDesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Security
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int UserID { get; set; }

        public string Token { get; set; }
        /// <summary>
        /// UTC Time for expiration
        /// </summary>
        public DateTime Expires { get; set; }
        /// <summary>
        /// UTC Time for creation
        /// </summary>
        public DateTime Created { get; set; }
        public string CreatedByIp { get; set; }
        /// <summary>
        /// UTC time when revoked
        /// </summary>
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }

        public string ReplacedByToken { get; set; }
        public string ReasonRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expires;
        public bool IsRevoked => Revoked != null;
        public bool IsActive => !IsRevoked && !IsExpired;


        public override string ToString()
        {
            return $"Token: {Token}. Created (UTC): {Created.FormatAsDateWithTime()}. Expires (UTC): {Expires.FormatAsDateWithTime()}. User: {UserID}. Is Active: {IsActive}";
        }

    }
}
