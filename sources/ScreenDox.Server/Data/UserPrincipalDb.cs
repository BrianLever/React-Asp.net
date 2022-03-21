namespace ScreenDox.Server.Data
{
    using FrontDesk.Common.Data;
    using FrontDesk.Common.Extensions;
    using FrontDesk.Server.Logging;
    using FrontDesk.Server.Membership;

    using ScreenDox.Server.Models;
    using ScreenDox.Server.Models.Security;
    using ScreenDox.Server.Security;

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Data;
    using System.Data.Common;


    public interface IUserPrincipalRepository : ITransactionalDatabase
    {
        int AddUser(UserPrincipalAccount model);
        void UpdateUser(UserPrincipalAccount user);

        void AddDetails(UserPrincipal model);
        void AddUserToRole(string userName, string roleName);
        void Block(int userID, bool isBlock);
        void Delete(int id);
        List<string> GetAllRoles();
        List<UserPrincipal> GetAll(int? branchLocationId, string orderBy);
        UserPrincipal GetUserByID(int userID);
        int GetUserIDByName(string username);
        UserPrincipalAccount GetActiveUserByName(string username);
        UserPrincipalAccount GetByID(int userId);
        bool IsBlocked(string userName);
        bool isLocked(string userName);
        bool IsUserInBranchLocation(int userID);
        bool IsUserInRole(string userName);
        bool SetBranchLocation(int userID, int? branchLocationID);
        void UpdateDetails(UserPrincipal model);
        bool UpdateBranchLocation(int userID, int? branchLocationID);
        void UpdatePassword(string userName, string newPwd);
        void UpdateUserRole(string userName, string roleName);
        void UpdateLastLoginDate(int userID, DateTime dateTime);
        void UpdatePasswordFailure(int userID, int failureCount, DateTime failureWindowStart);
        void UpdatePasswordAnswerFailure(int userID, int failureCount, DateTime failureWindowStart);
        void Lock(int userID, DateTime dateTime);
        void ChangePassword(int userId, string newPassword, DateTime currentTime);
        bool ValidateNewPasswordInHistory(int userId, string hashedPassword);
        bool UpdateSecurityQuestion(int userID, string newQuestion, string newAnswer);
        void UnlockUser(string username);
    }

    public class UserPrincipalDb : DBDatabase, IUserPrincipalRepository
    {
        #region constructor


        public UserPrincipalDb(string connectionString)
            : base(connectionString) { }

        public UserPrincipalDb()
            : base(ConfigurationManager.ConnectionStrings[0].ConnectionString)
        { }

        public UserPrincipalDb(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region GET

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        public UserPrincipal GetUserByID(int userID)
        {
            UserPrincipal user = null;
            string sql = @"SELECT ud.*, u.Username, u.Email, u.Comment, ubl.BranchLocationID, ur.Rolename
FROM dbo.UserDetails ud INNER JOIN Users u ON ud.UserID = u.PKID
LEFT JOIN dbo.Users_BranchLocation ubl ON ud.UserID = ubl.UserID
Left JOIN dbo.UsersInRoles ur ON u.UserName = ur.UserName
WHERE ud.UserID = @UserID";

            CommandObject.Parameters.Clear();
            AddParameter("@UserID", DbType.Int32).Value = userID;
            try
            {
                base.Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        user = UserPrincipalFactory.CreateFromDbReader(reader);
                    }
                }
            }
            finally
            {
                base.Disconnect();
            }
            return user;
        }

        /// <summary>
        /// True if User is blocked
        /// </summary>
        public bool IsBlocked(string userName)
        {
            bool isVaild = false;

            string sql = @"Select isBlock FROM dbo.UserDetails ud
INNER JOIN dbo.Users u ON ud.UserID = u.PKID Where u.Username = @Username";
            CommandObject.Parameters.Clear();
            AddParameter("@Username", DbType.String).Value = userName;
            try
            {
                base.Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        isVaild = !Convert.ToBoolean(reader["isBlock"]);
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
                base.Disconnect();
            }

            return isVaild;
        }

        /// <summary>
        /// True if user account had been locked
        /// </summary>
        public bool isLocked(string userName)
        {
            bool isLocked = false;

            string sql = @"Select u.IsLockedOut FROM dbo.UserDetails ud
INNER JOIN dbo.Users u ON ud.UserID = u.PKID Where u.Username = @Username";
            CommandObject.Parameters.Clear();
            AddParameter("@Username", DbType.String).Value = userName;
            try
            {
                base.Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        isLocked = Convert.ToBoolean(reader["IsLockedOut"]);
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
                base.Disconnect();
            }

            return isLocked;

        }

        #endregion

        #region ADD/UPDATE/DELETE

        /// <summary>
        /// Add user details
        /// </summary>
        public int AddUser(UserPrincipalAccount model)
        {
            string sql = @"
INSERT INTO dbo.Users (
Username, 
Password, 
Email, 
PasswordQuestion,
PasswordAnswer, 
IsApproved,
Comment, 
CreationDate, 
LastPasswordChangedDate, 
LastActivityDate,
IsLockedOut, 
LastLockedOutDate,
FailedPasswordAttemptCount, 
FailedPasswordAttemptWindowStart, 
FailedPasswordAnswerAttemptCount, 
FailedPasswordAnswerAttemptWindowStart
)
Values(
@Username, 
@Password, 
@Email, 
@PasswordQuestion,
@PasswordAnswer, 
@IsApproved,
@Comment,
@CreationDate,
@LastPasswordChangedDate,
@LastActivityDate, 
@IsLockedOut,
@LastLockedOutDate,
@FailedPasswordAttemptCount,
@FailedPasswordAttemptWindowStart, 
@FailedPasswordAnswerAttemptCount, 
@FailedPasswordAnswerAttemptWindowStart
);

SET @UserID = SCOPE_IDENTITY()
";
            ClearParameters();

            var userIdParam = AddParameter("@UserID", DbType.Int32);
            userIdParam.Direction = ParameterDirection.Output;

            AddParameter("@Username", DbType.AnsiString, 255).Value = model.UserName;
            AddParameter("@Password", DbType.AnsiString, 128).Value = model.Password;

            AddParameter("@Email", DbType.String, 128).Value = model.Email.AsSqlParameter();
            AddParameter("@PasswordQuestion", DbType.AnsiString, 255).Value = model.PasswordQuestion.AsSqlParameter();
            AddParameter("@PasswordAnswer", DbType.AnsiString, 255).Value = model.PasswordAnswer.AsSqlParameter();
            AddParameter("@IsApproved", DbType.Boolean).Value = true;
            AddParameter("@Comment", DbType.AnsiString, 255).Value = model.Comments.AsSqlParameter();
            AddParameter("@CreationDate", DbType.DateTime).Value = model.CreationDate;

            AddParameter("@LastPasswordChangedDate", DbType.DateTime).Value = model.CreationDate;
            AddParameter("@LastActivityDate", DbType.DateTime).Value = model.CreationDate;
            AddParameter("@IsLockedOut", DbType.Boolean).Value = false;
            AddParameter("@LastLockedOutDate", DbType.DateTime).Value = model.CreationDate;
            AddParameter("@FailedPasswordAttemptCount", DbType.Int32).Value = 0;
            AddParameter("@FailedPasswordAttemptWindowStart", DbType.DateTime).Value = model.CreationDate;
            AddParameter("@FailedPasswordAnswerAttemptCount", DbType.Int32).Value = 0;
            AddParameter("@FailedPasswordAnswerAttemptWindowStart", DbType.DateTime).Value = model.CreationDate;

            try
            {
                RunNonSelectQuery(sql);
                model.UserID = userIdParam.Value.As<int>();
                
            }
            finally
            {
                base.Disconnect();
            }

            return model.UserID;
        }

        public void UpdateUser(UserPrincipalAccount model)
        {
            var sql = @"UPDATE dbo.Users SET 
Email = @Email, 
Comment = @Comment,
IsLockedOut=@IsLockedOut 
WHERE Username = @Username AND (UserName<> 'su' OR @Username='su')";

            ClearParameters();
            AddParameter("@Email", DbType.String, 128).Value = model.Email.AsSqlParameter();
            AddParameter("@Comment", DbType.AnsiString, 255).Value = model.Comments.AsSqlParameter();
            AddParameter("@Username", DbType.AnsiString, 255).Value = model.UserName;
            AddParameter("@IsLockedOut", DbType.Boolean).Value = model.IsLockedOut;


            try
            {
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
            if (model.UserName.Equals("su") && model.IsLockedOut)
            {
                UnlockUser("su");
            }
        }

        public void UnlockUser(string username)
        {
        var sql = @"UPDATE dbo.Users SET 
IsLockedOut = 0, 
LastLockedOutDate = @LastLockedOutDate,
FailedPasswordAttemptCount = 0,
FailedPasswordAnswerAttemptCount = 0
WHERE Username = @Username
";
            ClearParameters();
            AddParameter("@LastLockedOutDate", DbType.DateTime).Value = DateTime.Now;
            AddParameter("@Username", DbType.AnsiString, 255).Value = username;

            
            try
            {
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Add user details
        /// </summary>
        public void AddDetails(UserPrincipal model)
        {
            string sql = @"INSERT INTO dbo.UserDetails 
(
[UserID]
,[FirstName]
,[LastName]
,[MiddleName]
,[ContactPhone]
,[StateCode]
,[City]
,[AddressLine1]
,[AddressLine2]
,[PostalCode]
)
VALUES(
@UserID
,@FirstName
,@LastName
,@MiddleName
,@ContactPhone
,@StateCode
,@City
,@AddressLine1
,@AddressLine2
,@PostalCode
)";
            CommandObject.Parameters.Clear();
            AddParameter("@UserID", DbType.Int32).Value = model.UserID;
            AddParameter("@FirstName", DbType.String, 128).Value = model.FirstName;
            AddParameter("@LastName", DbType.String, 128).Value = model.LastName;
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(model.MiddleName);
            AddParameter("@ContactPhone", DbType.String, 24).Value = SqlParameterSafe(model.ContactPhone);
            AddParameter("@StateCode", DbType.AnsiStringFixedLength, 2).Value = SqlParameterSafe(model.StateCode);
            AddParameter("@City", DbType.String, 128).Value = SqlParameterSafe(model.City);
            AddParameter("@AddressLine1", DbType.String, 128).Value = SqlParameterSafe(model.AddressLine1);
            AddParameter("@AddressLine2", DbType.String, 128).Value = SqlParameterSafe(model.AddressLine2);
            AddParameter("@PostalCode", DbType.String, 24).Value = SqlParameterSafe(model.PostalCode);

            try
            {
                base.Connect();
                RunNonSelectQuery(sql);
            }
            finally
            {
                base.Disconnect();
            }
        }

        /// <summary>
        /// Update user details
        /// Change password
        /// </summary>
        public void UpdateDetails(UserPrincipal model)
        {
            string sqlUserDetails = @"
UPDATE dbo.Users SET
[Email] = @Email
,[Comment] = @Comment
WHERE PKID = @UserID;


UPDATE dbo.UserDetails SET
[FirstName] = @FirstName
,[LastName] = @LastName
,[MiddleName] = @MiddleName
,[ContactPhone] = @ContactPhone
,[StateCode] = @StateCode
,[City] = @City
,[AddressLine1] = @AddressLine1
,[AddressLine2] = @AddressLine2
,[PostalCode] = @PostalCode
WHERE UserID = @UserID


";

            CommandObject.Parameters.Clear();
            AddParameter("@Email", DbType.String, 255).Value = SqlParameterSafe(model.Email);
            AddParameter("@Comment", DbType.String, 4000).Value = model.Comments;
            AddParameter("@FirstName", DbType.String, 128).Value = model.FirstName;
            AddParameter("@LastName", DbType.String, 128).Value = model.LastName;
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(model.MiddleName);
            AddParameter("@ContactPhone", DbType.String, 24).Value = SqlParameterSafe(model.ContactPhone);
            AddParameter("@StateCode", DbType.AnsiStringFixedLength, 2).Value = SqlParameterSafe(model.StateCode);
            AddParameter("@City", DbType.String, 128).Value = SqlParameterSafe(model.City);
            AddParameter("@AddressLine1", DbType.String, 128).Value = SqlParameterSafe(model.AddressLine1);
            AddParameter("@AddressLine2", DbType.String, 128).Value = SqlParameterSafe(model.AddressLine2);
            AddParameter("@PostalCode", DbType.String, 24).Value = SqlParameterSafe(model.PostalCode);
            AddParameter("@UserID", DbType.Int32).Value = model.UserID;

            try
            {
                base.Connect();
                RunNonSelectQuery(sqlUserDetails);
            }
            finally
            {
                base.Disconnect();
            }
        }



        /// <summary>
        /// Delete user
        /// </summary>
        public void Delete(int id)
        {
            string sql = @"
Delete From dbo.SecurityLog where PKID=@UserID;
Delete from dbo.UserDetails WHERE UserID = @UserID;
Delete FROM dbo.Users Where PKID = @UserID;";


            AddParameter("@UserID", DbType.Int32).Value = id;

            try
            {
                Connect();
                BeginTransaction();
                StartConnectionSharing();

                //check is user have importnant security log records that can't be deleted
                QueryBuilder check = new QueryBuilder(@"select null from SecurityLog sl");
                check.AppendWhereCondition("sl.PKID=@UserID", ClauseType.And);
                check.AppendWhereCondition(String.Format(@"sl.SecurityEventID IN ({0},{1},{2})",
                    (int)SecurityEvents.PrintBHR,
                    (int)SecurityEvents.ViewBHR,
                    (int)SecurityEvents.BHRDeleted), ClauseType.And);

                string checkSql = String.Format("if exists({0}) SET @CanBeDeleted = 0 else SET @CanBeDeleted=1",
                    check.ToString());

                CommandObject.Parameters.Clear();
                AddParameter("@UserID", DbType.Int32).Value = id;
                DbParameter canBeDeletedParam = AddParameter("@CanBeDeleted", DbType.Boolean, ParameterDirection.Output);
                RunNonSelectQuery(checkSql);

                if (Convert.ToBoolean(canBeDeletedParam.Value))
                {
                    //delete user
                    CommandObject.Parameters.Clear();
                    AddParameter("@UserID", DbType.Int32).Value = id;
                    RunNonSelectQuery(sql);

                }
                else
                {
                    throw new SecurityLogException();
                }
                StopConnectionSharing();
                CommitTransaction();
            }
            catch
            {
                StopConnectionSharing();
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        /// <summary>
        /// Block/Unblock user
        /// </summary>
        public void Block(int userID, bool isBlock)
        {
            string sql = "Update dbo.UserDetails SET IsBlock=@IsBlock WHERE UserID = @UserID";
            try
            {
                base.Connect();
                CommandObject.Parameters.Clear();
                AddParameter("@IsBlock", DbType.Boolean).Value = SqlParameterSafe(isBlock);
                AddParameter("@UserID", DbType.Int32).Value = userID;
                RunNonSelectQuery(sql);
            }
            finally
            {
                base.Disconnect();
            }
        }


        /// <summary>
        /// Use only if user enters correct security question and answer.
        /// Do not check for previous passwords.
        /// </summary>
        /// <param name="user"></param>
        public void UpdatePassword(string userName, string newPwd)
        {
            string checkPwd = @"
DECLARE @LoweredNewPassword nvarchar(128) = LOWER(@Password)

IF EXISTS (
    SELECT NULL
    FROM Users u
        LEFT JOIN [UserPasswordHistory] uph on uph.PKID = u.PKID
    WHERE	u.Username = @Username 
            AND (@LoweredNewPassword = LOWER(u.[Password])
                OR @LoweredNewPassword = LOWER(uph.Password1)
                OR @LoweredNewPassword = LOWER(uph.Password2)))
            
BEGIN
    SET @IsValid = 0
END			
ELSE
BEGIN
    SET @IsValid = 1
END";

            string changePwd = @"
DECLARE @PKID int,
        @CurrentPassword nvarchar(128)

SELECT @PKID=PKID, @CurrentPassword = [Password]
FROM [Users] u
WHERE u.Username=@Username

UPDATE [Users] 
    SET [Password] = @Password,
        LastPasswordChangedDate = @LastPasswordChangedDate 
WHERE Username=@Username

IF EXISTS(SELECT NULL FROM [UserPasswordHistory] WHERE PKID = @PKID)
BEGIN
    UPDATE [UserPasswordHistory]
        SET Password2 = Password1,
            Password1 = @CurrentPassword
    WHERE PKID = @PKID
END		
ELSE
BEGIN
    INSERT INTO [UserPasswordHistory] (PKID, Password1, Password2)
    VALUES(@PKID, @CurrentPassword, NULL)
END;";



            try
            {
                Connect();
                BeginTransaction();

                //check for password duplicating
                CommandObject.Parameters.Clear();
                AddParameter("@Username", DbType.String, 129).Value = userName;
                AddParameter("@Password", DbType.String, 128).Value = newPwd;
                DbParameter isValidParam = AddParameter("@IsValid", DbType.Boolean, ParameterDirection.Output);

                RunNonSelectQuery(checkPwd);


                if (!Convert.ToBoolean(isValidParam.Value))
                {
                    throw new DuplicatePasswordException("Invalid password identity.");
                }

                //update pwd
                CommandObject.Parameters.Clear();
                AddParameter("@Username", DbType.String, 129).Value = userName;
                AddParameter("@Password", DbType.String, 128).Value = newPwd;
                AddParameter("@LastPasswordChangedDate", DbType.DateTime).Value = DateTime.Now;

                RunNonSelectQuery(changePwd);

                CommitTransaction();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                Disconnect();
            }
        }

        public int GetUserIDByName(string username)
        {
            const string sql = @"SELECT u.PKID FROM dbo.Users u WHERE u.Username = @Username";

            ClearParameters();
            AddParameter("@Username", DbType.String, 255).Value = username;
            try
            {
                Connect();
                return RunScalarQuery<int>(sql) ?? 0;

            }
            finally
            {
                Disconnect();
            }
        }




        #endregion



        #region User in Roles

        /// <summary>
        /// Add user to role
        /// </summary>
        public void AddUserToRole(string userName, string roleName)
        {
            string sql = @"INSERT INTO dbo.UsersInRoles (Username, Rolename) Values(@Username, @Rolename)";
            CommandObject.Parameters.Clear();
            AddParameter("@Username", DbType.String).Value = userName;
            AddParameter("@Rolename", DbType.String).Value = roleName;
            try
            {
                base.Connect();
                RunNonSelectQuery(sql);
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
                base.Disconnect();
            }
        }

        /// <summary>
        /// Update user role
        /// </summary>
        public void UpdateUserRole(string userName, string roleName)
        {
            string sql = @"Update dbo.UsersInRoles SET Rolename = @Rolename WHERE Username = @Username";
            CommandObject.Parameters.Clear();
            AddParameter("@Username", DbType.String).Value = userName;
            AddParameter("@Rolename", DbType.String).Value = roleName;
            try
            {
                base.Connect();
                RunNonSelectQuery(sql);
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
                base.Disconnect();
            }
        }

        /// <summary>
        /// true if user assign to role
        /// </summary>
        public bool IsUserInRole(string userName)
        {
            int row = 0;
            string sql = "Select Count(Username) from dbo.UsersInRoles Where Username = @Username";
            CommandObject.Parameters.Clear();
            AddParameter("@Username", DbType.String).Value = userName;

            try
            {
                base.Connect();
                row = Convert.ToInt32(RunScalarQuery(sql));
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
                base.Disconnect();
            }
            return row > 0;
        }

        /// <summary>
        /// Get roles without "Super Administrator"
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRoles()
        {
            List<string> ListOfRoles = new List<string>();
            string sql = @"Select * From dbo.Roles Where Rolename <> 'Super Administrator'";
            try
            {
                base.Connect();
                using (var reader = RunSelectQuery(sql))
                {
                    while (reader.Read())
                    {
                        ListOfRoles.Add(reader["Rolename"].ToString());
                    }
                }
            }
            catch (DbException ex)
            {
                throw ex;
            }
            finally
            {
                base.Disconnect();
            }
            return ListOfRoles;
        }


        #endregion

        #region User in Branch Location

        /// <summary>
        /// Create link to the branch location for the user.
        /// </summary>
        /// <param name="user">The user. BranchLocationID property specifies new branch location.</param>
        public bool SetBranchLocation(Int32 userID, Int32? branchLocationID)
        {
            string sql = @"
INSERT INTO dbo.Users_BranchLocation
(UserID, BranchLocationID)
VALUES
(@UserID, @BranchLocationID)
";
            CommandObject.Parameters.Clear();
            AddParameter("@UserID", DbType.Int32).Value = userID;
            AddParameter("@BranchLocationID", DbType.Int32).Value = (Int32)branchLocationID;

            if (branchLocationID == null)
            {
                // No branch location, do not create anything.
                return false;
            }
            try
            {
                base.Connect();
                int affected = this.RunNonSelectQuery(sql);
                return (affected == 1);
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
                base.Disconnect();
            }
        }

        /// <summary>
        /// Update user branch location
        /// </summary>
        public bool UpdateBranchLocation(Int32 userID, Int32? branchLocationID)
        {
            string sql = @"
UPDATE dbo.Users_BranchLocation
SET BranchLocationID = @BranchLocationID
WHERE UserID = @UserID
";
            string sqlDelete = @"
DELETE FROM dbo.Users_BranchLocation
WHERE UserID = @UserID
";
            ClearParameters();
            AddParameter("@UserID", DbType.Int32).Value = userID;

            try
            {
                base.Connect();

                if (branchLocationID == null)
                {
                    // new value is null - means no location
                    int affected = this.RunNonSelectQuery(sqlDelete);
                    return (affected == 0 || affected == 1);
                }
                else
                {
                    // update location
                    AddParameter("@BranchLocationID", DbType.Int32).Value = branchLocationID.Value;
                    int affected = this.RunNonSelectQuery(sql);
                    return (affected == 1);
                }
            }
            finally
            {
                base.Disconnect();
            }
        }

        /// <summary>
        /// true if user assign to branch location
        /// </summary>
        public bool IsUserInBranchLocation(Int32 userID)
        {
            int row = 0;
            string sql = "Select Count(UserID) from dbo.Users_BranchLocation Where UserID = @UserID";
            ClearParameters();
            AddParameter("@UserID", DbType.Int32).Value = userID;

            try
            {
                base.Connect();
                row = Convert.ToInt32(RunScalarQuery(sql));
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
                base.Disconnect();
            }
            return row > 0;
        }



        #endregion


        #region Auth Service

        public UserPrincipalAccount GetActiveUserByName(string username)
        {
            UserPrincipalAccount user = null;
            string sql = @"dbo.uspGetActiveUserByUsername";

            ClearParameters();
            AddParameter("@Username", DbType.String, 255).Value = username;
            try
            {
                base.Connect();
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        user = UserPrincipalFactory.CreateUserPrincipalAccount(reader);
                    }
                }
            }
            finally
            {
                base.Disconnect();
            }
            return user;
        }

        public UserPrincipalAccount GetByID(int userId)
        {
            UserPrincipalAccount user = null;
            string sql = @"dbo.uspGetUserByID";

            ClearParameters();
            AddParameter("@UserID", DbType.Int32).Value = userId;
            try
            {
                base.Connect();
                using (var reader = RunProcedureSelectQuery(sql))
                {
                    if (reader.Read())
                    {
                        user = UserPrincipalFactory.CreateUserPrincipalAccount(reader);
                    }
                }
            }
            finally
            {
                base.Disconnect();
            }
            return user;
        }

        public void UpdateLastLoginDate(int userID, DateTime dateTime)
        {
            string sql = "UPDATE dbo.Users SET LastLoginDate = @LastLoginDate WHERE PKID = @UserID";

            ClearParameters();
            AddParameter("@LastLoginDate", DbType.DateTime).Value = dateTime;
            AddParameter("@UserID", DbType.Int32).Value = userID;

            try
            {
                Connect();
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }


        public void UpdatePasswordFailure(int userID, int failureCount, DateTime failureWindowStart)
        {
            var sql = @"
UPDATE dbo.Users SET 
FailedPasswordAttemptCount = @Count, 
FailedPasswordAttemptWindowStart = @WindowStart
WHERE PKID = @UserID";

            ClearParameters();
            AddParameter("@Count", DbType.Int32).Value = failureCount;
            AddParameter("@WindowStart", DbType.DateTime).Value = failureWindowStart;
            AddParameter("@UserID", DbType.Int32).Value = userID;

            try
            {
                Connect();
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public void UpdatePasswordAnswerFailure(int userID, int failureCount, DateTime failureWindowStart)
        {
            var sql = @"
UPDATE dbo.Users SET 
FailedPasswordAnswerAttemptCount = @Count, 
FailedPasswordAnswerAttemptWindowStart = @WindowStart
WHERE PKID = @UserID";

            ClearParameters();
            AddParameter("@Count", DbType.Int32).Value = failureCount;
            AddParameter("@WindowStart", DbType.DateTime).Value = failureWindowStart;
            AddParameter("@UserID", DbType.Int32).Value = userID;

            try
            {
                Connect();
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public void Lock(int userID, DateTime dateTime)
        {

            var sql = @"
UPDATE dbo.Users SET IsLockedOut = @IsLockedOut, LastLockedOutDate = @LastLockedOutDate
WHERE PKID = @UserID";

            ClearParameters();
            AddParameter("@IsLockedOut", DbType.Boolean).Value = true;
            AddParameter("@LastLockedOutDate", DbType.DateTime).Value = dateTime;
            AddParameter("@UserID", DbType.Int32).Value = userID;

            try
            {
                Connect();
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }
        }

        public void ChangePassword(int userId, string newHashedPassword, DateTime currentTime)
        {
            var sql = @"
DECLARE @CurrentPassword nvarchar(128)

SELECT @CurrentPassword = [Password]
FROM dbo.Users u
WHERE u.PKID=@UserId

UPDATE dbo.Users 
    SET [Password] = @Password,
        LastPasswordChangedDate = @LastPasswordChangedDate 
WHERE PKID = @UserId

IF EXISTS(SELECT NULL FROM dbo.UserPasswordHistory WHERE PKID = @UserId)
BEGIN
    UPDATE dbo.UserPasswordHistory
        SET Password2 = Password1,
            Password1 = @CurrentPassword
    WHERE PKID = @UserId
END		
ELSE
BEGIN
    INSERT INTO dbo.UserPasswordHistory (PKID, Password1, Password2)
    VALUES(@UserId, @CurrentPassword, NULL)
END";

            ClearParameters();

            AddParameter("@Password", DbType.String, 128).Value = newHashedPassword;
            AddParameter("@LastPasswordChangedDate", DbType.DateTime).Value = currentTime;
            AddParameter("@UserId", DbType.Int32).Value = userId;


            try
            {
                RunNonSelectQuery(sql);
            }
            finally
            {
                Disconnect();
            }

        }

        public bool ValidateNewPasswordInHistory(int userId, string hashedPassword)
        {
            var sql = @"
DECLARE @LoweredNewPassword nvarchar(128) = LOWER(@Password)

IF EXISTS (
    SELECT NULL
    FROM dbo.Users u
        LEFT JOIN dbo.UserPasswordHistory uph on uph.PKID = u.PKID
    WHERE	u.PKID = @UserId 
            AND (@LoweredNewPassword = LOWER(u.[Password])
                OR @LoweredNewPassword = LOWER(uph.Password1)
                OR @LoweredNewPassword = LOWER(uph.Password2))
)
            
BEGIN
    SET @IsValid = 0
END			
ELSE
BEGIN
    SET @IsValid = 1
END";

            ClearParameters();

            AddParameter("@Password", DbType.String, 128).Value = hashedPassword;
            AddParameter("@UserId", DbType.Int32).Value = userId;
            var isValidParam = AddParameter("@IsValid", DbType.Boolean);
            isValidParam.Direction = ParameterDirection.Output;

            try
            {
                RunNonSelectQuery(sql);
                return isValidParam.Value.As<bool>();
            }
            finally
            {
                Disconnect();
            }

        }

        public bool UpdateSecurityQuestion(int userID, string newQuestion, string newAnswer)
        {

            var sql = @"UPDATE dbo.Users
                        SET PasswordQuestion = @Question, PasswordAnswer = @Answer
                        WHERE PKID = @UserID";

            ClearParameters();

            AddParameter("@Question", DbType.String, 255).Value = newQuestion;
            AddParameter("@Answer", DbType.String, 255).Value = newAnswer;
            AddParameter("@UserID", DbType.Int32).Value = userID;

            try
            {
                return RunNonSelectQuery(sql) > 0;
            }
            finally
            {
                Disconnect();
            }
        }


        /// <summary>
        /// Get list of users, suitable to display at webpage
        /// </summary>
        /// <returns></returns>
        public List<UserPrincipal> GetAll(int? branchLocationId, string orderBy)
        {

            var result = new List<UserPrincipal>();

            if (string.IsNullOrEmpty(orderBy)) orderBy = "FirstName ASC, LastName ASC"; // default sort order

            //map user field names to the query field names
            if (orderBy.Contains("FirstName"))
            {
                orderBy = orderBy.Replace("FirstName", "ud.FirstName");
            }
            if (orderBy.Contains("LastName"))
            {
                orderBy = orderBy.Replace("LastName", "ud.LastName");
            }
            if (orderBy.Contains("Username"))
            {
                orderBy = orderBy.Replace("Username", "ud.Username");
            }
            else if (orderBy.Contains("BranchLocationName"))
            {
                orderBy = orderBy.Replace("BranchLocationName", "bl.Name");
            }
            else if (orderBy.Contains("RoleName"))
            {
                orderBy = orderBy.Replace("RoleName", "ur.Rolename");
            }

            var sql = new QueryBuilder(@"
SELECT
    u.PKID as UserID
    ,u.Username
    ,u.Email
    ,ud.FirstName
    ,ud.LastName
    ,ud.MiddleName
    ,ud.FullName
    ,ud.ContactPhone
    ,bl.BranchLocationID
    ,bl.Name as BranchLocationName
    ,ur.Rolename
    ,ud.IsBlock
FROM dbo.Users u
LEFT JOIN dbo.UserDetails ud ON u.PKID = ud.UserID
LEFT JOIN dbo.Users_BranchLocation ubl ON u.PKID = ubl.UserID
LEFT JOIN dbo.BranchLocation bl ON ubl.BranchLocationID = bl.BranchLocationID
LEFT JOIN dbo.UsersInRoles ur ON u.UserName = ur.UserName
");

            sql.AppendOrderCondition(orderBy);

            ClearParameters();

            if (branchLocationId.HasValue)
            {
                sql.AppendWhereCondition("bl.BranchLocationID = @BranchLocationID", ClauseType.And);
                AddParameter("@BranchLocationID", DbType.Int32).Value = branchLocationId.Value;
            }

            try
            {
                using (var reader = RunSelectQuery(sql.ToString()))
                {
                    while (reader.Read())
                    {
                        result.Add(new UserPrincipal
                        {
                            UserID = reader.Get<int>("UserID"),
                            UserName = reader.Get<string>("Username"),
                            Email = reader.Get<string>("Email"),
                            FirstName = reader.Get<string>("FirstName"),
                            LastName = reader.Get<string>("LastName"),
                            MiddleName = reader.Get<string>("MiddleName"),
                            FullName = reader.Get<string>("FullName"),
                            ContactPhone = reader.Get<string>("ContactPhone"),
                            BranchLocationID = reader.Get<int>("BranchLocationID"),
                            BranchLocationName = reader.Get<string>("BranchLocationName"),
                            RoleName = reader.Get<string>("Rolename"),
                            IsBlock = reader.Get<bool>("IsBlock")
                        });
                    }
                }
               
            }
            finally
            {
                this.Disconnect();
            }

            return result;
        }


    }

    #endregion
}

