using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Server.Models.ViewModels
{
    public class UserSearchRequest : PagedSearchFilter
    {
        /// <summary>
        /// Filter by branch location
        /// </summary>
        public int? BranchLocationId { get; set; } = null;
    }
}
