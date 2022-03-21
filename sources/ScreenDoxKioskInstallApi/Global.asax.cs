using NSwag;
using NSwag.AspNet.Owin;
using NSwag.Generation.Processors.Security;
using ScreenDoxKioskInstallApi.Security;
using System.Web.Http;
using System.Web.Routing;

namespace ScreenDoxKioskInstallApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            GlobalConfiguration.Configure(DepedencyConfig.Register);

            RouteTable.Routes.MapOwinPath("swagger", app =>
              {
                  app.UseSwaggerUi3(typeof(WebApiApplication).Assembly, settings =>
                  {
                      settings.GeneratorSettings.Title = "ScreenDox Kiosk Installation API";
                      settings.GeneratorSettings.Version = "1.0.0";
                      settings.GeneratorSettings.Description = "ScreenDox Kiosk Installation API for automatic upgrade of kiosk applications";
                      settings.ValidateSpecification = true;

                      settings.MiddlewareBasePath = "/swagger";
                      settings.GeneratorSettings.DefaultUrlTemplate = "api/{controller}/{action}/{id}";

                      settings.GeneratorSettings.DocumentProcessors.Add(
                          new SecurityDefinitionAppender("KioskKey",
                          new OpenApiSecurityScheme
                          {
                              Type = OpenApiSecuritySchemeType.ApiKey,
                              Name = AuthHeaderDescriptor.KioskKey,
                              In = OpenApiSecurityApiKeyLocation.Header
                          }));
                      settings.GeneratorSettings.DocumentProcessors.Add(
                          new SecurityDefinitionAppender("KioskSecret",
                          new OpenApiSecurityScheme
                          {
                              Type = OpenApiSecuritySchemeType.ApiKey,
                              Name = "Authorization",
                              In = OpenApiSecurityApiKeyLocation.Header,
                              Description = "Bearer token"
                          }));

                      settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("KioskKey"));
                      settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("KioskSecret"));

                  });
              });



        }
    }
}
