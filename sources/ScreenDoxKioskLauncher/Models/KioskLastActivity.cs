using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDoxKioskLauncher.Models
{
    /// <summary>
    /// Time since last kiosk activity
    /// </summary>
    public class KioskLastActivity
    {
        /// <summary>
        /// Time span from the last kiosk activity
        /// </summary>
        public TimeSpan TimeSinceLastActivity { get; set; }
        /// <summary>
        /// Kiosk last activity date
        /// </summary>
        public DateTime? LastActivityUtc { get; set; }
    }
}
