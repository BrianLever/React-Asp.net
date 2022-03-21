
using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Configuration;
using FrontDesk.Common.Screening;

using ScreenDox.Server.Models.Configuration;

using System.Collections.Generic;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// Contract for sending updated value of system settings
    /// </summary>
    public class SystemSettingRequest
    {
        public List<SystemSettingItem> Items { get; set; }
    }

    /// <summary>
    /// Validator for SystemSettingRequest model
    /// </summary>
    public class SystemSettingRequestValidator : AbstractValidator<SystemSettingRequest>
    {
        public SystemSettingRequestValidator()
        {

            RuleFor(x => x.Items).NotEmpty().WithMessage("Items collection must not be empty");



            RuleForEach(x => x.Items).ChildRules(cr =>
            {
                cr.RuleFor(x => x.Key).NotEmpty().WithMessage(x => "Key value must not be empty");

            });
        }
    }
}
