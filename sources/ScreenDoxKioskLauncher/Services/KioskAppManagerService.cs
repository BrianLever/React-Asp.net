using Common.Logging;
using ScreenDoxKioskLauncher.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDoxKioskLauncher.Services
{
    /// <summary>
    /// Starts and Stops kiosk application
    /// </summary>
    public class KioskAppManagerService : IKioskAppManagerService
    {
        private readonly ILog _logger = LogManager.GetLogger<KioskAppManagerService>();
        private readonly CommandFactory _commandFactory;

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="commandFactory"></param>
        public KioskAppManagerService(CommandFactory commandFactory)
        {
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        /// <summary>
        /// Stop kiosk application
        /// </summary>
        public void StopKioskApp()
        {
            _logger.Info("Shutting down all open kiosk app instances...");

            _commandFactory.StopKioskCommand.Run();
        }

        /// <summary>
        /// Starts kiosk app
        /// </summary>
        public void StartKioskApp()
        {
            _logger.Debug("Starting kiosk app if not running...");
            _commandFactory.StartKioskCommand.Run();
        }

    }
}
