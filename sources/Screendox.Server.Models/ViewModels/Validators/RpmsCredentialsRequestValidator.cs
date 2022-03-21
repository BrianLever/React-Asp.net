using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class RpmsCredentialsRequestValidator : AbstractValidator<RpmsCredentialsRequest>
    {
        public RpmsCredentialsRequestValidator()
        {
            RuleFor(x => x.AccessCode).NotEmpty().WithName("Access Code");
            RuleFor(x => x.VerifyCode).NotEmpty().WithName("Verify Code");
            RuleFor(x => x.ExpireAt).NotEmpty();
        }
    }
}
