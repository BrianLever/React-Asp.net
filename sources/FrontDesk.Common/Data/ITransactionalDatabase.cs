using System.Data;
using System.Data.Common;

namespace FrontDesk.Common.Data
{
    public interface ITransactionalDatabase
    {
        /// <summary>
        /// Open connection
        /// </summary>
        void Connect();

        /// <summary>
        /// Close connection if it opened
        /// </summary>
        void Disconnect();

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
        void StartConnectionSharing();

        /// <summary>
        /// Stop connection sharing
        /// </summary>
        void StopConnectionSharing();

        /// <summary>
        /// True if connection sharing is activated
        /// </summary>
        bool IsConnectionShared { get; }

        /// <summary>
        /// Returns true if any transaction is active
        /// </summary>
        bool IsInTransaction { get; }

        /// <summary>
        /// Begin new transaction
        /// </summary>
        void BeginTransaction();

        /// <summary>
        /// Begin new transaction with non-default isolation level
        /// </summary>
        /// <param name="ilLevel"></param>
        void BeginTransaction(IsolationLevel ilLevel);

        /// <summary>
        /// Commit transaction - save changes
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Enlist current object into top transaction
        /// </summary>
        /// <param name="parentTransaction"></param>
        void EnlistTransaction(DbTransaction parentTransaction);

        /// <summary>
        /// Rollback transaction - revert all changes within transaction
        /// </summary>
        void RollbackTransaction();
    }
}