using FluentValidation;

using FrontDesk;
using FrontDesk.Configuration;

using ScreenDox.Server.Models.Resources;

using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.Server.Models
{
    public class ScreeningProfileAgeRequest
    {
        public List<ScreeningSectionAge> Items = new List<ScreeningSectionAge>();
    }

    public class ScreeningProfileAgeRequestValidator : AbstractValidator<ScreeningProfileAgeRequest>
    {
        public ScreeningProfileAgeRequestValidator()
        {
            RuleFor(x => x.Items).NotEmpty().WithMessage("Screening Age Settings collection must not be empty") ;
            RuleForEach(x => x.Items).NotNull().WithMessage("Screening Age Settings collection must not include null values");

            var supportedScreeningKeys = ScreeningSectionDescriptor.AllScreeningSections;
            
            RuleForEach(x => x.Items).ChildRules(cr =>
            {
                cr.RuleFor(x => (int)x.MinimalAge).InclusiveBetween(0, 200)
                .WithMessage(TextMessages.ScreeningMinimalAgeForm_RangeError);
            });

            RuleForEach(x => x.Items).ChildRules(cr =>
            {
                cr.RuleFor(x => x.ScreeningSectionID)
                    .Must(x => supportedScreeningKeys.Contains(x))
                .WithMessage(x => "Invalid screening section key: {0}".FormatWith(x.ScreeningSectionID));
            });

        }
    }
}
