using FluentValidation;

using FrontDesk;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class UpdatePatientRequestValidator : AbstractValidator<UpdatePatientRequest>
    {
        public UpdatePatientRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Birthday).NotEmpty();

            RuleFor(x => x.StreetAddress).NotEmpty().When(x => !x.IsEmptyContactInfo());
            RuleFor(x => x.City).NotEmpty().When(x => !x.IsEmptyContactInfo());
            RuleFor(x => x.StateID).NotEmpty().When(x => !x.IsEmptyContactInfo());
            RuleFor(x => x.Phone).NotEmpty().When(x => !x.IsEmptyContactInfo());
            RuleFor(x => x.ZipCode).NotEmpty().When(x => !x.IsEmptyContactInfo());
        }
    }
}
