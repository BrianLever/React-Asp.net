using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDox.Server.Api.Models.Security
{
    public abstract class UseCaseResponseMessage
    {
        public bool Success { get; }
        public string Message { get; }

        protected UseCaseResponseMessage(bool success = false, string message = null)
        {
            Success = success;
            Message = message;
        }
    }
}