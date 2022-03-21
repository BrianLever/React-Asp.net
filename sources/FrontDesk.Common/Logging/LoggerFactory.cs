using Common.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FrontDesk.Common.Logging
{
    public static class LoggerFactory
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LoggerFactory));

        public static ILog GetLogger()
        {
            return logger;

        }


        public static ILog GetLogger(string name)
        {
            return LogManager.GetLogger(name);
        }
    }
}
