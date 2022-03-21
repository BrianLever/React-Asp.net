using System;
using FrontDesk.Common.Debugging;

namespace FrontDesk.Kiosk.Settings
{
	/// <summary>
	/// Helper class to read AppSettings from the configuration file
	/// </summary>
	internal class AppSettings : FrontDesk.Common.Configuration.AppSettingsProxy
    {
        /// <summary>
        /// Timeout in seconds for the "Thank You" page
        /// </summary>
        /// <remarks>Default value 10 seconds</remarks>
        public static int ThankYouPageTimeoutInSeconds
        {
            get
            {
                return GetIntValue("ThankYouPageTimeoutInSeconds", 10);
            }
        }
        /// <summary>
        /// Timeout in seconds for the error message when application cannot send results to the server
        /// </summary>
        public static int SaveResultErrorPageTimeoutInSeconds
        {
            get
            {
                return GetIntValue("SaveResultErrorPageTimeoutInSeconds", 15);
            }
        }

        /// <summary>
        /// User session timeout in seconds for Kiosk Application.
        /// </summary>
        public static int UserSessionTimeoutInSeconds
        {
            get
            {
                return GetIntValue("UserSessionTimeoutInSeconds", 90);
            }
        }

        /// <summary>
        /// Number of seconds before use session is expired to show "Press Now.." button. By defauilt 5 seconds
        /// </summary>
        public static int PressMeButtonTimeBeforeUserSessionTimeoutInSeconds
        {
            get
            {
                return GetIntValue("PressMeButtonTimeBeforeUserSessionTimeoutInSeconds", 5);
            }
        }


        

        /// <summary>
        /// Kiosk installation unique identifier that have to be registered in the system
        /// </summary>
        public static short KioskID
        {
            get
            {
                short kioskID = (short)0;

                var packedString = GetStringValue("KioskKey", string.Empty);

                if (!string.IsNullOrEmpty(packedString))
                {
                    try
                    {
                        //Convert from alphabet representation to short int
                        kioskID = FrontDesk.Common.TextFormatHelper.UnpackStringInt16(packedString);

                    }
                    catch (ArgumentException ex)
                    {
                        DebugLogger.TraceException(ex, "Invalid Kiosk Key value");
                        throw new ApplicationException("Invalid Kiosk Key value", ex);
                    }
                    catch (Exception ex)
                    {
                        DebugLogger.TraceException(ex);
                        kioskID = 0;
                    }
                }
                return kioskID;
            }
        }


        /// <summary>
        /// Kiosk installation unique identifier as alpanumeric string
        /// </summary>
        public static string KioskKey
        {
            get
            {
                var packedString = GetStringValue("KioskKey", string.Empty);
                return packedString;
            }
        }
        /// <summary>
        /// Kiosk secret
        /// </summary>
        public static string KioskSecret
        {
            get
            {
                var packedString = GetStringValue("KioskSecret", string.Empty);
                return packedString;
            }
        }

        /// <summary>
        /// Interval for pinging server connection availability
        /// </summary>
        public static int ServerConnectionPingIntervalInSeconds
        {
            get
            {
                return GetIntValue("ServerConnectionPingIntervalInSeconds", 10);
            }
        }

        /// <summary>
        /// The max sequence length of unsuccessful ping requests after which server connection is considered to be unavailable
        /// </summary>
        /// <remarks>Default value is 6</remarks>
        public static int MaxFailedPingRequestSequenceLength
        {
            get
            {
                return GetIntValue("MaxFailedPingRequestSequenceLength", 6);
            }
        }

        public static int SucceedEventsProbLength
        {
            get
            {
                return GetIntValue("SucceedEventsProbLength", 3);
            }
        }


        /// <summary>
        /// Show or hide cursor on screen
        /// </summary>
        /// <remarks>Cursor: 1 (true) - show cursor, for test mode, 0 (false) - hide cursor, for touchscreen</remarks>
        public static bool ShowCursor
        {
            get
            {
                var intValue = GetIntValue("Cursor", 0);
                return intValue > 0;
            }
        }


        /// <summary>
        /// Interval for requesting new updates of screening section minimal age values from server
        /// </summary>
        public static int ServerMinAgeDataUpdateIntervalInSeconds
        {
            get
            {
                return GetIntValue("ServerMinAgeDataUpdateIntervalInSeconds", 300);
            }
        }

        /// <summary>
        /// Interval for requesting new updates of lookup tables, by default 1 hour
        /// </summary>
        public static int LookupValuesDataUpdateIntervalInSeconds
        {
            get
            {
                return GetIntValue("LookupValuesDataUpdateIntervalInSeconds", 600);
            }
        }

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

        /// <summary>
        /// When enabled kiosk version is shown on the main screen
        /// </summary>
        /// <remarks>Default value is false</remarks>
        public static bool DisplayVersionOnMainScreen
        {
            get
            {
                return GetIntValue("DisplayVersionOnMainScreen", 0) > 0;
            }
        }

        public static int SendResultsRetryIntervalInMilliseconds
        {
            get
            {
                return GetIntValue("SendResultsRetryIntervalInMilliseconds", 500);
            }
        }
    }
}
