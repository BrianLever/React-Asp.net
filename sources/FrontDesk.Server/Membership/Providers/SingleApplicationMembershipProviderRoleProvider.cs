using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;
using System.Web.Security;

namespace FrontDesk.Server.Membership.Providers
{
    [Obfuscation(Feature = "renaming", Exclude = true, ApplyToMembers = true)]  // class name, method names AND method arguments are not renamed - need for datasources on pages
    public class SingleApplicationMembershipProviderRoleProvider : RoleProvider
    {

        //
        // Global connection string, generic exception message, event log info.
        //

        private string rolesTable = "Roles";
        private string usersInRolesTable = "UsersInRoles";

        private string eventSource = "FrontDesk MembershipProviderRoleProvider";
        private string eventLog = "Application";
        private string exceptionMessage = "An exception occurred. Please check the Event Log.";

        private ConnectionStringSettings pConnectionStringSettings;
        private string connectionString;


        //
        // If false, exceptions are thrown to the caller. If true,
        // exceptions are written to the event log.
        //

        private bool pWriteExceptionsToEventLog = false;

        public bool WriteExceptionsToEventLog
        {
            get { return pWriteExceptionsToEventLog; }
            set { pWriteExceptionsToEventLog = value; }
        }



        //
        // System.Configuration.Provider.ProviderBase.Initialize Method
        //

        public override void Initialize(string name, NameValueCollection config)
        {

            //
            // Initialize values from web.config.
            //

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "SingleApplicationMembershipProviderRoleProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Role provider for NuVasive application");
            }

            // Initialize the abstract base class.
            base.Initialize(name, config);


            if (config["applicationName"] == null || config["applicationName"].Trim() == "")
            {
                pApplicationName = System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath;
            }
            else
            {
                pApplicationName = config["applicationName"];
            }


            if (config["writeExceptionsToEventLog"] != null)
            {
                if (config["writeExceptionsToEventLog"].ToUpper() == "TRUE")
                {
                    pWriteExceptionsToEventLog = true;
                }
            }


            //
            // Initialize SqlConnection.
            //

            pConnectionStringSettings = ConfigurationManager.
              ConnectionStrings[config["connectionStringName"]];

            if (pConnectionStringSettings == null || pConnectionStringSettings.ConnectionString.Trim() == "")
            {
                throw new ProviderException("Connection string cannot be blank.");
            }

