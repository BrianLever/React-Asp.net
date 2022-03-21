using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Common.Data;
using System.Data.Common;
using System.Data;
using System.Configuration;
using FrontDesk.Server;

namespace FrontDesk.Server.Data
{
    internal class UserDetailsDatabase : DBDatabase
    {
        #region constructor
        
        public UserDetailsDatabase(string connectionString)
            : base(connectionString) { }

        public UserDetailsDatabase()
            : base(ConfigurationManager.ConnectionStrings[0].ConnectionString)
        { }

        public UserDetailsDatabase(DbConnection sharedConnection)
            : base(sharedConnection)
        { }

        #endregion

        #region GET

        /// <summary>
        /// Get user details by user ID
        /// </summary>
        internal UserDetails GetUserByID(int userID)
        {
            UserDetails user = null;
            string sql = @"SELECT ud.*, u.Username, u.Email, u.Comment
FROM UserDetails ud INNER JOIN Users u ON ud.UserID = u.PKID
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
                        user = new UserDetails(reader);
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
            string sql = @"
SELECT ud.*, u.Username, u.Email, u.Comment, u.Username
FROM UserDetails ud INNER JOIN Users u ON ud.UserID = u.PKID";

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

        #endregion

        #region ADD/UPDATE/DELETE

        /// <summary>
        /// Add user details
        /// </summary>
        internal void AddDetails(UserDetails userDetails)
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
            AddParameter("@UserID", DbType.Int32).Value = userDetails.UserID;
            AddParameter("@FirstName", DbType.String).Value = userDetails.FirstName;
            AddParameter("@LastName", DbType.String).Value = userDetails.LastName;
            AddParameter("@MiddleName", DbType.String).Value = SqlParameterSafe(userDetails.MiddleName);
            AddParameter("@ContactPhone", DbType.String).Value = SqlParameterSafe(userDetails.ContactPhone);
            AddParameter("@StateCode", DbType.String).Value = SqlParameterSafe(userDetails.StateCode);
            AddParameter("@City", DbType.String).Value = SqlParameterSafe(userDetails.City);
            AddParameter("@AddressLine1", DbType.String).Value = SqlParameterSafe(userDetails.AddressLine1);
            AddParameter("@AddressLine2", DbType.String).Value = SqlParameterSafe(userDetails.AddressLine2);
            AddParameter("@PostalCode", DbType.String).Value = SqlParameterSafe(userDetails.PostalCode);

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
        internal void Update(UserDetails userDetails)
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
            AddParameter("@FirstName", DbType.String).Value = userDetails.FirstName;
            AddParameter("@LastName", DbType.String).Value = userDetails.LastName;
            AddParameter("@MiddleName", DbType.String).Value = SqlParameterSafe(userDetails.MiddleName);
            AddParameter("@ContactPhone", DbType.String).Value = SqlParameterSafe(userDetails.ContactPhone);
            AddParameter("@StateCode", DbType.String).Value = SqlParameterSafe(userDetails.StateCode);
            AddParameter("@City", DbType.String).Value = SqlParameterSafe(userDetails.City);
            AddParameter("@AddressLine1", DbType.String).Value = SqlParameterSafe(userDetails.AddressLine1);
            AddParameter("@AddressLine2", DbType.String).Value = SqlParameterSafe(userDetails.AddressLine2);
            AddParameter("@PostalCode", DbType.String).Value = SqlParameterSafe(userDetails.PostalCode);
            AddParameter("@UserID", DbType.Int32).Value = userDetails.UserID;

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
        /// Delete user details
        /// </summary>
        internal int DeleteDetails(int userID)
        {
            int rowEffective = 0;
            string sql = @"Delete from dbo.UserDetails WHERE UserID = @UserID";

            try
            {
                    CommandObject.Parameters.Clear();
                    AddParameter("@UserID", DbType.Int32).Value = userID;
                    rowEffective = RunNonSelectQuery(sql);
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
            return rowEffective;
        }

        /// <summary>
        /// Delete user
        /// </summary>
        internal void Delete(int userID)
        {
            string sql = @"Delete FROM Users Where PKID = @UserID";
            CommandObject.Parameters.Clear();
            AddParameter("@UserID", DbType.Int32).Value = userID;

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
        #endregion

        #region User in Branch Location

        /// <summary>
        /// Create link to the branch location for the user.
        /// </summary>
        /// <param name="user">The user. BranchLocationID property specifies new branch location.</param>
        public bool SetBranchLocation(Int32 userID, Int32 ? branchLocationID)
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
        public bool UpdateBranchLocation(Int32 userID, Int32 ? branchLocationID)
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


        #endregion
    }
}
