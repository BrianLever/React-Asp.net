using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

using FrontDesk.Common.Data;

using ScreenDox.Server.Common.Models;

namespace ScreenDox.Server.Common.Data
{
    public class KioskDatabase : DBDatabase, IKioskRepository
    {
        #region constructor

        public KioskDatabase(string connectionString)
            : base(connectionString) { }

        public KioskDatabase()
            : base(0)
        { }

        public KioskDatabase(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region GET

        /// <summary>
        /// Get FrontDesk Kiosk by ID
        /// </summary>
        public Kiosk GetByID(Int16 kioskID)
        {
            Kiosk kiosk = null;

            string sql = @"
SELECT 
    KioskID, 
    KioskName, 
    Description, 
    CreatedDate ,
    LastActivityDate, 
    BranchLocationID, 
    Disabled,
    IpAddress,
    KioskAppVersion,
    ScreeningProfileID,
    ScreeningProfileName,
    SecretKey
FROM dbo.vKiosk
WHERE KioskID = @KioskID";

            CommandObject.Parameters.Clear();
            AddParameter("@KioskID", DbType.Int16).Value = kioskID;
            try
            {
                base.Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        kiosk = KioskFactory.Create(reader);
                    }
                }
            }
            finally
            {
                base.Disconnect();
            }

            return kiosk;
        }

