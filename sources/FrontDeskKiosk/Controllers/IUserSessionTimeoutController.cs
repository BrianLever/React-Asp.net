using System;
namespace FrontDesk.Kiosk
{
    public interface IUserSessionTimeoutController
    {
        DateTime LastUsedTime { get; set; }
        TimeSpan SessionExpiringNotificationTimeout { get; set; }
        TimeSpan SessionTimeoutPeriod { get; set; }
        void StartMonitoring();
        void StopMonitoring();
        event EventHandler UserSessionExpired;
        event EventHandler UserSessionExpiring;
    }
}
