using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Services.Security
{
    public class KioskAuthorizeException : FaultException
    {
        public KioskAuthorizeException(string reason) : base(reason)
        {

        }
    }
}
