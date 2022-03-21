namespace FrontDesk.Kiosk.Discovery
{
    public interface ISelfDiscoveryService
    {
        string GetAppVersion();
        string GetIpAddress();
    }
}