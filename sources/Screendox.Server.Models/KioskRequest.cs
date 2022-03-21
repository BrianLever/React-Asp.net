using FluentValidation;

namespace ScreenDox.Server.Models
{
    public class KioskRequest
    {
        public short KioskID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BranchLocationID { get; set; }
        public bool Disabled { get; set; }

        public string SecretKey { get; set; }
    }


    public class KioskRequestValidator : AbstractValidator<KioskRequest>
    {
        public KioskRequestValidator()
        {
            RuleFor(x => x.Name).Length(0, 255).WithMessage("Kiosk name should not exceed 255 characters");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kisk name is required");
            RuleFor(x => x.Description).Length(0, 4000).WithMessage("Kiosk description should not exceed 4000 characters");
            RuleFor(x => x.BranchLocationID).GreaterThan(0).WithMessage("Branch location is required");
            RuleFor(x => x.SecretKey).Length(0, 64).WithMessage("Kiosk secret should not exceed 64 characters");

        }
    }
}
