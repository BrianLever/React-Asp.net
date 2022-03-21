using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class UserSearchItemResponse
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
        public string Email { get; set; }
        public string UserName { get; set; }

        public bool IsBlock { get; set; }
    }
}
