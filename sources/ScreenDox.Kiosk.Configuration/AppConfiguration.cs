using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDox.Kiosk.Configuration
{
    public class AppConfiguration
    {
        public string Key { get; set; }
        public string Secret { get; set; }
        public string ScreendoxServerBaseUrl { get; set; }
        public string ScreenDoxInstallationServiceBaseUrl { get; set; }
        public string KioskExeName { get; set; }
        public string KioskApplicationDirectory { get; set; }
    }
}
