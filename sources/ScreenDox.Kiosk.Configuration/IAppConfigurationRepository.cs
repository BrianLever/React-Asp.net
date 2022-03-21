namespace ScreenDox.Kiosk.Configuration
{
    public interface IAppConfigurationRepository
    {
        AppConfiguration Read(string appConfigurationFileFullPath);
        void Write(AppConfiguration configuration, string appConfigurationFileFullPath);
    }
}