using Common.Logging;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace FrontDesk.Common.Debugging
{
    /// <summary>
    /// Write the initial exeption's message and stack trace into the application Debug and Trace
    /// </summary>
    public static class DebugLogger
    {
        private static ILog _logger = LogManager.GetLogger("General");

        /// <summary>
        /// Write promary expeption's message and stack trace into the System.Debugging.Trace context
        /// </summary>
        /// <param name="ex">Application exception</param>
        public static void TraceException(Exception ex)
        {
            TraceException(ex, string.Empty);
        }

        /// <summary>
        /// Write primary expeption's message and stack trace into the System.Debugging.Trace context
        /// </summary>
        /// <param name="ex">Application exception</param>
        /// <param name="customText">Custom text that writed before exception details</param>
        public static void TraceException(Exception ex, string customText)
        {
            _logger.ErrorFormat(customText, ex);
        }


        public static void WriteError(string format, params object[] args)
        {
            _logger.ErrorFormat(format, args);
        }
        public static void WriteWarning(string format, params object[] args)
        {
            _logger.WarnFormat(format, args);
        }

        /// <summary>
        /// Write promary expeption's message and stack trace into the System.Debugging.Debug context
        /// </summary>
        /// <param name="ex">Application exception</param>
        public static void DebugException(Exception ex)
        {
            _logger.Error(ex);
        }

        /// <summary>
        /// Write primary expeption's message and stack trace into the System.Debugging.Debug context
        /// </summary>
        /// <param name="ex">Application exception</param>
        /// <param name="customText">Custom text that writed before exception details</param>
        public static void DebugException(Exception ex, string customText)
        {
            _logger.Error(customText, ex);
        }

        public static void WriteTraceMessage(string message, params object[] args)
        {
            if (!String.IsNullOrEmpty(message)) _logger.InfoFormat(message, args);
        }

        public static void WriteObjectInformation(string message, object model)
        { 
            if(_logger.IsDebugEnabled)
            {
                _logger.Info(message);
                string json = JsonConvert.SerializeObject(model);
                _logger.Info(json);
            }
        }
    }
}
