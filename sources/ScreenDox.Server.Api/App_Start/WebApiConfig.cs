using Newtonsoft.Json;

using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;

namespace ScreenDox.Server.Api
{
    /// <summary>
    /// Register Web Api configurations
    /// </summary>
    public static class WebApiConfig
    {
        /// <summary>
        /// Register configuration
        /// </summary>
        /// <param name="config"></param>
        public static void Register(HttpConfiguration config)
        {

            // for jwt authentication
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter("Bearer"));
            config.Filters.Add(new AuthorizeAttribute()); // force authentication to all methods

            // Web API configuration and services
            config.Filters.Add(new Infrastructure.ExceptionFilterAttribute());

            SetFormatters(config);

            // enables CORS for OPTIONS verb handling
            var cors = new EnableCorsAttribute("*", "*", "*");
            config.EnableCors(cors);
           
            // Web API routes
            config.MapHttpAttributeRoutes();

            SetRoutes(config);
            
            AutoMapperConfiguration.Configure();

        }

        private static void SetRoutes(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
             name: "SystemToolsRouting",
             routeTemplate: "api/systemtools/{controller}/{id}",
             defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }

            );

            config.Routes.MapHttpRoute(
                name: "ScreeningProfileFrequencyApi",
                routeTemplate: "api/screeningprofile/{id}/frequency",
                defaults: new
                {
                    controller = "screeningprofilefrequency",
                }
            );

            config.Routes.MapHttpRoute(
                name: "ScreeningProfileFrequencyApiList",
                routeTemplate: "api/screeningprofile/frequency/list",
                defaults: new
                {
                    controller = "screeningprofilefrequency",
                }
            );

            config.Routes.MapHttpRoute(
                name: "ScreeningProfileAgeApi",
                routeTemplate: "api/screeningprofile/{id}/age",
                defaults: new
                {
                    controller = "screeningprofileage",
                }
            );

            config.Routes.MapHttpRoute(
               name: "ScreeningProfileAgeGroupApi",
               routeTemplate: "api/screeningprofile/age/groups",
               defaults: new
               {
                   controller = "screeningprofileage",
               }
            );
        }


        private static void SetFormatters(HttpConfiguration config)
        {
            config.Formatters.JsonFormatter.SupportedMediaTypes
              .Add(new MediaTypeHeaderValue("application/json"));

            JsonMediaTypeFormatter jsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            JsonSerializerSettings jSettings = new JsonSerializerSettings()
            {
                Formatting = Formatting.None,
                DateTimeZoneHandling = DateTimeZoneHandling.Utc
            };
            jsonFormatter.SerializerSettings = jSettings;
        }
    }
}
