using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Membership
{
    public class UserService : IUserService
    {

        public int GetExportSystemUserID()
        {
            return FDUser.GetExportSystemUserID();
        }
    }
}
