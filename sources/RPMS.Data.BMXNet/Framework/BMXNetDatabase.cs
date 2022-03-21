using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using IndianHealthService.BMXNet;
using RPMS.Common.Configuration;
using RPMS.Common.Security;
using Common.Logging;
using CuttingEdge.Conditions;

namespace RPMS.Data.BMXNet.Framework
{

	public abstract class BMXNetDatabase : IDisposable
	{
		private static BMXNetConnectionProxy _connectionObjectProxy;
		private static readonly ReaderWriterLockSlim _bmxConnectionLock = new ReaderWriterLockSlim();

		private readonly ILog _logger = LogManager.GetLogger<BMXNetDatabase>();
		public BMXNetConnectionProxy Proxy { get { return _connectionObjectProxy; } }

		protected IDbConnection ConnectionObject;
		protected IDbCommand CommandObject;

		protected bool IsConnectionShared = false;
		/// <summary>
		/// True if ConnectionObject connected to database
		/// </summary>
		protected bool IsConnected;


		protected BMXNetDatabase()
		{
			ConnectionInfo connectionParams = BMXNetConnectionBuilder.FromConnectionString(ConfigurationManager.ConnectionStrings["IHS"].ConnectionString);
			_logger.DebugFormat("Received EHR connection parameters. Server: {0}, Port: {1}.", connectionParams.ServerAddress, connectionParams.Port);

			CommandObject = null;
			InitBmxNetConnection(connectionParams);

			IsConnected = false;

		}

		private void InitBmxNetConnection(ConnectionInfo connectionParams)
		{
			_bmxConnectionLock.EnterUpgradeableReadLock();
			try
			{
				if (_connectionObjectProxy != null)
				{
					//compare access and verify codes
					if (string.CompareOrdinal(_connectionObjectProxy.ConnectionParams.AccessCode, connectionParams.AccessCode) == 0 &&
					    string.CompareOrdinal(_connectionObjectProxy.ConnectionParams.VerifyCode, connectionParams.VerifyCode) == 0)
					{
						return;
					}


					_logger.Info("Access or Verify codes has been changed. Reconnecting to the BXMNET...");

					_bmxConnectionLock.EnterWriteLock();
					try
					{
						try
						{
							_connectionObjectProxy.CloseConnection();
						}
						catch (Exception ex)
						{
							_logger.Warn("Error has occured during BMXConnection closure.", ex);
						}
						finally
						{
							_connectionObjectProxy = null;

						}
						_connectionObjectProxy = new BMXNetConnectionProxy(connectionParams);
					}
					finally
					{
						_bmxConnectionLock.ExitWriteLock();
					}
				}
				else
				{

					_bmxConnectionLock.EnterWriteLock();
					try
					{
						_connectionObjectProxy = new BMXNetConnectionProxy(connectionParams);
					}
					finally
					{
						_bmxConnectionLock.ExitWriteLock();
					}

				}
			}
			finally
			{
				_bmxConnectionLock.ExitUpgradeableReadLock();
			}
		}

		protected BMXNetDatabase(IRpmsCredentialsService credentialsService)
		{
			if (credentialsService == null)
			{
				throw new ArgumentNullException("credentialsService");
			}


			ConnectionInfo connectionParams = BMXNetConnectionBuilder.FromConnectionString(ConfigurationManager.ConnectionStrings["IHS"].ConnectionString);
			_logger.DebugFormat("Received EHR connection parameters. Server: {0}, Port: {1}.", connectionParams.ServerAddress, connectionParams.Port);


			var credentials = credentialsService.GetCredentialsCached();

			Condition.Requires(credentials)
				.IsNotNull(
					"credentialsService.GetAllCredentialsCached should return non-null model with access and verify code values.");

			connectionParams.AccessCode = credentials.AccessCode;
			connectionParams.VerifyCode = credentials.VerifyCode;

			CommandObject = null;

			InitBmxNetConnection(connectionParams);

			IsConnected = false;

		}


		/// <summary>
		/// Open connection
		/// </summary>
		public void Connect()
		{
			_logger.Debug("BMXNetDatabase: Connecting.");
			if (!IsConnected && !IsConnectionShared)
			{
				_bmxConnectionLock.EnterReadLock();
				try
				{
					ConnectionObject = _connectionObjectProxy.OpenConnection();
				}
				finally
				{
					_bmxConnectionLock.ExitReadLock();
				}

				CommandObject = ConnectionObject.CreateCommand();

				IsConnected = true;
			}
			_logger.Debug("BMXNetDatabase: Connected.");
		}
		/// <summary>
		/// Close connection if it opened
		/// </summary>
		public void Disconnect()
		{
			if (IsConnected && !IsConnectionShared)
			{
				//DO Nothing
				//if (_connectionObjectProxy != null) _connectionObjectProxy.CloseConnection();
				IsConnected = false;
			}
		}