        public DateTimeOffset? GetLastActivityDate(short kioskID)
        {
            var sql = "select LastActivityDate from dbo.Kiosk where KioskID = @KioskID";

            ClearParameters();
            AddParameter("@KioskID", DbType.Int16).Value = kioskID;

            try
            {
                return RunScalarQuery<DateTimeOffset>(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Get All FrontDesk Kiosks
        /// </summary>
        public DataSet GetAll(bool showDisabled = true)
        {
            DataSet ds = null;
            var sql = new QueryBuilder(@"SELECT * FROM dbo.vKiosk k");

            if (!showDisabled)
            {
                sql.AppendWhereCondition("k.Disabled = 0", ClauseType.And);
            }
            try
            {
                ds = GetDataSet(sql.ToString());
            }
            finally
            {
                Disconnect();
            }
            return ds;
        }

        public List<Kiosk> GetAll(int? kioskID,
                                  string name,
                                  int? branchLocationId,
                                  int? screeningProfileId,
                                  bool showDisabled,
                                  int? userID,
                                  int startRowIndex,
                                  int maximumRows,
                                  string orderBy)
        {
            var result = new List<Kiosk>();

            orderBy = string.IsNullOrWhiteSpace(orderBy) ? "KIOSKID ASC" : orderBy.ToUpperInvariant();

            if (orderBy.Contains("KIOSKID"))
            {
                orderBy = orderBy.Replace("KIOSKID", "v.KioskID");
            }

            //query to select kiosk
            string sql = String.Format(@"SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
v.KioskID
FROM dbo.vKiosk v
", orderBy);


            //query to select kiosk with filter by name  and ID
            ClearParameters();

            QueryBuilder sqlBuilder = new QueryBuilder(sql);

            //parameters for filtering
            if (kioskID.HasValue && kioskID > 0)
            {
                sqlBuilder.AppendWhereCondition("v.KioskID = @KioskID", ClauseType.And);
                AddParameter("@KioskID", DbType.Int16).Value = kioskID;
            }
            if (!String.IsNullOrEmpty(name))
            {
                sqlBuilder.AppendWhereCondition("v.KioskName LIKE @KioskName", ClauseType.And);
                AddParameter("@KioskName", DbType.String, 255).Value = SqlLikeStringPrepeare(name, LikeCondition.Contains);
            }
            if (branchLocationId.HasValue && branchLocationId > 0)
            {
                sqlBuilder.AppendWhereCondition("v.BranchLocationID = @BranchLocationID", ClauseType.And);
                AddParameter("@BranchLocationID", DbType.Int32).Value = SqlParameterSafe(branchLocationId);
            }

            if (screeningProfileId.HasValue && screeningProfileId > 0)
            {
                sqlBuilder.AppendWhereCondition("v.ScreeningProfileID = @ScreeningProfileID", ClauseType.And);
                AddParameter("@ScreeningProfileID", DbType.Int32).Value = SqlParameterSafe(screeningProfileId);
            }

            if (!showDisabled)
            {
                sqlBuilder.AppendWhereCondition("v.Disabled = 0", ClauseType.And);
            }


            if (userID.HasValue && userID > 0)
            {
                sqlBuilder.AppendJoinStatement(" INNER JOIN dbo.Users_BranchLocation ul ON ul.BranchLocationID = v.BranchLocationID ");
                sqlBuilder.AppendWhereCondition("ul.UserID = @UserID", ClauseType.And);
                AddParameter("@UserID", DbType.Int32).Value = userID;
            }

            //query to select branch location with pagination
            string sqlFormat = String.Format(@"
WITH OrderedKiosk(RowNumber, KioskID)
AS (
    {0}
)
SELECT 
    v.KioskID, 
    v.KioskName, 
    v.Description, 
    v.CreatedDate , 
    v.LastActivityDate, 
    v.BranchLocationID, 
    v.Disabled, 
    v.BranchLocationName,
    v.IpAddress,
    v.KioskAppVersion,
    v.ScreeningProfileID,
    v.ScreeningProfileName
FROM OrderedKiosk o 
INNER JOIN dbo.vKiosk v ON o.KioskID = v.KioskID 
", sqlBuilder.ToString());

            //create query to select kiosk with filter by branch location
            sqlBuilder = new QueryBuilder(sqlFormat);
            //parameters for filtering by branch location name
            sqlBuilder.AppendWhereCondition("RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)", ClauseType.And);
            sqlBuilder.AppendOrderCondition(orderBy);
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            try
            {
                Connect();

                using (var reader = RunSelectQuery(sqlBuilder.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(KioskFactory.Create(reader));
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }


        #region ObjectDataSource methods with pagination and sorting

        public DataSet GetAllWithFiltering(int? kioskID, string name, int? branchLocationId, int? screeningProfileId, bool showDisabled,
            int? userID,
            int startRowIndex, int maximumRows, string orderBy)
        {
            DataSet ds = null;

            orderBy = string.IsNullOrWhiteSpace(orderBy) ? "KioskID ASC" : orderBy;

            if (orderBy.Contains("KioskID"))
            {
                orderBy = orderBy.Replace("KioskID", "v.KioskID");
            }

            //query to select kiosk
            string sql = String.Format(@"SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
v.KioskID
FROM dbo.vKiosk v
", orderBy);


            //query to select kiosk with filter by name  and ID
            ClearParameters();

            QueryBuilder sqlBuilder = new QueryBuilder(sql);

            //parameters for filtering
            if (kioskID.HasValue && kioskID > 0)
            {
                sqlBuilder.AppendWhereCondition("v.KioskID = @KioskID", ClauseType.And);
                AddParameter("@KioskID", DbType.Int16).Value = kioskID;
            }
            if (!String.IsNullOrEmpty(name))
            {
                sqlBuilder.AppendWhereCondition("v.KioskName LIKE @KioskName", ClauseType.And);
                AddParameter("@KioskName", DbType.String, 255).Value = SqlLikeStringPrepeare(name, LikeCondition.Contains);
            }
            if (branchLocationId.HasValue && branchLocationId > 0)
            {
                sqlBuilder.AppendWhereCondition("v.BranchLocationID = @BranchLocationID", ClauseType.And);
                AddParameter("@BranchLocationID", DbType.Int32).Value = SqlParameterSafe(branchLocationId);
            }

            if (screeningProfileId.HasValue && screeningProfileId > 0)
            {
                sqlBuilder.AppendWhereCondition("v.ScreeningProfileID = @ScreeningProfileID", ClauseType.And);
                AddParameter("@ScreeningProfileID", DbType.Int32).Value = SqlParameterSafe(screeningProfileId);
            }

            if (!showDisabled)
            {
                sqlBuilder.AppendWhereCondition("v.Disabled = 0", ClauseType.And);
            }


            if (userID.HasValue && userID > 0)
            {
                sqlBuilder.AppendJoinStatement(" INNER JOIN dbo.Users_BranchLocation ul ON ul.BranchLocationID = v.BranchLocationID ");
                sqlBuilder.AppendWhereCondition("ul.UserID = @UserID", ClauseType.And);
                AddParameter("@UserID", DbType.Int32).Value = userID;
            }

            //query to select branch location with pagination
            string sqlFormat = String.Format(@"
WITH OrderedKiosk(RowNumber, KioskID)
AS (
    {0}
)
SELECT 
    v.KioskID, 
    v.KioskName, 
    v.Description, 
    v.CreatedDate , 
    v.LastActivityDate, 
    v.BranchLocationID, 
    v.Disabled, 
    v.BranchLocationName,
    v.IpAddress,
    v.KioskAppVersion,
    v.ScreeningProfileID,
    v.ScreeningProfileName
FROM OrderedKiosk o 
INNER JOIN dbo.vKiosk v ON o.KioskID = v.KioskID 
", sqlBuilder.ToString());

            //create query to select kiosk with filter by branch location
            sqlBuilder = new QueryBuilder(sqlFormat);
            //parameters for filtering by branch location name
            sqlBuilder.AppendWhereCondition("RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)", ClauseType.And);
            sqlBuilder.AppendOrderCondition(orderBy);
            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            try
            {
                base.Connect();
                ds = GetDataSet(sqlBuilder.ToString());

            }
            finally
            {
                base.Disconnect();
            }
            return ds;
        }

        /// <summary>
        /// Get kiosks count
        /// </summary>
        public int GetKioskCount()
        {
            int rowCount = 0;
            string sql = @"Select COUNT_BIG(*) from dbo.Kiosk k
INNER JOIN dbo.BranchLocation l ON k.BranchLocationID = l.BranchLocationID";

            try
            {
                base.Connect();
                rowCount = Convert.ToInt32(RunScalarQuery(sql));
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }
            return rowCount;
        }

        public int GetKioskCount(int? kioskID, string name, int? branchLocationId, int? screeningProfileId, bool showDisabled,
            int? userID)
        {
            int rowCount = 0;
            string sql = @"Select COUNT_BIG(*) from dbo.vKiosk k";

            QueryBuilder sqlBuilder = new QueryBuilder(sql);
            ClearParameters();

            //parameters for filtering
            if (kioskID.HasValue && kioskID > 0)
            {
                sqlBuilder.AppendWhereCondition("k.KioskID = @KioskID", ClauseType.And);
                AddParameter("@KioskID", DbType.Int16).Value = kioskID;
            }
            if (!String.IsNullOrEmpty(name))
            {
                sqlBuilder.AppendWhereCondition("k.KioskName LIKE @KioskName", ClauseType.And);
                AddParameter("@KioskName", DbType.String, 255).Value = SqlLikeStringPrepeare(name, LikeCondition.Contains);
            }
            if (branchLocationId.HasValue && branchLocationId > 0)
            {
                sqlBuilder.AppendWhereCondition("k.BranchLocationID = @BranchLocationID", ClauseType.And);
                AddParameter("@BranchLocationID", DbType.Int32).Value = SqlParameterSafe(branchLocationId);
            }

            if (screeningProfileId.HasValue && screeningProfileId > 0)
            {
                sqlBuilder.AppendWhereCondition("k.ScreeningProfileID = @ScreeningProfileID", ClauseType.And);
                AddParameter("@ScreeningProfileID", DbType.Int32).Value = SqlParameterSafe(screeningProfileId);
            }

            if (!showDisabled)
            {
                sqlBuilder.AppendWhereCondition("k.Disabled = 0", ClauseType.And);
            }


            if (userID.HasValue && userID > 0)
            {
                sqlBuilder.AppendJoinStatement(" INNER JOIN dbo.Users_BranchLocation ul ON ul.BranchLocationID = k.BranchLocationID ");
                sqlBuilder.AppendWhereCondition("ul.UserID = @UserID", ClauseType.And);
                AddParameter("@UserID", DbType.Int32).Value = userID;

            }
            try
            {
                base.Connect();
                rowCount = Convert.ToInt32(RunScalarQuery(sqlBuilder.ToString()));
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }
            return rowCount;
        }

        /// <summary>
        /// Get count of all not disabled kiosks 
        /// </summary>
        public int GetNotDisabledCount()
        {
            int count = 0;
            string sql = @"
SELECT COUNT_BIG(*)
FROM dbo.Kiosk k
WHERE Disabled = 'false' OR Disabled IS NULL
";
            try
            {
                count = Convert.ToInt32(RunScalarQuery(sql));
            }
            finally
            {
                this.Disconnect();
            }

            return count;
        }


        #endregion

        #endregion

        #region ADD/UPDATE/DELETE

        /// <summary>
        /// Add FrontDesk Kiosk to the system
        /// KioskID is automatically generated
        /// </summary>
        public Int16 Add(Kiosk kiosk)
        {
            string sql = @"
INSERT INTO dbo.Kiosk
(KioskName, Description, CreatedDate ,LastActivityDate, BranchLocationID, Disabled, SecretKey)
VALUES (@KioskName, @Description, @CreatedDate , @LastActivityDate, @BranchLocationID, @Disabled, @SecretKey);
SET @KioskID = SCOPE_IDENTITY();

-- update disabled status from branch location
UPDATE k
    SET k.Disabled = l.Disabled
FROM dbo.Kiosk k INNER JOIN dbo.BranchLocation l ON k.BranchLocationID = l.BranchLocationID
WHERE k.KioskID = @KioskID;
";

            try
            {
                base.Connect();

                BeginTransaction();

                CommandObject.Parameters.Clear();
                AddParameter("@KioskName", DbType.String, 255).Value = kiosk.Name;
                AddParameter("@Description", DbType.String).Value = kiosk.Description;
                AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = kiosk.CreatedDate;
                AddParameter("@LastActivityDate", DbType.DateTimeOffset).Value = SqlParameterSafe(kiosk.LastActivityDate);
                AddParameter("@BranchLocationID", DbType.Int32).Value = kiosk.BranchLocationID;
                AddParameter("@Disabled", DbType.Boolean).Value = SqlParameterSafe(kiosk.Disabled);
                AddParameter("@SecretKey", DbType.String, 64).Value = SqlParameterSafe(kiosk.SecretKey);
                var IdParam = AddParameter("@KioskID", DbType.Int16);
                IdParam.Direction = ParameterDirection.Output;

                // additional license check.
                // Do not throw exception, silently do not add kiosk.

                RunNonSelectQuery(sql);

                kiosk.KioskID = (short)(IdParam.Value);

                CommitTransaction();
            }
            catch (Exception ex)
            {
                RollbackTransaction();
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }
            return kiosk.KioskID;
        }

        /// <summary>
        /// True if name of kiosk already is used in system 
        /// </summary>
        public bool KioskNameIsAlreadyUsed(string kioskName)
        {
            int rowCount = 0;

            string sql = "SELECT Count(KioskName) FROM dbo.Kiosk WHERE KioskName = @KioskName";
            CommandObject.Parameters.Clear();
            AddParameter("@KioskName", DbType.String, 255).Value = kioskName;

            try
            {
                base.Connect();
                rowCount = Convert.ToInt32(RunScalarQuery(sql));
            }
            finally
            {
                base.Disconnect();
            }
            return rowCount > 0;
        }

        /// <summary>
        /// Delete FrontDesk kiosk
        /// </summary>
        public void Delete(short kioskID)
        {
            string checkInSql = @"
if exists(select null 
            from dbo.[ScreeningResult] s
            where s.KioskID=@KioskID)
    set @HasCheckIn=1
else
    set @HasCheckIn=0";

            string sql = @"DELETE FROM dbo.Kiosk WHERE KioskID = @KioskID";
            CommandObject.Parameters.Clear();
            AddParameter("@KioskID", DbType.Int16).Value = kioskID;

            try
            {
                Connect();
                BeginTransaction();

                CommandObject.Parameters.Clear();
                AddParameter("@KioskID", DbType.Int16).Value = kioskID;
                DbParameter hasCheckIns = AddParameter("@HasCheckIn", DbType.Boolean, ParameterDirection.Output);
                RunNonSelectQuery(checkInSql);

                if (!Convert.ToBoolean(hasCheckIns.Value))
                {
                    CommandObject.Parameters.Clear();
                    AddParameter("@KioskID", DbType.Int16).Value = kioskID;
                    RunNonSelectQuery(sql);
                    CommitTransaction();
                }
                else
                {
                    throw new ApplicationException("Unable to delete Kiosk because there are patient reports associated with it.");
                }
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Update FrontDesk Kiosk
        /// </summary>
        public void Update(Kiosk kiosk)
        {
            string sql = @"UPDATE dbo.Kiosk SET 
KioskName = @KioskName,
Description = @Description,
BranchLocationID = @BranchLocationID,
SecretKey = @SecretKey
WHERE KioskID = @KioskID";

            CommandObject.Parameters.Clear();
            AddParameter("@KioskName", DbType.String, 255).Value = kiosk.Name;
            AddParameter("@Description", DbType.String).Value = kiosk.Description;
            AddParameter("@BranchLocationID", DbType.Int32).Value = kiosk.BranchLocationID;
            AddParameter("@KioskID", DbType.Int16).Value = kiosk.KioskID;
            AddParameter("@SecretKey", DbType.String, 64).Value = SqlParameterSafe(kiosk.SecretKey);

            try
            {
                base.Connect();
                RunSelectQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }
        }

        /// <summary>
        /// Change last activity date of FrontDesk kiosk. Date and other properties are being updated regardless kiosk enabled or disabled
        /// </summary>
        /// <returns>True if kiosk enabled, and False when disabled</returns>
        public bool ChangeLastActivityDate(KioskPingMessage kioskPingMessage, DateTimeOffset lastActivityDate)
        {
            const string sql = @"dbo.uspChangeKioskLastActivityDate";

            ClearParameters();

            AddParameter("@KioskID", DbType.Int16).Value = kioskPingMessage.KioskID;
            AddParameter("@IpAddress", DbType.AnsiString).Value = SqlParameterSafe(kioskPingMessage.IpAddress);
            AddParameter("@KioskAppVersion", DbType.AnsiString).Value = SqlParameterSafe(kioskPingMessage.KioskAppVersion);
            AddParameter("@LastActivityDate", DbType.DateTimeOffset).Value = SqlParameterSafe(lastActivityDate);
            try
            {
                return RunProcedureScalarQuery<bool>(sql) ?? true;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Enabled/Disabled kiosk
        /// </summary>
        public void SetKioskEnabledStatus(Int16 kioskID, bool isDisabled)
        {
            string sql = @"
UPDATE k
SET Disabled = @Disabled
FROM dbo.Kiosk k INNER JOIN dbo.BranchLocation l ON k.BranchLocationID = l.BranchLocationID
WHERE KioskID = @KioskID AND (@Disabled = 1 OR l.Disabled = 0)";

            CommandObject.Parameters.Clear();
            AddParameter("@KioskID", DbType.Int32).Value = kioskID;
            AddParameter("@Disabled", DbType.Boolean).Value = SqlParameterSafe(isDisabled);

            try
            {
                base.Connect();
                RunNonSelectQuery(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }
        }

        #endregion

    }
}
