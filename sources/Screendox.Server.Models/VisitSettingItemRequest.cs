using FluentValidation;

using FrontDesk;
using FrontDesk.Configuration;
using FrontDesk.Server.Screening.Models;

using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.Server.Models
{
    public class VisitSettingItemRequest
    {
        public List<VisitSettingItem> Items = new List<VisitSettingItem>();
    }

    public class VisitSettingItemRequestValidator : AbstractValidator<VisitSettingItemRequest>
    {
        public VisitSettingItemRequestValidator()
        {
            RuleFor(x => x.Items).NotEmpty().WithMessage("Visit Settings collection must not be empty") ;

            var supportedScreeningKeys = VisitSettingsDescriptor.KnownMeasurements;

            RuleForEach(x => x.Items).ChildRules(cr =>
            {
                cr.RuleFor(x => x.Id)
                    .Must(x => supportedScreeningKeys.Contains(x))
                .WithMessage(x => "Invalid screening section key: {0}".FormatWith(x.Id));
            });

            RuleForEach(x => x.Items).ChildRules(cr =>
            {
                cr.RuleFor(x => x.CutScore)
                .InclusiveBetween(1, 30)
                .WithMessage("'Cut Score' field accepts values from 1 to 30. Default value is '1'.");
            });
        }
    }
}
