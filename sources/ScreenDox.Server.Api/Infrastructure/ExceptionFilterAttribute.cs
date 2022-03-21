using Common.Logging;

using FrontDesk.Common.Entity;
using FrontDesk.Server.Logging;

using ScreenDox.Server.Resources;

using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Filters;

namespace ScreenDox.Server.Api.Infrastructure
{
    /// <summary>
    /// Default Exception handling filter
    /// </summary>
    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        private ILog _logger = LogManager.GetLogger("default");

        /// <summary>
        /// Exception handler
        /// </summary>
        /// <param name="context">Context</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NonValidEntityException)
            {
                _logger.WarnFormat("Validation error has occured. Message: {0}.", context.Exception, context.Exception.Message);

                ResponseDataFactory.ThrowBadRequestMessage(new[] { context.Exception.Message });
            }
            else if (context.Exception is ApplicationException)
            {
                _logger.WarnFormat("Validation error has occured. Message: {0}.", context.Exception, context.Exception.Message);

                ResponseDataFactory.ThrowBadRequestMessage(new[] { context.Exception.Message });
            }
            else if (context.Exception is HttpResponseException) // this is handled exception. Do not log
            {

                context.Response = ((HttpResponseException)context.Exception).Response;
                return;
            }

            // handle unhandled exception

            ErrorLog.AddServerException("Error has occured",context.Exception);
            
            _logger.Error("Error has occured.", context.Exception);

            if (context.Exception is Exception)
            {
                var result = new ValidationFailureResponse(new string[] { TextMessages.UnhandledExceptionMessage});

                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = "Internal server error",
                    Content = new StringContent(result.ToString()),
                };
            }
        }
    }
}