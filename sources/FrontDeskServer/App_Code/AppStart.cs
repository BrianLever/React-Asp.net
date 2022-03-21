using Common.Logging;

using FrontDesk.Server.App;

public static class AppStart
{
    private static ILog _logger = LogManager.GetLogger<Startup>();
    public static void AppInitialize()
    {
        _logger.Info("AppStart.AppInitialize was called.");

        //var container = Startup.RegisterDependecies();
        //AutofacHostFactory.Container = container;

        //_logger.Info("Registered WCF IoC dependencies.");
    }
}