using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Security.Models
{
    public class ExchangeRefreshTokenResponse
    {
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }

        public ExchangeRefreshTokenResponse(AccessToken accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}