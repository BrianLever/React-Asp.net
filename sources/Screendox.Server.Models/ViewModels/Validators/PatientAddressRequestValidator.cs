using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class PatientAddressRequestValidator : AbstractValidator<PatientAddressRequest>
    {
        public PatientAddressRequestValidator()
        {
            RuleFor(x => x.PatientID).NotEmpty();
            RuleFor(x => x.StreetAddress).NotEmpty();
            RuleFor(x => x.City).NotEmpty();
            RuleFor(x => x.StateID).NotEmpty();
            RuleFor(x => x.ZipCode).NotEmpty();
            RuleFor(x => x.Phone).NotEmpty();
        }
    }
}
