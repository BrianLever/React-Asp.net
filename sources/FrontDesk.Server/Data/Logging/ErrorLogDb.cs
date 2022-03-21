using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using FrontDesk.Common.Data;
using FrontDesk.Server.Logging;

namespace FrontDesk.Server.Data.Logging
{
    public class ErrorLogDb : DBDatabase, IErrorLogRepository
    {
        #region Constructors

        public ErrorLogDb() : base(0) { }

        public ErrorLogDb(DbConnection sharedConnection)
            : base(sharedConnection)
        {
        }

        #endregion

        /// <summary>
        /// add new record
        /// </summary>
        /// <param name="logItem"></param>
        /// <returns></returns>
        public long Add(ErrorLog logItem)
        {
            string sql = @"
INSERT INTO dbo.ErrorLog(
    KioskID
    ,ErrorMessage
    ,ErrorTraceLog
    ,CreatedDate
)
VALUES(
     @KioskID
    ,@ErrorMessage
    ,@ErrorTraceLog
    ,@CreatedDate
);
SET @ErrorLogID = SCOPE_IDENTITY();
";
            CommandObject.Parameters.Clear();
            var IdParam = AddParameter("@ErrorLogID", DbType.Int64, ParameterDirection.Output);
            AddParameter("@KioskID", DbType.Int16).Value = logItem.KioskID.HasValue ? logItem.KioskID.Value : (object)DBNull.Value;
            AddParameter("@ErrorMessage", DbType.String).Value = SqlParameterSafe(logItem.ErrorMessage);
            AddParameter("@ErrorTraceLog", DbType.String).Value = SqlParameterSafe(logItem.ErrorTraceLog);
            AddParameter("@CreatedDate", DbType.DateTimeOffset).Value = logItem.CreatedDate;

            try
            {
                RunNonSelectQuery(sql);
                logItem.ErrorLogID = Convert.ToInt64(IdParam.Value);
            }
            finally
            {
                Disconnect();
            }
            return logItem.ErrorLogID;
        }

        /// <summary>
        /// delete record
        /// </summary>
        /// <param name="logItem"></param>
        /// <returns></returns>
        public int Delete(long errorLogID)
        {
            int affectedRows = 0;
            string sql = @"
DELETE FROM dbo.ErrorLog
WHERE ErrorLogID = @ErrorLogID;
";
            CommandObject.Parameters.Clear();
            AddParameter("@ErrorLogID", DbType.Int64).Value = errorLogID;

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
        /// <summary>
        /// Delete range of error log items
        /// </summary>
        /// <param name="errorLogIds"></param>
        /// <returns></returns>
        public int Delete(IEnumerable<long> errorLogIds)
        {
            int affectedRows = 0;
            StringBuilder sql = new StringBuilder(@"
DELETE FROM dbo.ErrorLog
WHERE ErrorLogID IN(");
            CommandObject.Parameters.Clear();
            int i = 0;
            foreach (var id in errorLogIds)
            {
                sql.AppendFormat("@Id{0},", i);
                AddParameter("@Id" + i, DbType.Int64).Value = id;
                i++;
            }
            sql.Length--;
            sql.Append(");");
            try
            {
                affectedRows = RunNonSelectQuery(sql.ToString());
            }
            finally
            {
                Disconnect();
            }
            return affectedRows;
        }
        /// <summary>
        /// Get error log filtered by date range
        /// </summary>
        public List<ErrorLog> Get(DateTimeOffset startDate, DateTimeOffset endDate, int startRowIndex, int maximumRows)
        {
            var result = new List<ErrorLog>();
            string sql = @"
;WITH tblA(RowNumber, ErrorLogID) as ( 
    SELECT TOP(@startRowIndex + @maxRows) 
     ROW_NUMBER() OVER ( ORDER BY e.CreatedDate DESC) as RowNumber, 
     e.ErrorLogID 
    FROM dbo.ErrorLog e
)
SELECT 
e.ErrorLogID, 
e.KioskID, 
e.ErrorMessage, 
e.ErrorTraceLog, 
e.CreatedDate,
NULL as KioskLabel
FROM dbo.ErrorLog e
    INNER JOIN tblA ON e.ErrorLogID = tblA.ErrorLogID
WHERE e.CreatedDate >= @StartDate AND e.CreatedDate < @EndDate
    AND tblA.RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY e.CreatedDate DESC
";
            ClearParameters();
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            AddParameter("@StartDate", DbType.DateTimeOffset).Value = startDate;
            AddParameter("@EndDate", DbType.DateTimeOffset).Value = endDate;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        result.Add(new ErrorLog(reader));
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return result;
        }

        /// <summary>
        /// Get error log filtered by date range
        /// </summary>
        public int GetCount(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            string sql = @"
SELECT COUNT(e.ErrorLogID) 
FROM dbo.ErrorLog e
WHERE e.CreatedDate >= @StartDate AND e.CreatedDate < @EndDate
";
            ClearParameters();
            AddParameter("@StartDate", DbType.DateTimeOffset).Value = startDate;
            AddParameter("@EndDate", DbType.DateTimeOffset).Value = endDate;

            try
            {
                return RunScalarQuery<int>(sql) ?? 0;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Get error log filtered by date range
        /// </summary>
        public DataSet GetAsDataSet(DateTimeOffset startDate, DateTimeOffset endDate, int startRowIndex, int maximumRows)
        {
            string sql = @"
;WITH tblA(RowNumber, ErrorLogID) as ( 
    SELECT TOP(@startRowIndex + @maxRows) 
     ROW_NUMBER() OVER ( ORDER BY e.CreatedDate DESC) as RowNumber, 
     e.ErrorLogID 
    FROM dbo.ErrorLog e
)
SELECT 
e.ErrorLogID, 
e.KioskID, 
e.ErrorMessage, 
e.ErrorTraceLog, 
e.CreatedDate,
NULL as KioskLabel
FROM dbo.ErrorLog e
    INNER JOIN tblA ON e.ErrorLogID = tblA.ErrorLogID
WHERE e.CreatedDate >= @StartDate AND e.CreatedDate < @EndDate
    AND tblA.RowNumber BETWEEN @startRowIndex AND (@startRowIndex + @maxRows)
ORDER BY e.CreatedDate DESC
";
            ClearParameters();
            AddParameter("@startRowIndex", DbType.Int32).Value = startRowIndex + 1;
            AddParameter("@maxRows", DbType.Int32).Value = maximumRows;

            AddParameter("@StartDate", DbType.DateTimeOffset).Value = startDate;
            AddParameter("@EndDate", DbType.DateTimeOffset).Value = endDate;

            try
            {
                return GetDataSet(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Get single error log item
        /// </summary>
        /// <param name="errorLogID"></param>
        /// <returns></returns>
        public ErrorLog Get(long errorLogID)
        {
            ErrorLog item = null;
            string sql = @"
SELECT 
e.ErrorLogID, 
e.KioskID, 
e.ErrorMessage, 
e.ErrorTraceLog, 
e.CreatedDate,
NULL as KioskLabel
FROM dbo.ErrorLog e
WHERE e.ErrorLogID = @ErrorLogID
";
            CommandObject.Parameters.Clear();
            AddParameter("@ErrorLogID", DbType.Int64).Value = errorLogID;

            try
            {
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        item = new ErrorLog(reader);
                    }
                }
            }
            finally
            {
                Disconnect();
            }
            return item;
        }
        /// <summary>
        /// Clear error log
        /// </summary>
        public void DeleteAll()
        {
            string sql = @"DELETE FROM dbo.ErrorLog;";
            CommandObject.Parameters.Clear();

            try
            {
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }
    }

}
