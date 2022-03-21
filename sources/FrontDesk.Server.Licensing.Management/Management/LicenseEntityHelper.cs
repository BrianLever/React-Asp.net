using System;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace FrontDesk.Server.Licensing.Management
{
    /// <summary>
    /// Provides methods to work with License entity on FrontDesk License Manager application
    /// </summary>
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed - need for datasources on pages
    public class LicenseEntityHelper
    {
        #region Constructor

        private static object _syncObj = new object();
        private static LicenseEntityHelper _instance = null;
        public static LicenseEntityHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncObj)
                    {
                        if (_instance == null) _instance = new LicenseEntityHelper();
                    }
                }
                return _instance;
            }
        }


        private LicenseEntityHelper()
        {
        }

        #endregion

        private LicenseDb DbObject
        {
            get
            {
                return new LicenseDb();
            }
        }
        /// <summary>
        /// create license object from data reader row
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public LicenseEntry CreateFromDataReader(IDataReader reader)
        {
            LicenseEntry license = null;

            // NOTE here we do not re-calculate it, just read what was stored.            
            string licenseKeyString = Convert.IsDBNull(reader["LicenseString"]) ? string.Empty : Convert.ToString(reader["LicenseString"]);
            if (string.IsNullOrEmpty(licenseKeyString))
            {
                license =  new LicenseEntry();
                license.SerialNumber = Convert.ToInt32(reader["SerialNumber"]);
                license.Years = Convert.ToInt32(reader["Years"]);
                license.MaxKiosks = Convert.ToInt32(reader["MaxKiosks"]);
                license.MaxBranchLocations = Convert.ToInt32(reader["MaxBranchLocations"]);
            }
            else
            {
                //init from license key string
                license = new LicenseEntry(licenseKeyString);
            }

            license.LicenseID = Convert.ToInt32(reader["LicenseID"]);
            if (Convert.IsDBNull(reader["ClientID"]))
            {
                license.ClientID = null;
            }
            else
            {
                license.ClientID = Convert.ToInt32(reader["ClientID"]);
            }

                       
            license.Issued = (DateTimeOffset)reader["Issued"];
            license.CompanyName = Convert.IsDBNull(reader["CompanyName"]) ? string.Empty : Convert.ToString(reader["CompanyName"]);

            license.IsActivated = !Convert.IsDBNull(reader["ActivationID"]);

            return license;
        }


        public DataSet GetAll()
        {
            return DbObject.GetAll();
        }

        public LicenseEntry GetByID(int licenseId)
        {
            return DbObject.GetByID(licenseId);
        }

        public LicenseEntry GetBySerialNumber(int serialNumber)
        {
            return DbObject.GetBySerialNumber(serialNumber);
        }

        public LicenseEntry GetByLicenseKey(string licenseKey)
        {
            return DbObject.GetByLicenseKey(licenseKey);
        }

        public static DataSet GetAllForDisplay()
        {
            return Instance.DbObject.GetAllForDisplay();        
        }

        /// <summary>
        /// Get all licenses which have been filtered on license key
        /// </summary>
        public static DataSet GetAllWithFilter(string licenseKey, int startRowIndex, int maximumRows, string orderBy)
        {
            return new LicenseDb().GetAllWithFilter(licenseKey, startRowIndex, maximumRows, orderBy);
        }

        /// <summary>
        /// get number of records
        /// </summary>
        public static int CountAll(string licenseKey)
        {
            return new LicenseDb().CountAll(licenseKey);
        }

        /// <summary>
        /// Get licenses for client
        /// </summary>
        //[Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)] // used in ObjectDataSource by name
        //[Obfuscation(Feature = "properties renaming", Exclude = true)]
        // EzFuscator does not allow to exclude method name AND argument names - need to exclude whole class
        public static DataSet GetForClient(Int32 clientID)
        {
            return new LicenseDb().GetForClient(clientID);
        }

        public void Delete(int licenseId)
        {
            try
            {
                DbObject.Delete(licenseId);
            }
            catch (DbException ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Create new License
        /// </summary>
        /// <param name="template"></param>
        /// <returns>ID of created License</returns>
        public int Create(LicenseEntry template)
        {
            try
            {
                int newId = DbObject.Create(template);
                return newId;
            }
            catch (DbException ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Create set of license with the same parameters
        /// </summary>
        /// <param name="template"></param>
        /// <param name="quantity"></param>
        /// <returns>Number of created licenses</returns>
        public int CreatePack(LicenseEntry template, int quantity)
        {
            try
            {
                int count = DbObject.Create(template, quantity);
                return count;
            }
            catch (DbException ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }
        }

        public void AssignToClient(LicenseEntry license, int clientId)
        {
            try
            {
                DbObject.AssignToClient(license, clientId);                
            }
            catch (DbException ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }

        }
    }
}

