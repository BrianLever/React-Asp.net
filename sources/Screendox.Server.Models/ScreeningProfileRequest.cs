using FluentValidation;

namespace ScreenDox.Server.Models
{
    public class ScreeningProfileRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }


    public class ScreeningProfileRequestValidator : AbstractValidator<ScreeningProfileRequest>
    {
        public ScreeningProfileRequestValidator()
        {
            RuleFor(x => x.Name).Length(0, 128).WithMessage("Profile name should not exceed 128 characters");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Profile name is required");
            RuleFor(x => x.Description).Length(0, 4000).WithMessage("Profile description should not exceed 4000 characters");
        }
    }
}
