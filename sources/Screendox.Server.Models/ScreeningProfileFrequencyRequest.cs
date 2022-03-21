using FluentValidation;

using FrontDesk;
using FrontDesk.Configuration;

using System.Collections.Generic;

namespace ScreenDox.Server.Models
{
    public class ScreeningProfileFrequencyRequest
    {
        public List<ScreeningFrequencyItem> Items = new List<ScreeningFrequencyItem>();
    }

    public class ScreeningProfileFrequencyRequestValidator : AbstractValidator<ScreeningProfileFrequencyRequest>
    {
        public ScreeningProfileFrequencyRequestValidator()
        {
            RuleFor(x => x.Items).NotEmpty().WithMessage("Screening Frequency Settings collection must not be empty") ;

            var supportedScreeningKeys = ScreeningSectionDescriptor.AllScreeningSections;

            RuleForEach(x => x.Items).ChildRules(cr =>
            {
                cr.RuleFor(x => x.ID)
                    .Must(x => supportedScreeningKeys.Contains(x))
                .WithMessage(x => "Invalid screening section key: {0}".FormatWith(x.ID));
            });
        }
    }
}
