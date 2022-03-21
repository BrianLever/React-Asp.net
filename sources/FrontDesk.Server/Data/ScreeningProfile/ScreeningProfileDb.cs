namespace FrontDesk.Server.Data.ScreeningProfile
{
    using FrontDesk;
    using FrontDesk.Common.Data;
    using FrontDesk.Common.Extensions;

    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;


    public interface IScreeningProfileRepository
    {
        ScreeningProfile Get(int id);
        List<ScreeningProfile> GetAll(string filterByName, int startRowIndex, int maximumRows, string orderBy);
        int CountAll(string filterByName);
        int Add(ScreeningProfile model);
        bool Update(ScreeningProfile model);
        bool Delete(int id);
        void RefreshKioskSettings(int screeningProfileID, DateTime newLastModifiedDateUTC);
    }

    public class ScreeningProfileDb : DBDatabase, IScreeningProfileRepository
    {
        #region Constructors

        public ScreeningProfileDb() : base(0) { }

        public ScreeningProfileDb(DbConnection sharedConnection) : base(sharedConnection)
        {
        }

        #endregion


        public ScreeningProfile Get(int id)
        {
            string sql = @"
SELECT
    ID,
    Name,
    Description
FROM dbo.ScreeningProfile
WHERE ID = @ID
";

            try
            {
                ClearParameters();
                AddParameter("@ID", DbType.Int32).Value = id;

                using (IDataReader reader = this.RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        return ScreeningProfileFactory.Create(reader);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }

            return null;
        }

        public List<ScreeningProfile> GetAll(string filterByName, int startRowIndex, int maximumRows, string orderBy)
        {

            List<ScreeningProfile> result = new List<ScreeningProfile>();

            //query to select screening profile
            string sql = String.Format(@"SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
ID 
FROM dbo.ScreeningProfile", !string.IsNullOrEmpty(orderBy) ? orderBy : "NAME ASC");

            QueryBuilder sqlBuilder = new QueryBuilder(sql);
            ClearParameters();

            //parameter for filtering
            if (!String.IsNullOrEmpty(filterByName))
            {
                sqlBuilder.AppendWhereCondition("Name LIKE @Name", ClauseType.And);
                AddParameter("@Name", DbType.String).Value = SqlLikeStringPrepeare(filterByName, LikeCondition.Contains);
            }

            //query to select screening profile with pagination
            string sqlFormat = string.Format(@"
WITH OrderedLocations(RowNumber, ID)
AS (
    {1}
)
SELECT 
    l.ID,
    l.Name,
    l.Description 
FROM  OrderedLocations o INNER JOIN dbo.ScreeningProfile l 
    ON o.ID = l.ID
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
order by {0}", !string.IsNullOrEmpty(orderBy) ? orderBy : "l.Name ASC", sqlBuilder.ToString());

            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            try
            {
                using (IDataReader reader = this.RunSelectQuery(sqlFormat))
                {
                    while (reader.Read())
                    {
                        result.Add(ScreeningProfileFactory.Create(reader));
                    }
                }
            }
            finally
            {
                Disconnect();
            }

            return result;
        }

        /// <summary>
        /// get number of records
        /// </summary>
        /// <param name="filterByName"></param>
        /// <returns></returns>
        public int CountAll(string filterByName)
        {
            int count = 0;
            QueryBuilder sql = new QueryBuilder(@"
SELECT COUNT_BIG(*)
FROM dbo.ScreeningProfile
");
            if (!string.IsNullOrEmpty(filterByName))
            {
                sql.AppendWhereCondition("Name LIKE @NameFilter", ClauseType.And);
                AddParameter("@NameFilter", DbType.String, 128).Value = SqlLikeStringPrepeare(filterByName, LikeCondition.Contains);
            }

            try
            {

                count = RunScalarQuery(sql.ToString()).As<int>();
            }
            finally
            {
                Disconnect();
            }

            return count;
        }


        /// <summary>
        /// Create new item
        /// </summary>
        /// <param name="model">The screening profile to put to database. ID must be 0.</param>
        /// <returns>ID for new screening profile record. Also, ScreeningProfile argument gets this new ID value.</returns>
        /// <exception cref="System.ArgumentException">screening profile ID equal to zero</exception>
        public int Add(ScreeningProfile model)
        {
            string sql = "dbo.[uspCreateNewScreeningProfile]";
            string sqlUnique = "SELECT COUNT(ID) FROM dbo.ScreeningProfile WHERE Name = @Name";

            ClearParameters();

            AddParameter("@Name", DbType.String, 128).Value = SqlParameterSafe(model.Name);
            AddParameter("@Description", DbType.String).Value = SqlParameterSafe(model.Description);
            var idParam = AddParameter("@NewID", DbType.Int32, ParameterDirection.Output);


            try
            {
                BeginTransaction();

                //check unique name
                var nameIsUnique = RunScalarQuery<int>(sqlUnique) == 0;

                if (!nameIsUnique)
                {
                    throw new ApplicationException("Screening Profile with the name {0} already exists.".FormatWith(model.Name));
                }

                int affected = this.RunProcedureNonSelectQuery(sql);
                int newId = idParam.Value.As<int>();

                model.ID = newId;

                CommitTransaction();

            }
            catch (Exception)
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
            return model.ID;
        }

        public bool Update(ScreeningProfile model)
        {

            string sqlUnique = "SELECT COUNT(ID) FROM dbo.ScreeningProfile WHERE Name = @Name AND ID <> @ID";

            string sql = @"
UPDATE dbo.ScreeningProfile
SET
Name = @Name,
Description = @Description
WHERE ID = @ID
";


            ClearParameters();
            AddParameter("@Name", DbType.String, 128).Value = model.Name;
            AddParameter("@Description", DbType.String).Value = SqlParameterSafe(model.Description);
            AddParameter("@ID", DbType.Int32).Value = model.ID;

            try
            {
                BeginTransaction();

                //check unique name
                var nameIsUnique = RunScalarQuery<int>(sqlUnique) == 0;

                if (!nameIsUnique)
                {
                    throw new ApplicationException("Screening Profile with the name {0} already exists.".FormatWith(model.Name));
                }

                int affected = this.RunNonSelectQuery(sql);

                CommitTransaction();

                return (affected == 1);
            }
            catch (Exception)
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
        /// Delete location.
        /// </summary>
        /// <param name="id">profile ID</param>
        /// <returns>True if single profile was deleted, false if no profile were found, or several profiles were deleted,
        /// or existing profile was not deleted due to foreign key violation (profile is used by location)</returns>
        public bool Delete(int id)
        {
            string sql = @"
DELETE FROM dbo.ScreeningProfile
WHERE ID = @ID
";

            if (id == ScreeningProfile.DefaultProfileID)
            {
                throw new ArgumentException("Cannot delete defailt screening profile.");
            }

            ClearParameters();
            AddParameter("@ID", DbType.Int32).Value = id;

            try
            {
                int affected = this.RunNonSelectQuery(sql);
                return (affected == 1);
            }
            catch (SqlException ex)
            {
                if (ex.Class == 16 && ex.Number == 547) // foreign key violation
                {
                    // location are present
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
                Disconnect();
            }
        }

        public void RefreshKioskSettings(int screeningProfileID, DateTime newLastModifiedDateUTC)
        {
            var sql = "dbo.uspScreeningProfileRefreshKioskSettings";

            ClearParameters();

            AddParameter("@ScreeningProfileID", DbType.Int32).Value = screeningProfileID;
            AddParameter("@LastModifiedDateUTC", DbType.DateTime).Value = newLastModifiedDateUTC;


            try
            {
                RunProcedureNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }
    }
}