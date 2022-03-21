using System;

namespace FrontDesk.Server.Logging
{
    public interface IErrorLoggerService
    {
        ErrorLog Add(ErrorLog logItem);
        ErrorLog Add(string errorMessage, string traceLog, short? kioskID);
        ErrorLog AddServerException(string errorMessage, Exception exception);
    }
}