            connectionString = pConnectionStringSettings.ConnectionString;
        }



        //
        // System.Web.Security.RoleProvider properties.
        //


        private string pApplicationName;


        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        //
        // System.Web.Security.RoleProvider methods.
        //

        //
        // RoleProvider.AddUsersToRoles
        //

        public override void AddUsersToRoles(string[] usernames, string[] rolenames)
        {
            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                if (username.IndexOf(',') > 0)
                {
                    throw new ArgumentException("User names cannot contain commas.");
                }

                foreach (string rolename in rolenames)
                {
                    if (IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is already in role.");
                    }
                }
            }


            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"INSERT INTO [{0}]
                    (Username, Rolename)
                    Values(@Username, @Rolename)", usersInRolesTable), conn);

            SqlParameter userParm = cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255);
            SqlParameter roleParm = cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255);

            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    if (!username.Equals("su"))
                    {
                        foreach (string rolename in rolenames)
                        {
                            userParm.Value = username;
                            roleParm.Value = rolename;
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                tran.Commit();
            }
            catch (SqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "AddUsersToRoles");
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


        //
        // RoleProvider.CreateRole
        //

        public override void CreateRole(string rolename)
        {
            if (rolename.IndexOf(',') > 0)
            {
                throw new ArgumentException("Role names cannot contain commas.");
            }

            if (RoleExists(rolename))
            {
                throw new ProviderException("Role name already exists.");
            }

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"INSERT INTO [{0}]
                     (Rolename) 
                     Values(@Rolename)", rolesTable), conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "CreateRole");
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


        //
        // RoleProvider.DeleteRole
        //

        public override bool DeleteRole(string rolename, bool throwOnPopulatedRole)
        {
            if (!RoleExists(rolename))
            {
                throw new ProviderException("Role does not exist.");
            }

            if (throwOnPopulatedRole && GetUsersInRole(rolename).Length > 0)
            {
                throw new ProviderException("Cannot delete a populated role.");
            }

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"DELETE FROM [{0}]
                     WHERE Rolename = @Rolename", rolesTable), conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;

            SqlCommand cmd2 = new SqlCommand(string.Format(@"DELETE FROM [{0}]
                     WHERE Rolename = @Rolename", usersInRolesTable), conn);

            cmd2.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;

            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;
                cmd2.Transaction = tran;

                cmd2.ExecuteNonQuery();
                cmd.ExecuteNonQuery();

                tran.Commit();
            }
            catch (SqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "DeleteRole");

                    return false;
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

            return true;
        }


        //
        // RoleProvider.GetAllRoles
        //

        public override string[] GetAllRoles()
        {
            List<string> roles = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"SELECT Rolename FROM [{0}]", rolesTable), conn);

            SqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader.GetString(0));
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetAllRoles");
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


            return roles.ToArray();
        }


        //
        // RoleProvider.GetRolesForUser
        //

        public override string[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"SELECT Rolename FROM [{0}]
                     WHERE Username = @Username", usersInRolesTable), conn);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;

            SqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader.GetString(0));
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetRolesForUser");
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

                return roles.ToArray();
        }

        //
        // RoleProvider.GetUsersInRole
        //

        public override string[] GetUsersInRole(string rolename)
        {
            List<string> users = new List<string>();

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"SELECT Username FROM [{0}]
                       WHERE Rolename = @Rolename", usersInRolesTable), conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;

            SqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(reader.GetString(0));
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "GetUsersInRole");
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

           
            return users.ToArray();
        }


        //
        // RoleProvider.IsUserInRole
        //

        public override bool IsUserInRole(string username, string rolename)
        {
            bool userIsInRole = false;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"SELECT COUNT(*) FROM [{0}]
                     WHERE Username = @Username AND Rolename = @Rolename", usersInRolesTable), conn);

            cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255).Value = username;
            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;

            try
            {
                conn.Open();

                int numRecs = (int)cmd.ExecuteScalar();

                if (numRecs > 0)
                {
                    userIsInRole = true;
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "IsUserInRole");
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

            return userIsInRole;
        }


        //
        // RoleProvider.RemoveUsersFromRoles
        //

        public override void RemoveUsersFromRoles(string[] usernames, string[] rolenames)
        {

            foreach (string rolename in rolenames)
            {
                if (!RoleExists(rolename))
                {
                    throw new ProviderException("Role name not found.");
                }
            }

            foreach (string username in usernames)
            {
                foreach (string rolename in rolenames)
                {
                    if (!IsUserInRole(username, rolename))
                    {
                        throw new ProviderException("User is not in role.");
                    }
                }
            }


            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"DELETE FROM [{0}]" +
                    " WHERE Username = @Username AND Rolename = @Rolename",usersInRolesTable), conn);

            SqlParameter userParm = cmd.Parameters.Add("@Username", SqlDbType.VarChar, 255);
            SqlParameter roleParm = cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255);

            SqlTransaction tran = null;

            try
            {
                conn.Open();
                tran = conn.BeginTransaction();
                cmd.Transaction = tran;

                foreach (string username in usernames)
                {
                    foreach (string rolename in rolenames)
                    {
                        if (username.Equals("su"))
                        {
                            continue;
                        }
                        userParm.Value = username;
                        roleParm.Value = rolename;
                        cmd.ExecuteNonQuery();
                    }
                }

                tran.Commit();
            }
            catch (SqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch { }


                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RemoveUsersFromRoles");
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


        //
        // RoleProvider.RoleExists
        //

        public override bool RoleExists(string rolename)
        {
            bool exists = false;

            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"SELECT COUNT(*) FROM [{0}]
                      WHERE Rolename = @Rolename",rolesTable), conn);

            cmd.Parameters.Add("@Rolename", SqlDbType.VarChar, 255).Value = rolename;

            try
            {
                conn.Open();

                int numRecs = (int)cmd.ExecuteScalar();

                if (numRecs > 0)
                {
                    exists = true;
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "RoleExists");
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

            return exists;
        }

        //
        // RoleProvider.FindUsersInRole
        //

        public override string[] FindUsersInRole(string rolename, string usernameToMatch)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(string.Format(@"SELECT Username FROM [{0}] 
                      WHERE Username LIKE @UsernameSearch AND RoleName = @RoleName",usersInRolesTable), conn);
            cmd.Parameters.Add("@UsernameSearch", SqlDbType.VarChar, 255).Value = usernameToMatch;
            cmd.Parameters.Add("@RoleName", SqlDbType.VarChar, 255).Value = rolename;

           List<string> users = new List<string>();
            SqlDataReader reader = null;

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(reader.GetString(0));
                }
            }
            catch (SqlException e)
            {
                if (WriteExceptionsToEventLog)
                {
                    WriteToEventLog(e, "FindUsersInRole");
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

           return users.ToArray();
        }

        //
        // WriteToEventLog
        //   A helper function that writes exception detail to the event log. Exceptions
        // are written to the event log as a security measure to avoid private database
        // details from being returned to the browser. If a method does not return a status
        // or boolean indicating the action succeeded or failed, a generic exception is also 
        // thrown by the caller.
        //

        private void WriteToEventLog(SqlException e, string action)
        {
            EventLog log = new EventLog();
            log.Source = eventSource;
            log.Log = eventLog;

            string message = exceptionMessage + "\n\n";
            message += "Action: " + action + "\n\n";
            message += "Exception: " + e.ToString();

            log.WriteEntry(message);
        }

        
    }
}
