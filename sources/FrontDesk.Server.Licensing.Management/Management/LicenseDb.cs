using System;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;

namespace FrontDesk.Server.Licensing.Management
{
    internal class LicenseDb: DBDatabase
    {
        #region Constructors

        internal LicenseDb() : base(0) { }

        internal LicenseDb(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }

        #endregion

        public DataSet GetAll()
        {
            string sql = @"
SELECT [LicenseID]
      ,[ClientID]
      ,[SerialNumber]
      ,[Years]
      ,[MaxKiosks]
      ,[MaxBranchLocations]
      ,[LicenseString]
      ,[Issued]
FROM [dbo].[License]
ORDER BY [LicenseID]
";
            try
            {
                this.Connect();
                DataSet ds = this.GetDataSet(sql);
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

        public DataSet GetAllForDisplay()
        {
            string sql = @"
SELECT [LicenseID]
      ,l.[ClientID]
      ,[SerialNumber]
      ,[Years]
      ,[MaxKiosks]
      ,[MaxBranchLocations]
      ,[LicenseString]
      ,[Issued]
      
      , c.CompanyName
      
FROM dbo.License l
LEFT JOIN dbo.Client c ON l.ClientID = c.ClientID
ORDER BY LicenseID
";
            try
            {
                this.Connect();
                DataSet ds = this.GetDataSet(sql);
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

        /// <summary>
        /// Get all license which have been filtered on license key
        /// </summary>
        internal DataSet GetAllWithFilter(string licenseKey, int startRowIndex, int maximumRows, string orderBy)
        {
            DataSet ds = null;
            string innerOrderBy = string.Empty;

            if (string.IsNullOrEmpty(orderBy)) orderBy = "l.LicenseID DESC"; // default sort order

            if (!string.IsNullOrEmpty(orderBy) && orderBy.Contains("Activated"))
            {
                innerOrderBy = orderBy.Replace("Activated", "a.Issued");
            }
            if (!string.IsNullOrEmpty(orderBy) && orderBy.Contains("ExpirationDate"))
            {
                innerOrderBy = orderBy.Replace("ExpirationDate", "a.ExpirationDate");
            }
            if (!string.IsNullOrEmpty(orderBy) && orderBy.Contains("CompanyName"))
            {
                innerOrderBy = orderBy.Replace("CompanyName", "c.CompanyName");
            }
            if (!string.IsNullOrEmpty(orderBy) && orderBy.Contains("Issued"))
            {
                innerOrderBy = orderBy.Replace("Issued", "l.Issued");
            }
            //query to select license
            string sql = String.Format(@"SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, 
l.LicenseID 
,c.CompanyName
,a.Issued as Activated
,a.ExpirationDate
FROM dbo.License l
LEFT JOIN dbo.Client c ON l.ClientID = c.ClientID
LEFT JOIN dbo.Activation a ON a.LicenseID = l.LicenseID", string.IsNullOrEmpty(innerOrderBy) ? orderBy : innerOrderBy);
            //create query to select branch location with filter by name
            QueryBuilder sqlBuilder = new QueryBuilder(sql);
            CommandObject.Parameters.Clear();

            //parameters for filtering
            if (!String.IsNullOrEmpty(licenseKey))
            {
                sqlBuilder.AppendWhereCondition("LicenseString LIKE @LicenseString", ClauseType.And);
                AddParameter("@LicenseString", DbType.String, 255).Value = SqlLikeStringPrepeare(licenseKey, LikeCondition.StartsWith);
            }

            innerOrderBy = String.Empty;
            //query to select license with pagination
            string sqlFormat = string.Format(@"
WITH OrderedLicense(RowNumber, LicenseID, CompanyName, Activated, ExpirationDate)
AS (
	{0}
)
SELECT l.[LicenseID]
      ,l.[ClientID]
      ,l.[SerialNumber]
      ,l.[Years]
      ,l.[MaxKiosks]
      ,l.[MaxBranchLocations]
      ,l.[LicenseString]
      ,l.[Issued]
      ,o.Activated
      ,o.CompanyName
      ,o.ExpirationDate
FROM OrderedLicense o 
INNER JOIN dbo.License l  ON o.LicenseID = l.LicenseID
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
order by {1}", sqlBuilder.ToString(), string.IsNullOrEmpty(innerOrderBy) ? orderBy : innerOrderBy);

            //parameters for pagination
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            try
            {
                base.Connect();
                ds = this.GetDataSet(sqlFormat);
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
                this.Disconnect();
            }
            return ds;
        }

        public LicenseEntry GetByID(int licenseId)
        {
            LicenseEntry license = null;

            string sql = @"
SELECT l.[LicenseID]
      ,l.[ClientID]
      ,[SerialNumber]
      ,[Years]
      ,[MaxKiosks]
      ,[MaxBranchLocations]
      ,[LicenseString]
      ,l.[Issued]
      , c.CompanyName      
      , a.ActivationID
FROM [dbo].[License] l
LEFT JOIN dbo.Client c ON c.ClientID = l.ClientID
LEFT JOIN dbo.Activation a ON a.LicenseID = l.LicenseID
WHERE l.LicenseID = @LicenseID
";

            CommandObject.Parameters.Clear();
            AddParameter("@LicenseID", DbType.Int32).Value = licenseId;
            try
            {
                this.Connect();
                using (IDataReader reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        license = LicenseEntityHelper.Instance.CreateFromDataReader(reader);
                    }
                }
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
                this.Disconnect();
            }
            
            return license;
        }

        public LicenseEntry GetBySerialNumber(int serialNumber)
        {
            LicenseEntry license = null;

            string sql = @"
SELECT l.[LicenseID]
      ,l.[ClientID]
      ,[SerialNumber]
      ,[Years]
      ,[MaxKiosks]
      ,[MaxBranchLocations]
      ,[LicenseString]
      ,l.[Issued]
      , c.CompanyName      
      , a.ActivationID
FROM [dbo].[License] l
LEFT JOIN dbo.Client c ON c.ClientID = l.ClientID
LEFT JOIN dbo.Activation a ON a.LicenseID = l.LicenseID
WHERE SerialNumber = @SerialNumber
";

            CommandObject.Parameters.Clear();
            AddParameter("@SerialNumber", DbType.Int32).Value = serialNumber;
            try
            {
                this.Connect();
                using (IDataReader reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        license = LicenseEntityHelper.Instance.CreateFromDataReader(reader);
                    }
                }
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
                this.Disconnect();
            }

            return license;
        }

        public LicenseEntry GetByLicenseKey(string licenseKey)
        {
            LicenseEntry license = null;

            string sql = @"
SELECT l.[LicenseID]
      ,l.[ClientID]
      ,[SerialNumber]
      ,[Years]
      ,[MaxKiosks]
      ,[MaxBranchLocations]
      ,[LicenseString]
      ,l.[Issued]
      , c.CompanyName      
      , a.ActivationID
FROM [dbo].[License] l
LEFT JOIN dbo.Client c ON c.ClientID = l.ClientID
LEFT JOIN dbo.Activation a ON a.LicenseID = l.LicenseID
WHERE LicenseString = @LicenseString
";

            CommandObject.Parameters.Clear();
            AddParameter("@LicenseString", DbType.String).Value = licenseKey;
            try
            {
                this.Connect();
                using (IDataReader reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        license = LicenseEntityHelper.Instance.CreateFromDataReader(reader);
                    }
                }
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
                this.Disconnect();
            }

            return license;
        }

        /// <summary>
        /// Get License for client
        /// </summary>
        internal DataSet GetForClient(Int32 clientID)
        {
            DataSet ds = null;

            string sql = @"
SELECT l.[LicenseID]
      ,l.[ClientID]
      ,[SerialNumber]
      ,[Years]
      ,[MaxKiosks]
      ,[MaxBranchLocations]
      ,[LicenseString]
      ,l.[Issued]
      
      , c.CompanyName
      , a.Issued as Activated
      , a.ExpirationDate
      
FROM dbo.License l
LEFT JOIN dbo.Client c ON l.ClientID = c.ClientID
LEFT JOIN dbo.Activation a ON a.LicenseID = l.LicenseID
WHERE c.ClientID = @ClientID
ORDER BY l.Issued DESC
";
            CommandObject.Parameters.Clear();
            AddParameter("@ClientID", DbType.Int32).Value = clientID;
            try
            {
                base.Connect();
                ds = GetDataSet(sql);
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

            return ds;
        }

        /// <summary>
        /// get number of records
        /// </summary>
        internal int CountAll(string licenseKey)
        {
            int count = 0;
            QueryBuilder sql = new QueryBuilder(@"
SELECT COUNT_BIG(*) 
FROM dbo.License l
LEFT JOIN dbo.Client c ON l.ClientID = c.ClientID
LEFT JOIN dbo.Activation a ON a.LicenseID = l.LicenseID
");
            if (!string.IsNullOrEmpty(licenseKey))
            {
                sql.AppendWhereCondition("LicenseString LIKE @LicenseString", ClauseType.And);
                AddParameter("@LicenseString", DbType.String, 128).Value = SqlLikeStringPrepeare(licenseKey, LikeCondition.StartsWith);
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

        public void Delete(int licenseId)
        {
            string sql = @"DELETE FROM dbo.License WHERE LicenseID = @LicenseID";
            CommandObject.Parameters.Clear();
            AddParameter("@LicenseID", DbType.Int32).Value = licenseId;

            try
            {
                this.Connect();
                RunNonSelectQuery(sql);
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
                this.Disconnect();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="license"></param>
        /// <returns>ID of new license</returns>
        public int Create(LicenseEntry license)
        {
            return Create(license, 1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="license"></param>
        /// <param name="quantity"></param>
        /// <returns>Quantity of actually created licenses if quantity is > 1, or ID of new license if quantity is 1</returns>
        public int Create(LicenseEntry license, int quantity)
        {
            string sql = @"
SET @NewSerial = (SELECT ISNULL(MAX(SerialNumber),0) + 1 FROM dbo.License);

INSERT INTO [dbo].[License]
           ([ClientID]
           ,[SerialNumber]
           ,[Years]
           ,[MaxKiosks]
           ,[MaxBranchLocations]
           ,[LicenseString]
           ,[Issued]
           )
     VALUES
           (@ClientID
--           ,@SerialNumber
           , @NewSerial
           ,@Years
           ,@MaxKiosks
           ,@MaxBranchLocations
           ,@LicenseString
           ,@Issued
           );

SET @NewID = SCOPE_IDENTITY();
";
            string sqlUpdateSerial = @"
UPDATE [dbo].[License]
SET LicenseString = @LicenseString
WHERE LicenseID = @LicenseID
";

            if (license.LicenseID != 0)
            {
                throw new ArgumentException("License ID must be 0 to insert new one");
            }

            try
            {
                this.Connect();

                if (license.IsAssignedToClient)
                {
                    this.AddParameter("@ClientID", DbType.Int32).Value = license.ClientID;
                }
                else
                {
                    this.AddParameter("@ClientID", DbType.Int32).Value = DBNull.Value;
                }

                this.AddParameter("@Years", DbType.Int32).Value = license.Years;
                this.AddParameter("@MaxKiosks", DbType.Int32).Value = license.MaxKiosks;
                this.AddParameter("@MaxBranchLocations", DbType.Int32).Value = license.MaxBranchLocations;
                var licenseStringParam = this.AddParameter("@LicenseString", DbType.String, 128);

                licenseStringParam.Value = "<<not calculated>>";   // not null, must be present
                this.AddParameter("@Issued", DbType.DateTimeOffset).Value = DateTimeOffset.Now;

                var idParam = this.AddParameter("@NewID", DbType.Int32, ParameterDirection.Output);
                var newSerialParam = this.AddParameter("@NewSerial", DbType.Int32, ParameterDirection.Output);

                if (quantity == 1)
                {
                    // insert features
                    int affected = this.RunNonSelectQuery(sql);
                    int newId = Convert.ToInt32(idParam.Value);
                    
                    // calculate and update license key string
                    this.AddParameter("@LicenseID", DbType.Int32).Value = newId;
                    
                    license.LicenseID = newId;
                    license.SerialNumber = Convert.ToInt32(newSerialParam.Value);
                    string licenseString = License.CreateLicenseKeyString(license);
                    licenseStringParam.Value = licenseString;
                    affected = this.RunNonSelectQuery(sqlUpdateSerial);

                    return newId;
                }
                else
                {
                    int created = 0;
                    var licenseIdParam = this.AddParameter("@LicenseID", DbType.Int32);
                    licenseIdParam.Value = DBNull.Value;
                    
                    for (int i = 0; i < quantity; i++)
                    {
                        // insert features
                        int affected = this.RunNonSelectQuery(sql);
                        int newId = Convert.ToInt32(idParam.Value);

                        // calculate and update license key string
                        licenseIdParam.Value = newId;                        
                        license.LicenseID = newId;
                        license.SerialNumber = Convert.ToInt32(newSerialParam.Value);
                        string licenseString = License.CreateLicenseKeyString(license);
                        licenseStringParam.Value = licenseString;
                        affected = this.RunNonSelectQuery(sqlUpdateSerial);

                        created++;
                    }

                    return created;
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
        }

        public void AssignToClient(LicenseEntry license, int clientId)
        {
            string sql = @"
UPDATE [dbo].[License]
   SET [ClientID] = @ClientID
WHERE LicenseID = @LicenseID
";
            if (clientId == 0 || license.ClientID.HasValue)
            {
                throw new ArgumentException("Client ID must be non 0 and license must be not assigned");
            }

            try
            {
                this.Connect();

                this.AddParameter("@ClientID", DbType.Int32).Value = clientId;
                this.AddParameter("@LicenseID", DbType.Int32).Value = license.LicenseID;

                int affected = this.RunNonSelectQuery(sql);

                return;
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
