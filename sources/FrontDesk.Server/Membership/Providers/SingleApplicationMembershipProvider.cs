using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;

namespace FrontDesk.Server.Membership.Providers
{
    /// <summary>
    /// Default simple membership provider
    /// </summary>
    [Obsolete("Migrated to AuthService")]
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed - need for datasources on pages
    public class SingleApplicationMembershipProvider : MembershipProvider
    {

        //
        // Global connection string, generated password length, generic exception message, event log info.
        //

        private int newPasswordLength = 3;
        private string eventSource = "FrontDesk MembershipProvider";
        private string eventLog = "Application";
        protected string exceptionMessage = "An exception occurred. Please check the Event Log.";
        protected string tableName = "Users";
        protected string passwordHistoryTableName = "UserPasswordHistory";
        protected string connectionString;


        #region Database connection

        protected SqlConnection _conn;

        protected SqlConnection Connection
        {
            get
            {
                if (_conn == null) _conn = new SqlConnection(connectionString);
                return _conn;
            }
        }

        protected void Connect()
        {
            if (_conn == null) _conn = new SqlConnection(connectionString);
            _conn.Open();
        }
        protected void Disconnect()
        {
            if (_conn != null)
            {
                _conn.Close();
                _conn = null;
            }
        }

        #endregion

        /// <summary>
        /// Used when determining encryption key values.
        /// </summary>
        private MachineKeySection machineKey;


        /// <summary>
        /// If false, exceptions are thrown to the caller. If true,
        /// exceptions are written to the event log.
        /// </summary>
        private bool pWriteExceptionsToEventLog;

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }

