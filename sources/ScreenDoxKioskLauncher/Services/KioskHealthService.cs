using Common.Logging;

using ScreenDoxKioskLauncher.Infrastructure;
using ScreenDoxKioskLauncher.Models;
using System;

namespace ScreenDoxKioskLauncher.Services
{
    public class KioskHealthService : IKioskHealthService
    {
        private readonly IKioskInstallApiClient _client;

        private ILog _logger = LogManager.GetLogger<KioskHealthService>();

        public KioskHealthService(IKioskInstallApiClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public TimeSpan GeTimeSinceLastKioskActivity()
        {
            return _client.GeTimeSinceLastKioskActivity().TimeSinceLastActivity;
        }

        public KioskLastActivity GetKioskLastActivity()
        {
            return _client.GeTimeSinceLastKioskActivity();
        }

    }
}
