using Common.Logging;
using ScreenDox.Server.Common.Services;
using ScreenDoxKioskInstallApi.Controllers;
using ScreenDoxKioskInstallApi.Security;

using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ScreenDoxKioskInstallApi.Filters
{
    public class KioskAuthenticationFilter : AuthorizeAttribute
    {
        private ILog _logger = LogManager.GetLogger<KioskAuthenticationFilter>();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (Authorize(actionContext))
            {
                return;
            }

            HandleUnauthorizedRequest(actionContext);
        }


        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            base.HandleUnauthorizedRequest(actionContext);
        }

        private bool Authorize(HttpActionContext actionContext)
        {
            var requestScope = actionContext.Request.GetDependencyScope();

            var kioskService = requestScope.GetService(typeof(IKioskService)) as IKioskService;

            string kioskKey = string.Empty;
            string kioskSecret = string.Empty;

            var headers = actionContext.Request.Headers;

            if (headers.Contains(AuthHeaderDescriptor.KioskKey))
            {
                kioskKey = headers.GetValues(AuthHeaderDescriptor.KioskKey).FirstOrDefault();
            }
            var kioskSecretEncoded = headers.Authorization?.Parameter;

            var controller = actionContext.ControllerContext.Controller as KioskApiControllerBase;

            if (controller != null)
            {
                controller.KioskKey = kioskKey;
            }

            try
            {
                //decode secret key from Base64

                if (string.IsNullOrWhiteSpace(kioskSecretEncoded))
                {

                    _logger.WarnFormat("Failed to validate kiosk. Secret is empty. Key:{0}. Secret: {1}", kioskKey, kioskSecret);

                    return false;
                }
                kioskSecret = Encoding.UTF8.GetString(Convert.FromBase64String(kioskSecretEncoded));


                bool result = kioskService.ValidateKiosk(kioskKey, kioskSecret);

                if (!result)
                {
                    _logger.WarnFormat("Rejected unauthorized request from the kiok. Key:{0}. Secret: {1}", kioskKey, kioskSecret);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.ErrorFormat("Failed to validate kiosk. Server error. Key:{0}. Secret: {1}", ex, kioskKey, kioskSecret);

                return false;
            }
        }

    }
}