        /// <summary>
        /// System.Configuration.Provider.ProviderBase.Initialize Method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, NameValueCollection config)
        {
            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "SIMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Membership provider");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);

            pApplicationName = GetConfigValue(config["applicationName"],
                                            System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            pMaxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            pPasswordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            pMinRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonAlphanumericCharacters"], "1"));
            pMinRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "7"));
            pPasswordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));
            pEnablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            pEnablePasswordRetrieval = Convert.ToBoolean(GetConfigValue(config["enablePasswordRetrieval"], "true"));
            pRequiresQuestionAndAnswer = Convert.ToBoolean(GetConfigValue(config["requiresQuestionAndAnswer"], "false"));
            pRequiresUniqueEmail = Convert.ToBoolean(GetConfigValue(config["requiresUniqueEmail"], "true"));
            pRequiresUniqueUserName = Convert.ToBoolean(GetConfigValue(config["requiresUniqueUserName"], "true"));
            pWriteExceptionsToEventLog = Convert.ToBoolean(GetConfigValue(config["writeExceptionsToEventLog"], "true"));

            string temp_format = config["passwordFormat"];
            if (temp_format == null)
            {
                temp_format = "Hashed";
            }

            switch (temp_format)
            {
                case "Hashed":
                    pPasswordFormat = MembershipPasswordFormat.Hashed;
                    break;
                case "Encrypted":
                    pPasswordFormat = MembershipPasswordFormat.Encrypted;
                    break;
                case "Clear":
                    pPasswordFormat = MembershipPasswordFormat.Clear;
                    break;
                default:
                    throw new ProviderException("Password format not supported.");
            }

            //
            // Initialize SqlConnection.
            //

            ConnectionStringSettings ConnectionStringSettings =
              ConfigurationManager.ConnectionStrings[config["connectionStringName"]];

            if (ConnectionStringSettings == null || ConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = ConnectionStringSettings.ConnectionString;


            // Get encryption and decryption key information from the configuration.
            machineKey = (MachineKeySection)ConfigurationManager.GetSection("system.web/machineKey");
        }

        /// <summary>
        /// A helper function to retrieve config values from the configuration file.
        /// </summary>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        protected string GetConfigValue(string configValue, string defaultValue)
        {
            if (String.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }


        //
        // System.Web.Security.MembershipProvider properties.
        //
        private string pApplicationName;
        private bool pEnablePasswordReset;
        private bool pEnablePasswordRetrieval;
        private bool pRequiresQuestionAndAnswer;
        private bool pRequiresUniqueEmail;
        private bool pRequiresUniqueUserName;
        private int pMaxInvalidPasswordAttempts;
        private int pPasswordAttemptWindow;
        private MembershipPasswordFormat pPasswordFormat;

        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }


        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }


        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }


        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }

        public bool RequiresUniqueUserName
        {
            get { return pRequiresUniqueUserName; }
        }


        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }


        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }


        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        private int pMinRequiredNonAlphanumericCharacters;

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        private int pMinRequiredPasswordLength;

        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        private string pPasswordStrengthRegularExpression;

        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        //
        // System.Web.Security.MembershipProvider methods.
        //


        /// <summary>
        /// MembershipProvider.ChangePassword
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPwd"></param>
        /// <param name="newPwd"></param>
        /// <returns></returns>
        public override bool ChangePassword(string username, string oldPwd, string newPwd)
        {
            if (!ValidateUser(username, oldPwd))
                return false;

            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, newPwd, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Change password canceled due to new password validation failure.");


            SqlCommand checkPswCmd = new SqlCommand(
                String.Format(@"
DECLARE @LoweredNewPassword nvarchar(128) = LOWER(@Password)

IF EXISTS (
    SELECT NULL
    FROM [{0}] u
        LEFT JOIN [{1}] uph on uph.PKID = u.PKID
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
END", tableName, passwordHistoryTableName));

            checkPswCmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = EncodePassword(newPwd);
            checkPswCmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
            SqlParameter IsPswValidParameter = new SqlParameter("@IsValid", SqlDbType.Bit);
            IsPswValidParameter.Direction = ParameterDirection.Output;
            checkPswCmd.Parameters.Add(IsPswValidParameter);



            SqlCommand changePwdCmd = new SqlCommand(
                String.Format(@"
DECLARE @PKID int,
        @CurrentPassword nvarchar(128)

SELECT @PKID=PKID, @CurrentPassword = [Password]
FROM [{0}] u
WHERE u.Username=@Username

UPDATE [{0}] 
    SET [Password] = @Password,
        LastPasswordChangedDate = @LastPasswordChangedDate 
WHERE Username = @Username

IF EXISTS(SELECT NULL FROM [{1}] WHERE PKID = @PKID)
BEGIN
    UPDATE [{1}]
        SET Password2 = Password1,
            Password1 = @CurrentPassword
    WHERE PKID = @PKID
END		
ELSE
BEGIN
    INSERT INTO [{1}] (PKID, Password1, Password2)
    VALUES(@PKID, @CurrentPassword, NULL)
END", tableName, passwordHistoryTableName));

            changePwdCmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = EncodePassword(newPwd);
            changePwdCmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = DateTime.Now;
            changePwdCmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            int rowsAffected = 0;

            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlTransaction t = conn.BeginTransaction())
                    {
                        //check password identity
                        checkPswCmd.Connection = conn;
                        checkPswCmd.Transaction = t;
                        checkPswCmd.ExecuteNonQuery();
                        if (!Convert.ToBoolean(IsPswValidParameter.Value))
                        {
                            throw new DuplicatePasswordException("Invalid password identity.");
                        }

                        //update passsword
                        changePwdCmd.Connection = conn;
                        changePwdCmd.Transaction = t;
                        rowsAffected = changePwdCmd.ExecuteNonQuery();

                        t.Commit();
                    }
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ChangePassword");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }

            return rowsAffected > 0;
        }

        /// <summary>
        /// MembershipProvider.ChangePasswordQuestionAndAnswer
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPwdQuestion"></param>
        /// <param name="newPwdAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string username,
                      string password,
                      string newPwdQuestion,
                      string newPwdAnswer)
        {
            if (ValidateUser(username, password))
            {

                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(string.Format(
                        @"UPDATE [{0}]
                        SET PasswordQuestion = @Question, PasswordAnswer = @Answer
                        WHERE UserName = @Username", tableName)
                            , conn);

                cmd.Parameters.Add("@Question", SqlDbType.VarChar, 255).Value = newPwdQuestion;
                cmd.Parameters.Add("@Answer", SqlDbType.VarChar, 255).Value = EncodePassword(newPwdAnswer);
                cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                int rowsAffected = 0;

                try
                {
                    conn.Open();
                    rowsAffected = cmd.ExecuteNonQuery();
                }
                catch (SqlException e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "ChangePasswordQuestionAndAnswer");

                        throw new ProviderException(exceptionMessage);
                    }
                    else
                    {
                        throw e;
                    }
                }
                finally
                {
                    conn.Close();
                }
                if (rowsAffected > 0)
                {
                    return true;
                }
            }
            return false;
        }



        /// <summary>
        /// MembershipProvider.CreateUser
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(string username,
                 string password,
                 string email,
                 string passwordQuestion,
                 string passwordAnswer,
                 bool isApproved,
                 object providerUserKey,
                 out MembershipCreateStatus status)
        {

            System.Diagnostics.Trace.Write("SIMembershipProvider.CreateUser");

            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && GetUserNameByEmail(email) != "")
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            if (RequiresUniqueUserName && GetUser(username, false) != null)
            {
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            MembershipUser u = GetUser(username, false);

            if (u == null)
            {
                DateTime createDate = DateTime.Now;

                if (providerUserKey == null)
                {
                    providerUserKey = Int32.MinValue;
                }
                else
                {
                    if (!(providerUserKey is Int32))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(string.Format(
                     @"INSERT INTO [{0}]
                       (Username, Password, Email, PasswordQuestion,
                       PasswordAnswer, IsApproved,
                       Comment, CreationDate, LastPasswordChangedDate, LastActivityDate,
                       IsLockedOut, LastLockedOutDate,
                       FailedPasswordAttemptCount, FailedPasswordAttemptWindowStart, 
                       FailedPasswordAnswerAttemptCount, FailedPasswordAnswerAttemptWindowStart)
                       Values(@Username, @Password, @Email, @PasswordQuestion, @PasswordAnswer, 
                       @IsApproved, @Comment, @CreationDate, @LastPasswordChangedDate, @LastActivityDate, 
                       @IsLockedOut, @LastLockedOutDate, @FailedPasswordAttemptCount, @FailedPasswordAttemptWindowStart, 
                       @FailedPasswordAnswerAttemptCount, @FailedPasswordAnswerAttemptWindowStart);
                       SET @PKID = SCOPE_IDENTITY();
                       ", tableName), conn);

                cmd.Parameters.Add("@PKID", SqlDbType.Int).Direction = ParameterDirection.Output;
                cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
                cmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = EncodePassword(password);
                if (String.IsNullOrEmpty(email))
                {
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = DBNull.Value;
                }
                else
                {
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = email;
                }

                cmd.Parameters.Add("@PasswordQuestion", SqlDbType.VarChar, 255).Value = checkDBNull(passwordQuestion);
                cmd.Parameters.Add("@PasswordAnswer", SqlDbType.VarChar, 255).Value = checkDBNull(EncodePassword(passwordAnswer));
                cmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = isApproved;
                cmd.Parameters.Add("@Comment", SqlDbType.VarChar, 255).Value = "";
                cmd.Parameters.Add("@CreationDate", SqlDbType.DateTime).Value = createDate;
                cmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = createDate;
                cmd.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = createDate;
                cmd.Parameters.Add("@IsLockedOut", SqlDbType.Bit).Value = false;
                cmd.Parameters.Add("@LastLockedOutDate", SqlDbType.DateTime).Value = createDate;
                cmd.Parameters.Add("@FailedPasswordAttemptCount", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FailedPasswordAttemptWindowStart", SqlDbType.DateTime).Value = createDate;
                cmd.Parameters.Add("@FailedPasswordAnswerAttemptCount", SqlDbType.Int).Value = 0;
                cmd.Parameters.Add("@FailedPasswordAnswerAttemptWindowStart", SqlDbType.DateTime).Value = createDate;

                try
                {
                    conn.Open();

                    int recAdded = cmd.ExecuteNonQuery();

                    if (recAdded > 0)
                    {
                        status = MembershipCreateStatus.Success;
                    }
                    else
                    {
                        status = MembershipCreateStatus.UserRejected;
                    }
                }
                catch (SqlException e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "CreateUser");
                    }

                    status = MembershipCreateStatus.ProviderError;
                }
                finally
                {
                    conn.Close();
                }


                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }


            return null;
        }



        /// <summary>
        /// MembershipProvider.DeleteUser
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            int rowsAffected = 0;
            ///don't delete su user
            if (!username.Equals("su"))
            {
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand cmd = new SqlCommand(string.Format(
                        @"DELETE FROM [{0}]
                        WHERE Username = @Username ", tableName), conn);

                cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                try
                {
                    conn.Open();

                    rowsAffected = cmd.ExecuteNonQuery();

                    if (deleteAllRelatedData)
                    {
                        // Process commands to delete all data for the user in the database.
                    }
                }
                catch (SqlException e)
                {
                    if (WriteExceptionsToEventLog)
                    {
                        WriteToEventLog(e, "DeleteUser");

                        throw new ProviderException(exceptionMessage);
                    }
                    else
                    {
                        throw e;
                    }
                }
                finally
                {
                    conn.Close();
                }
            }
            if (rowsAffected > 0)
                return true;

            return false;
        }

        /// <summary>
        /// MembershipProvider.GetAllUsers
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(
                @"SELECT Count(*) FROM [{0}]", tableName), conn);

            MembershipUserCollection users = new MembershipUserCollection();

            SqlDataReader reader = null;
            totalRecords = 0;

            try
            {
                conn.Open();
                totalRecords = (int)cmd.ExecuteScalar();

                if (totalRecords <= 0) { return users; }

                cmd.CommandText = string.Format(
                        @"SELECT PKID, Username, Email, PasswordQuestion,
                          Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate,
                          LastActivityDate, LastPasswordChangedDate, LastLockedOutDate 
                          FROM [{0}]
                          ORDER BY Username Asc", tableName);

                reader = cmd.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { cmd.Cancel(); }

                    counter++;
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllUsers");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            return users;
        }



        /// <summary>
        /// MembershipProvider.GetNumberOfUsersOnline
        /// </summary>
        /// <returns></returns>
        public override int GetNumberOfUsersOnline()
        {

            TimeSpan onlineSpan = new TimeSpan(0, System.Web.Security.Membership.UserIsOnlineTimeWindow, 0);
            DateTime compareTime = DateTime.Now.Subtract(onlineSpan);

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"SELECT Count(*) FROM [{0}]
                    WHERE LastActivityDate > @CompareDate", tableName), conn);

            cmd.Parameters.Add("@CompareDate", SqlDbType.DateTime).Value = compareTime;

            int numOnline = 0;

            try
            {
                conn.Open();

                numOnline = (int)cmd.ExecuteScalar();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetNumberOfUsersOnline");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            return numOnline;
        }

        /// <summary>
        /// MembershipProvider.GetPassword
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string username, string answer)
        {
            if (!EnablePasswordRetrieval)
            {
                throw new ProviderException("Password Retrieval Not Enabled.");
            }

            if (PasswordFormat == MembershipPasswordFormat.Hashed)
            {
                throw new ProviderException("Cannot retrieve Hashed passwords.");
            }

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(
                @"SELECT Password, PasswordAnswer, IsLockedOut FROM [{0}]
                  WHERE Username = @Username", tableName), conn);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            string password = "";
            string passwordAnswer = "";
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(2))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    password = reader.GetString(0);
                    passwordAnswer = reader.GetString(1);
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetPassword");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }


            if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new MembershipPasswordException("Incorrect password answer.");
            }


            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                password = UnEncodePassword(password);
            }

            return password;
        }



        //
        // MembershipProvider.GetUser(string, bool)
        //

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT PKID, Username, Email, PasswordQuestion," +
                 " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                 " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate" + //, LastName, FirstName, MiddleName
                 " FROM [" + tableName + "] WHERE Username = @Username", conn);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            MembershipUser u = null;
            SqlDataReader reader = null;

            try
            {

                conn.Open();

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    u = GetUserFromReader(reader);
                    reader.Close();
                    if (userIsOnline)
                    {
                        SqlCommand updateCmd = new SqlCommand("UPDATE [" + tableName + "] " +
                                  "SET LastActivityDate = @LastActivityDate " +
                                  "WHERE Username = @Username", conn);

                        updateCmd.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = DateTime.Now;
                        updateCmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                        updateCmd.ExecuteNonQuery();
                    }


                }

            }
            catch (SqlException e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(String, Boolean)");

                    //throw new ProviderException(exceptionMessage);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(e.Message);
                    //throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }


        //
        // MembershipProvider.GetUser(object, bool)
        //

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT PKID, Username, Email, PasswordQuestion," +
                  " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                  " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate " + //, LastName, FirstName, MiddleName
                  " FROM [" + tableName + "] WHERE PKID = @PKID", conn);

            cmd.Parameters.Add("@PKID", SqlDbType.Int).Value = providerUserKey;

            FDMembershipUser u = null;
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    reader.Read();
                    u = (FDMembershipUser)GetUserFromReader(reader);

                    if (userIsOnline)
                    {
                        SqlCommand updateCmd = new SqlCommand("UPDATE [" + tableName + "] " +
                                  "SET LastActivityDate = @LastActivityDate " +
                                  "WHERE PKID = @PKID", conn);

                        updateCmd.Parameters.Add("@LastActivityDate", SqlDbType.DateTime).Value = DateTime.Now;
                        updateCmd.Parameters.Add("@PKID", SqlDbType.UniqueIdentifier).Value = providerUserKey;

                        updateCmd.ExecuteNonQuery();
                    }
                }

            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUser(Object, Boolean)");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                conn.Close();
            }

            return u;
        }


        //
        // GetUserFromReader
        //    A helper function that takes the current row from the SqlDataReader
        // and hydrates a MembershiUser from the values. Called by the 
        // MembershipUser.GetUser implementation.
        //
        private MembershipUser GetUserFromReader(SqlDataReader reader)
        {
            object providerUserKey = reader.GetValue(0);
            string username = reader.GetString(1);
            string email = reader.GetValue(2).ToString();

            string passwordQuestion = "";
            if (reader.GetValue(3) != DBNull.Value)
                passwordQuestion = reader.GetString(3);

            string comment = "";
            if (reader.GetValue(4) != DBNull.Value)
                comment = reader.GetString(4);
            bool isApproved = false;
            if (reader.GetValue(5) != DBNull.Value)
            {
                isApproved = reader.GetBoolean(5);
            }
            bool isLockedOut = false;
            if (reader.GetValue(6) != DBNull.Value)
            {
                isLockedOut = reader.GetBoolean(6);
            }
            DateTime creationDate = reader.GetDateTime(7);

            DateTime lastLoginDate = new DateTime();
            if (reader.GetValue(8) != DBNull.Value)
                lastLoginDate = reader.GetDateTime(8);

            DateTime lastActivityDate = reader.IsDBNull(9) ? DateTime.MinValue : reader.GetDateTime(9);
            DateTime lastPasswordChangedDate = reader.IsDBNull(10) ? DateTime.MinValue : reader.GetDateTime(10);

            DateTime lastLockedOutDate = new DateTime();
            if (reader.GetValue(11) != DBNull.Value)
                lastLockedOutDate = reader.GetDateTime(11);

            //string lastName = reader.IsDBNull(12) ? "" : reader.GetString(12);
            //string firstName = reader.IsDBNull(13) ? "" : reader.GetString(13);
            //string middleName = reader.IsDBNull(14) ? "" : reader.GetString(14);

            FDMembershipUser u = new FDMembershipUser(this.Name,
                                                  username,
                                                  providerUserKey,
                                                  email,
                                                  passwordQuestion,
                                                  comment,
                                                  isApproved,
                                                  isLockedOut,
                                                  creationDate,
                                                  lastLoginDate,
                                                  lastActivityDate,
                                                  lastPasswordChangedDate,
                                                  lastLockedOutDate);
            //u.FirstName = firstName;
            //u.LastName = lastName;
            //u.MiddleName = middleName;

            return u;
        }


        //
        // MembershipProvider.UnlockUser
        //

        public override bool UnlockUser(string username)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("UPDATE [" + tableName + "] " +
                                              " SET IsLockedOut = 0, LastLockedOutDate = @LastLockedOutDate, " +
                                              " FailedPasswordAttemptCount = 0, " +
                                              " FailedPasswordAnswerAttemptCount = 0 " +
                                              " WHERE Username = @Username", conn);

            cmd.Parameters.Add("@LastLockedOutDate", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            int rowsAffected = 0;

            try
            {
                conn.Open();

                rowsAffected = cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UnlockUser");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (rowsAffected > 0)
                return true;

            return false;
        }


        //
        // MembershipProvider.GetUserNameByEmail
        //

        public override string GetUserNameByEmail(string email)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Username" +
                  " FROM [" + tableName + "] WHERE Email = @Email", conn);

            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = email;

            string username = "";

            try
            {
                conn.Open();

                username = (string)cmd.ExecuteScalar();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUserNameByEmail");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                conn.Close();
            }

            if (username == null)
                username = "";

            return username;
        }


        //
        // MembershipProvider.ResetPassword
        //

        public override string ResetPassword(string username, string answer)
        {
            if (!EnablePasswordReset)
            {
                throw new NotSupportedException("Password reset is not enabled.");
            }

            if (answer == null && RequiresQuestionAndAnswer)
            {
                UpdateFailureCount(username, "passwordAnswer");

                throw new ProviderException("Password answer required for password reset.");
            }


            string newPassword =
              System.Web.Security.Membership.GeneratePassword(newPasswordLength, MinRequiredNonAlphanumericCharacters);


            ValidatePasswordEventArgs args =
              new ValidatePasswordEventArgs(username, newPassword, true);

            OnValidatingPassword(args);

            if (args.Cancel)
                if (args.FailureInformation != null)
                    throw args.FailureInformation;
                else
                    throw new MembershipPasswordException("Reset password canceled due to password validation failure.");


            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT PasswordAnswer, IsLockedOut FROM [" + tableName + "]" +
                  " WHERE Username = @Username", conn);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            //int rowsAffected = 0;
            string passwordAnswer = "";
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (reader.GetBoolean(1))
                        throw new MembershipPasswordException("The supplied user is locked out.");

                    passwordAnswer = Convert.ToString(reader[0]);
                }
                else
                {
                    throw new MembershipPasswordException("The supplied user name is not found.");
                }

                if (!reader.IsClosed)
                {
                    reader.Close();
                }

                if (RequiresQuestionAndAnswer && !CheckPassword(answer, passwordAnswer))
                {
                    UpdateFailureCount(username, "passwordAnswer");

                    throw new MembershipPasswordException("Incorrect password answer.");
                }

                /*
                SqlCommand updateCmd = new SqlCommand("UPDATE [" + tableName + "]" +
                    " SET Password = @Password, LastPasswordChangedDate = @LastPasswordChangedDate" +
                    " WHERE Username = @Username AND IsLockedOut = 0", conn);

                updateCmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = EncodePassword(newPassword);
                updateCmd.Parameters.Add("@LastPasswordChangedDate", SqlDbType.DateTime).Value = DateTime.Now;
                updateCmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                rowsAffected = updateCmd.ExecuteNonQuery();
                 */
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ResetPassword");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                conn.Close();
            }

            //if (rowsAffected > 0)
            //{
            return newPassword;
            //}
            //else
            //{
            //  throw new MembershipPasswordException("User not found, or user is locked out. Password not Reset.");
            //}
        }


        //
        // MembershipProvider.UpdateUser
        //

        public override void UpdateUser(MembershipUser user)
        {
            FDMembershipUser oUser = (FDMembershipUser)user;

            SqlCommand cmd = new SqlCommand(string.Format(
                  @"UPDATE [{0}]
                    SET Email = @Email, Comment = @Comment,
                    IsApproved = @IsApproved, 
                    IsLockedOut=@IsLockedOut 
                    WHERE Username = @Username AND (UserName<> 'su' OR @Username='su')", tableName), this.Connection);

            if (String.IsNullOrEmpty(oUser.Email))
            {
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = DBNull.Value;
            }
            else
            {
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 128).Value = oUser.Email;
            }
            cmd.Parameters.Add("@Comment", SqlDbType.VarChar, 255).Value = oUser.Comment;
            cmd.Parameters.Add("@IsApproved", SqlDbType.Bit).Value = oUser.IsApproved;
            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = oUser.UserName;
            cmd.Parameters.Add("@IsLockedOut", SqlDbType.Bit).Value = oUser.IsLockedOut;

            try
            {
                Connect();
                cmd.ExecuteNonQuery();

                if (oUser.UserName.Equals("su") && oUser.IsLockedOut)
                {
                    UnlockUser("su");
                }

                /*
                ///change password
                if (!String.IsNullOrEmpty(oUser.NewPasswordValue))
                {

                    cmd.Parameters.Clear();

                    cmd.CommandText = "UPDATE [" + tableName + "]" +
                    " SET Password = @Password, LastPasswordChangedDate = @LastPasswordChangedDate " +
                    " WHERE Username = @Username";

                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = oUser.UserName;
                    cmd.Parameters.Add("@Password", SqlDbType.VarChar, 255).Value = EncodePassword(oUser.NewPasswordValue);
                    cmd.Parameters.Add("@PasswordQuestion", SqlDbType.VarChar, 255).Value = oUser.PasswordQuestion;

                    cmd.ExecuteNonQuery();
                }
                 * */
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateUser");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                Disconnect();
            }
        }


        //
        // MembershipProvider.ValidateUser
        // True if password and user name are correct and account not blocked od locked
        //
        public override bool ValidateUser(string username, string password)
        {
            bool isValid = false;

            //SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT u.Password, u.IsApproved FROM [" + tableName + "] u " +
                    "INNER JOIN dbo.UserDetails ud ON u.PKID = ud.UserID " +
                    "WHERE u.Username = @Username AND u.IsLockedOut = 0 AND ud.isBlock = 0", this.Connection);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            SqlDataReader reader = null;
            bool isApproved = false;
            string pwd = "";

            try
            {
                Connect();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();
                    pwd = reader.GetString(0);
                    isApproved = reader.GetBoolean(1);
                }
                else
                {
                    return false;
                }

                reader.Close();

                if (CheckPassword(password, pwd))
                {
                    if (isApproved)
                    {
                        isValid = true;

                        SqlCommand updateCmd = new SqlCommand("UPDATE [" + tableName + "] SET LastLoginDate = @LastLoginDate" +
                                                                " WHERE Username = @Username", this.Connection);

                        updateCmd.Parameters.Add("@LastLoginDate", SqlDbType.DateTime).Value = DateTime.Now;
                        updateCmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                        updateCmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    Disconnect();
                    if (!username.Equals("su"))
                    {
                        UpdateFailureCount(username, "password");
                    }
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "ValidateUser");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                Disconnect();
            }

            return isValid;
        }


        //
        // UpdateFailureCount
        //   A helper method that performs the checks and updates associated with
        // password failure tracking.
        //

        protected void UpdateFailureCount(string username, string failureType)
        {
            //SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT FailedPasswordAttemptCount, " +
                                              "  FailedPasswordAttemptWindowStart, " +
                                              "  FailedPasswordAnswerAttemptCount, " +
                                              "  FailedPasswordAnswerAttemptWindowStart " +
                                              "  FROM [" + tableName + "] " +
                                              "  WHERE Username = @Username", this.Connection);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            SqlDataReader reader = null;
            DateTime windowStart = new DateTime();
            int failureCount = 0;

            try
            {
                Connect();

                reader = cmd.ExecuteReader(CommandBehavior.SingleRow);

                if (reader.HasRows)
                {
                    reader.Read();

                    if (failureType == "password")
                    {
                        failureCount = reader.GetInt32(0);
                        windowStart = reader.GetDateTime(1);
                    }

                    if (failureType == "passwordAnswer")
                    {
                        failureCount = reader.GetInt32(2);
                        windowStart = reader.GetDateTime(3);
                    }
                }

                reader.Close();

                DateTime windowEnd = windowStart.AddMinutes(PasswordAttemptWindow);

                if (failureCount == 0 || DateTime.Now > windowEnd)
                {
                    // First password failure or outside of PasswordAttemptWindow. 
                    // Start a new password failure count from 1 and a new window starting now.

                    if (failureType == "password")
                        cmd.CommandText = "UPDATE [" + tableName + "] " +
                                          "  SET FailedPasswordAttemptCount = @Count, " +
                                          "      FailedPasswordAttemptWindowStart = @WindowStart " +
                                          "  WHERE Username = @Username";

                    if (failureType == "passwordAnswer")
                        cmd.CommandText = "UPDATE [" + tableName + "] " +
                                          "  SET FailedPasswordAnswerAttemptCount = @Count, " +
                                          "      FailedPasswordAnswerAttemptWindowStart = @WindowStart " +
                                          "  WHERE Username = @Username";

                    cmd.Parameters.Clear();

                    cmd.Parameters.Add("@Count", SqlDbType.Int).Value = 1;
                    cmd.Parameters.Add("@WindowStart", SqlDbType.DateTime).Value = DateTime.Now;
                    cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                    if (cmd.ExecuteNonQuery() < 0)
                        throw new ProviderException("Unable to update failure count and window start.");
                }
                else
                {
                    if (failureCount++ >= MaxInvalidPasswordAttempts)
                    {
                        // Password attempts have exceeded the failure threshold. Lock out
                        // the user.

                        cmd.CommandText = "UPDATE [" + tableName + "] " +
                                          "  SET IsLockedOut = @IsLockedOut, LastLockedOutDate = @LastLockedOutDate " +
                                          "  WHERE Username = @Username";

                        cmd.Parameters.Clear();

                        cmd.Parameters.Add("@IsLockedOut", SqlDbType.Bit).Value = true;
                        cmd.Parameters.Add("@LastLockedOutDate", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to lock out user.");
                    }
                    else
                    {
                        // Password attempts have not exceeded the failure threshold. Update
                        // the failure counts. Leave the window the same.

                        if (failureType == "password")
                            cmd.CommandText = "UPDATE [" + tableName + "] " +
                                              "  SET FailedPasswordAttemptCount = @Count" +
                                              "  WHERE Username = @Username";

                        if (failureType == "passwordAnswer")
                            cmd.CommandText = "UPDATE [" + tableName + "] " +
                                              "  SET FailedPasswordAnswerAttemptCount = @Count" +
                                              "  WHERE Username = @Username";

                        cmd.Parameters.Clear();

                        cmd.Parameters.Add("@Count", SqlDbType.Int).Value = failureCount;
                        cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

                        if (cmd.ExecuteNonQuery() < 0)
                            throw new ProviderException("Unable to update failure count.");
                    }
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "UpdateFailureCount");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }
                Disconnect();
            }
        }


        //
        // CheckPassword
        //   Compares password values based on the MembershipPasswordFormat.
        //

        protected bool CheckPassword(string password, string dbpassword)
        {
            string pass1 = password;
            string pass2 = dbpassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    pass2 = UnEncodePassword(dbpassword);
                    break;
                case MembershipPasswordFormat.Hashed:
                    pass1 = EncodePassword(password);
                    break;
                default:
                    break;
            }

            if (pass1 == pass2)
            {
                return true;
            }

            return false;
        }


        //
        // EncodePassword
        //   Encrypts, Hashes, or leaves the password clear based on the PasswordFormat.
        //

        public string EncodePassword(string password)
        {
            string encodedPassword = password;
            if (encodedPassword == null)
            {
                return null;
            }

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                      Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    var hash = CreateHmacHashProvider();
                    hash.Key = HexToByte(machineKey.ValidationKey);

                    encodedPassword =
                      Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return encodedPassword;
        }

        private HMAC CreateHmacHashProvider()
        {
            if (string.Compare(machineKey.ValidationAlgorithm, "HMACSHA256", true) == 0)
            {
                return new HMACSHA256();
            }
            else if (string.Compare(machineKey.ValidationAlgorithm, "HMACSHA384", true) == 0)
            {
                return new HMACSHA384();
            }
            else
            {
                return new HMACSHA1();
            }
        }

        //
        // UnEncodePassword
        //   Decrypts or leaves the password clear based on the PasswordFormat.
        //


        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                      Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Cannot unencode a hashed password.");
                default:
                    throw new ProviderException("Unsupported password format.");
            }

            return password;
        }

        //
        // HexToByte
        //   Converts a hexadecimal string to a byte array. Used to convert encryption
        // key values from the configuration.
        //

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }


        //
        // MembershipProvider.FindUsersByName
        //

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {

            //SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM [" + tableName + "] " +
                      "WHERE Username LIKE @UsernameSearch", this.Connection);
            cmd.Parameters.Add("@UsernameSearch", SqlDbType.VarChar, 255).Value = usernameToMatch;

            MembershipUserCollection users = new MembershipUserCollection();

            SqlDataReader reader = null;

            try
            {
                Connect();
                totalRecords = (int)cmd.ExecuteScalar();

                if (totalRecords <= 0) { return users; }

                cmd.CommandText = string.Format(
                 @"SELECT PKID, Username, Email, PasswordQuestion,
                   Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate,
                   LastActivityDate, LastPasswordChangedDate, LastLockedOutDate 
                   FROM  [{0}] 
                   WHERE Username LIKE @UsernameSearch 
                   ORDER BY Username Asc", tableName);

                reader = cmd.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { cmd.Cancel(); }

                    counter++;
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByName");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                Disconnect();
            }

            return users;
        }

        /// <summary>
        /// MembershipProvider.FindUsersByEmail
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            //SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand("SELECT Count(*) FROM [" + tableName + "] " +
                                              "WHERE Email LIKE @EmailSearch", this.Connection);
            cmd.Parameters.Add("@EmailSearch", SqlDbType.VarChar, 255).Value = emailToMatch;

            MembershipUserCollection users = new MembershipUserCollection();

            SqlDataReader reader = null;
            totalRecords = 0;

            try
            {
                Connect();
                totalRecords = (int)cmd.ExecuteScalar();

                if (totalRecords <= 0) { return users; }

                cmd.CommandText = "SELECT PKID, Username, Email, PasswordQuestion," +
                         " Comment, IsApproved, IsLockedOut, CreationDate, LastLoginDate," +
                         " LastActivityDate, LastPasswordChangedDate, LastLockedOutDate " +
                         " FROM [" + tableName + "] " +
                         " WHERE Email LIKE @EmailSearch " +
                         " ORDER BY Username Asc";

                reader = cmd.ExecuteReader();

                int counter = 0;
                int startIndex = pageSize * pageIndex;
                int endIndex = startIndex + pageSize - 1;

                while (reader.Read())
                {
                    if (counter >= startIndex)
                    {
                        MembershipUser u = GetUserFromReader(reader);
                        users.Add(u);
                    }

                    if (counter >= endIndex) { cmd.Cancel(); }

                    counter++;
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersByEmail");

                    throw new ProviderException(exceptionMessage);
                }
                else
                {
                    throw e;
                }
            }
            finally
            {
                if (reader != null) { reader.Close(); }

                Disconnect();
            }

            return users;
        }

        /// <summary>
        /// WriteToEventLog
        /// </summary>
        /// <remarks>
        /// A helper function that writes exception detail to the event log. Exceptions
        /// are written to the event log as a security measure to avoid private database
        /// details from being returned to the browser. If a method does not return a status
        /// or boolean indicating the action succeeded or failed, a generic exception is also 
        /// thrown by the caller.
        /// </remarks>
        /// <param name="e"></param>
        /// <param name="action"></param>
        protected void WriteToEventLog(Exception e, string action)
        {
            try
            {
                EventLog log = new EventLog();
                log.Source = eventSource;
                log.Log = eventLog;

                string message = "An exception occurred communicating with the data source.\n\n";
                message += "Action: " + action + "\n\n";
                message += "Exception: " + e.ToString();

                log.WriteEntry(message);
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine(ex.Message); }
        }


        protected Object checkDBNull(object Value)
        {
            return Value == null ? DBNull.Value : Value;
        }
    }
}
