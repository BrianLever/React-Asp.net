using FluentValidation;

namespace ScreenDox.Server.Models
{
    public class BranchLocationRequest
    {
        public int BranchLocationID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Disabled { get; set; }
        public int ScreeningProfileID { get; set; }

    }


    public class BranchLocationRequestValidator : AbstractValidator<BranchLocationRequest>
    {
        public BranchLocationRequestValidator()
        {
            RuleFor(x => x.Name).Length(0, 128).WithMessage("Location name should not exceed 128 characters");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Location name is required");
            RuleFor(x => x.Description).Length(0, 4000).WithMessage("Location description should not exceed 4000 characters");
            RuleFor(x => x.ScreeningProfileID).GreaterThan(0).WithMessage("Screen Profile is required");
        }
    }
}
