using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterSystems.Data.CacheClient;
using System.Configuration;
using System.Data;
using FrontDesk.Common.Data;

namespace RPMS.Common.Data
{
    /* Yevgeniy's implementation */
    public class CacheDatabase
    {
        protected CacheConnection ConnectionObject;
        protected CacheCommand CommandObject;

        public CacheDatabase() 
        {
            ConnectionObject = new InterSystems.Data.CacheClient.CacheConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);

            CommandObject = new CacheCommand();
            CommandObject.Connection = ConnectionObject;
        }

        protected void Connect()
        {
            ConnectionObject.Open();
        }

        protected void Disconnect()
        {
            ConnectionObject.Close();
            
        }

        #region [Add Command Paramenter]

        protected CacheParameter AddParameter(string name, DbType type, int size)
        {
            CacheParameter parameter = AddParameter(name, type);
            parameter.Size = size;
            return parameter;
        }

        protected CacheParameter AddParameter(string name, DbType type)
        {
            CacheParameter parameter = new CacheParameter();
            parameter.DbType = type;
            parameter.ParameterName = name;
            CommandObject.Parameters.Add(parameter);
            return parameter;
        }

        protected CacheParameter AddParameter(string name, DbType type, ParameterDirection direction)
        {
            CacheParameter parameter = AddParameter(name, type);
            parameter.Direction = direction;
            return parameter;
        }

        #endregion

        protected DataSet GetDataSet(string sql)
        { 
            DataSet ds = new DataSet();
            CommandObject.CommandText = sql;
            CacheDataAdapter adapter = new CacheDataAdapter(CommandObject);
            adapter.Fill(ds);
            return ds;
        }
    }

    /* DB Factory Implementation 
    /// <summary>
    /// Provide basic interface for MS SQL database Compact edition 
    /// </summary>
    /// <remarks>
    /// Create derived class to implement Database Access Logic in your application
    /// </remarks>
    public abstract class CacheDatabase : DBDatabase
    {
        /// <summary>
        /// Get database access factory name.
        /// Returns "System.Data.SqlClient" string be default
        /// </summary>
        protected override string DatabaseFactoryName
        {
            get { return "RPMS.Common.Data.CacheProvider"; }
        }

        protected override int GetDefaultCommandTimeout()
        {
            return 0;
        }

        /// <summary>
        /// Create new object
        /// </summary>
        /// <param name="sConnectionString">Connection string</param>
        public CacheDatabase(string connectionStringName)
            : base(connectionStringName)
        {
        }

        public CacheDatabase()
            : base()
        {

            string connectionString = ConfigurationManager.ConnectionStrings[0].ConnectionString;
            ConnectionObject.ConnectionString = connectionString;


            //string connectionString = string.Format(@"Data Source={0}\Data\FrontDeskKiosk.sdf;Persist Security Info=false",
            //    System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

            this.ConnectionObject.ConnectionString = connectionString;

        }

    }
     *      */

}
