using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class UserChangeSecurityQuestionRequestValidator : AbstractValidator<UserChangeSecurityQuestionRequest>
    {
        public UserChangeSecurityQuestionRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.SecurityQuestion).NotEmpty();
            RuleFor(x => x.SecurityQuestionAnswer).NotEmpty();
        }
    }
}
