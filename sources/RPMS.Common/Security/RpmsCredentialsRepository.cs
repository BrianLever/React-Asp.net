using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using FrontDesk.Common.Data;

namespace RPMS.Common.Security
{
    public class RpmsCredentialsRepository : DBDatabase, IRpmsCredentialsRepository
    {
        public RpmsCredentialsRepository() : base()
        {
            this.ConnectionString = ConfigurationManager.ConnectionStrings["LocalConnection"].ConnectionString;

        }

        public RpmsCredentialsRepository(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }


        public virtual List<RpmsCredentials> GetAll()
        {
            var resut = new List<RpmsCredentials>();
            const string sql =
                 @"SELECT Id, AccessCode, VerifyCode, ExpireAt FROM dbo.[RpmsCredentials] 
ORDER BY ExpireAt DESC ";

            CommandObject.Parameters.Clear();

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        resut.Add(new RpmsCredentials
                        {
                            Id = reader.GetGuid(0),
                            AccessCode = reader.GetString(1),
                            VerifyCode = reader.GetString(2),
                            ExpireAt = reader.GetDateTime(3)
                        });
                    }

                }
            }
            finally
            {
                Disconnect();
            }

            return resut;
        }

        public virtual RpmsCredentials Get()
        {
            return GetAll().FirstOrDefault();
        }

        public virtual void Add(RpmsCredentials openTextCredentials)
        {
            const string sql =
                @"INSERT INTO dbo.[RpmsCredentials](Id, AccessCode, VerifyCode, ExpireAt) VALUES(@Id, @AccessCode, @VerifyCode, @ExpireAt)";

            CommandObject.Parameters.Clear();

            AddParameter("@Id", DbType.Guid).Value = openTextCredentials.Id;
            AddParameter("@AccessCode", DbType.String).Value = SqlParameterSafe(openTextCredentials.AccessCode);
            AddParameter("@VerifyCode", DbType.String).Value = SqlParameterSafe(openTextCredentials.VerifyCode); ;
            AddParameter("@ExpireAt", DbType.DateTime).Value = openTextCredentials.ExpireAt;

            try
            {
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public virtual bool Delete(Guid id)
        {
            const string sql =
               @"DELETE FROM dbo.[RpmsCredentials] WHERE Id = @Id";

            CommandObject.Parameters.Clear();

            AddParameter("@Id", DbType.Guid).Value = id;

            try
            {
                return RunNonSelectQuery(sql) > 0;
            }
            finally
            {
                Disconnect();
            }
        }
    }
}
