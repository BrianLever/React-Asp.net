using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Security.Models
{
    public class LoginResponse
    {
        public AccessToken AccessToken { get; }
        public string RefreshToken { get; }

        public LoginResponse(AccessToken accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}