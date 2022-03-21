using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Models.Security
{
    /// <summary>
    /// Login request
    /// </summary>
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}