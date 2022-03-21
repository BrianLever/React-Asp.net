namespace ScreenDox.Server.Data
{
    using FrontDesk.Common.Data;
    using FrontDesk.Server.Logging;
    using FrontDesk.Server.Membership;

    using ScreenDox.Server.Models;
    using ScreenDox.Server.Models.Security;
    using ScreenDox.Server.Security;

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;

    public interface IUserRefreshTokenRepository
    {
        bool AddRefreshToken(RefreshToken token);
        RefreshToken GetToken(string refreshToken);
        bool RevokeUserTokens(int userID, DateTime revokeDate, string ipAddress, string reason);
        bool Revoke(RefreshToken token);
    }

    public class UserRefreshTokenDb : DBDatabase, IUserRefreshTokenRepository
    {
        #region constructor

        public UserRefreshTokenDb(string connectionString)
            : base(connectionString) { }

        public UserRefreshTokenDb()
            : base(ConfigurationManager.ConnectionStrings[0].ConnectionString)
        { }

        public UserRefreshTokenDb(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion


        /// <summary>
        /// Get user details by user ID
        /// </summary>
        public bool AddRefreshToken(RefreshToken token)
        {

            string sql = @"dbo.uspAddUsersRefressToken";

            ClearParameters();
            AddParameter("@Id", DbType.Guid).Value = token.Id;
            AddParameter("@UserID", DbType.Int32).Value = token.UserID;
            AddParameter("@Token", DbType.String, 64).Value = token.Token;
            AddParameter("@Created", DbType.DateTime).Value = token.Created;
            AddParameter("@Expires", DbType.DateTime).Value = token.Expires;
            AddParameter("@CreatedByIp", DbType.AnsiString).Value = SqlParameterSafe(token.CreatedByIp);
            AddParameter("@Revoked", DbType.DateTime).Value = SqlParameterSafe(token.Revoked);
            AddParameter("@RevokedByIp", DbType.AnsiString).Value = SqlParameterSafe(token.RevokedByIp);
            AddParameter("@ReplacedByToken", DbType.String, 64).Value = SqlParameterSafe(token.ReplacedByToken);
            AddParameter("@ReasonRevoked", DbType.String, 128).Value = SqlParameterSafe(token.ReasonRevoked);

            try
            {
                Connect();
                return RunProcedureNonSelectQuery(sql) > 0;


            }
            finally
            {
                Disconnect();
            }

        }

        public RefreshToken GetToken(string refreshToken)
        {
            RefreshToken result = null;
            string sql = @"dbo.uspGetUsersRefressToken";

            ClearParameters();
            AddParameter("@Token", DbType.String, 64).Value = refreshToken;

            try
            {
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        result = RefreshTokenFactory.Create(reader);
                    }
                }

                return result;
            }
            finally
            {
                Disconnect();
            }
        }

        public bool RevokeUserTokens(int userID, DateTime revokedDate, string ipAddress, string reason)
        {
            string sql = @"dbo.uspRevokeAllUsersRefressToken";

            ClearParameters();
          
            AddParameter("@UserID", DbType.Int32).Value = userID;
            AddParameter("@Revoked", DbType.DateTime).Value = SqlParameterSafe(revokedDate);
            AddParameter("@RevokedByIp", DbType.AnsiString).Value = SqlParameterSafe(ipAddress);
            AddParameter("@ReplacedByToken", DbType.String, 64).Value = SqlParameterSafe(null);
            AddParameter("@ReasonRevoked", DbType.String, 128).Value = SqlParameterSafe(reason);

            try
            {
                Connect();
                return RunProcedureNonSelectQuery(sql) > 0;
            }
            finally
            {
                Disconnect();
            }
        }

        public bool Revoke(RefreshToken token)
        {
            string sql = @"dbo.uspRevokeUsersRefressToken";

            ClearParameters();
            AddParameter("@Id", DbType.Guid).Value = token.Id;
          
            AddParameter("@Revoked", DbType.DateTime).Value = SqlParameterSafe(token.Revoked);
            AddParameter("@RevokedByIp", DbType.AnsiString).Value = SqlParameterSafe(token.RevokedByIp);
            AddParameter("@ReplacedByToken", DbType.String, 64).Value = SqlParameterSafe(token.ReplacedByToken);
            AddParameter("@ReasonRevoked", DbType.String, 128).Value = SqlParameterSafe(token.ReasonRevoked);

            try
            {
                Connect();
                return RunProcedureNonSelectQuery(sql) > 0;


            }
            finally
            {
                Disconnect();
            }
        }
    }
}

