using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using FrontDesk.Common.Data;
using FrontDesk.Common.Debugging;
using FrontDesk.Server.Data.Logging;

namespace FrontDesk.Server.Logging
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed - need for datasources on pages
    public class ErrorLog
    {
        #region properties

        public long ErrorLogID { get; set; }
        public short? KioskID { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorTraceLog { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public string KioskLabel { get; internal set; }
        #endregion

        #region constructors

        public ErrorLog() { } 

        public ErrorLog(IDataReader reader)
        {
            ErrorLogID = Convert.ToInt64(reader["ErrorLogID"]);
            KioskID = !Convert.IsDBNull(reader["KioskID"]) ? Convert.ToInt16(reader["KioskID"]) : (short?)null;
            ErrorMessage = Convert.ToString(reader["ErrorMessage"]);
            ErrorTraceLog = Convert.ToString(reader["ErrorTraceLog"]);
            CreatedDate = (DateTimeOffset)(reader["CreatedDate"]);

            if (reader.FieldCount > 4)
            {
                var cols = DBDatabase.GetReaderColumnNames(reader);
                if(cols.Contains("KioskLabel"))
                {
                    KioskLabel = Convert.ToString(reader["KioskLabel"]);
                }
            }
        }

        #endregion

        #region Static Methods

        private static ErrorLogDb DbObject { get { return new ErrorLogDb(); } }
        /// <summary>
        /// Get error log filtered by date range
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate">End date. If null, select all items till the next day after today</param>
        /// <returns></returns>
        public static List<ErrorLog> Get(DateTimeOffset? startDate, DateTimeOffset? endDate, int startRowIndex, int maximumRows)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTimeOffset.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTimeOffset.Now.AddDays(1);
            }
            else
            {
                endDate = endDate.Value.AddDays(1);
            }

            return DbObject.Get(startDate.Value, endDate.Value, startRowIndex, maximumRows);
        }

        public static int GetCount(DateTimeOffset? startDate, DateTimeOffset? endDate)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTimeOffset.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTimeOffset.Now.AddDays(1);
            }
            else
            {
                endDate = endDate.Value.AddDays(1);
            }

            return DbObject.GetCount(startDate.Value, endDate.Value);
        }

        /// <summary>
        /// Get error log filtered by date range
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate">End date. If null, select all items till the next day after today</param>
        /// <returns></returns>
        public static DataSet GetForExport(DateTimeOffset? startDate, DateTimeOffset? endDate, int startRowIndex, int maximumRows)
        {
            if (!startDate.HasValue)
            {
                startDate = DateTimeOffset.MinValue;
            }
            if (!endDate.HasValue)
            {
                endDate = DateTimeOffset.Now.AddDays(1);
            }
            else
            {
                endDate = endDate.Value.AddDays(1);
            }

            return DbObject.GetAsDataSet(startDate.Value, endDate.Value, startRowIndex, maximumRows);
        }

        /// <summary>
        /// Get single error log item
        /// </summary>
        public static ErrorLog Get(long errorLogID)
        {
            return DbObject.Get(errorLogID);
        }
        /// <summary>
        /// add new record
        /// </summary>
        public static ErrorLog Add(ErrorLog logItem)
        {
            logItem.CreatedDate = DateTimeOffset.Now;
            try
            {
                logItem.ErrorLogID = DbObject.Add(logItem);
                
                // write to file
                DebugLogger.WriteError(logItem.ErrorMessage);
            }
            catch (Exception ex)
            {
                DebugLogger.TraceException(ex, "Failed to add new item to the Error Log table.");
            }
            return logItem;
        }
        
        /// <summary>
        /// add new record to error log
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="traceLog"></param>
        /// <param name="kioskID"></param>
        /// <returns></returns>
        public static ErrorLog Add(string errorMessage, string traceLog, Int16? kioskID)
        {
           return Add(new ErrorLog(){ ErrorMessage = errorMessage, ErrorTraceLog = traceLog, KioskID = kioskID});
        }

        /// <summary>
        /// Log server exception
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">exception is null</exception>
        public static ErrorLog AddServerException(string errorMessage, Exception exception)
        {
            if (exception == null) throw new ArgumentNullException("exception");
            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }
            if (!string.IsNullOrEmpty(errorMessage) && !errorMessage.EndsWith(".") && !errorMessage.EndsWith(","))
            {
                errorMessage = errorMessage + ".";
            }

            string message = !string.IsNullOrEmpty(errorMessage)? errorMessage + " " + exception.Message : exception.Message;


            return Add(new ErrorLog() { ErrorMessage = message, ErrorTraceLog = exception.ToString(), KioskID = null });
        }
        
        /// <summary>
        /// delete record
        /// </summary>
        public static bool Delete(long errorLogID)
        {
            return DbObject.Delete(errorLogID) > 0;
        }
        /// <summary>
        /// Delete range of error log items
        /// </summary>
        public static bool Delete(IEnumerable<long> errorLogIds)
        {
            return DbObject.Delete(errorLogIds) > 0;
        }

        #endregion
        /// <summary>
        /// Clear error log
        /// </summary>
        public static void DeleteAll()
        {
            DbObject.DeleteAll();
        }
    }
}
