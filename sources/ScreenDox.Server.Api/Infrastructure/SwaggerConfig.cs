using NSwag.AspNet.Owin;

using Owin;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Infrastructure
{
    /// <summary>
    /// Open API configuration
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Register method
        /// </summary>
        /// <param name="app"></param>
        public static void Register(IAppBuilder app)
        {
            app.Map("/swagger", x =>
            {

                x.UseSwaggerUi3(typeof(Startup).Assembly, settings =>
                {

                    settings.GeneratorSettings.Title = "ScreenDox Server API";
                    settings.GeneratorSettings.Version = "1.0.0";
                    settings.GeneratorSettings.Description = "ScreenDox Server API for headless ";
                    settings.ValidateSpecification = true;


                    settings.MiddlewareBasePath = "/swagger";
                    settings.GeneratorSettings.DefaultUrlTemplate = "api/{controller}/{action}/{id}";

                    settings.GeneratorSettings.GenerateExamples = true;

                    //settings.GeneratorSettings.DocumentProcessors.Add(
                    //    new SecurityDefinitionAppender("KioskKey",
                    //    new OpenApiSecurityScheme
                    //    {
                    //        Type = OpenApiSecuritySchemeType.ApiKey,
                    //        Name = AuthHeaderDescriptor.KioskKey,
                    //        In = OpenApiSecurityApiKeyLocation.Header
                    //    }));
                    //settings.GeneratorSettings.DocumentProcessors.Add(
                    //    new SecurityDefinitionAppender("KioskSecret",
                    //    new OpenApiSecurityScheme
                    //    {
                    //        Type = OpenApiSecuritySchemeType.ApiKey,
                    //        Name = "Authorization",
                    //        In = OpenApiSecurityApiKeyLocation.Header,
                    //        Description = "Bearer token"
                    //    }));

                    //settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("KioskKey"));
                    //settings.GeneratorSettings.OperationProcessors.Add(new OperationSecurityScopeProcessor("KioskSecret"));

                });
            });


        }
    }
}