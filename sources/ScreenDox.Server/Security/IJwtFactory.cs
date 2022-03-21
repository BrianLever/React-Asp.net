namespace Web.Api.Infrastructure.Auth
{
    using Microsoft.IdentityModel.Tokens;

    using ScreenDox.Server.Api.Security.Models;
    using ScreenDox.Server.Models;

    using System;
    using System.Security.Claims;

    public interface IJwtFactory
    {
        string ApplicationId { get; }
        string Issuer { get; }
        TimeSpan ValidFor { get; }

        AccessToken GenerateEncodedToken(IUserPrincipal principal);


        ClaimsPrincipal ValidateToken(string token);

        TokenValidationParameters GetTokenValidationParameters();
    }
}