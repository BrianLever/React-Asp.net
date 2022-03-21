using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;

namespace FrontDesk.Server.Data
{
    internal class StateDatabase : DBDatabase
    {
 
        #region constructor
        
        public StateDatabase(string connectionString)
            : base(connectionString) { }

        public StateDatabase()
            : base(ConfigurationManager.ConnectionStrings[0].ConnectionString)
        { }

        public StateDatabase(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region GET

        /// <summary>
        /// Get all state
        /// </summary>
        internal DataSet GetAllState(bool isComboBox)
        {
            DataSet ds = null;

            string sql = @"Select StateCode, Name FROM dbo.State Order by Name";
            CommandObject.Parameters.Clear();
            try
            {
                base.Disconnect();
                ds = GetDataSet(sql);
                if (IsHasOneTable(ds) && isComboBox)
                {
                    DataRow row = ds.Tables[0].NewRow();
                    row["StateCode"] = DBNull.Value;
                    row["Name"] = "<not selected>";
                    ds.Tables[0].Rows.InsertAt(row, 0);
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
                base.Disconnect();
            }

            return ds;
        }



        #endregion

    }
}
