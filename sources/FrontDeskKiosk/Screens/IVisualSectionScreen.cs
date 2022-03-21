namespace FrontDesk.Kiosk.Screens
{
	public interface IVisualSectionScreen
    {

        ScreeningSection ScreenSection { get; set; }
        ScreeningSectionQuestion ScreenSectionQuestion { get; set; }
        ScreeningResult UserSessionResult { get; set; }

        void BeginDataBinding();
        void EndDataBinding();
    }
}
