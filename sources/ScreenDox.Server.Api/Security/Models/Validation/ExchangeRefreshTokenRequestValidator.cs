namespace ScreenDox.Server.Api.Security.Models.Validation
{
    using FluentValidation;

    using ScreenDox.Server.Api.Models.Security;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ExchangeRefreshTokenRequestValidator : AbstractValidator<ExchangeRefreshTokenRequest>
    {
        public ExchangeRefreshTokenRequestValidator()
        {
            RuleFor(x => x.RefreshToken).NotEmpty();
        }
    }
}