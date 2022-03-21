using System;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;

namespace FrontDesk.Server.Licensing.Management
{
    internal class ClientDb: DBDatabase
    {
        #region Constructors

        internal ClientDb() : base(0) { }

        internal ClientDb(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }

        #endregion

        public DataSet GetAll()
        {
            string sql = @"
SELECT [ClientID]
      ,[CompanyName]
      ,[StateCode]
      ,[City]
      ,[AddressLine1]
      ,[AddressLine2]
      ,[PostalCode]
      ,[Email]
      ,[ContactPerson]
      ,[ContactPhone]
      ,[Notes]
      ,[LastModified]
FROM [dbo].[Client]
ORDER BY ClientID
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
        /// Get all clients with paging
        /// </summary>
        public DataSet GetAllWithPaging(int startRowIndex, int maximumRows, string orderBy)
        {
            if (string.IsNullOrEmpty(orderBy)) orderBy = "ClientID ASC"; // default sort order

            string sql = String.Format(@"
WITH OrderedClient(RowNumber, ClientID)
AS (
SELECT TOP(@startRowIndex + @maxRows) 
ROW_NUMBER() OVER ( ORDER BY {0}) as RowNumber, ClientID
FROM dbo.Client
)
SELECT c.ClientID
      ,c.[CompanyName]
      ,c.[StateCode]
      ,c.[City]
      ,c.[AddressLine1]
      ,c.[AddressLine2]
      ,c.[PostalCode]
      ,c.[Email]
      ,c.[ContactPerson]
      ,c.[ContactPhone]
      ,c.[Notes]
      ,c.[LastModified]
FROM OrderedClient o 
INNER JOIN dbo.Client c ON o.ClientID = c.ClientID
WHERE RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)", orderBy);
            CommandObject.Parameters.Clear();
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;
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

        public int Add(Client client)
        {
            string sql = @"
INSERT INTO [dbo].[Client]
           ([CompanyName]
           ,[StateCode]
           ,[City]
           ,[AddressLine1]
           ,[AddressLine2]
           ,[PostalCode]
           ,[Email]
           ,[ContactPerson]
           ,[ContactPhone]
           ,[Notes]
           ,[LastModified])
     VALUES
           (@CompanyName
           ,@StateCode
           ,@City
           ,@AddressLine1
           ,@AddressLine2
           ,@PostalCode
           ,@Email
           ,@ContactPerson
           ,@ContactPhone
           ,@Notes
           ,@LastModified);

SET @NewID = SCOPE_IDENTITY();
";
            if (client.ClientID != 0)
            {
                throw new ArgumentException("Client ID must be 0 to insert new one");
            }

            try
            {
                this.Connect();

                this.AddParameter("@CompanyName", DbType.String, 128).Value = string.IsNullOrEmpty(client.CompanyName) ? DBNull.Value : (object)client.CompanyName;
                this.AddParameter("@StateCode", DbType.String, 2).Value = string.IsNullOrEmpty(client.StateCode) ? DBNull.Value : (object)client.StateCode;
                this.AddParameter("@City", DbType.String, 128).Value = string.IsNullOrEmpty(client.City) ? DBNull.Value : (object)client.City;
                this.AddParameter("@AddressLine1", DbType.String, 128).Value = string.IsNullOrEmpty(client.AddressLine1) ? DBNull.Value : (object)client.AddressLine1;
                this.AddParameter("@AddressLine2", DbType.String, 128).Value = string.IsNullOrEmpty(client.AddressLine2) ? DBNull.Value : (object)client.AddressLine2;
                this.AddParameter("@PostalCode", DbType.String, 24).Value = string.IsNullOrEmpty(client.PostalCode) ? DBNull.Value : (object)client.PostalCode;
                this.AddParameter("@Email", DbType.String, 128).Value = string.IsNullOrEmpty(client.Email) ? DBNull.Value : (object)client.Email;
                this.AddParameter("@ContactPerson", DbType.String, 128).Value = string.IsNullOrEmpty(client.ContactPerson) ? DBNull.Value : (object)client.ContactPerson;
                this.AddParameter("@ContactPhone", DbType.String, 24).Value = string.IsNullOrEmpty(client.ContactPhone) ? DBNull.Value : (object)client.ContactPhone;
                this.AddParameter("@Notes", DbType.String).Value = string.IsNullOrEmpty(client.Notes) ? DBNull.Value : (object)client.Notes;
                this.AddParameter("@LastModified", DbType.DateTimeOffset).Value = DateTimeOffset.Now;

                var idParam = this.AddParameter("@NewID", DbType.Int32, ParameterDirection.Output);

                int affected = this.RunNonSelectQuery(sql);
                int newId = Convert.ToInt32(idParam.Value);

                return newId;
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

        public bool Update(Client client)
        {
            string sql = @"
UPDATE [dbo].[Client]
   SET [CompanyName] = @CompanyName
      ,[StateCode] = @StateCode
      ,[City] = @City
      ,[AddressLine1] = @AddressLine1
      ,[AddressLine2] = @AddressLine2
      ,[PostalCode] = @PostalCode
      ,[Email] = @Email
      ,[ContactPerson] = @ContactPerson
      ,[ContactPhone] = @ContactPhone
      ,[Notes] = @Notes
      ,[LastModified] = @LastModified
WHERE ClientID = @ClientID
";
            if (client.ClientID == 0)
            {
                throw new ArgumentException("Client ID must be non 0 to update");
            }

            try
            {
                this.Connect();

                this.AddParameter("@ClientID", DbType.Int32).Value = client.ClientID;

                this.AddParameter("@CompanyName", DbType.String, 128).Value = string.IsNullOrEmpty(client.CompanyName) ? DBNull.Value : (object)client.CompanyName;
                this.AddParameter("@StateCode", DbType.String, 2).Value = string.IsNullOrEmpty(client.StateCode) ? DBNull.Value : (object)client.StateCode;
                this.AddParameter("@City", DbType.String, 128).Value = string.IsNullOrEmpty(client.City) ? DBNull.Value : (object)client.City;
                this.AddParameter("@AddressLine1", DbType.String, 128).Value = string.IsNullOrEmpty(client.AddressLine1) ? DBNull.Value : (object)client.AddressLine1;
                this.AddParameter("@AddressLine2", DbType.String, 128).Value = string.IsNullOrEmpty(client.AddressLine2) ? DBNull.Value : (object)client.AddressLine2;
                this.AddParameter("@PostalCode", DbType.String, 24).Value = string.IsNullOrEmpty(client.PostalCode) ? DBNull.Value : (object)client.PostalCode;
                this.AddParameter("@Email", DbType.String, 128).Value = string.IsNullOrEmpty(client.Email) ? DBNull.Value : (object)client.Email;
                this.AddParameter("@ContactPerson", DbType.String, 128).Value = string.IsNullOrEmpty(client.ContactPerson) ? DBNull.Value : (object)client.ContactPerson;
                this.AddParameter("@ContactPhone", DbType.String, 24).Value = string.IsNullOrEmpty(client.ContactPhone) ? DBNull.Value : (object)client.ContactPhone;
                this.AddParameter("@Notes", DbType.String).Value = string.IsNullOrEmpty(client.Notes) ? DBNull.Value : (object)client.Notes;
                this.AddParameter("@LastModified", DbType.DateTimeOffset).Value = DateTimeOffset.Now;

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

        public Client GetByID(int clientId)
        {
            Client client = null;

            string sql = @"
SELECT [ClientID]
      ,[CompanyName]
      ,[StateCode]
      ,[City]
      ,[AddressLine1]
      ,[AddressLine2]
      ,[PostalCode]
      ,[Email]
      ,[ContactPerson]
      ,[ContactPhone]
      ,[Notes]
      ,[LastModified]
FROM [dbo].[Client]
WHERE ClientID = @ClientID
";

            CommandObject.Parameters.Clear();
            AddParameter("@ClientID", DbType.Int32).Value = clientId;
            try
            {
                this.Connect();
                using (IDataReader reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        client = new Client(reader);
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

            return client;
        }

        public void Delete(int clientId)
        {
            string sql = @"DELETE FROM dbo.Client WHERE ClientID = @ClientID";
            CommandObject.Parameters.Clear();
            AddParameter("@ClientID", DbType.Int32).Value = clientId;

            try
            {
                BeginTransaction();
                StartConnectionSharing();

                this.Connect();
                if (!HasLicense(clientId))
                {
                    RunNonSelectQuery(sql);
                }
                else
                {
                    throw new ApplicationException("Failed to delete client because the client has a license");
                }

                StopConnectionSharing();
                CommitTransaction();
            }
            catch (DbException ex)
            {
                StopConnectionSharing();
                RollbackTransaction();
                throw ex;
            }
            catch (Exception ex)
            {
                StopConnectionSharing();
                RollbackTransaction();
                throw ex;
            }
            finally
            {
                this.Disconnect();
            }

        }

        public bool HasLicense(int ClientID)
        {
            int count = 0;
            string sql = @"Select COUNT(c.ClientID) from dbo.License l
Inner JOIN dbo.Client c ON l.ClientID = c.ClientID
Where c.ClientID = @ClientID";

            CommandObject.Parameters.Clear();
            AddParameter("@ClientID", DbType.Int32).Value = ClientID;

            try
            {
                base.Connect();
                count = Convert.ToInt32(RunScalarQuery(sql));
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
            return count > 0;
        }

        /// <summary>
        /// Get client count
        /// </summary>
        internal int GetCount()
        {
            int rowCount = 0;
            string sql = @"Select COUNT_BIG(*) from dbo.Client";
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
    }
}
