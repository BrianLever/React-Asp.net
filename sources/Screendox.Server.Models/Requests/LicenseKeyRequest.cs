using FluentValidation;

using FrontDesk;
using FrontDesk.Configuration;
using FrontDesk.Server.Screening.Models;

using System.Collections.Generic;
using System.Linq;

namespace ScreenDox.Server.Models
{
    public class LicenseKeyRequest
    {
        public string LicenseKey { get; set; }

        public string LicenseActivationKey { get; set; }
    }

    public class LicenseKeyRequestItemRequestValidator : AbstractValidator<LicenseKeyRequest>
    {
        public LicenseKeyRequestItemRequestValidator()
        {
            RuleFor(x => x.LicenseKey).NotEmpty().WithMessage("License Key must not be empty") ;
        }
    }
}
