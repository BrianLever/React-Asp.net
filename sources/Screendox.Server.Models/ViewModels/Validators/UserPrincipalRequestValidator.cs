using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class UserPrincipalRequestValidator : AbstractValidator<UserPrincipalRequest>
    {
        public UserPrincipalRequestValidator()
        {
            RuleFor(x => x.RoleName).NotEmpty().WithName("Group");
            RuleFor(x => x.BranchLocationID).GreaterThan(0).WithMessage("Branch location is required");
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
        }
    }
}
