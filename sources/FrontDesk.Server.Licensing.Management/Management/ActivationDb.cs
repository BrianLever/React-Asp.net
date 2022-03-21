using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Data;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;

namespace FrontDesk.Server.Licensing.Management
{
    internal class ActivationDb: DBDatabase
    {
        #region Constructors

        internal ActivationDb() : base(0) { }

        internal ActivationDb(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="activation"></param>
        /// <param name="license"></param>
        /// <returns>ID of new activation record, or 0 if license is already activated.</returns>
        public int Add(ActivationEntry activation, LicenseEntry license)
        {
            string sql = @"
INSERT INTO [dbo].[Activation]
           ([ActivationRequest]
           ,[LicenseID]
           ,[ProductId]
           ,[ExpirationDate]
           ,[ActivationKey]
           ,[Issued])
     VALUES
           (@ActivationRequest
           ,@LicenseID
           ,@ProductId
           ,@ExpirationDate
           ,@ActivationKey
           ,@Issued);

SET @NewID = SCOPE_IDENTITY();
";

            try
            {
                this.Connect();

                this.AddParameter("@ActivationRequest", DbType.String, 128).Value = activation.ActivationRequest;
                this.AddParameter("@LicenseID", DbType.Int32).Value = license.LicenseID;
                this.AddParameter("@ProductId", DbType.String, 128).Value = activation.WindowsProductIDPart;
                this.AddParameter("@ExpirationDate", DbType.DateTime).Value = activation.ExpirationDate;
                this.AddParameter("@ActivationKey", DbType.String, 128).Value = activation.ActivationKey;
                this.AddParameter("@Issued", DbType.DateTimeOffset).Value = DateTimeOffset.Now;

                var idParam = this.AddParameter("@NewID", DbType.Int32, ParameterDirection.Output);
                
                int affected = this.RunNonSelectQuery(sql);
                int newId = Convert.ToInt32(idParam.Value);

                return newId;                
            }
            catch (SqlException ex)
            {
                if (ex.Class == 14 && ex.Number == 2627) // Violation of UNIQUE KEY constraint 'UQ_Activation_LicenseID'. Cannot insert duplicate key
                {
                    // already activated
                    return 0;
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

        public ActivationEntry GetByLicenseID(int licenseId)
        {
            ActivationEntry act = null;

            string sql = @"
SELECT [ActivationID]
      ,[ActivationRequest]
--      ,[LicenseID]
      ,[ProductId]
      ,[ExpirationDate]
      ,[ActivationKey]
      ,a.[Issued]
      , l.SerialNumber
FROM [dbo].[Activation] a
LEFT JOIN [dbo].[License] l ON l.LicenseID = a.LicenseID
WHERE a.LicenseID = @LicenseID
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
                        act = ActivationEntry.CreateFromDataReader(reader);
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

            return act;
        }

    }
}
