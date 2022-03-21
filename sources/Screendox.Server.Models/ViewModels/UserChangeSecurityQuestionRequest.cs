using ScreenDox.Server.Models.ViewModels.Validators;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class UserChangeSecurityQuestionRequest : IRequestModelWithValidation<UserChangeSecurityQuestionRequestValidator>
    {
        /// <summary>
        /// Current password, used for validation
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// New Password is optional, it is used only at first-time login when there is no security question
        /// </summary>
        public string NewPassword { get; set; }
        /// <summary>
        /// Security question text
        /// </summary>
        public string SecurityQuestion { get; set; }
        /// <summary>
        /// The answer on the security question, used to reset the password
        /// </summary>
        public string SecurityQuestionAnswer { get; set; }

    }
}
