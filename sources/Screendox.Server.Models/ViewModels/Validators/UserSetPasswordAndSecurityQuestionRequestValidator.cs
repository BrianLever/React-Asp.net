using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class UserSetPasswordAndSecurityQuestionRequestValidator : AbstractValidator<UserChangeSecurityQuestionRequest>
    {
        public UserSetPasswordAndSecurityQuestionRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.NewPassword).NotEmpty();
            RuleFor(x => x.SecurityQuestion).NotEmpty();
            RuleFor(x => x.SecurityQuestionAnswer).NotEmpty();
        }
    }
}
