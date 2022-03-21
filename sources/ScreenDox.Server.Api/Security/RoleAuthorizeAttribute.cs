using ScreenDox.Server.Api.Infrastructure;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace ScreenDox.Server.Api.Security
{
    public class RestrictAccessToRolesAttribute : AuthorizationFilterAttribute
    {
        private string[] _allowedRoles;

        public RestrictAccessToRolesAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
           var principal = HttpContext.Current.User as ClaimsPrincipal;


            if (!(principal?.Identity?.IsAuthenticated ?? false))
            {
                actionContext.Response = new System.Net.Http.HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            }
            else
            {
                // role 
                string[] allowedRoles = _allowedRoles.ToArray(); //Roles.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var role = principal.FindFirstValue(ClaimTypes.Role);

                bool allowAccess = allowedRoles.Any(x => string.Compare(x, role, true) == 0);

                if(!allowAccess)
                {
                    var result = new ValidationFailureResponse(new string[] { "Access denied" });
                    var httpMessage = new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.Forbidden,
                        ReasonPhrase = "Access denied",
                        Content = new StringContent(result.ToString()),
                    };
                    httpMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    
                    actionContext.Response = httpMessage;
                }
            }
        }
    }
}