using ScreenDox.Server.Models.ViewModels.Validators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class UserChangePasswordRequest: IRequestModelWithValidation<UserChangePasswordRequestValidator>
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

    }
}
