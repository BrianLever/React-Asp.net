namespace ScreenDox.Server.Api.Security.Models
{
    using ScreenDox.Server.Api.Models.Security;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ExchangeRefreshTokenRequest
    {
        public string RefreshToken { get; }


        public ExchangeRefreshTokenRequest(string accessToken, string refreshToken, string signingKey)
        {
            RefreshToken = refreshToken;
        }
    }
}