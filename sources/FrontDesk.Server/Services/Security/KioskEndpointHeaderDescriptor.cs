using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrontDesk.Server.Services.Security
{
    public static class KioskEndpointHeaderDescriptor
    {
        public const string KioskIDHeader = "ScreenDox-KioskKey";
        public const string KioskSecretHeader = "ScreenDox-KioskSecret";
    }
}
