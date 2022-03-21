using Common.Logging;

using FrontDesk.Common.InfrastructureServices;

using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin;
using Microsoft.Owin.Security.Jwt;

using Owin;

using ScreenDox.Server.Api.Infrastructure;

using System;
using System.Text;
using System.Web.Http;

using Web.Api.Infrastructure.Auth;

[assembly: OwinStartup(typeof(ScreenDox.Server.Api.Startup))]
namespace ScreenDox.Server.Api
{
    public class Startup
    {
        private readonly ILog _logger = LogManager.GetLogger<Startup>();
        protected IJwtFactory jwtFactory = new JwtFactory(new TimeService());


        public void Configuration(IAppBuilder app)
        {
            var httpConfig = new HttpConfiguration();

            _logger.Info("Called Startup.Configuration.");
            try
            {

                ConfigureAuth(app);

                WebApiConfig.Register(httpConfig);

                var container = DepedencyConfig.Register(httpConfig);
                
                app.UseWebApi(httpConfig);
                app.UseStaticFiles();

                app.UseAutofacMiddleware(container);


                // add swagger
                SwaggerConfig.Register(app);
            }
            catch (Exception ex)
            {
                _logger.Error("Init failure.", ex);
                throw;
            }

        }



        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseJwtBearerAuthentication(
               new JwtBearerAuthenticationOptions
               {
                   AuthenticationMode = Microsoft.Owin.Security.AuthenticationMode.Active,
                   TokenValidationParameters = jwtFactory.GetTokenValidationParameters()
               }
               );

        }


    }
}