		#region [Add Command Paramenter]

		protected IDataParameter AddParameter(string name, DbType type)
		{
			IDataParameter parameter = new BMXNetParameter();
			parameter.DbType = type;
			parameter.ParameterName = name;
			CommandObject.Parameters.Add(parameter);
			return parameter;
		}

		protected IDataParameter CreateParameter(string name, DbType type)
		{
			IDataParameter parameter = new BMXNetParameter();
			parameter.DbType = type;
			parameter.ParameterName = name;
			return parameter;
		}

		protected IDataParameter AddParameter(string name, DbType type, ParameterDirection direction)
		{
			IDataParameter parameter = AddParameter(name, type);
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
			var stopwatch = new Stopwatch();
			Connect();

			CommandObject.CommandText = FixSqlCommandText(sqlText);
			CommandObject.CommandType = CommandType.Text;

			_logger.DebugFormat("Execute BMX-SQL: {0}", sqlText);


			if (_logger.IsDebugEnabled)
			{
				stopwatch.Start();
			}
			var oRd = CommandObject.ExecuteReader();

			if (_logger.IsDebugEnabled)
			{
				_logger.DebugFormat("RunSelectQuery: SQL query execute time: {0}", stopwatch.Elapsed);
			}

			return oRd;
		}



		/// <summary>
		/// Execute non select query
		/// </summary>
		/// <param name="sqlText">Non select sql statement</param>
		/// <returns>Number of rows affected</returns>
		protected int RunNonSelectQuery(string sqlText)
		{
			Connect();
			int result;
			var stopwatch = new Stopwatch();
			CommandObject.CommandText = FixSqlCommandText(sqlText);
			CommandObject.CommandType = CommandType.Text;

			_logger.DebugFormat("Execute BMX-SQL: {0}", sqlText);

			if (_logger.IsDebugEnabled)
			{
				stopwatch.Start();
			}
			result = CommandObject.ExecuteNonQuery();
			if (_logger.IsDebugEnabled)
			{
				_logger.DebugFormat("RunNonSelectQuery: SQL query execute time: {0}", stopwatch.Elapsed);
			}
			return result;
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
		public static object SqlParameterSafe(object value, bool bNull, object EmptyValueExpression)
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
				sValue = Convert.ToString(value).Trim().Replace("'", "''");
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
		public static object SqlParameterSafe(object value)
		{
			return SqlParameterSafe(value, true, null);
		}

		/// <summary>
		/// Prepeare string value for LIKE statement
		/// </summary>
		/// <param name="likeValue">Search value</param>
		/// <param name="UseProtection">Escape all special characters</param>
		/// <param name="likeCondition">Format string for searching</param>
		public static object SqlLikeStringPrepeare(string likeValue, bool UseProtection, LikeCondition likeCondition)
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
		public static object SqlLikeStringPrepeare(string likeValue, LikeCondition likeCondition)
		{
			return SqlLikeStringPrepeare(likeValue, true, likeCondition);
		}

		/// <summary>
		/// Replace empty string with DBNull.Value
		/// SQL Injection protection is turned on
		/// </summary>
		public static object SqlLikeStringPrepeare(string likeValue)
		{
			return SqlLikeStringPrepeare(likeValue, true, LikeCondition.None);
		}


		/// <summary>
		/// Replace empty string with % that will select all rows if condition is empty
		/// SQL Injection protection is turned on
		/// </summary>
		public static object SqlLikeAllIfEmpty(string likeValue)
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
		public static object SqlLikeAllIfEmpty(string likeValue, LikeCondition likeCondition)
		{
			if (string.IsNullOrEmpty(likeValue))
			{
				return "%";
			}
			else
				return SqlLikeStringPrepeare(likeValue, true, likeCondition);

		}


		#endregion




		#region IDisposable Members

		public void Dispose()
		{
			Disconnect();
			GC.SuppressFinalize(this);
		}

		~BMXNetDatabase()
		{
			Disconnect();
		}

		#endregion
	}

	/// <summary>
	/// SQL LIKE statement condition type
	/// </summary>
	[Flags]
	public enum LikeCondition
	{
		None = 0,
		StartsWith = 1,
		Contains = 2
	}

}
