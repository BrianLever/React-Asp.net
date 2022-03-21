using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// User account information
    /// </summary>
    public class UserPrincipal : IUserPrincipal
    {
        /// <summary>
        /// Unique user ID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// First name
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last name
        /// </summary>
        public string LastName { get; set; }
        /// <summary>
        /// Middle name
        /// </summary>
        public string MiddleName { get; set; }
        /// <summary>
        /// Contact phone
        /// </summary>
        public string ContactPhone { get; set; }
        /// <summary>
        /// State Code
        /// </summary>
        public string StateCode { get; set; }
        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Address Line 1
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Address Line 2
        /// </summary>
        public string AddressLine2 { get; set; }
        /// <summary>
        /// Postal Code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Assigned system role
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Associated Branch Location. Null for Super Admin users
        /// </summary>
        public int? BranchLocationID
        {
            get;
            set;
        }
        // <summary>
        /// Associated Branch Location. Null for Super Admin users
        /// </summary>
        public string BranchLocationName
        {
            get;
            set;
        }
        public bool IsBlock { get; set; }
        public string Comments { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }


        public DateTime? LastPasswordChangedDate { get; set; }

        /// <summary>
        /// User's full name. Populated only when reading from the database.
        /// </summary>
        public string FullName { get; set; }


        public bool IsUserInRole(string roleName)
        {
            return string.Compare(this.RoleName, roleName, StringComparison.OrdinalIgnoreCase) == 0;
        }

    }
}
