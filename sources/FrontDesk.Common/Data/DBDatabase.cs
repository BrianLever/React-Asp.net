using Common.Logging;

using FrontDesk.Common.Extensions;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Reflection;

namespace FrontDesk.Common.Data
{


    /// <summary>
    /// Provide basic interface for MS SQL database using System.Data.Common components
    /// </summary>
    /// <remarks>
    /// Create derived class to implement Database Access Logic in your application
    /// </remarks>
    public abstract class DBDatabase : ITransactionalDatabase
    {
        #region Protected Properties
        /// <summary>
        /// Get database access factory name.
        /// Returns "System.Data.SqlClient" string be default
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual string DatabaseFactoryName
        {
            get { return "System.Data.SqlClient"; }
        }

        /// <summary>
        /// Db factory
        /// </summary>
        protected DbProviderFactory providerFactory;
        /// <summary>
        /// Databse connection
        /// </summary>
        protected IDbConnection ConnectionObject;
        /// <summary>
        /// Database command object
        /// </summary>
        protected IDbCommand CommandObject;


        /// <summary>
        /// Database Transaction
        /// </summary>
        protected IDbTransaction TransactionObject;

        /// <summary>
        /// True if ConnectionObject connected to database
        /// </summary>
        protected bool isConnected;
        #endregion


        #region Public Accessors

        /// <summary>
        /// Get connection object to use in shared transaction
        /// </summary>
        /// <returns></returns>
        public IDbConnection GetConnectionObject()
        {
            return this.ConnectionObject;
        }
        /// <summary>
        /// Get transaction object to use in shared transaction
        /// </summary>
        public IDbTransaction GetTransactionObject()
        {
            return this.TransactionObject;
        }

        protected void ClearParameters()
        {
            CommandObject.Parameters.Clear();
        }

        #endregion

        #region Constructors


        /// <summary>
        /// Initialize object. Use connection string from the application configuration file
        /// </summary>
        /// <param name="connectionStringIndex">Connection string index in the Configuration.connectionStrings section.</param>
        /// <remarks>
        /// Ensure that you put <clear/> node before your connections string when you are using connection string index
        /// </remarks>
        public DBDatabase(int connectionStringIndex)
            : this()
        {
            string connectionString = ConfigurationManager.ConnectionStrings[connectionStringIndex].ConnectionString;
            ConnectionObject.ConnectionString = connectionString;

        }

        /// <summary>
        /// Create new object
        /// </summary>
        /// <param name="sConnectionString">Connection string</param>
        public DBDatabase(string connectionString)
            : this()
        {
            ConnectionObject.ConnectionString = connectionString;

        }



        /// <summary>
        /// Parameterless constructor
        /// </summary>
        public DBDatabase()
        {
            Logger = LogManager.GetLogger("DBDatabase");

            Logger.DebugFormat("DBDatabase getting provider factory: {0}.", this.DatabaseFactoryName);

            ///Get SQL PRovider Factory
            ///this can be overriden from derived class 
            providerFactory = DbProviderFactories.GetFactory(this.DatabaseFactoryName); //"System.Data.SqlClient"

            Logger.DebugFormat("DBDatabase creating connection.");
            //Set Connection string to connection object and assign connection to command object
            ConnectionObject = providerFactory.CreateConnection();

            Logger.DebugFormat("DBDatabase creating command.");
            CommandObject = providerFactory.CreateCommand();
            CommandObject.Connection = ConnectionObject;
            CommandObject.CommandTimeout = GetDefaultCommandTimeout();



            isConnected = false;

            Logger.Debug("DBDatabase initialized.");

        }
        /// <summary>
        /// Get default provider's command timeout
        /// </summary>
        /// <returns></returns>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual int GetDefaultCommandTimeout() { return 300; }

        /// <summary>
        /// Initialize object. Use shared connection.
        /// Parent object that host the connection should be responsible for connection closing.
        /// </summary>
        /// <param name="sharedConnection">Connection object that is shared with the other class. Connection should be null.</param>
        /// <remarks>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">sharedConnection is null</exception>
        public DBDatabase(IDbConnection sharedConnection)
            : this()
        {
            if (sharedConnection != null)
            {
                this.ConnectionObject = sharedConnection;
                this.CommandObject.Connection = this.ConnectionObject;

                //start connection sharing that should be closed normally
                this.StartConnectionSharing();
                this.isConnected = true;
            }
            else throw new ArgumentNullException("sharedConnection");

        }


