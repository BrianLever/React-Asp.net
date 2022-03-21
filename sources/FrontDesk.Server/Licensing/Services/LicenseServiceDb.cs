using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using Common.Logging;

namespace FrontDesk.Server.Licensing.Services
{
    internal class LicenseServiceDb : DBDatabase
    {
        private ILog _logger = LogManager.GetLogger<LicenseServiceDb>();
        #region Constructors

        internal LicenseServiceDb() : base(0) { }

        internal LicenseServiceDb(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }

        #endregion

        #region License Key


        #endregion

        internal DataSet GetAllProductLicenses()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Get all licenses.
        /// </summary>
        /// <returns>List of valid licenses</returns>
        internal List<LicenseCertificate> GetAllLicenses()
        {
            List<LicenseCertificate> list = new List<LicenseCertificate>();

            string sql = @"
SELECT 
LicenseKey, ActivationKey, ActivationRequest,
CreatedDate, ActivationRequestDate, ActivatedDate
FROM dbo.License 
ORDER BY ActivatedDate DESC, CreatedDate DESC
";
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        try
                        {
                            list.Add(new LicenseCertificate(reader));
                        }
                        catch(DataException ex)
                        {
                            _logger.Error(ex);
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return list;
        }

        /// <summary>
        /// Get all licenses.
        /// </summary>
        /// <returns>List of valid licenses</returns>
        internal IEnumerable<LicenseCertificate> GetActivatedLicenses()
        {
            string sql = @"
SELECT 
 LicenseKey, ActivationKey, ActivationRequest,
 CreatedDate, ActivationRequestDate, ActivatedDate
FROM dbo.License 
WHERE ActivationKey IS NOT NULL
ORDER BY ActivatedDate DESC, CreatedDate DESC
";
            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        yield return new LicenseCertificate(reader);
                    }
                }
            }
            finally
            {
                Disconnect();
            }
        }
        /// <summary>
        /// Register new product license key
        /// </summary>
        /// <param name="licenseKey"></param>
        /// <returns>True, if new license key has been inserted. False, if specified key is already exists and no changes have been done.</returns>
        internal bool RegisterProductLicense(LicenseCertificate licenseKey)
        {
            int affectedRows = 0;
            string sql = @"
IF NOT EXISTS(SELECT NULL FROM dbo.License WHERE LicenseKey=@LicenseKey)
    INSERT INTO dbo.License (LicenseKey, CreatedDate)
    VALUES(@LicenseKey, @CreatedDate);
";
            CommandObject.Parameters.Clear();
            AddParameter("@LicenseKey", DbType.AnsiString, 128).Value = licenseKey.License.LicenseString;
            AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = licenseKey.CreatedDate;

            try
            {
                affectedRows = RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
            return (affectedRows > 0);

        }


        /// <summary>
        /// Get license certificate
        /// </summary>
        /// <param name="licenseKey"></param>
        /// <returns></returns>
        internal LicenseCertificate GetLicenseCertificate(string licenseKey)
        {
            LicenseCertificate cert = null;

            string sql = @"
SELECT
LicenseKey, ActivationKey, ActivationRequest,
CreatedDate, ActivationRequestDate, ActivatedDate
FROM dbo.License 
WHERE LicenseKey = @LicenseKey
";
            CommandObject.Parameters.Clear();
            AddParameter("@LicenseKey", DbType.AnsiString, 128).Value = licenseKey;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        try
                        {
                            cert = new LicenseCertificate(reader);
                        }
                        catch (ApplicationException)
                        {
                            //invalid serial key - do nothing
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return cert;
        }
        /// <summary>
        /// Get most recent valid license certificate
        /// </summary>
        /// <returns></returns>
        internal LicenseCertificate GetMostRecentLicenseCertificate()
        {
            LicenseCertificate cert = null;

            string sql = @"
SELECT 
LicenseKey, ActivationKey, ActivationRequest,
CreatedDate, ActivationRequestDate, ActivatedDate
FROM dbo.License 
ORDER BY ActivatedDate DESC, CreatedDate DESC
";
            CommandObject.Parameters.Clear();


            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        try
                        {
                            cert = new LicenseCertificate(reader);
                            break;
                        }
                        catch (ApplicationException)
                        {
                            //invalid serial key - do nothing
                        }
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return cert;
        }

        /// <summary>
        /// Overrite activation request key and activation key
        /// </summary>
        /// <param name="cert"></param>
        /// <returns></returns>
        internal bool UpdateActivationKeys(LicenseCertificate cert)
        {
            int affectedRows = 0;
            string sql = @"
UPDATE dbo.License SET 
    ActivationRequest = @ActivationRequest,
    ActivationRequestDate = @ActivationRequestDate,
    ActivationKey = @ActivationKey,
    ActivatedDate = @ActivatedDate
WHERE LicenseKey = @LicenseKey
";
            CommandObject.Parameters.Clear();
            AddParameter("@ActivationRequest", DbType.AnsiString, 128).Value = SqlParameterSafe(cert.ActivationRequestCode);
            AddParameter("@ActivationKey", DbType.AnsiString, 128).Value = SqlParameterSafe(cert.ActivationKeyString);
            AddParameter("@LicenseKey", DbType.AnsiString, 128).Value = cert.License.LicenseString;
            AddParameter("@ActivationRequestDate", DbType.DateTimeOffset).Value = cert.ActivationRequestDate.HasValue ? (object)cert.ActivationRequestDate : DBNull.Value;
            AddParameter("@ActivatedDate", DbType.DateTimeOffset).Value = cert.ActivatedDate.HasValue ? (object)cert.ActivatedDate : DBNull.Value;

            try
            {
                affectedRows = RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
            return (affectedRows > 0);
        }
        /// <summary>
        /// Remove license
        /// </summary>
        /// <param name="licenseKey"></param>
        /// <returns></returns>
        internal int RemoveLicense(string licenseKey)
        {
            string sql = @"DELETE FROM dbo.License WHERE LicenseKey = @LicenseKey";
            int affectedRows = 0;

            CommandObject.Parameters.Clear();
            AddParameter("@LicenseKey", DbType.AnsiString, 128).Value = licenseKey;
            try
            {
                affectedRows = RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
            return affectedRows;
        }
    }
}
