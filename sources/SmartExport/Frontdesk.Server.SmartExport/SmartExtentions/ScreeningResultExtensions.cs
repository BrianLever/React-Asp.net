using FrontDesk;

namespace Frontdesk.Server.SmartExport.SmartExtentions
{
    public static class ScreeningResultExtensions
    {
        public static void CleanUpAddress(this ScreeningResult screeningResult)
        {
            if (screeningResult == null) return;

            screeningResult.StreetAddress = string.Empty;
            screeningResult.City = string.Empty;
            screeningResult.StateID = string.Empty;
            screeningResult.StateName = string.Empty;
            screeningResult.ZipCode = string.Empty;
            screeningResult.Phone = string.Empty;
        }
    }
}
