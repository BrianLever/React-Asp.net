
using FluentValidation;

using FrontDesk;
using FrontDesk.Common.Configuration;
using FrontDesk.Common.Screening;

using ScreenDox.Server.Models.Configuration;

namespace ScreenDox.Server.Models.ViewModels
{
    /// <summary>
    /// Contract for sending updated value to Age Groups Setting
    /// </summary>
    public class AgeGroupsSettingRequest
    {
        public SystemSettingItem Value { get; set; }
    }

    /// <summary>
    /// Validator for AgeGroupsSettingRequest model
    /// </summary>
    public class AgeGroupsSettingRequestValidator : AbstractValidator<AgeGroupsSettingRequest>
    {
        public AgeGroupsSettingRequestValidator()
        {
            var appConfigSettingName = "IndicatorReport_AgeGroups";


            RuleFor(x => x.Value).NotEmpty().WithMessage("Value must not be empty");



            RuleFor(x => x.Value).ChildRules(cr =>
            {
                cr.RuleFor(x => x.Key).Must(x => string.Compare(x, appConfigSettingName, false) == 0)
                .WithMessage("Invalid Value.Key string. Must be '{0}'".FormatWith(appConfigSettingName));


                // regular expression
                var re = ScreeningAgesSettingsProvider.RegexValidationExpression;
                var defaultAgeGroupsValue = AppSettingsProxy.GetStringValue(appConfigSettingName, string.Empty);

                cr.RuleFor(x => x.Value)
                    .Matches(re)
                    .WithMessage("Age groups should follow the sample format: {0}"
                    .FormatWith(defaultAgeGroupsValue));
            });
        }
    }
}
