using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndianHealthService.BMXNet;
using System.Threading;
using System.Data;
using Common.Logging;

namespace RPMS.Data.BMXNet.Framework
{
	public sealed class BMXNetConnectionProxy : IDisposable
	{
		private readonly ILog _logger = LogManager.GetLogger<BMXNetConnectionProxy>();


		public ConnectionInfo ConnectionParams { get; private set; }
		private static ReaderWriterLockSlim _rwl = new ReaderWriterLockSlim();

		private BMXNetLib _rpx = null;
		private BMXNetConnection _conn = null;

		public BMXNetConnection Connection { get { return _conn; } }

		//Return BMXLib component safely
		public BMXNetLib BMXRpcProxy
		{
			get
			{
				BMXNetLib result = null;
				_rwl.EnterReadLock();
				try
				{
					result = _rpx;
				}
				finally
				{
					_rwl.ExitReadLock();
				}
				return result;
			}
		}


		public BMXNetConnectionProxy(string connectionString)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentNullException("connectionString");

			ConnectionParams = BMXNetConnectionBuilder.FromConnectionString(connectionString);

		}

		public BMXNetConnectionProxy(ConnectionInfo connectionInfo)
		{
			ConnectionParams = connectionInfo;
		}

		public BMXNetConnection OpenConnection()
		{
			_logger.Debug("BMXNetConnectionProxy: Entered OpenConnection");

			_rwl.EnterUpgradeableReadLock();

			try
			{
				//if connection is open, just return existing connection
				if (_rpx != null && _rpx.Connected == true && _conn != null)
				{
					_logger.Debug("BMXNetConnectionProxy: returning open connection.");
					return _conn;
				}
				else
				{
					_rwl.EnterWriteLock();
					try
					{
						BMXNetLib lib = null;
						if (_rpx != null)
						{
							_logger.Debug("BMXNetConnectionProxy: closing existing connection.");
							_rpx.CloseConnection();
							lib = _rpx;
						}
						else
						{
							lib = new BMXNetLib();
						}

						lib.MServerPort = ConnectionParams.Port;
						lib.MServerNamespace = ConnectionParams.Namespace;

						_logger.Debug("BMXNetConnectionProxy: opening new BMX connection...");
						if (lib.OpenConnection(ConnectionParams.ServerAddress, ConnectionParams.AccessCode, ConnectionParams.VerifyCode))
						{
							_rpx = lib;
							_rpx.AppContext = "BMXRPC";
							_logger.Debug("BMXNetConnectionProxy: opened BMX connection. Creating BMXNetConnection...");

							_conn = new BMXNetConnection(_rpx);

							_logger.Debug("BMXNetConnectionProxy: returning BMXNetConnection...");

							return _conn;
						}
						else
						{
							throw new BMXNetException("Connection to " + ConnectionParams.ToString());
						}
					}
					finally
					{
						_rwl.ExitWriteLock();
					}


				}
			}
			finally
			{
				_logger.Debug("BMXNetConnectionProxy: exit lock.");
				_rwl.ExitUpgradeableReadLock();
			}
		}

		public void CloseConnection()
		{
			_logger.Debug("BMXNetConnectionProxy: entered CloseConnection.");
			if (_rpx != null || _conn != null)
			{
				_rwl.EnterWriteLock();
				try
				{
					if (_conn != null)
					{
						_logger.Debug("BMXNetConnectionProxy: closing BMXNETConnection.");
						_conn.Close();
						_conn = null;

					}
					else
					{
						_logger.Debug("BMXNetConnectionProxy: closing BMXLib connection.");
						_rpx.CloseConnection();
					}


				}
				finally
				{
					_logger.Debug("BMXNetConnectionProxy: CloseConnection: exit lock.");
					_rwl.ExitWriteLock();
				}

			}
		}


		#region IDisposable Members

		public void Dispose()
		{
			CloseConnection();
		}

		#endregion
	}
}
