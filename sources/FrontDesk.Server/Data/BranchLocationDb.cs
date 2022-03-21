using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

namespace FrontDesk.Server.Data
{
    public class BranchLocationDb : DBDatabase, IBranchLocationDb
    {
        #region Constructors

        // TODO: 1st connection string is used. Any other way to specify connection string, with parameterless constructor?
        public BranchLocationDb() : base(0) { }

        public BranchLocationDb(DbConnection sharedConnection) : base(sharedConnection)
        {
        }

        #endregion

        #region GET

        public BranchLocation Get(int id)
        {
            string sql = @"dbo.[uspGetBranchLocation]";
            BranchLocation branchLocation = null;

            ClearParameters();
            this.AddParameter("@BranchLocationID", DbType.Int32).Value = id;

            try
            {
                using (IDataReader reader = this.RunProcedureSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        branchLocation = new BranchLocation(reader.GetInt32(0))
                        {
                            //BranchLocationID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Disabled = reader.IsDBNull(3) ? false : reader.GetBoolean(3),
                            ScreeningProfileID = reader.Get<int>("ScreeningProfileID"),
                            ScreeningProfileName = reader.Get<string>("ScreeningProfileName"),
                        };
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }

            return branchLocation;
        }

        public List<BranchLocation> GetAll()
        {
            string sql = @"[dbo].[uspGetAllBranchLocations]";
            List<BranchLocation> branchLocations = new List<BranchLocation>();

            try
            {
                this.Connect();

                using (IDataReader reader = this.RunProcedureSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        branchLocations.Add(new BranchLocation(reader.GetInt32(0))
                        {
                            Name = reader.GetString(1),
                            Description = reader.IsDBNull(2) ? null : reader.GetString(2),
                            Disabled = reader.IsDBNull(3) ? false : reader.GetBoolean(3),
                            ScreeningProfileID = reader.Get<int>("ScreeningProfileID"),
                            ScreeningProfileName = reader.Get<string>("ScreeningProfileName"),
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }

            return branchLocations;
        }

        /// <summary>
        /// GEt branch locations for User
        /// </summary>
        public List<BranchLocation> GetForUserID(Int32 userID)
        {
            string sql = @"SELECT bl.BranchLocationID, bl.Name, bl.Disabled
FROM dbo.BranchLocation bl
INNER JOIN dbo.Users_BranchLocation ubr ON ubr.BranchLocationID = bl.BranchLocationID
Where ubr.UserID = @UserID
ORDER BY Name ASC";
            List<BranchLocation> branchLocations = new List<BranchLocation>();

            CommandObject.Parameters.Clear();
            AddParameter("@UserID", DbType.Int32).Value = userID;
            try
            {
                this.Connect();

                using (IDataReader reader = this.RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        branchLocations.Add(new BranchLocation(reader.GetInt32(0))
                        {
                            BranchLocationID = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            Disabled = reader.IsDBNull(2) ? false : reader.GetBoolean(2)
                            //Description = Convert.ToString(reader[2])   // this sets empty string instead of null
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Disconnect();
            }
            return branchLocations;
        }

        #region ObjectDataSource methods with pagination and sorting


        public List<BranchLocation> GetAll(string filterByName, bool showDisabled, int? screeningProfileId, int startRowIndex, int maximumRows, string orderBy)
        {
            string orderByInner = orderBy;
            string orderByFinal = orderBy;

            if (orderBy.Contains("ScreeningProfileName"))
            {
                orderByInner = orderBy.Replace("ScreeningProfileName", "p.Name");
            }
            else if (orderBy.StartsWith("Name"))
            {
                orderByInner = orderByFinal = "l." + orderBy;
             }
            else if (orderBy.StartsWith("Description"))
            {
                orderByInner = orderByFinal = "l." + orderBy;
            }

            //query to select branch location
            string sql = String.Format(@"SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
BranchLocationID, p.Name as ScreeningProfileName
FROM dbo.BranchLocation l INNER JOIN dbo.ScreeningProfile p ON l.ScreeningProfileID = p.ID

", !string.IsNullOrEmpty(orderByInner) ? orderByInner : "BranchLocationID ASC");

            //create query to select branch location with filter by name
            QueryBuilder sqlBuilder = new QueryBuilder(sql);
            ClearParameters();

            //parameter for filtering
            if (!String.IsNullOrEmpty(filterByName))
            {
                sqlBuilder.AppendWhereCondition("l.Name LIKE @Name", ClauseType.And);
                AddParameter("@Name", DbType.String).Value = SqlLikeStringPrepeare(filterByName, LikeCondition.Contains);
            }

            if (!showDisabled)
            {
                sqlBuilder.AppendWhereCondition("l.Disabled = 0", ClauseType.And);
            }

            if (screeningProfileId.HasValue && screeningProfileId > 0)
            {
                sqlBuilder.AppendWhereCondition("p.ID = @ScreeningProfileID", ClauseType.And);
                AddParameter("@ScreeningProfileID", DbType.Int32).Value = SqlParameterSafe(screeningProfileId);
            }


            //query to select branch location with pagination
            string sqlFormat = string.Format(@"
WITH OrderedLocations(RowNumber, BranchLocationID, ScreeningProfileName)
AS (
    {1}
)
SELECT 
    l.BranchLocationID,
    l.Name,
    l.Description,
    l.Disabled,
    l.ScreeningProfileId,
    p.ScreeningProfileName
FROM  OrderedLocations p 
    INNER JOIN dbo.BranchLocation l ON p.BranchLocationID = l.BranchLocationID
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
order by {0}", !string.IsNullOrEmpty(orderByFinal) ? orderByFinal : "l.BranchLocationID ASC", sqlBuilder.ToString());
            List<BranchLocation> branchLocations = new List<BranchLocation>();

            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            try
            {
                this.Connect();
                using (IDataReader reader = this.RunSelectQuery(sqlFormat))
                {
                    while (reader.Read())
                    {
                        branchLocations.Add(new BranchLocation(reader.GetInt32(0))
                        {
                            Name = reader.GetString(1),
                            Description = Convert.ToString(reader[2]),
                            Disabled = reader.IsDBNull(3) ? false : reader.GetBoolean(3),
                            ScreeningProfileID = reader.Get<int>(4) ?? 0,
                            ScreeningProfileName = reader.Get<string>("ScreeningProfileName")
                        });
                    }
                }
            }
            finally
            {
                this.Disconnect();
            }

            return branchLocations;
        }
        /// <summary>
        /// get number of records
        /// </summary>
        /// <param name="filterByName"></param>
        /// <returns></returns>
        public int CountAll(string filterByName, bool showDisabled, int? screeningProfileId)
        {
            int count = 0;
            QueryBuilder sql = new QueryBuilder(@"
SELECT COUNT_BIG(*)
FROM dbo.BranchLocation l INNER JOIN dbo.ScreeningProfile p ON l.ScreeningProfileID = p.ID
");

            ClearParameters();

            if (!string.IsNullOrEmpty(filterByName))
            {
                sql.AppendWhereCondition("l.Name LIKE @NameFilter", ClauseType.And);
                AddParameter("@NameFilter", DbType.String, 128).Value = SqlLikeStringPrepeare(filterByName, LikeCondition.Contains);
            }
            if (!showDisabled)
            {
                sql.AppendWhereCondition("Disabled = 0", ClauseType.And);
            }
            
            if (screeningProfileId.HasValue && screeningProfileId > 0)
            {
                sql.AppendWhereCondition("p.ID = @ScreeningProfileID", ClauseType.And);
                AddParameter("@ScreeningProfileID", DbType.Int32).Value = SqlParameterSafe(screeningProfileId);
            }

            try
            {

                count = Convert.ToInt32(RunScalarQuery(sql.ToString()));
            }
            finally
            {
                this.Disconnect();
            }

            return count;
        }


        /// <summary>
        /// Get count of all not diaabled branch locations 
        /// </summary>
        public int GetNotDisabledCount()
        {
            int count = 0;
            string sql = @"
SELECT COUNT_BIG(*)
FROM dbo.BranchLocation
WHERE Disabled = 0 OR Disabled IS NULL
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


        /// <summary>
        /// True if branch location has a kiosk and kiosk is not disabled
        /// </summary>
        public bool HasActiveKiosk(int branchLocationID)
        {
            int rowCount = 0;
            string sql = @"SELECT COUNT(k.KioskID) FROM dbo.BranchLocation l
INNER JOIN dbo.Kiosk k ON l.BranchLocationID = k.BranchLocationID
Where l.BranchLocationID = @BranchLocationID AND (k.Disabled = 'false' OR k.Disabled IS NULL)";

            CommandObject.Parameters.Clear();
            AddParameter("@BranchLocationID", DbType.Int32).Value = branchLocationID;
            try
            {
                base.Connect();
                rowCount = Convert.ToInt32(RunScalarQuery(sql));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }

            return rowCount > 0;

        }

        #endregion

        #endregion

        #region ADD/UPDATE/DELETE

        /// <summary>
        /// Create new branch location.
        /// </summary>
        /// <param name="branchLocation">The branch location to put to database. ID must be 0.</param>
        /// <returns>ID for new branch location record. Also, branchLocation argument gets this new ID value.</returns>
        /// <exception cref="System.ArgumentException">Branch location ID equal to zero</exception>
        public int Add(BranchLocation branchLocation)
        {
            string sql = @"
INSERT INTO dbo.BranchLocation
(Name, Description, Disabled, ScreeningProfileID)
VALUES
(@Name, @Description, @Disabled, @ScreeningProfileID);

SET @NewID = SCOPE_IDENTITY();
";
            if (branchLocation.BranchLocationID != 0)
            {
                throw new ArgumentException("Branch location ID must be 0 to insert new one");
            }


            ClearParameters();

            var idParam = AddParameter("@NewID", DbType.Int32, ParameterDirection.Output);
            AddParameter("@Name", DbType.String, 128).Value = branchLocation.Name;
            AddParameter("@Description", DbType.String).Value =
                (branchLocation.Description != null) ? (object)branchLocation.Description : DBNull.Value;

            AddParameter("@Disabled", DbType.Boolean).Value = SqlParameterSafe(branchLocation.Disabled);
            AddParameter("@ScreeningProfileID", DbType.Int32).Value = SqlParameterSafe(branchLocation.ScreeningProfileID);


            try
            {

                int affected = this.RunNonSelectQuery(sql);
                int newId = Convert.ToInt32(idParam.Value);

                branchLocation.BranchLocationID = newId;

                return branchLocation.BranchLocationID;
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

        public bool Update(BranchLocation branchLocation)
        {
            string sql = @"
UPDATE dbo.BranchLocation
SET
Name = @Name,
Description = @Description,
ScreeningProfileID = @ScreeningProfileID
WHERE BranchLocationID = @BranchLocationID
";

            ClearParameters();

            AddParameter("@Name", DbType.String, 128).Value = branchLocation.Name;
            AddParameter("@Description", DbType.String).Value =
                (branchLocation.Description != null) ? (object)branchLocation.Description : DBNull.Value;
            AddParameter("@BranchLocationID", DbType.Int32).Value = branchLocation.BranchLocationID;
            AddParameter("@ScreeningProfileID", DbType.Int32).Value = SqlParameterSafe(branchLocation.ScreeningProfileID);

            try
            {
                int affected = this.RunNonSelectQuery(sql);

                return (affected == 1);
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

        /// <summary>
        /// Delete location.
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>True if single location was deleted, false if no locations were found, or several locations were deleted,
        /// or existing location was not deleted due to foreign key violation (users are present in the location)</returns>
        public bool Delete(int id)
        {
            string sql = @"
DELETE FROM dbo.BranchLocation
WHERE BranchLocationID = @BranchLocationID
";

            ClearParameters();
            AddParameter("@BranchLocationID", DbType.Int32).Value = id;

            try
            {
                int affected = this.RunNonSelectQuery(sql);

                return (affected == 1);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 16 && ex.Number == 547) // foreign key violation
                {
                    // users are present
                    return false;
                }
                throw;
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

        /// <summary>
        /// Enabled/Disabled Branch Location
        /// </summary>
        public void SetBranchLocationDisabledStatus(int branchLocationID, bool isDisabled)
        {
            string sql = @"
UPDATE dbo.BranchLocation SET Disabled = @Disabled
WHERE BranchLocationID = @BranchLocationID";

            CommandObject.Parameters.Clear();
            AddParameter("@BranchLocationID", DbType.Int32).Value = branchLocationID;
            AddParameter("@Disabled", DbType.Boolean).Value = SqlParameterSafe(isDisabled);

            try
            {
                base.BeginTransaction();
                base.StartConnectionSharing();

                if (isDisabled)
                {
                    //TODO: enabled if kiosk count does not exceed 
                }
                RunNonSelectQuery(sql);

                base.StopConnectionSharing();
                base.CommitTransaction();
            }
            catch (DbException ex)
            {
                base.StopConnectionSharing();
                base.RollbackTransaction();
                throw ex;
            }
            catch (Exception ex)
            {
                base.StopConnectionSharing();
                base.RollbackTransaction();
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }

        }

        public int? GetScreeningProfileByKioskID(short kiokID)
        {
            int? result = null;


            var sql = "dbo.[uspGetScreeningProfileByKioskID]";

            ClearParameters();

            AddParameter("@KioskID", DbType.Int16).Value = kiokID;

            try
            {
                result = RunProcedureScalarQuery<int>(sql);
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        #endregion
    }
}
