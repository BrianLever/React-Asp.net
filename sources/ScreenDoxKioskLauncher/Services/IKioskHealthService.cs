using ScreenDoxKioskLauncher.Models;
using System;

namespace ScreenDoxKioskLauncher.Services
{
    public interface IKioskHealthService
    {
        TimeSpan GeTimeSinceLastKioskActivity();

        KioskLastActivity GetKioskLastActivity();
    }
}