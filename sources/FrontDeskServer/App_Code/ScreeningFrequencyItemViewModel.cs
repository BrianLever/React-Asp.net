using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FrontDesk.Configuration;

namespace FrontDesk.Server.Screening
{
    [Obsolete("moved to ScreenDox.Models")]
    public class ScreeningFrequencyItemViewModel : ScreeningFrequencyItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
