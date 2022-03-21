using System;

namespace ScreenDox.Server.Models
{
    /// <summary>
    /// Update user request
    /// </summary>
    public class UserPrincipalRequest
    {
        /// <summary>
        /// Assigned system role (group)
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// Associated Branch Location
        /// </summary>
        public int BranchLocationID { get; set; }

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
        /// Optional Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// City
        /// </summary>
        public string City { get; set; }
        
        /// <summary>
        /// State Code
        /// </summary>
        public string StateCode { get; set; }
        
        /// <summary>
        /// Address Line 1
        /// </summary>
        public string AddressLine1 { get; set; }
        /// <summary>
        /// Address Line 2
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Contact phone
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// Postal Code
        /// </summary>
        public string PostalCode { get; set; }

        public string Comments { get; set; }
    }
}
