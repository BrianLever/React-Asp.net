using FrontDesk.Common.Data;
using FrontDesk.Common.Extensions;

using ScreenDox.Server.Models;
using ScreenDox.Server.Models.Security;

using System;
using System.Collections.Generic;
using System.Data;

namespace ScreenDox.Server.Security
{
    public static class UserPrincipalFactory
    {
        public static UserPrincipal CreateFromDbReader(IDataReader reader)
        {

            var user = new UserPrincipal();

            PopulatePrincipalObject(user, reader);


            return user;
        }

        private static UserPrincipal PopulatePrincipalObject(UserPrincipal user, IDataReader reader)
        {
            List<string> columnName = DBDatabase.GetReaderColumnNames(reader);

            user.UserID = Convert.IsDBNull(reader["UserID"]) ? 0 : Convert.ToInt32(reader["UserID"]);
            user.FirstName = Convert.ToString(reader["FirstName"]);
            user.LastName = Convert.ToString(reader["LastName"]);
            user.MiddleName = Convert.ToString(reader["MiddleName"]);
            user.ContactPhone = Convert.ToString(reader["ContactPhone"]);
            user.StateCode = Convert.ToString(reader["StateCode"]);
            user.City = Convert.ToString(reader["City"]);
            user.AddressLine1 = Convert.ToString(reader["AddressLine1"]);
            user.AddressLine2 = Convert.ToString(reader["AddressLine2"]);
            user.PostalCode = Convert.ToString(reader["PostalCode"]);
            user.IsBlock = Convert.ToBoolean(reader["IsBlock"]);


            if (columnName.Contains("Username"))
            {
                user.UserName = Convert.ToString(reader["Username"]);
            }
            if (columnName.Contains("Email"))
            {
                user.Email = Convert.ToString(reader["Email"]);
            }
            if (columnName.Contains("Comment"))
            {
                user.Comments = Convert.ToString(reader["Comment"]);
            }
            if (columnName.Contains("BranchLocationID"))
            {
                user.BranchLocationID = Convert.IsDBNull(reader["BranchLocationID"]) ? (Int32?)null : Convert.ToInt32(reader["BranchLocationID"]);
            }
            if (columnName.Contains("Rolename"))
            {
                user.RoleName = Convert.IsDBNull(reader["Rolename"]) ? String.Empty : Convert.ToString(reader["Rolename"]);
            }
            if (columnName.Contains("LastPasswordChangedDate"))
            {
                user.LastPasswordChangedDate = Convert.IsDBNull(reader["LastPasswordChangedDate"]) ? (DateTime?)null : Convert.ToDateTime(reader["LastPasswordChangedDate"]);
            }
            if (columnName.Contains("FullName"))
            {
                user.FullName = Convert.ToString(reader["FullName"]);
            }


            return user;
        }

        private static UserPrincipalAccount PopulateAdditionalAccountProperties(UserPrincipalAccount user, IDataReader reader)
        {
            user.Password = reader.Get<string>("Password");
            user.PasswordQuestion = reader.Get<string>("PasswordQuestion");
            user.PasswordAnswer = reader.Get<string>("PasswordAnswer");
            user.FailedPasswordAttemptCount = reader.Get<int>("FailedPasswordAttemptCount");
            user.FailedPasswordAttemptWindowStart = reader.Get<DateTime>("FailedPasswordAttemptWindowStart");
            user.FailedPasswordAnswerAttemptCount = reader.Get<int>("FailedPasswordAnswerAttemptCount");
            user.FailedPasswordAnswerAttemptWindowStart = reader.Get<DateTime>("FailedPasswordAnswerAttemptWindowStart");

            return user;
        }

        public static UserPrincipalAccount CreateUserPrincipalAccount(IDataReader reader)
        {
            var user = new UserPrincipalAccount();

            PopulatePrincipalObject(user, reader);
            PopulateAdditionalAccountProperties(user, reader);

            return user;
        }
    }
}
