using FrontDesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Security
{
    public class RefreshTokenFactory
    {
        public static RefreshToken Create(IDataReader reader)
        {
            return new RefreshToken
            {
                Id = reader["Id"].As<Guid>(),
                UserID = reader["UserID"].As<int>(),
                Token = reader["Token"].As<string>(),
                Created = reader["Created"].As<DateTime>(),
                Expires = reader["Expires"].As<DateTime>(),
                CreatedByIp = reader["CreatedByIp"].As<string>(),
                Revoked = reader["Revoked"].AsNullable<DateTime>(),
                RevokedByIp = reader["RevokedByIp"].As<string>(),
                ReplacedByToken = reader["ReplacedByToken"].As<string>(),
                ReasonRevoked = reader["ReasonRevoked"].As<string>()
            };
        }
    }
}
