namespace ScreenDoxKioskLauncher.Infrastructure
{
    /// <summary>
    /// Helper class to read AppSettings from the configuration file
    /// </summary>
    internal class AppSettings : FrontDesk.Common.Configuration.AppSettingsProxy
    {
       

        /// <summary>
        /// When enabled the title bar is rendered and window can be scaled. Used only for testing
        /// </summary>
        /// <remarks>Default value is false</remarks>
        public static bool WindowModeEnabled
        {
            get
            {
                return GetIntValue("WindowModeEnabled", 0) > 0;
            }
        }

    }
}
