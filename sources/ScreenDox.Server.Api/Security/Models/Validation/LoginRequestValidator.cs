namespace ScreenDox.Server.Api.Security.Models.Validation
{
    using FluentValidation;

    using ScreenDox.Server.Api.Models.Security;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty();
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}