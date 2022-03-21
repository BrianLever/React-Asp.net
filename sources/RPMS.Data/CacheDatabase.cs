using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using InterSystems.Data.CacheClient;
using System.Configuration;
using System.Data;


namespace RPMS.Data
{
    /* Yevgeniy's implementation */
    public abstract class CacheDatabase : IDisposable
    {
        protected IDbConnection ConnectionObject;
        protected IDbCommand CommandObject;

        protected bool IsConnectionShared = false;
        /// <summary>
        /// True if ConnectionObject connected to database
        /// </summary>
        protected bool isConnected;


        public CacheDatabase() 
        {
            ConnectionObject = new InterSystems.Data.CacheClient.CacheConnection(ConfigurationManager.ConnectionStrings[0].ConnectionString);

            CommandObject = new CacheCommand();
            CommandObject.Connection = ConnectionObject;
            isConnected = false;

        }

        /// <summary>
        /// Open connection
        /// </summary>
        public void Connect()
        {
            if (!isConnected && !IsConnectionShared)
            {
                ConnectionObject.Open();
                isConnected = true;
            }
        }
        /// <summary>
        /// Close connection if it opened
        /// </summary>
        public void Disconnect()
        {
            if (isConnected && !IsConnectionShared)
            {
                if (ConnectionObject != null) ConnectionObject.Close();
                isConnected = false;
            }
        }

        #region [Add Command Paramenter]

        protected IDbDataParameter AddParameter(string name, DbType type, int size)
        {
            IDbDataParameter parameter = AddParameter(name, type);
            parameter.Size = size;
            return parameter;
        }

        protected IDbDataParameter AddParameter(string name, DbType type)
        {
            IDbDataParameter parameter = new CacheParameter();
            parameter.DbType = type;
            parameter.ParameterName = name;
            CommandObject.Parameters.Add(parameter);
            return parameter;
        }

        protected IDbDataParameter CreateParameter(string name, DbType type)
        {
            IDbDataParameter parameter = new CacheParameter();
            parameter.DbType = type;
            parameter.ParameterName = name;
            return parameter;
        }

        protected IDbDataParameter AddParameter(string name, DbType type, ParameterDirection direction)
        {
            IDbDataParameter parameter = AddParameter(name, type);
            parameter.Direction = direction;
            return parameter;
        }

        #endregion

        #region Execute SQL

        /// <summary>
        /// Execute SELECT query and return SqlDataReader object
        /// </summary>
        /// <param name="sqlText">Sql Select statement</param>
        /// <returns>Result SqlDataReader object</returns>
        protected IDataReader RunSelectQuery(string sqlText)
        {
            Connect();
            
            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.Text;
            IDataReader oRd = CommandObject.ExecuteReader();

            return oRd;
        }

        /// <summary>
        /// Execute SELECT query and return SqlDataReader object
        /// </summary>
        /// <param name="sqlText">Sql Select statement</param>
        /// <param name="commandBehavior">Result SqlDataReader object</param>
        /// <returns></returns>
        protected IDataReader RunSelectQuery(string sqlText, CommandBehavior commandBehavior)
        {
            Connect();

            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.Text;
            IDataReader oRd = CommandObject.ExecuteReader(commandBehavior);

            return oRd;
        }

        /// <summary>
        /// Execute SELECT query and return SqlDataReader object
        /// </summary>
        /// <param name="sqlText">Sql Select statement</param>
        /// <returns>Result SqlDataReader object</returns>
        protected IDataReader RunProcedureSelectQuery(string sqlText)
        {
            Connect();

            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.StoredProcedure;
            IDataReader oRd = CommandObject.ExecuteReader();

            return oRd;
        }

        /// <summary>
        /// Execute Scalar query
        /// </summary>
        /// <param name="sqlText">Scalar SQL statement</param>
        /// <returns>Result object</returns>
        protected object RunProcedureScalarQuery(string sqlText)
        {
            Connect();
            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.StoredProcedure;

            return CommandObject.ExecuteScalar();
        }

        /// <summary>
        /// Execute Scalar query
        /// </summary>
        /// <param name="sqlText">Scalar SQL statement</param>
        /// <returns>Result object</returns>
        protected object RunScalarQuery(string sqlText)
        {
            Connect();

            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.Text;

