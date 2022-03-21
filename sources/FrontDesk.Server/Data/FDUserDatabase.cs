using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using FrontDesk.Common.Data;
using FrontDesk.Server.Logging;
using FrontDesk.Server.Membership;

namespace FrontDesk.Server.Data
{
    public class FDUserDatabase : DBDatabase
    {
        #region constructor
        
        public FDUserDatabase(string connectionString)
            : base(connectionString) { }

        public FDUserDatabase()
            : base(ConfigurationManager.ConnectionStrings[0].ConnectionString)
        { }

        public FDUserDatabase(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region GET

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        internal FDUser GetUserByID(int userID)
        {
            FDUser user = null;
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
                using(var reader = RunSelectQuery(sql))
                {
                    if(reader.Read())
                    {
                        user = new FDUser(reader);
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
            return user;
        }

        /// <summary>
        /// Get all user
        /// </summary>
        internal DataSet GetAllUser()
        {
            DataSet ds = null;
            string sql = @"SELECT ud.*, u.Username, u.Email, u.Comment, ubl.BranchLocationID, ur.Rolename
FROM dbo.UserDetails ud INNER JOIN Users u ON ud.UserID = u.PKID
LEFT JOIN dbo.Users_BranchLocation ubl ON ud.UserID = ubl.UserID
Left JOIN dbo.UsersInRoles ur ON u.UserName = ur.UserName";

            try
            {
                base.Connect();
                ds = GetDataSet(sql);
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

            return ds;
        }

        /// <summary>
        /// True if User is block
        /// </summary>
        internal bool ValidateFDUser(string userName)
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
        internal bool isLocked(string userName)
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
        internal void AddDetails(FDUser FDUser)
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
            AddParameter("@UserID", DbType.Int32).Value = FDUser.UserID;
            AddParameter("@FirstName", DbType.String, 128).Value = FDUser.FirstName;
            AddParameter("@LastName", DbType.String, 128).Value = FDUser.LastName;
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(FDUser.MiddleName);
            AddParameter("@ContactPhone", DbType.String, 24).Value = SqlParameterSafe(FDUser.ContactPhone);
            AddParameter("@StateCode", DbType.AnsiStringFixedLength, 2).Value = SqlParameterSafe(FDUser.StateCode);
            AddParameter("@City", DbType.String, 128).Value = SqlParameterSafe(FDUser.City);
            AddParameter("@AddressLine1", DbType.String, 128).Value = SqlParameterSafe(FDUser.AddressLine1);
            AddParameter("@AddressLine2", DbType.String, 128).Value = SqlParameterSafe(FDUser.AddressLine2);
            AddParameter("@PostalCode", DbType.String, 24).Value = SqlParameterSafe(FDUser.PostalCode);

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
        /// Update user details
        /// Change password
        /// </summary>
        internal void Update(FDUser FDUser)
        {
            string sql = @"UPDATE dbo.UserDetails SET
[FirstName] = @FirstName
,[LastName] = @LastName
,[MiddleName] = @MiddleName
,[ContactPhone] = @ContactPhone
,[StateCode] = @StateCode
,[City] = @City
,[AddressLine1] = @AddressLine1
,[AddressLine2] = @AddressLine2
,[PostalCode] = @PostalCode
WHERE UserID = @UserID";

            CommandObject.Parameters.Clear();
            AddParameter("@FirstName", DbType.String, 128).Value = FDUser.FirstName;
            AddParameter("@LastName", DbType.String, 128).Value = FDUser.LastName;
            AddParameter("@MiddleName", DbType.String, 128).Value = SqlParameterSafe(FDUser.MiddleName);
            AddParameter("@ContactPhone", DbType.String, 24).Value = SqlParameterSafe(FDUser.ContactPhone);
            AddParameter("@StateCode", DbType.AnsiStringFixedLength, 2).Value = SqlParameterSafe(FDUser.StateCode);
            AddParameter("@City", DbType.String, 128).Value = SqlParameterSafe(FDUser.City);
            AddParameter("@AddressLine1", DbType.String, 128).Value = SqlParameterSafe(FDUser.AddressLine1);
            AddParameter("@AddressLine2", DbType.String, 128).Value = SqlParameterSafe(FDUser.AddressLine2);
            AddParameter("@PostalCode", DbType.String, 24).Value = SqlParameterSafe(FDUser.PostalCode);
            AddParameter("@UserID", DbType.Int32).Value = FDUser.UserID;

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
        /// Delete user
        /// </summary>
        internal void Delete(int userID)
        {
            string sql = @"
Delete From dbo.SecurityLog where PKID=@UserID;
Delete from dbo.UserDetails WHERE UserID = @UserID;
Delete FROM dbo.Users Where PKID = @UserID;";


            AddParameter("@UserID", DbType.Int32).Value = userID;

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
                AddParameter("@UserID", DbType.Int32).Value = userID;
                DbParameter canBeDeletedParam = AddParameter("@CanBeDeleted", DbType.Boolean, ParameterDirection.Output);
                RunNonSelectQuery(checkSql);

                if (Convert.ToBoolean(canBeDeletedParam.Value))
                {
                    ////delete user details
                    //DeleteDetails(userID);

                    //delete user
                    CommandObject.Parameters.Clear();
                    AddParameter("@UserID", DbType.Int32).Value = userID;
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
        internal void Block(int userID, bool isBlock)
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
        /// Use only if user enters correct security question and answer.
        /// Do not check for previous passwords.
        /// </summary>
        /// <param name="user"></param>
        internal void UpdatePassword(string userName, string newPwd)
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

        internal int GetUserIDByName(string username)
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
        internal void AddUserToRole(string userName, string roleName)
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
        internal void UpdateUserRole(string userName, string roleName)
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
        internal bool IsUserInRole(string userName)
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
        internal bool SetBranchLocation(Int32 userID, Int32 ? branchLocationID)
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
        internal bool UpdateBranchLocation(Int32 userID, Int32 ? branchLocationID)
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
            try
            {
                base.Connect();

                if (branchLocationID == null)
                {
                    // new value is null - means no location
                    CommandObject.Parameters.Clear();
                    AddParameter("@UserID", DbType.Int32).Value = userID;
                    int affected = this.RunNonSelectQuery(sqlDelete);
                    return (affected == 0 || affected == 1);
                }
                else
                {
                    // update location
                    this.AddParameter("@UserID", DbType.Int32).Value = userID;
                    this.AddParameter("@BranchLocationID", DbType.Int32).Value = (Int32)branchLocationID;
                    int affected = this.RunNonSelectQuery(sql);
                    return (affected == 1);
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
        }

        /// <summary>
        /// true if user assign to branch location
        /// </summary>
        internal bool IsUserInBranchLocation(Int32 userID)
        {
            int row = 0;
            string sql = "Select Count(UserID) from dbo.Users_BranchLocation Where UserID = @UserID";
            CommandObject.Parameters.Clear();
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
    }
}
