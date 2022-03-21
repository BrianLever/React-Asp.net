using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Web.Security;
using FrontDesk.Common.Data;
using FrontDesk.Server.Configuration;
using FrontDesk.Server.Data;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;

using ScreenDox.Server.Models;

namespace FrontDesk.Server
{
    /// <summary>
    /// User details
    /// </summary>
    //[Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed
    public class FDUser: IUserPrincipal
    {
        #region public fields

        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string ContactPhone { get; set; }
        public string StateCode { get; set; }
        public string City { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostalCode { get; set; }
        public bool IsBlock { get; set; }

        #endregion

        #region Membership User properties

        FDMembershipUser MembershipUser { get; set; }
        /// <summary>
        /// Read user details from Database if it wasn't initialized before or ID is not empty
        /// </summary>
        private void EnsureMembershipUserInitialized()
        {
            if (MembershipUser == null && this.UserID != Int32.MinValue)
            {
                this.ReadMembersipFDUser();
            }
        }

        /// <summary>
        /// Initialize membership user object from Database
        /// </summary>
        private void ReadMembersipFDUser()
        {
            MembershipUser = System.Web.Security.Membership.GetUser(this.UserID, false) as FDMembershipUser;
        }

        /// <summary>
        /// Update employee object from the Membership user
        /// </summary>
        /// <param name="?"></param>
        public void InitFromMembershipUser(FDMembershipUser userObject)
        {
            if (userObject == null) return;
            this.UserID = userObject.UserID;
        }

        /// <summary>
        /// Get or set user email
        /// </summary>
        public string Email
        {
            get
            {
                string email = string.Empty;
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) email = MembershipUser.Email;
                return email;
            }
            set
            {
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) MembershipUser.Email = value;
                else
                {
                    MembershipUser = new FDMembershipUser();
                    MembershipUser.Email = value;
                }
            }
        }

