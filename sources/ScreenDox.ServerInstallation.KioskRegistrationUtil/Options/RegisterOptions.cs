using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScreenDox.ServerInstallation.KioskRegistrationUtil.Options
{
    [Verb("register", HelpText = "Generate kiosk numbers and installation script.")]
    public class RegisterOptions
    {
        [Option('s', "start", Required = true, HelpText = "Start seq. number for the kiosk.")]
        public short Start { get; set; }

        [Option('c', "count", Required = true, HelpText = "Number of kiosks.")]
        public short Count { get; set; }
    }
}