            return CommandObject.ExecuteScalar();
        }

        /// <summary>
        /// Execute non select query
        /// </summary>
        /// <param name="sqlText">Non select sql statement</param>
        /// <returns>Number of rows affected</returns>
        protected int RunProcedureNonSelectQuery(string sqlText)
        {
            Connect();

            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.StoredProcedure;

            return CommandObject.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute non select query
        /// </summary>
        /// <param name="sqlText">Non select sql statement</param>
        /// <returns>Number of rows affected</returns>
        protected int RunNonSelectQuery(string sqlText)
        {
            Connect();

            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.Text;

            return CommandObject.ExecuteNonQuery();
        }

        protected IDbDataAdapter CreateDataAdapter()
        {
            return new CacheDataAdapter();
        }

        /// <summary>
        /// Return Filled DataSet Object with one table
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        protected DataSet GetDataSet(string sqlText)
        {
            DataSet dsResult;
            Connect();

            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.Text;

            IDbDataAdapter dbAdapter = CreateDataAdapter();
            dbAdapter.SelectCommand = CommandObject;

            dsResult = new DataSet("table");
            dbAdapter.Fill(dsResult);

            return dsResult;

        }

        /// <summary>
        /// Return Filled DataSet Object with one table as result of stored procedure execution
        /// </summary>
        /// <returns></returns>
        protected DataSet GetProcedureDataSet(string sqlText)
        {
            DataSet dsResult = new DataSet("table");
            Connect();

            CommandObject.CommandText = FixSqlCommandText(sqlText);
            CommandObject.CommandType = CommandType.StoredProcedure;

            IDbDataAdapter dbAdapter = CreateDataAdapter();
            dbAdapter.SelectCommand = CommandObject;

            dbAdapter.Fill(dsResult);

            CommandObject.CommandType = CommandType.Text;
            return dsResult;

        }

        /// <summary>
        /// Execute simply (one sql select statement) and return DataSet result object
        /// </summary>
        /// <param name="sqlText"></param>
        /// <returns></returns>
        protected DataSet GetSimplyDataSetQuery(string sqlText)
        {
            DataSet oDs = null;


            Connect();

            try
            {
                oDs = GetDataSet(FixSqlCommandText(sqlText));
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                Disconnect();
            }
            return oDs;
        }

        #endregion

        #region Helper Methods for Cache

        /// <summary>
        /// Run additional rules over SQL string and remove not supported characters
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public virtual string FixSqlCommandText(string sql)
        {
            if (!string.IsNullOrWhiteSpace(sql))
            {
                return sql.Replace("\r", " ");
            }
            else
            {
                return sql;
            }
        }

        /// <summary>
        /// Replace empty parameter values to DBNull.Value
        /// </summary>
        /// <param name="value">Paramter value</param>
        /// <param name="bNull">Flag, replace empty values to DBNull</param>
        /// <param name="EmptyValueExpression">Value which should be interprates as "empty" object value</param>
        /// <returns></returns>
        static public object SqlParameterSafe(object value, bool bNull, object EmptyValueExpression)
        {
            object oValue = value;
            string sValue = "";

            if (value == null)
            {
                oValue = DBNull.Value;
            }
            else if (value is System.DateTime)
            {
                if (Convert.ToDateTime(value) == DateTime.MinValue)
                {
                    oValue = DBNull.Value;
                }
                else
                {
                    oValue = value;
                }
            }
            else if (value is System.String)
            {
                sValue = Convert.ToString(value).Trim();
                if (sValue.Length == 0 && bNull) oValue = DBNull.Value;

            }
            else if (value.Equals(EmptyValueExpression) && bNull)
            {
                oValue = DBNull.Value;
            }

            return oValue;
        }
        /// <summary>
        /// Replace empty parameter values to DBNull.Value
        /// Replace null & empty values to DbNull
        /// </summary>
        static public object SqlParameterSafe(object value)
        {
            return SqlParameterSafe(value, true, null);
        }

        #endregion




        #region IDisposable Members

        public void Dispose()
        {
            Disconnect();
            GC.SuppressFinalize(this);
        }

        ~CacheDatabase()
        {
            Disconnect();
        }

        #endregion
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
