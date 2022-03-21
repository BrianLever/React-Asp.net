namespace ScreenDoxKioskInstallApi.Infrastructure
{
    using Common.Logging;
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Filters;

    public class ExceptionFilterAttribute : System.Web.Http.Filters.ExceptionFilterAttribute
    {
        private ILog _logger = LogManager.GetLogger("default");

        public override void OnException(HttpActionExecutedContext context)
        {
            _logger.Error("Error has occured.", context.Exception);


            if (context.Exception is Exception)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = "Internal server error"
                };
            }
        }
    }
}