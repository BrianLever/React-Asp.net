
using FluentValidation;

using System.Collections.Generic;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// Contract for sending updated value of security log settings
    /// </summary>
    public class SecurityLogSettingRequest
    {
        public List<SecurityLogEventSettingItemRequest> Items { get; set; }
    }

    /// <summary>
    /// Validator for SecurityLogSettingRequest model
    /// </summary>
    public class SecurityLogSettingRequestValidator : AbstractValidator<SecurityLogSettingRequest>
    {
        public SecurityLogSettingRequestValidator()
        {

            RuleFor(x => x.Items).NotEmpty().WithMessage("Items collection must not be empty");

            RuleForEach(x => x.Items).ChildRules(cr =>
            {
                cr.RuleFor(x => x.ID).NotEmpty().WithMessage(x => "ID value must not be empty");
                cr.RuleFor(x => x.IsEnabled).NotNull().WithMessage(x => "IsEnabled value must not be empty");

            });
        }
    }
}
