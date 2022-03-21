using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class UserResetPasswordRequestValidator : AbstractValidator<UserResetPasswordRequest>
    {
        public UserResetPasswordRequestValidator()
        {
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.SecurityQuestionAnswer).NotEmpty();
            RuleFor(x => x.NewPassword).Matches(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{6,}$")
                .WithMessage("Password must contain one lowercase character, one uppercase character, one numeric character and should be at least 6 characters");
        }
    }
}
