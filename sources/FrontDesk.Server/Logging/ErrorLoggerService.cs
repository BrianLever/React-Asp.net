using Common.Logging;
using FrontDesk.Common.Debugging;
using FrontDesk.Server.Data.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Server.Logging
{
    public class ErrorLoggerService : IErrorLoggerService
    {
        private readonly IErrorLogRepository _repository;
        private readonly ILog _logger = LogManager.GetLogger<ErrorLoggerService>();


        public ErrorLoggerService(): this(new ErrorLogDb())
        {

        }

        public ErrorLoggerService(IErrorLogRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        /// <summary>
        /// add new record
        /// </summary>
        public ErrorLog Add(ErrorLog logItem)
        {
            logItem.CreatedDate = DateTimeOffset.Now;
            try
            {
                logItem.ErrorLogID = _repository.Add(logItem);
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
        public ErrorLog Add(string errorMessage, string traceLog, Int16? kioskID)
        {
            return Add(new ErrorLog() { ErrorMessage = errorMessage, ErrorTraceLog = traceLog, KioskID = kioskID });
        }

        /// <summary>
        /// Log server exception
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">exception is null</exception>
        public ErrorLog AddServerException(string errorMessage, Exception exception)
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

            string message = !string.IsNullOrEmpty(errorMessage) ? errorMessage + " " + exception.Message : exception.Message;

            _logger.Error(message, exception);

            return Add(new ErrorLog() { ErrorMessage = message, ErrorTraceLog = exception.ToString(), KioskID = null });
        }

    }
}
