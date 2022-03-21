using FluentValidation;

using FrontDesk;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class PatientSearchFilterValidator : AbstractValidator<PatientSearchFilter>
    {
        public PatientSearchFilterValidator()
        {
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Birthday).NotEmpty();
        }
    }
}
