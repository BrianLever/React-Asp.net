using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Newtonsoft.Json;

namespace RPMS.Common.Extensions
{
    public static class LoggingExtensions
    {
        public static void LogPayload<T>(this T value, ILog log)
        {
            if (log == null) return;
            if (value == null) return;

            if (log.IsTraceEnabled)
            {
               log.TraceFormat("[Log payload][{0}] {1}", typeof(T), JsonConvert.SerializeObject(value));
            }
        }
    }
}
