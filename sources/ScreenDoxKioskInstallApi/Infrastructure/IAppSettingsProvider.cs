namespace ScreenDoxKioskInstallApi.Infrastructure
{
    public interface IAppSettingsProvider
    {
        string KioskInstallationDirectoryRoot { get; }
    }
}