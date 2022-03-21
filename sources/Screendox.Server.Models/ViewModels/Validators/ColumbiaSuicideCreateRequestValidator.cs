using FluentValidation;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels.Validators
{
    public class ColumbiaSuicideCreateRequestValidator : AbstractValidator<ColumbiaSuicideCreateRequest>
    {
        public ColumbiaSuicideCreateRequestValidator()
        {
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.Birthday).NotEmpty();
            RuleFor(x => x.BranchLocationID).GreaterThan(0);

        }
    }
}