        /// <summary>
        /// Initialize object. Use shared connection with transaction.
        /// Parent object that host the connection should be responsible for connection closing and transaction commit/rollback.
        /// </summary>
        /// <param name="sharedConnection">Connection object that is shared with the other class. Connection should be null.</param>
        /// <remarks>
        /// </remarks>
        /// <exception cref="System.ArgumentNullException">sharedConnection is null</exception>
        public DBDatabase(IDbConnection sharedConnection, DbCommand commandObject)
            : this()
        {
            if (sharedConnection != null)
            {
                ConnectionObject = sharedConnection;
                //start connection sharing that should be closed normally
                this.StartConnectionSharing();
                this.isConnected = true;
            }
            else throw new ArgumentNullException("sharedConnection");

        }


        /// <summary>
        /// Read connection string from the application config file using ConnectionStrings key
        /// </summary>
        public string ConnectionStringNameFromConfigFile
        {
            set
            {
                string connectionString = ConfigurationManager.ConnectionStrings[value].ConnectionString;
                ConnectionObject.ConnectionString = connectionString;
            }
        }

        #endregion

        /// <summary>
        /// Set or Get Connection string value
        /// </summary>
        protected string ConnectionString
        {
            get { return ConnectionObject.ConnectionString; }
            set { ConnectionObject.ConnectionString = value; }
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

        #region Execute SQL

        /// <summary>
        /// Execute SELECT query and return SqlDataReader object
        /// </summary>
        /// <param name="sqlText">Sql Select statement</param>
        /// <returns>Result SqlDataReader object</returns>
        protected IDataReader RunSelectQuery(string sqlText)
        {
            Connect();

            CommandObject.CommandText = sqlText;
            CommandObject.CommandType = CommandType.Text;


            Logger.TraceFormat("Execute SQL: {0}", sqlText);

            var oRd = CommandObject.ExecuteReader();

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

            CommandObject.CommandText = sqlText;
            CommandObject.CommandType = CommandType.Text;

            Logger.TraceFormat("Execute SQL: {0}", sqlText);

            return CommandObject.ExecuteReader(commandBehavior);
        }

        /// <summary>
        /// Execute SELECT query and return SqlDataReader object
        /// </summary>
        /// <param name="SqlText">Sql Select statement</param>
        /// <returns>Result SqlDataReader object</returns>
        protected IDataReader RunProcedureSelectQuery(string SqlText)
        {
            Connect();

            CommandObject.CommandText = SqlText;
            CommandObject.CommandType = CommandType.StoredProcedure;
            return CommandObject.ExecuteReader();

        }

        /// <summary>
        /// Execute Scalar query
        /// </summary>
        /// <param name="SqlText">Scalar SQL statement</param>
        /// <returns>Result object</returns>
        protected T? RunProcedureScalarQuery<T>(string SqlText) where T : struct
        {
            Connect();
            CommandObject.CommandText = SqlText;
            CommandObject.CommandType = CommandType.StoredProcedure;

            var result = CommandObject.ExecuteScalar();

            return (result == null || Convert.IsDBNull(result)) ? (T?)null : result.As<T>();
        }

        /// <summary>
        /// Execute Scalar query
        /// </summary>
        /// <param name="SqlText">Scalar SQL statement</param>
        /// <returns>Result object</returns>
        protected object RunScalarQuery(string SqlText)
        {
            Connect();

            CommandObject.CommandText = SqlText;
            CommandObject.CommandType = CommandType.Text;

            return CommandObject.ExecuteScalar();
        }

        protected T? RunScalarQuery<T>(string sql) where T : struct
        {
            var result = RunScalarQuery(sql);

            return (result == null || Convert.IsDBNull(result)) ? (T?)null : result.As<T>();
        }

        /// <summary>
        /// Read string scalar value. Return null of not exists and string of the value when found
        /// </summary>
        protected string ReadScalarStringQueryResult(string sql)
        {
            var result = RunScalarQuery(sql);

            return (result == null || Convert.IsDBNull(result)) ? null : result.As<string>();
        }


        /// <summary>
        /// Execute non select query
        /// </summary>
        /// <param name="SqlText">Non select sql statement</param>
        /// <returns>Number of rows affected</returns>
        protected int RunProcedureNonSelectQuery(string SqlText)
        {
            Connect();

            CommandObject.CommandText = SqlText;
            CommandObject.CommandType = CommandType.StoredProcedure;

            return CommandObject.ExecuteNonQuery();
        }

        /// <summary>
        /// Execute non select query
        /// </summary>
        /// <param name="SqlText">Non select sql statement</param>
        /// <returns>Number of rows affected</returns>
        protected int RunNonSelectQuery(string SqlText)
        {
            Connect();

            CommandObject.CommandText = SqlText;
            CommandObject.CommandType = CommandType.Text;

            return CommandObject.ExecuteNonQuery();
        }


        /// <summary>
        /// Return Filled DataSet Object with one table
        /// </summary>
        /// <param name="SqlText"></param>
        /// <returns></returns>
        protected DataSet GetDataSet(string SqlText)
        {
            DataSet dsResult;
            Connect();

            CommandObject.CommandText = SqlText;
            CommandObject.CommandType = CommandType.Text;

            Logger.TraceFormat("Execute SQL: {0}", SqlText);

            IDbDataAdapter dbAdapter = providerFactory.CreateDataAdapter();
            dbAdapter.SelectCommand = CommandObject;

            dsResult = new DataSet("table");
            dbAdapter.Fill(dsResult);

            return dsResult;

        }

        /// <summary>
        /// Return Filled DataSet Object with one table as result of stored procedure execution
        /// </summary>
        /// <returns></returns>
        protected DataSet GetProcedureDataSet(string SqlText)
        {
            DataSet dsResult;
            Connect();

            CommandObject.CommandText = SqlText;
            CommandObject.CommandType = CommandType.StoredProcedure;

            IDbDataAdapter dbAdapter = providerFactory.CreateDataAdapter();
            dbAdapter.SelectCommand = CommandObject;

            dsResult = new DataSet("table");
            dbAdapter.Fill(dsResult);

            CommandObject.CommandType = CommandType.Text;
            return dsResult;

        }

        /// <summary>
        /// Execute simply (one sql select statement) and return DataSet result object
        /// </summary>
        /// <param name="SqlText"></param>
        /// <returns></returns>
        protected DataSet GetSimplyDataSetQuery(string SqlText)
        {
            DataSet oDs = null;


            Connect();

            try
            {
                oDs = GetDataSet(SqlText);
            }
            finally
            {
                Disconnect();
            }
            return oDs;
        }

        /// <summary>
        /// Execute simply (one sql select statement) and return DataSet result object
        /// </summary>
        /// <param name="SqlText"></param>
        /// <returns></returns>
        protected DataSet GetSimplyProcedureDataSetQuery(string SqlText)
        {
            DataSet oDs = null;


            Connect();

            try
            {
                oDs = GetProcedureDataSet(SqlText);
            }
            finally
            {
                Disconnect();
            }
            return oDs;
        }

        #endregion

        #region Util Methods

        /// <summary>
        /// Make values sql safed ( replace empty strings with "null" if needed, and add quotes if needed) 
        /// </summary>
        /// <param name="value">Value needed to safe</param>
        /// <param name="bQuotes">Add quotes at begin and end</param>
        /// <param name="bNull">Raplace empty values with "NULL" string</param>
        /// <returns>Result string</returns>
        static public string SqlSafe(object value, bool bQuotes, bool bNull)
        {
            string sValue = "";

            if (value is System.DateTime)
            {
                if (Convert.ToDateTime(value) == DateTime.MinValue)
                {
                    sValue = "";
                }
                else
                {
                    sValue = Convert.ToDateTime(value).ToShortDateString();
                }
            }
            else
            {
                sValue = Convert.ToString(value).Trim();
            }

            if (sValue.Length == 0)
            {
                if (bNull) return "NULL";
                else sValue = "";
            }

            sValue = sValue.Replace("'", "''");
            if (bQuotes)
            {
                sValue = "'" + sValue + "'";
            }

            return sValue;
        }

        /// <summary>
        /// Make values sql safed ( replace empty strings with "null" and quote values ) 
        /// </summary>
        /// <param name="value">Value needed to safe</param>
        /// <returns></returns>
        static public string SqlSafe(object value)
        {
            return SqlSafe(value, true, true);
        }

        /// <summary>
        /// Make values sql safed ( replace empty strings with "null" and add quotes if needed) 
        /// </summary>
        /// <param name="value">Value needed to safe</param>
        /// <param name="bQuotes">Add quotes at begin and end</param>
        /// <returns></returns>
        static public string SqlSafe(object value, bool bQuotes)
        {
            return SqlSafe(value, bQuotes, true);
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



        /// <summary>
        /// Prepeare string value for LIKE statement
        /// </summary>
        /// <param name="likeValue">Search value</param>
        /// <param name="UseProtection">Escape all special characters</param>
        /// <param name="likeCondition">Format string for searching</param>
        static public object SqlLikeStringPrepeare(string likeValue, bool UseProtection, LikeCondition likeCondition)
        {
            object result = DBNull.Value;
            if (!String.IsNullOrEmpty(likeValue))
            {
                //protect from SQL injection - escape special characters
                if (UseProtection)
                {
                    likeValue.Replace("[", "[[]");
                    likeValue.Replace("%", "[%]");
                    likeValue.Replace("%", "[%]");
                }
                // add special characters for search StartWith
                if ((likeCondition & LikeCondition.Contains) > 0)
                {
                    likeValue = string.Concat('%', likeValue, '%');
                }
                if ((likeCondition & LikeCondition.StartsWith) > 0)
                {
                    likeValue = string.Concat(likeValue, '%');
                }
                result = likeValue;
            }
            return result;
        }
        /// <summary>
        /// Replace empty string with DBNull.Value and format value for search condition.
        /// SQL Injection protection is turned on
        /// </summary>
        static public object SqlLikeStringPrepeare(string likeValue, LikeCondition likeCondition)
        {
            return SqlLikeStringPrepeare(likeValue, true, likeCondition);
        }

        /// <summary>
        /// Replace empty string with DBNull.Value
        /// SQL Injection protection is turned on
        /// </summary>
        static public object SqlLikeStringPrepeare(string likeValue)
        {
            return SqlLikeStringPrepeare(likeValue, true, LikeCondition.None);
        }


        /// <summary>
        /// Replace empty string with % that will select all rows if condition is empty
        /// SQL Injection protection is turned on
        /// </summary>
        static public object SqlLikeAllIfEmpty(string likeValue)
        {
            if (string.IsNullOrEmpty(likeValue))
            {
                return "%";
            }
            else
                return SqlLikeStringPrepeare(likeValue, true, LikeCondition.None);

        }

        /// <summary>
        /// Replace empty string with % that will select all rows if condition is empty
        /// If condition is not empty it formatted to fit passed LIKE CONDITION parameter
        /// 
        /// SQL Injection protection is turned on
        /// </summary>
        static public object SqlLikeAllIfEmpty(string likeValue, LikeCondition likeCondition)
        {
            if (string.IsNullOrEmpty(likeValue))
            {
                return "%";
            }
            else
                return SqlLikeStringPrepeare(likeValue, true, likeCondition);

        }

        /// <summary>
        /// Return DataSet with Table Scheme
        /// </summary>
        /// <param name="tableName">The name if the table which scheme we need to read</param>
        /// <param name="shemaName">Table schema</param>
        /// <returns>
        /// Returns dataset with 1 datatable that has refrelct table's schema 
        /// </returns>
        public DataSet GetTableSchema(string tableName, string shemaName)
        {
            DataSet oDs = new DataSet();
            try
            {
                Connect();
                CommandObject.Parameters.Clear();
                CommandObject.CommandType = CommandType.Text;
                CommandObject.CommandText = string.Format("SELECT TOP 1 * FROM [{0}].[{1}] ",
                     shemaName.Replace("[", ""),
                     tableName.Replace("[", "")
                    );

                IDbDataAdapter dbAdapter = providerFactory.CreateDataAdapter();
                dbAdapter.SelectCommand = CommandObject;

                dbAdapter.FillSchema(oDs, SchemaType.Source);

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                Disconnect();
            }
            return oDs;
        }

        /// <summary>
        /// Check that dataset has at least one table with one row
        /// </summary>
        /// <param name="dsDataset">Dataset object</param>
        /// <returns>Return True if dataset has at least one table with one row</returns>
        public static bool IsHasOneRow(DataSet dsDataset)
        {
            if (dsDataset != null && dsDataset.Tables.Count > 0 && dsDataset.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Check that dataset has at least one table
        /// </summary>
        public static bool IsHasOneTable(DataSet dsDataset)
        {
            if (dsDataset != null && dsDataset.Tables.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region [Handler Connection Paramenters]

        protected DbParameter AddParameter(string sName, DbType Type, int iSize)
        {
            DbParameter dbParameter = providerFactory.CreateParameter();
            dbParameter.DbType = Type;
            dbParameter.ParameterName = sName;
            dbParameter.Size = iSize;
            CommandObject.Parameters.Add(dbParameter);
            return dbParameter;
        }

        protected DbParameter AddParameter(string sName, DbType Type)
        {
            DbParameter dbParameter = providerFactory.CreateParameter();
            dbParameter.DbType = Type;
            dbParameter.ParameterName = sName;
            CommandObject.Parameters.Add(dbParameter);
            return dbParameter;
        }

        protected DbParameter AddParameter(string sName, DbType Type, ParameterDirection pmDirection)
        {
            DbParameter dbParameter = providerFactory.CreateParameter();
            dbParameter.DbType = Type;
            dbParameter.ParameterName = sName;
            dbParameter.Direction = pmDirection;
            CommandObject.Parameters.Add(dbParameter);
            return dbParameter;
        }

        #endregion

        #region Connection Sharing

        private int _connectionSharingNestedLevel = 0;

        /// <summary>
        /// Start connection sharing.
        /// </summary>
        /// <remarks>
        /// Connection sharing is a way to enclose different database class methods in on connection.
        /// When connection sharing is active when you call DbDatabase.Connect(), DbDatabase.Disconnect(),
        /// DbDatabase.BeginTransaction(), DbDatabase.RollbackTransaction() they didn't create new connection (or transaction) and didn't
        /// close existing connection/transaction until you call StopConnectionSharing() methods that stop connection sharing.
        /// 
        /// Use Connection Sharing to execute different database class methods inside public methods within one transaction context.
        /// </remarks>
        public void StartConnectionSharing()
        {
            this._connectionSharingNestedLevel++;
        }
        /// <summary>
        /// Stop connection sharing
        /// </summary>
        public void StopConnectionSharing()
        {
            _connectionSharingNestedLevel--;
            if (_connectionSharingNestedLevel < 0)
            {
                _connectionSharingNestedLevel = 0;
                //throw new InvalidOperationException("Cannot stop connection sharing. It wasn't started before.");
            }
        }
        /// <summary>
        /// True if connection sharing is activated
        /// </summary>
        public bool IsConnectionShared
        {
            get { return (_connectionSharingNestedLevel > 0); }
        }

        #endregion
        #region Transaction

        private int _transactionNestedLevel = 0;

        /// <summary>
        /// Begin new transaction
        /// </summary>
        public void BeginTransaction()
        {
            _transactionNestedLevel++;

            BeginTransaction(IsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Begin new transaction with non-default isolation level
        /// </summary>
        /// <param name="ilLevel"></param>
        public void BeginTransaction(IsolationLevel ilLevel)
        {
            if (TransactionObject == null)
            {
                Connect();
                TransactionObject = ConnectionObject.BeginTransaction(ilLevel);
                CommandObject.Transaction = TransactionObject;
                _transactionNestedLevel++;
            }
        }

        /// <summary>
        /// Commit transaction - save changes
        /// </summary>
        public void CommitTransaction()
        {
            _transactionNestedLevel--;
            if (_transactionNestedLevel < 0) _transactionNestedLevel = 0;

            if (TransactionObject != null && !IsConnectionShared)
            {
                try
                {
                    TransactionObject.Commit();
                    TransactionObject = null;
                }
                catch
                {
                    //if transaction was started shared from other parent method and we failed to commit,
                    //that is meaning that we will try to StopSharingConnection on CATCH Rollback,
                    //so we need to revert _transactionNestedLevel back to prev level
                    //Because when we commit we need to use next pattern
                    //try{
                    //    StartConnectionSharing()
                    //     ................
                    //    StopConnectionSharing();
                    //    CommitTransaction();
                    //}
                    //catch (Exception ex)
                    //{
                    //    StopConnectionSharing();
                    //    RollbackTransaction();
                    //    throw ex;
                    //}

                    _transactionNestedLevel++;

                    //and then raise exception again
                    throw;
                }
            }
        }

        /// <summary>
        /// Enlist current object into top transaction
        /// </summary>
        /// <param name="parentTransaction"></param>
        public void EnlistTransaction(DbTransaction parentTransaction)
        {
            if (TransactionObject == null)
            {
                TransactionObject = parentTransaction;
                if (this.CommandObject.Transaction == null)
                {
                    CommandObject.Transaction = TransactionObject;
                    _transactionNestedLevel++;
                }
            }
        }

        /// <summary>
        /// Rollback transaction - revert all changes within transaction
        /// </summary>
        public void RollbackTransaction()
        {
            _transactionNestedLevel--;
            if (_transactionNestedLevel < 0) _transactionNestedLevel = 0;

            if (TransactionObject != null && !IsConnectionShared)
            {
                TransactionObject.Rollback();
                TransactionObject = null;
            }
        }
        /// <summary>
        /// Returns true if any transaction is active
        /// </summary>
        public bool IsInTransaction
        {
            get { return TransactionObject != null; }
        }

        protected virtual ILog Logger { get; set; }

        #endregion

        #region LINQ integration
        /// <summary>
        /// Enlist System.Data.Linq.DataContext object into currently active transaction.
        /// If transaction wasn't started new transaction will be started
        /// </summary>
        /// <param name="dataContext"></param>
        protected void EnlistContextInTransaction(System.Data.Linq.DataContext dataContext)
        {
            if (dataContext == null) throw new ArgumentNullException("dataContext");
            if (!IsInTransaction) BeginTransaction();

            dataContext.Transaction = this.TransactionObject as DbTransaction;
        }

        #endregion


        #region IDENTITY FIELDS

        /// <summary>
        /// Get Last inserted Identity column value (in scope)
        /// </summary>
        /// <typeparam name="T">Identity column type</typeparam>
        /// <returns>Identity column value</returns>
        /// <remarks>Connection have to be oppened</remarks>
        [Obfuscation(Feature = "renaming", Exclude = true)]
        protected virtual T GetIdentityColumnValue<T>()
        {
            T pkID = default(T);

            string sqlText = "SELECT SCOPE_IDENTITY()";
            var tmpID = RunScalarQuery(sqlText);
            if (Convert.IsDBNull(tmpID))
                throw new InvalidOperationException("Failed to get Indetity column value");

            pkID = (T)tmpID;

            return pkID;
        }

        #endregion

        #region IDataReader Operations

        /// <summary>
        /// Get list of column names for searching for optional fields in the Business Object Class initializing
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<string> GetReaderColumnNames(IDataReader reader)
        {
            List<string> columnList = new List<string>(reader.FieldCount);

            for (int i = 0; i < reader.FieldCount; columnList.Add(reader.GetName(i++))) ;
            return columnList;
        }



        /// <summary>
        /// Merge 2 lists with rows IDs for Insert Into Database
        /// </summary>
        /// <typeparam name="T">Key data type</typeparam>
        /// <param name="currentDatabaseList">List with actual values in the database</param>
        /// <param name="listToSynchronize">Updating /new list</param>
        /// <param name="listToInsert">Returned IDs that need to be inserted</param>
        /// <param name="listToDelete">Returned IDs that need to be deleted</param>
        /// <remarks>Use this method when you need to update Many-To-Many table</remarks>
        public static void MergeList<T>(List<T> listToSynchronize, List<T> currentDatabaseList, out List<T> listToInsert, out List<T> listToDelete)
        {
            listToInsert = new List<T>();
            listToDelete = new List<T>();

            //Get item for insert on database
            foreach (T insertItem in listToSynchronize)
            {
                if (!currentDatabaseList.Contains(insertItem))
                {
                    listToInsert.Add(insertItem);
                }
            }
            //Get item for delete from database
            foreach (T deleteItem in currentDatabaseList)
            {
                if (!listToSynchronize.Contains(deleteItem))
                {
                    listToDelete.Add(deleteItem);
                }
            }
        }



        #endregion
    }

    public static class DbDatabaseExtensions
    {
        /// <summary>
        /// Replace empty parameter values to DBNull.Value
        /// Replace null & empty values to DbNull
        /// </summary>
        public static object AsSqlParameter(this object value)
        {
            return DBDatabase.SqlParameterSafe(value);
        }


        public static object AsSafeIntParameter(this bool? value)
        {
            int? intVal = null;
            if(value.HasValue)
            {
                intVal = value.Value ? 1 : 0;
            }

            return DBDatabase.SqlParameterSafe(intVal);
        }
    }

}
