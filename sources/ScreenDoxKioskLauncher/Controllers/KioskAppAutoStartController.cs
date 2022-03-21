using Common.Logging;

using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScreenDoxKioskLauncher.Controllers
{
    public class KioskAppAutoStartController : TimerEventController
    {
        private readonly IKioskAppManagerService _kioskAppManagerService;
        private readonly IApplicationStateService _applicationStateService;
        private readonly IEnvironmentProvider _environmentProvider;
        private readonly ILog _logger = LogManager.GetLogger<KioskAppAutoStartController>();

        public KioskAppAutoStartController(
            IKioskAppManagerService kioskAppManagerService,
            IApplicationStateService applicationStateService,
            IEnvironmentProvider environmentProvider
            )
        {
            _kioskAppManagerService = kioskAppManagerService ?? throw new ArgumentNullException(nameof(kioskAppManagerService));
            _applicationStateService = applicationStateService ?? throw new ArgumentNullException(nameof(applicationStateService));
            _environmentProvider = environmentProvider ?? throw new ArgumentNullException(nameof(environmentProvider));
        }

        protected override TimeSpan TimerInterval
        {
            get
            {
                return TimeSpan.FromSeconds(_environmentProvider.KioskAppAutoStartIntervalInSeconds);
            }
        }

        protected override string ControllerName
        {
            get { return "KioskAppAutoStartController"; }
        }

        public override void OnTimerTickAction()
        {
            if (_applicationStateService.GetState() == KioskApplicationState.Normal)
            {
                // start kiosk only when normal mode
                _kioskAppManagerService.StartKioskApp();
            }
            else
            {
                _logger.Info("Skip kiosk start because application state not in Normal state.");
            }
        }
    }
}