        /// <summary>
        /// Get or set user username
        /// </summary>
        public string UserName
        {
            get
            {
                string username = string.Empty;
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) username = MembershipUser.UserName;
                return username;
            }
            set
            {
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) MembershipUser.UserName = value;
                else
                {
                    MembershipUser = new FDMembershipUser();
                    MembershipUser.UserName = value;
                }
            }
        }

        /// <summary>
        /// Get or Set user comment
        /// </summary>
        public string Comments
        {
            get
            {
                string comment = string.Empty;
                EnsureMembershipUserInitialized();
                if (MembershipUser != null) comment = MembershipUser.Comment;
                return comment;
            }
            set
            {
                EnsureMembershipUserInitialized();
                if (MembershipUser != null) MembershipUser.Comment = value;
                else
                {
                    MembershipUser = new FDMembershipUser();
                    MembershipUser.Comment = value;
                }
            }
        }

        /// <summary>
        /// Get or set user full name consists of (first last and middle name)
        /// </summary>
        public string FullName
        {
            get
            {
                string str = String.Empty;

                if (!String.IsNullOrEmpty(this.FirstName))
                {
                    str += this.FirstName;
                }
                if (!String.IsNullOrEmpty(this.MiddleName))
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        str += " ";
                    }
                    str += this.MiddleName;
                }
                if (!String.IsNullOrEmpty(this.LastName))
                {
                    if (!String.IsNullOrEmpty(str))
                    {
                        str += " ";
                    }
                    str += this.LastName;
                }

                return str;
            }
        }

        /// <summary>
        /// Get or set employee new password property
        /// </summary>
        /// <returns></returns>
        public string NewPasswordValue
        {
            get
            {
                string newPasswordValue = String.Empty;
                EnsureMembershipUserInitialized();
                if (MembershipUser != null) newPasswordValue = MembershipUser.NewPasswordValue;
                return newPasswordValue;
            }
            set
            {
                EnsureMembershipUserInitialized();
                if (MembershipUser != null) MembershipUser.NewPasswordValue = value;
                else
                {
                    MembershipUser = new FDMembershipUser();
                    MembershipUser.NewPasswordValue = value;
                }
            }
        }

        public static int CurrentUserID
        {
            get
            {
                var user = ((FDMembershipUser)System.Web.Security.Membership.GetUser());
                if (user != null)
                    return user.UserID;
                else
                    return 0;
            }
        }

        /// <summary>
        /// Get or set employee IsLockedOut property
        /// </summary>
        public bool IsLockedOut
        {
            get
            {
                bool locked = false;
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) locked = MembershipUser.IsLockedOut;
                return locked;
            }
            set
            {
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) MembershipUser.IsLockedOut = value;
                else
                {
                    MembershipUser = new FDMembershipUser();
                    MembershipUser.IsLockedOut = value;
                }
            }
        }

        /// <summary>
        /// Get or set user IsApproved property
        /// </summary>
        public bool IsApproved
        {
            get
            {
                bool approved = false;
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) approved = MembershipUser.IsApproved;
                return approved;
            }
            set
            {
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) MembershipUser.IsApproved = value;
                else
                {
                    MembershipUser = new FDMembershipUser();
                    MembershipUser.IsApproved = value;
                }
            }
        }


        public string RoleName { get; set; }

        public string _passwordQuestion = string.Empty;
        /// <summary>
        /// Get user Security Question property
        /// </summary>
        public string PasswordQuestion
        {
            get
            {
                if (string.IsNullOrEmpty(_passwordQuestion))
                {
                    EnsureMembershipUserInitialized();
                    if (MembershipUser != null) _passwordQuestion = MembershipUser.PasswordQuestion;
                }
                return _passwordQuestion;
            }
            set
            {
                _passwordQuestion = value;
            }
        }

        public string PasswordAnswer { get; set; }

        public DateTime? LastPasswordChangedDate
        {
            get
            {
                DateTime? LastPasswordChangedDate = null;
                EnsureMembershipUserInitialized();
                //if not still empty
                if (MembershipUser != null) LastPasswordChangedDate = (DateTime)MembershipUser.LastPasswordChangedDate;
                return LastPasswordChangedDate;
            }
            set { ;}
        }

        #endregion

        #region Branch Location User properties

        public int? BranchLocationID
        {
            get;
            set;
        }

        #endregion


        /// <summary>
        /// Get new database object
        /// </summary>
        private static FDUserDatabase DbObject { get { return new FDUserDatabase(); } }

        #region constructor

        public FDUser()
        {
            UserID = 0;
            FirstName = String.Empty;
            LastName = String.Empty;
            MiddleName = String.Empty;
            ContactPhone = String.Empty;
            StateCode = String.Empty;
            City = String.Empty;
            AddressLine1 = String.Empty;
            AddressLine2 = String.Empty;
            PostalCode = String.Empty;
        }

        public FDUser(IDataReader reader)
        {
            List<string> columnName = DBDatabase.GetReaderColumnNames(reader);

            UserID = Convert.IsDBNull(reader["UserID"]) ? 0 : Convert.ToInt32(reader["UserID"]);
            FirstName = Convert.ToString(reader["FirstName"]);
            LastName = Convert.ToString(reader["LastName"]);
            MiddleName = Convert.ToString(reader["MiddleName"]);
            ContactPhone = Convert.ToString(reader["ContactPhone"]);
            StateCode = Convert.ToString(reader["StateCode"]);
            City = Convert.ToString(reader["City"]);
            AddressLine1 = Convert.ToString(reader["AddressLine1"]);
            AddressLine2 = Convert.ToString(reader["AddressLine2"]);
            PostalCode = Convert.ToString(reader["PostalCode"]);
            IsBlock = Convert.ToBoolean(reader["IsBlock"]);

            if (columnName.Contains("Username"))
            {
                this.UserName = Convert.ToString(reader["Username"]);
            }
            if (columnName.Contains("Email"))
            {
                this.Email = Convert.ToString(reader["Email"]);
            }
            if (columnName.Contains("Comment"))
            {
                this.Comments = Convert.ToString(reader["Comment"]);
            }
            if (columnName.Contains("BranchLocationID"))
            {
                this.BranchLocationID = Convert.IsDBNull(reader["BranchLocationID"]) ? (Int32?)null : Convert.ToInt32(reader["BranchLocationID"]);
            }
            if (columnName.Contains("Rolename"))
            {
                this.RoleName = Convert.IsDBNull(reader["Rolename"]) ? String.Empty : Convert.ToString(reader["Rolename"]);
            }
            if (columnName.Contains("LastPasswordChangedDate"))
            {
                this.LastPasswordChangedDate = Convert.IsDBNull(reader["LastPasswordChangedDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["LastPasswordChangedDate"]);
            }
        }

        #endregion

        #region GET static metods

        /// <summary>
        /// Get current employee details
        /// </summary>
        /// <returns>Current user details object</returns>
        public static FDUser GetCurrentUser()
        {
            FDMembershipUser User = ((FDMembershipUser)System.Web.Security.Membership.GetUser());
            return FDUser.GetUserByID(User.UserID);
        }

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        public static FDUser GetUserByID(int userID)
        {
            return new FDUserDatabase().GetUserByID(userID);
        }

        public static int GetExportSystemUserID()
        {
            return new FDUserDatabase().GetUserIDByName("export_service");
        }

        /// <summary>
        /// Get all users in system
        /// </summary>
        public static DataSet GetAllUser()
        {
            return new FDUserDatabase().GetAllUser();
        }

        /// <summary>
        /// True if User is block
        /// </summary>
        public static bool ValidateFDUser(string userName)
        {
            return DbObject.ValidateFDUser(userName);
        }

        /// <summary>
        /// True if user account had been locked
        /// </summary>
        public static bool IsLocked(string userName)
        {
            return DbObject.isLocked(userName);
        }

        /// <summary>
        /// Check for account expiration
        /// </summary>
        public static bool IsExpired(MembershipUser user)
        {
            // if password has never been changed use account creation date 
            // as start period of account's living
            // else last password changed date

            DateTime start = user.LastPasswordChangedDate == DateTime.MinValue ?
                user.CreationDate : user.LastPasswordChangedDate;

            return DateTime.Now.Subtract(start).TotalDays > AppSettings.PasswordRenewalPeriodDays;
        }

        public static bool ValidateSecurityQuestionAndAnswer(string username, string answer)
        {
            try
            {
                MembershipUser user = System.Web.Security.Membership.GetUser(username);
                if (user == null)
                {
                    return false;
                }

                user.ResetPassword(answer);
            }
            catch (MembershipPasswordException)
            {
                return false;
            }
            catch
            {
                throw;
            }

            return true;
        }
        [Obsolete]
        /// <summary>
        /// Get User by user name
        /// </summary>
        public static FDUser FindUsersByName(string userName)
        {
            FDUser FDUser = null;
            System.Web.Security.MembershipUserCollection userCollection = System.Web.Security.Membership.FindUsersByName(userName);
            if(userCollection.Count != 0)
            {
                FDMembershipUser user = (FDMembershipUser)userCollection[userName];
                FDUser = GetUserByID(user.UserID);
            }
            return FDUser;
        }

        #endregion

        #region ADD/UPDATE/DELETE

        public static Int32 Add(FDUser user)
        {
            int userID = 0;
            string errrorMessage = string.Empty;
            FDMembershipUser createUser = null;

            //true when user account created and subscriber has access to the system.
            bool IsUserAccountCreated = false;

            try
            {
                MembershipCreateStatus createUserStatus;
                createUser = (FDMembershipUser)System.Web.Security.Membership.CreateUser(user.UserName,
                    user.NewPasswordValue, user.Email == "" ? null : user.Email , null, null, true, int.MinValue, out createUserStatus);
                if (createUser != null)
                {
                    userID = (Int32)createUser.ProviderUserKey;
                    user.InitFromMembershipUser(createUser);
                    IsUserAccountCreated = true;

                    // Set user details
                    try
                    {
                        AddDetails(user);
                        if (!String.IsNullOrEmpty(user.RoleName))
                        {
                            try
                            {
                                //Assign user to role
                                AddUserToRole(user.UserName, user.RoleName.ToString());
                            }
                            catch (Exception ex)
                            {
                                //remove user details
                                DeleteUser(userID);
                                IsUserAccountCreated = false;
                                throw ex;
                            }
                        }
                        if (user.BranchLocationID != null)
                        {
                            try
                            {
                                //Add user to branch location
                                SetBranchLocation(userID, user.BranchLocationID);
                            }
                            catch (Exception ex)
                            {
                                //remove user details
                                DeleteUser(userID);
                                IsUserAccountCreated = false;
                                throw ex;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //remove user
                        System.Web.Security.Membership.DeleteUser(user.UserName);
                        IsUserAccountCreated = false;
                        throw ex;
                    }
                }
                else
                {
                    switch (createUserStatus)
                    {
                        case MembershipCreateStatus.DuplicateUserName:
                            errrorMessage = "User name is already in use. Please enter a different user name";
                            break;
                        case MembershipCreateStatus.DuplicateEmail:
                            errrorMessage = "The e-mail address that you entered is already in use. Please enter a different e-mail address";
                            break;
                        case MembershipCreateStatus.InvalidEmail:
                            errrorMessage = "Please enter a valid e-mail address";
                            break;
                        case MembershipCreateStatus.InvalidPassword:
                            errrorMessage = "Please enter a valid password";
                            break;
                        default:
                            errrorMessage = "Sorry, but user account was not created. Please try again";
                            break;
                    }

                    throw new System.ApplicationException(errrorMessage);
                }
            }
            catch (System.ApplicationException)
            {
                throw new System.ApplicationException(errrorMessage);
            }
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw ex;
            }

            //add error if error message is empty
            if (!IsUserAccountCreated)
            {
                throw new System.ApplicationException("Sorry, but user account was not created. Please try again");
            }

            return userID;
        }

        /// <summary>
        /// Add user details
        /// </summary>
        public static void AddDetails(FDUser user)
        {
            DbObject.AddDetails(user);
        }

        /// <summary>
        /// Update user details
        /// </summary>
        public static void Update(FDUser user)
        {
            var db = DbObject;
            try
            {
                System.Web.Security.Membership.UpdateUser((MembershipUser)user.MembershipUser);
                DbObject.Update(user);
                if (!String.IsNullOrEmpty(user.RoleName))
                {
                    try
                    {
                        if (IsUserInRole(user.UserName))
                        {
                            //Update user to role
                            UpdateUserRole(user.UserName, user.RoleName.ToString());
                        }
                        else
                        {
                            //Add user to role
                            AddUserToRole(user.UserName, user.RoleName.ToString());
                        }
                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Failed to update user role. Error has occurred.");
                    }
                }
                if (user.BranchLocationID != null)
                {
                    try
                    {
                        if (IsUserInBranchLocation(user.UserID))
                        {
                            //Update user to branch location
                            UpdateBranchLocation(user.UserID, user.BranchLocationID);
                        }
                        else
                        {
                            //Add user to branch location
                            SetBranchLocation(user.UserID, user.BranchLocationID);
                        }

                    }
                    catch (Exception)
                    {
                        throw new ApplicationException("Failed to update user branch location. Error has occurred.");
                    }
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                db.Disconnect();
            }
        }

        /// <summary>
        /// Delete user
        /// </summary>
        public static void DeleteUser(Int32 userID)
        {
            try
            {
                DbObject.Delete(userID);
            }
            catch (SecurityLogException)
            {
                throw;
            }
            //catch (DbException ex)
            //{
            //    FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
            //    throw ex;
            //}
            catch (Exception ex)
            {
                FrontDesk.Common.Debugging.DebugLogger.TraceException(ex);
                throw;
            }
        }

        /// <summary>
        /// Block/UnBlock User
        /// </summary>
        public static void Block(int userID, bool isBlock)
        {
            try
            {
                DbObject.Block(userID, isBlock);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unblock signage user account
        /// </summary>
        public void UnLock()
        {
            try
            {
                this.MembershipUser.UnlockUser();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Unblock signage user account
        /// </summary>
        public static bool Unblock(int userID)
        {
            bool succeed = false;
            var user = GetUserByID(userID);
            if (user != null)
            {
                succeed = user.MembershipUser.UnlockUser();
            }

            return succeed;
        }

        /// <summary>
        /// Change password
        /// </summary>
        public static void ChangePassword(FDUser user, string oldPassword)
        {
            ///change password
            if (!String.IsNullOrEmpty(user.NewPasswordValue))
            {
                try
                {
                    user.MembershipUser.ChangePassword(oldPassword, user.NewPasswordValue);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Use only if user enters correct security question and answer.
        /// Do not check for previous passwords.
        /// </summary>
        /// <param name="user"></param>
        public static void UpdatePassword(string userName, string newPwd)
        {
            DbObject.UpdatePassword(userName, newPwd);
        }

        #endregion

        #region User In Role



        /// <summary>
        /// Add user to role
        /// </summary>
        public static void AddUserToRole(string userName, string roleName)
        {
            DbObject.AddUserToRole(userName, roleName);
        }

        /// <summary>
        /// Update user role
        /// </summary>
        public static void UpdateUserRole(string userName, string roleName)
        {
            DbObject.UpdateUserRole(userName, roleName);
        }

        /// <summary>
        /// True if user assigned to role
        /// </summary>
        public static bool IsUserInRole(string userName)
        {
            return DbObject.IsUserInRole(userName);
        }

        /// <summary>
        /// Get roles without "Super Administrator"
        /// </summary>
        [Obfuscation(Feature = "renaming", Exclude = true)] // used in data binding by name
        public static List<string> GetAllRoles()
        {
            return DbObject.GetAllRoles();
        }

        #endregion

        #region User in Branch Location

        /// <summary>
        /// Assign user to the branch location.
        public static bool SetBranchLocation(Int32 userID, Int32? branchLocationID)
        {
            return DbObject.SetBranchLocation(userID, branchLocationID);
        }

        /// <summary>
        /// Update user branch location
        /// </summary>
        public static bool UpdateBranchLocation(Int32 userID, Int32? branchLocationID)
        {
            return DbObject.UpdateBranchLocation(userID, branchLocationID);
        }

        /// <summary>
        /// true if user assign to branch location
        /// </summary>
        public static bool IsUserInBranchLocation(Int32 userID)
        {
            return DbObject.IsUserInBranchLocation(userID);
        }

        #endregion
    }
}
