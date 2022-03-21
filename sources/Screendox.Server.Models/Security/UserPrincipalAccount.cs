using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.Security
{
    /// <summary>
    /// Includes additional fields for managing user's authentication
    /// </summary>
    public class UserPrincipalAccount : UserPrincipal
    {
        public string Password { get; set; }

        /// <summary>
        ///  Gets or sets the date and time when the membership user was last authenticated
        //     or accessed the application.
        /// </summary>
        public DateTime LastActivityDate { get; set; }
        /// <summary>
        /// Gets or sets the date and time when the user was last authenticated.
        /// </summary>
        public DateTime LastLoginDate { get; set; }
        /// <summary>
        /// Gets the date and time when the user was added to the membership data store.
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Gets the most recent date and time that the membership user was locked out.
        /// </summary>
        public DateTime LastLockoutDate { get; }
        /// <summary>
        /// Gets a value indicating whether the membership user is locked out and unable
        //     to be validated.
        /// </summary>
        public bool IsLockedOut { get; }
        //
      
        public bool IsApproved { get { return true; } }

        /// <summary>
        /// Gets the password question for the membership user.
        /// </summary>
        public string PasswordQuestion { get; set; }

        public string PasswordAnswer{ get; set; }

        public bool IsOnline { get; }
        public DateTime FailedPasswordAttemptWindowStart { get; set; }
        public int FailedPasswordAnswerAttemptCount { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime FailedPasswordAnswerAttemptWindowStart { get; set; }

        /// <summary>
        /// False if security question or answer is not set
        /// </summary>
        public bool IsSecuritySetupComplete
        {
            get
            {
                return !string.IsNullOrEmpty(this.PasswordQuestion) 
                    && !string.IsNullOrEmpty(this.PasswordAnswer);
            }
        }
    }
}
