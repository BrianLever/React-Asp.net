using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScreenDoxKioskInstallApi.Models
{
    public class KioskLastActivity
    {
        public TimeSpan TimeSinceLastActivity { get; set; }
        public DateTime? LastActivityUtc { get; set; }

    }
}