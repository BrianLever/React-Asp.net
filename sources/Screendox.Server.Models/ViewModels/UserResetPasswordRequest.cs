using ScreenDox.Server.Models.ViewModels.Validators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class UserResetPasswordRequest : IRequestModelWithValidation<UserResetPasswordRequestValidator>
    {
        public string SecurityQuestionAnswer { get; set; }
        public string NewPassword { get; set; }
    }
}
