namespace Frontdesk.Server.SmartExport.Services
{
    public interface IAppConfigurationService
    {
        bool GetRunIsTestModeFlag();
        string[] GetAllowedVisitCategories();

        int ExportAttemptCountOnIgnore { get; }
        int ExportAttemptCountOnFailure { get; }
    }
}