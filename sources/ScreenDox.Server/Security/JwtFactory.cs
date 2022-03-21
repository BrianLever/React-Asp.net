using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

using Common.Logging;

using FrontDesk.Common.Extensions;
using FrontDesk.Common.InfrastructureServices;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ScreenDox.Server.Api.Security;
using ScreenDox.Server.Api.Security.Models;
using ScreenDox.Server.Models;
using ScreenDox.Server.Security.Configuration;

namespace Web.Api.Infrastructure.Auth
{
    public class JwtFactory : IJwtFactory
    {

        private readonly ITimeService _timeService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly ILog _logger = LogManager.GetLogger<JwtFactory>();

        public string ApplicationId { get; private set; } // = "application_id";
        public string Issuer { get; private set; }// = "https://screendox.com";

        protected string SecretKey { get; private set; } // = "my_secret_key_.1!2345";
        /// <summary>
        /// Token validity diration
        /// </summary>
        public TimeSpan ValidFor { get; private set; }

        public JwtFactory(ITimeService timeService)
        {
            _timeService = timeService ?? throw new ArgumentNullException(nameof(timeService));

            var config = SecuritySecretsConfig.GetConfiguration();
            SecretKey = config.SecretKey;
            Issuer = config.Issuer;
            ApplicationId = config.ApplicationId;
            ValidFor = TimeSpan.FromMinutes(config.TokenExpirationInMin);
        }

        public AccessToken GenerateEncodedToken(IUserPrincipal principal)
        {
            var jwtTokenOptions = GetJwtOptions();

            //Create a List of Claims, Keep claims name short    
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            permClaims.Add(new Claim(JwtTokenClaims.UserId, principal.UserID.As<string>()));
            permClaims.Add(new Claim(JwtTokenClaims.FullName, principal.FullName));
            permClaims.Add(new Claim(ClaimTypes.Email, principal.Email ?? string.Empty));
            permClaims.Add(new Claim(JwtTokenClaims.BranchLocationId, principal.BranchLocationID.As<string>() ?? string.Empty));
            permClaims.Add(new Claim(JwtTokenClaims.UniqueName, principal.UserName));
            permClaims.Add(new Claim(ClaimTypes.Role, principal.RoleName));
            permClaims.Add(new Claim(JwtTokenClaims.IssuedAt,
                    ToUnixEpochDate(jwtTokenOptions.IssuedAt).ToString(), ClaimValueTypes.Integer64)
                );
            permClaims.Add(new Claim(JwtTokenClaims.ExpirationTime,
                    ToUnixEpochDate(jwtTokenOptions.Expiration).ToString(), ClaimValueTypes.Integer64)
                );


            //Create Security Token object by giving required parameters    
            var accessToken = new JwtSecurityToken(
                            jwtTokenOptions.Issuer, //Issure    
                            jwtTokenOptions.Audience,  //Audience    

                            permClaims,
                            jwtTokenOptions.Expiration,
                            signingCredentials: jwtTokenOptions.SigningCredentials
                            );
            var jwtAcessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);


            return new AccessToken(jwtAcessToken, jwtTokenOptions.ValidFor.TotalSeconds.As<int>());
        }

        /// <returns>Date converted to seconds since Unix epoch (Jan 1, 1970, midnight UTC).</returns>
        private static long ToUnixEpochDate(DateTime date)
          => (long)Math.Round((date.ToUniversalTime() -
                               new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero))
                              .TotalSeconds);

        private static void ThrowIfInvalidOptions(JwtIssuerOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (options.ValidFor <= TimeSpan.Zero)
            {
                throw new ArgumentException("Must be a non-zero TimeSpan.", nameof(JwtIssuerOptions.ValidFor));
            }

            if (options.SigningCredentials == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.SigningCredentials));
            }

            if (options.JtiGenerator == null)
            {
                throw new ArgumentNullException(nameof(JwtIssuerOptions.JtiGenerator));
            }
        }


        private JwtIssuerOptions GetJwtOptions()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.SecretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            return new JwtIssuerOptions
            {
                Issuer = this.Issuer,
                Audience = this.ApplicationId,
                IssuedAt = _timeService.GetUtcNow(),
                NotBefore = _timeService.GetUtcNow(),
                ValidFor = this.ValidFor,
                SigningCredentials = credentials
            };
        }

        public TokenValidationParameters GetTokenValidationParameters()
        {
            var jwtTokenOptions = GetJwtOptions();

            return new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidAudience = this.ApplicationId,
                ValidateIssuer = true,
                ValidIssuer = this.Issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = jwtTokenOptions.SigningCredentials.Key,
                ValidateLifetime = true // we check expired tokens here
            };
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenValidationParams = GetTokenValidationParameters();
            try
            {
                var principal = _jwtSecurityTokenHandler.ValidateToken(token, tokenValidationParams, out var securityToken);

                if (!(securityToken is JwtSecurityToken jwtSecurityToken)
                    || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch (Exception e)
            {
                _logger.Error("[Auth] Token validation failed", e);
                return null;
            }
        }
    }
}
