using ScreenDoxKioskLauncher.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDoxKioskLauncher.Commands
{
    public class CommandFactory
    {
        public ICommand<bool> StartKioskCommand { get; }
        public ICommand<bool> StopKioskCommand { get; }

        public CommandFactory(IEnvironmentProvider environmentProvider)
        {
            StartKioskCommand = new StartKioskApplicationCommand(environmentProvider);
            StopKioskCommand = new StopKioskApplicationCommand(environmentProvider);
        }



    }
}
