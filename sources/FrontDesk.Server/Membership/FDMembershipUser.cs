using System;
using System.Data;
using System.Reflection;
using System.Web.Security;
using FrontDesk.Common.Debugging;
using FrontDesk.Server.Data;

namespace FrontDesk.Server.Membership
{
    /// <summary>
    /// Overriden Membership User class that is used with a ASP.NET Membership providers
    /// </summary>
    [Serializable()]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class FDMembershipUser : System.Web.Security.MembershipUser
    {
        private string _username;
        public new string UserName
        {
            get { return _username; }
            set { _username = value; }
        }

        public new bool IsLockedOut;
        public string NewPasswordValue;

        public Int32 UserID
        {
            get
            {
                Int32 id = Int32.MinValue;
                if (this.ProviderUserKey != null) id = Convert.ToInt32(this.ProviderUserKey);
                return id;
            }
        }

        #region constructor

        public FDMembershipUser(string providerName, string name, Object providerUserKey, string email, string passwordQuestion, string comment, bool isApproved,
            bool isLockedOut, DateTime creationDate, DateTime lastLoginDate, DateTime lastActivityDate, DateTime lastPasswordChangedDate, DateTime lastLockoutDate)
            : base(providerName, name, providerUserKey, email, passwordQuestion, comment, isApproved, isLockedOut, creationDate, lastLoginDate, lastActivityDate, lastPasswordChangedDate, lastLockoutDate)
        {
            UserName = name;
            IsLockedOut = isLockedOut;
            NewPasswordValue = "";
        }

        public FDMembershipUser() : base() { NewPasswordValue = ""; }

        public FDMembershipUser(MembershipUser user)
            : base(user.ProviderName,
            user.UserName,
            user.ProviderUserKey,
            user.Email,
            user.PasswordQuestion,
            user.Comment,
            user.IsApproved,
            user.IsLockedOut,
            user.CreationDate,
            user.LastLoginDate,
            user.LastActivityDate,
            user.LastPasswordChangedDate,
            user.LastLockoutDate)
        {
            UserName = user.UserName;
            IsLockedOut = user.IsLockedOut;
            NewPasswordValue = "";
        }

        #endregion

        private static FDMembershipUserDb DbObject
        {
            get
            {
                return new FDMembershipUserDb();
            }
        }

        /// <summary>
        /// Create user and return all error as ApplicationExpception
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="System.ApplicationException">Custom error message</exception>
        public static MembershipUser CreateUser(FDMembershipUser user)
        {
            MembershipCreateStatus status;

            MembershipUser newUser = null;  // TODO: do we need to return FDMembershipUser instead?
            try
            {
                newUser = (MembershipUser)System.Web.Security.Membership.CreateUser(
                     user.UserName, user.NewPasswordValue, user.Email, null, null, true, out status);
            }
            catch (Exception ex)
            {
                DebugLogger.DebugException(ex, "Create user exception");
                throw new ApplicationException(Web.Resources.Strings.CreateUser_GeneralError);

            }
            if (status != MembershipCreateStatus.Success)
            {
                if (status == MembershipCreateStatus.DuplicateUserName)
                    throw new ApplicationException(Web.Resources.Strings.CreateUser_DuplicateUserName);
                else if (status == MembershipCreateStatus.DuplicateEmail)
                    throw new ApplicationException(Web.Resources.Strings.CreateUser_DuplicateEmail);
                else if (status == MembershipCreateStatus.InvalidEmail)
                    throw new ApplicationException(Web.Resources.Strings.CreateUser_InvalidEmail);
                else if (status == MembershipCreateStatus.InvalidPassword)
                    throw new ApplicationException(Web.Resources.Strings.CreateUser_InvalidPassword);
                else
                    throw new ApplicationException(Web.Resources.Strings.CreateUser_GeneralError);
            }
            return newUser;
        }

        public static DataSet GetListWithBranchLocation(int? branchLocationID)
        {
            return DbObject.GetListWithBranchLocation(branchLocationID);
        }
    }
}

