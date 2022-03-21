using System;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;

namespace FrontDesk.Server.Data
{
    [Obsolete("moved to userPrincipalService")]
    internal class FDMembershipUserDb : DBDatabase
    {
        #region Constructors

        internal FDMembershipUserDb() : base(0) { }

        internal FDMembershipUserDb(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }

        #endregion


        /// <summary>
        /// Get list of users, suitable to display at webpage
        /// </summary>
        /// <param name="filterByLocation"></param>
        /// <param name="branchLocationId"></param>
        /// <returns></returns>
        public DataSet GetListWithBranchLocation(int? branchLocationId)
        {
            var sql = new QueryBuilder(@"
SELECT
    u.PKID as UserID
    , u.Username
    , u.Email
    , ud.FirstName
    , ud.LastName
    , ud.ContactPhone
    , bl.Name as BranchLocationName
FROM dbo.Users u
LEFT JOIN dbo.UserDetails ud ON u.PKID = ud.UserID
LEFT JOIN dbo.Users_BranchLocation ubl ON u.PKID = ubl.UserID
LEFT JOIN dbo.BranchLocation bl ON ubl.BranchLocationID = bl.BranchLocationID
");

            sql.AppendOrderCondition("ud.FirstName ASC, ud.LastName ASC");
            CommandObject.Parameters.Clear();
            if (branchLocationId.HasValue)
            {
                sql.AppendWhereCondition("bl.BranchLocationID = @BranchLocationID", ClauseType.And);
                AddParameter("@BranchLocationID", DbType.Int32).Value = branchLocationId.Value;
            }

            try
            {
                this.Connect();

                DataSet ds = this.GetDataSet(sql.ToString());
                return ds;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }
        }
    }
}
